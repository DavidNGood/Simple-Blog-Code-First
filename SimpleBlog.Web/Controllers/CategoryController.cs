using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SimpleBlog.Entities;
using SimpleBlog.DAL;
using SimpleBlog.Web.Models;
using SimpleBlog.Web.Helpers;

namespace SimpleBlog.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Category/
        public ActionResult Index()
        {
            return View(unitOfWork.CategoryRepository.Get(orderBy: q => q.OrderBy(d => d.Name)));
        }

        // GET: /Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = unitOfWork.CategoryRepository.GetByID(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: /Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = model.Name
                };
                unitOfWork.CategoryRepository.Insert(category);
                unitOfWork.Save();
                SettingsCache.LoadCategories();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = unitOfWork.CategoryRepository.GetByID(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var viewModel = new EditCategoryViewModel
            {
                CategoryID = category.CategoryID,
                Name = category.Name
            };
            return View(viewModel);
        }

        // POST: /Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = unitOfWork.CategoryRepository.GetByID(model.CategoryID);
                category.Name = model.Name;
                unitOfWork.CategoryRepository.Update(category);
                unitOfWork.Save();
                SettingsCache.LoadCategories();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: /Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = unitOfWork.CategoryRepository.GetByID(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            unitOfWork.CategoryRepository.Delete(id);
            unitOfWork.Save();
            SettingsCache.LoadCategories();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
