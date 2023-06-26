using System.Linq.Expressions;
using University_API.Models;

namespace University_API.Repository.IRepository
{
    public interface IUniRepository
    {
        Task<List<University>> GetAllAsync(Expression<Func<University,bool>>? filter = null, int pageSize = 0, int pageNumber = 1);
        Task<University> GetAsync(Expression<Func<University,bool>>? filter = null, bool tracked=true);

        Task CreateAsync(University university);

        Task UpdateAsync(University university);

        Task RemoveAsync(University university);

        Task SaveAsync();

    }
}
