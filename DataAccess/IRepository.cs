using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetBy(int id);
        void Update(T t);
        void Edit(T t);
        // IEnumerabl;e<T> Get(Predicate<T> predicate);
        void Add(T t);
        void Delete(T t);

        NorthwindContext Context { get; set; }
    }
}