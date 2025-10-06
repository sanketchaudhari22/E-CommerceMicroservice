using System.Linq.Expressions;

namespace E_CommerceSharedLibrary.Interface
{
    public interface IGenericInterface<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> FindByIdAsync(int id);
        Task<T?> GetByAsync(Expression<Func<T, bool>> expression);
    }
}
