using HomeRun.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace HomeRun.Shared
{
    public class BaseRepository<EntityType> : IRepository<EntityType> where EntityType : class
    {
        private readonly DbContext _context;
        private readonly DbSet<EntityType> _entities;
        private readonly ILogger<BaseRepository<EntityType>> _logger;

        public BaseRepository(DbContext context, ILogger<BaseRepository<EntityType>> logger)
        {
            _context = context;
            _entities = context.Set<EntityType>();
            _logger = logger;
        }

        public async Task<EntityType?> GetById(int id) => await _entities.FindAsync(id);

        public async Task<EntityType?> Get(EntityType? entity) => await _entities.FindAsync(entity);

        public async Task<IEnumerable<EntityType>> Where(Expression<Func<EntityType, bool>> predicate) => await _entities.Where(predicate).ToListAsync();

        public async Task<IEnumerable<EntityType>> GetAll() => await _entities.ToListAsync();

        public async Task<EntityType?> Create(EntityType? entity)        
        {
            if (entity == null)
            {
                _logger.LogError("Entity cannot be null. Create operation failed.");
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                await _entities.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Entity {@entity} could not be added", entity);
                throw;
            }
        }

        public async Task Delete(int id)                                 
        {
            EntityType? entity = await _entities.FindAsync(id);
            try
            {
                if (entity == null)
                {
                    _logger.LogError("Item cannot be found and deleted. Item ID: {id}", id);
                    throw new HomeRunException("Item cannot be found and deleted.");
                }

                _entities.Remove(entity);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete operation failed for entity with ID {id}", id);
                throw;
            }
        }

        public async Task<EntityType?> Update(int id, EntityType? entity)
        {
            if (entity == null)
            {
                _logger.LogError("Entity cannot be null. Update operation failed.");
                throw new ArgumentNullException(nameof(entity));
            }

            EntityType? existingEntity = await _entities.FindAsync(id);

            if (existingEntity is null)
            {
                _logger.LogWarning("Entity cannot be found with ID {id}", id);
                throw new HomeRunException($"Entity cannot be found with ID: {id}");
            }

            try
            {
                _entities.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Entity with ID {id} updated successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update operation failed for entity with ID {id}", id);
                throw;
            }

            return existingEntity;
        }
    }
}
