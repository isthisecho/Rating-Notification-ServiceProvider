using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HomeRun.Shared
{
    public class BaseRepository<EntityType> : IRepository<EntityType> where EntityType : class
    {
        private readonly DbContext _context;
        private readonly DbSet<EntityType> _entities;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _entities = context.Set<EntityType>();
        }

        public async Task<EntityType?> Create(EntityType? entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<EntityType>?> CreateMany(IEnumerable<EntityType> entities)
        {

           await _entities.AddRangeAsync(entities);
           return entities;

        }

        public async Task Delete(Guid id)
        {

            EntityType? entity = await _entities.FindAsync(id);

            if(entity == null)
                throw new ArgumentException(nameof(entity));
             _entities.Remove(entity);

            await _context.SaveChangesAsync();


        }

        public async Task DeleteMany(IEnumerable<EntityType> entities)
        {
            _entities.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteMany(Expression<Func<EntityType, bool>> condition)
        {
            List<EntityType> entitiesToDelete = await _entities.Where(condition).ToListAsync();
            _entities.RemoveRange(entitiesToDelete);

            return await _context.SaveChangesAsync();
        }

        public async Task<EntityType?> Get(Guid id)
        {
           return await _entities.FindAsync(id);
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

        public async Task<EntityType?> Update(Guid id, EntityType? entity)
        {
            EntityType? existingEntity = await _entities.FindAsync(id);

            if (existingEntity != null && entity != null)
            {
                 _entities.Entry(existingEntity).CurrentValues.SetValues(entity);
                 await _context.SaveChangesAsync();
            }

            return existingEntity;
        }
    }
}
