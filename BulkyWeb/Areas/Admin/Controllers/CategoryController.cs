using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        
        private readonly IUnitOfWork _UnitOfWork;

        public CategoryController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> objcategorylist = _UnitOfWork.Category.GetAll().ToList();
            return View(objcategorylist);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category x)
        {
            if (x.Name == x.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Both Display Order and Name can't be same");
            }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Add(x);
                _UnitOfWork.save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categorybyidfromdb = _UnitOfWork.Category.Get(u => u.Id == id);
            if (categorybyidfromdb == null)
            {
                return NotFound();
            }
            return View(categorybyidfromdb);
        }
        [HttpPost]
        public IActionResult Edit(Category x)
        {
            //if (x.Name == x.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "Both Display Order and Name can't be same");
            //}
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Update(x);
                _UnitOfWork.save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categorybyidfromdb = _UnitOfWork.Category.Get(u => u.Id == id);
            if (categorybyidfromdb == null)
            {
                return NotFound();
            }
            return View(categorybyidfromdb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _UnitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }


            _UnitOfWork.Category.Remove(obj);
            _UnitOfWork.save();
            TempData["warning"] = "Category Deleted";
            return RedirectToAction("Index");
        }
    }
}
