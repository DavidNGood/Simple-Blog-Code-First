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

namespace SimpleBlog.Web.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Settings/
        public ActionResult Index()
        {
            return View(unitOfWork.SettingRepository.Get(orderBy: q => q.OrderBy(d => d.Name)));
        }

        // GET: /Settings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = unitOfWork.SettingRepository.GetByID(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // GET: /Settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SimpleBlog.Web.Models.CreateSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var setting = new Setting
                {
                    Name = model.Name,
                    Value = model.Value
                };
                unitOfWork.SettingRepository.Insert(setting);
                unitOfWork.Save();
                HttpRuntime.Cache.Insert(model.Name, model.Value);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = unitOfWork.SettingRepository.GetByID(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            var viewModel = new SimpleBlog.Entities.Setting
            {
                Name = setting.Name,
                Value = setting.Value,
                SettingID = setting.SettingID
            };
            return View(viewModel);
        }

        // POST: /Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SimpleBlog.Web.Models.EditSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var setting = unitOfWork.SettingRepository.GetByID(model.SettingID);
                setting.Name = model.Name;
                setting.Value = model.Value;
                unitOfWork.SettingRepository.Update(setting);
                unitOfWork.Save();
                HttpRuntime.Cache.Insert(model.Name, model.Value);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: /Settings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Setting setting = unitOfWork.SettingRepository.GetByID(id);
            if (setting == null)
            {
                return HttpNotFound();
            }
            return View(setting);
        }

        // POST: /Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var setting = unitOfWork.SettingRepository.GetByID(id);
            unitOfWork.SettingRepository.Delete(setting);
            HttpRuntime.Cache.Remove(setting.Name);
            unitOfWork.Save();

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
