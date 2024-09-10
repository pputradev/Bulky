using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public ProductController(IUnitOfWork UnitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _WebHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductlist = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(objProductlist);
        }
        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new()
            {
                TempCreatedList = _UnitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _UnitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwrootpath = _WebHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwrootpath, @"images\product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageURL))
                    {
                        var oldimgpath = Path.Combine(wwwrootpath, productVM.Product.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimgpath)) { System.IO.File.Delete(oldimgpath); }

                    }

                    using (var filestrem = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestrem);
                    }
                    productVM.Product.ImageURL = @"\images\product\" + filename;
                }
                if (productVM.Product.Id == 0)
                {
                    _UnitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _UnitOfWork.Product.Update(productVM.Product);
                }
                _UnitOfWork.save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.TempCreatedList = _UnitOfWork.Category
                    .GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
                return View(productVM);
            }
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductlist = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { Data = objProductlist });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _UnitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            var oldimgpath = Path.Combine(_WebHostEnvironment.WebRootPath, obj.ImageURL.TrimStart('\\'));
            if (System.IO.File.Exists(oldimgpath)) { System.IO.File.Delete(oldimgpath); }

            _UnitOfWork.Product.Remove(obj);
            _UnitOfWork.save();

            return Json(new { success = true, message = "Deleting................=====>Deleted Succesfully" });
        }
        #endregion
    }
}
