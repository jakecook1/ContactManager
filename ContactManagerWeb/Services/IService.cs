using System.Collections.Generic;
using System.Threading.Tasks;
using ContactManagerWeb.Data.Paging;

namespace ContactManagerWeb.Services
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll();

        Task<IPaginate<T>> GetAllAsync(string sort, string search, int pageNumber);

        T Get(object id);

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(object id);
    }
}