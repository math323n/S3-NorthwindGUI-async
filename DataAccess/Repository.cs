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
        const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        protected NorthwindContext context;
        private DbSet<T> entities = null;
        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance of Repository. Attempts to establish a consnection, and will throw an exception on connection error.
        /// </summary>
        public Repository(NorthwindContext context)
        {
            Context = context;
            entities = context.Set<T>();
        }

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
            protected set
            {
                context = value;
            }
        }
        #endregion



        #region Methods
        public IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }

        public T GetBy(int id)
        {
            return context.Set<T>().Find(id);
        }

        public void Update(T t)
        {
            context.SaveChanges();
        }

        public void Edit(T t)
        {
            throw new NotImplementedException();
        }
        public void Add(T t)
        {
            context.Set<T>().Add(t);
            context.SaveChanges();
        }

        public void Delete(T t)
        {
            context.Set<T>().Remove(t);
            context.SaveChanges();
        }


        #endregion

    }
}