using CashFlow.Domain.Entity;
using CashFlow.Domain.Interface.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace CashFlow.Infrastructure.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext MainContext;
        protected readonly DbSet<T> dbSetEntity;
        public DbContext MainDbContext { get => MainContext; }

        public Repository(DbContext context)
        {
            MainContext = context;
            dbSetEntity = MainContext.Set<T>();
        }

        public IQueryable<T> ReturnAll()
        {
            return dbSetEntity;
        }

        public T FindById(long id)
        {
            return (T)ReturnAll()
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        public T Add(T entity)
        {
            dbSetEntity.Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            if (MainContext.Entry(entity).State == EntityState.Modified)
                dbSetEntity.Update(entity);

            return entity;
        }

        public void Add(IEnumerable<T> list)
        {
            dbSetEntity.AddRange(list);
        }

        public T Remove(T entity)
        {
            dbSetEntity.Remove(entity);
            return entity;
        }

        public void Save()
        {
            MainContext.SaveChanges();
        }
    }
}
