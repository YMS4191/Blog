using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Entities.Core;

namespace Services.Core
{
    public interface IService<T> where T : CoreEntity
    {
        public bool Add(T item );
        public bool Update(T item);
        public T GetById(Guid id);
        public bool Remove(T item);
        public bool RemoveRange(Expression<Func<T, bool>> exp);
        public List<T> GetAll();
        public List<T> GetByDefault(Expression<Func<T,bool>> exp);
        public bool Any(Expression<Func<T, bool>> exp);
        public bool Active(Guid id);
        public int Save();
    } 
}
