using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CategoryDAO
    {
        public static CategoryDAO instance;
        private static readonly object instanceLock = new object();
        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }

        public Category FindOne(Expression<Func<Category, bool>> predicate)
        {
            Category category = null;
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    category = salesDB.Categories.SingleOrDefault(predicate);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return category;
        }

        public IEnumerable<Category> FindAll(Expression<Func<Category, bool>> predicate)
        {
            IEnumerable<Category> categories = new List<Category>();
            try
            {
                using (var salesDB = new As1storeContext())
                {
                    categories = salesDB.Categories.Where(predicate).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return categories;
        }

        public IEnumerable<Category> ListCategory()
        {
            try
            {
                using(var saleDB = new As1storeContext())
                {
                    return saleDB.Categories.ToList();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }


    }
}
