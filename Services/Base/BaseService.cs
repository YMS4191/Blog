using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Entities.Core;
using Entities.ProjectContext;
using Services.Core;

namespace Services.Base
{
    public class BaseService<T> : IService<T> where T : CoreEntity
    {
        private BlogContext _context;
        public BaseService(BlogContext context)
        {
            this._context = context;
        }

        public bool Active(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Add(T item)
        {
            // .Set<T>() => parametrede gönderdiğini nesnenin tipine göre contexti ayarlar
            _context.Set<T>().Add(item);
            return true;
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            return _context.Set<T>().Any(exp);
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public List<T> GetByDefault(Expression<Func<T, bool>> exp)
        {
            return _context.Set<T>().Where(exp).ToList();
        }

        public T GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public bool Remove(T item)
        {
            _context.Set<T>().Remove(item);
            return true;
        }

        public bool RemoveRange(Expression<Func<T, bool>> exp)
        {
            _context.Set<T>().RemoveRange(_context.Set<T>().Where(exp));
            return true;
        }

        public int Save()
        {
           return _context.SaveChanges();
        }

        public bool Update(T item)
        {
            try
            {
                _context.Set<T>().Update(item);
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
