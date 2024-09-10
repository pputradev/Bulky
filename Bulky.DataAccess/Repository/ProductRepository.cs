using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public void Update(Product product)
        {
            var objfromdb = _context.Products.FirstOrDefault(u=>u.Id == product.Id);
            if (objfromdb != null)
            {
                objfromdb.Title = product.Title;
                objfromdb.Description = product.Description;
                objfromdb.Price = product.Price;
                objfromdb.CategoryId = product.CategoryId;
                objfromdb.ListPrice = product.ListPrice;
                objfromdb.Price50 = product.Price50;
                objfromdb.Price100 = product.Price100;
                objfromdb.Author = product.Author;
                objfromdb.ISBN = product.ISBN;
                if (product.ImageURL != null) { objfromdb.ImageURL = product.ImageURL; }
            }
        }
    }
}
