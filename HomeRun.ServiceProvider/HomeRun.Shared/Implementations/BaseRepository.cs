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
        private readonly  ILogger<BaseRepository<EntityType>> _logger;  
        public BaseRepository(DbContext context , ILogger<BaseRepository<EntityType>> logger)
        {
            _context = context;
            _entities = context.Set<EntityType>();
            _logger = logger;
        }

        public async Task<EntityType?> Create(EntityType? entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                await _entities.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Entity {entity} can not be added | {ex.Message}");
                throw new HomeRunException($"Entity {entity} can not be added | {ex.Message} ");
            }

            return entity;
        }
        public async Task Delete(int id)
        {
            EntityType? entity = await _entities.FindAsync(id);

            if (entity == null)
                throw new HomeRunException("Item can not be found and deleted.");

             _entities.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<EntityType?> GetById(int id)
        {
            EntityType? values = await _entities.FindAsync(id);

            if (values is not null)
                return values;

            else
                throw new HomeRunException($"Values is null with id : {id}");

        }

        public async Task<EntityType?> Get(EntityType? entity)
        {
            return await _entities.FindAsync(entity);
        }

        public async Task<IEnumerable<EntityType>> Where(Expression<Func<EntityType, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<EntityType>> GetAll()
        {
            return await _entities.ToListAsync();   
        }

        public async Task<EntityType?> Update(int id, EntityType? entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));


            EntityType? existingEntity = await _entities.FindAsync(id);

            if (existingEntity is null)
               throw new HomeRunException("Entity can not be found with id :{}");

            try
            {
               _entities.Entry(existingEntity).CurrentValues.SetValues(entity);
               await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
               throw new Exception($"Update operation failed: {ex.Message}");
            }

            return existingEntity;
        }
    }
}
