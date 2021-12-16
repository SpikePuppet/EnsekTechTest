using System.Linq.Expressions;
namespace EnsekTechTestServices.Interfaces
{
    public interface IEntityService<T> where T : class, new()
    {
        List<T> GetAll();

        List<T> Get(List<int> ids);

        List<T> Search(Expression<Func<T, bool>> predicate);

        List<T> Add(List<T> entities);

        void Delete(List<T> entities);

        void Update(List<T> entities);
    }
}