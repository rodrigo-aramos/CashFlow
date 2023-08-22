using CashFlow.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CashFlow.Domain.Interface.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbContext MainDbContext { get; }
        IQueryable<T> ReturnAll();
        T FindById(long id);
        T Add(T entity);
        void Add(IEnumerable<T> entity);
        T Update(T entity);
        T Remove(T entity);
        void Save();
    }
}
