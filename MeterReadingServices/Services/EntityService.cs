using System.Linq.Expressions;
using MeterReadingDatabase;
using MeterReadingServices.Interfaces;

namespace MeterReadingServices.Services
{
    public class EntityService<T> : IEntityService<T> where T : class, new()
    {
        private readonly MeterReadingDbContext _dbContext;

        public EntityService(MeterReadingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public List<T> Get(List<int> ids)
        {
            return ids.Select(id => _dbContext.Set<T>().Find(id)).ToList();
        }

        public List<T> Search(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).ToList();
        }

        public List<T> Add(List<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
            _dbContext.SaveChanges();

            return entities;
        }

        public void Delete(List<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public void Update(List<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            _dbContext.SaveChanges();
        }
    }
}