using Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Repository<T>: IRepository<T> where T : class
    {

        #region Fields and constants
       
        protected NorthwindContext context;

        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance of Repository. Attempts to establish a consnection, and will throw an exception on connection error.
        /// </summary>
        public Repository(NorthwindContext context)
        {
            Context = context;
        }
        public Repository() { }

        /// <summary>
        /// Initialize the Repository, try get Connection, else catch Exception
        /// </summary>
        /// <returns></returns>
        public virtual void InitRepository()
        {

            try
            {
               context.Database.EnsureCreated();

            }
            catch(Exception e)
            {
                throw new Exception("Data access error. See inner exception for details", e);
            }

        }
        #endregion

        #region Properties
        public virtual NorthwindContext Context
        {
            get
            {
                return context;
            }
            set
            {
                context = value;
            }
        }
        #endregion



        #region Methods
        public virtual IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }

        public virtual T GetBy(int id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual void Update(T t)
        {
            context.SaveChanges();
        }

        public virtual void Edit(T t)
        {
            throw new NotImplementedException();
        }
        public virtual void Add(T t)
        {
            context.Set<T>().Add(t);
            context.SaveChanges();
        }

        public virtual void Delete(T t)
        {
            context.Set<T>().Remove(t);
            context.SaveChanges();
        }


        #endregion

    }
}