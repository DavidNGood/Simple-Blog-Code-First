using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SimpleBlog.DAL;
using SimpleBlog.Web.Models;
using SimpleBlog.Entities;
using Microsoft.AspNet.Identity;

namespace SimpleBlog.Web.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Post/
        public ActionResult Index()
        {
            return View(unitOfWork.PostRepository.Get(orderBy: q => q.OrderByDescending(d => d.PostDate)));
        }

        // GET: /Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SimpleBlog.Entities.Post post = unitOfWork.PostRepository.GetByID(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            if (!string.IsNullOrEmpty(post.ImageName))
            {
                post.ImageName = @HttpRuntime.Cache["ImagePath"] + post.ImageName;
            }
            return View(post);
        }

        // GET: /Post/Create
        public ActionResult Create()
        {
            PopulateCategoryDropDownList();
            return View();
        }

        // POST: /Post/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SimpleBlog.Web.Models.CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                HttpPostedFileBase photo = Request.Files["photo"];

                if (photo.ContentType.Contains("image"))
                {
                    fileName = SaveImage(photo, "");
                }
                else
                {
                    fileName = model.ImageName;
                }

                var post = new SimpleBlog.Entities.Post
                {
                    Title = model.Title,
                    CategoryID = model.CategoryID,
                    Summary = model.Summary,
                    PostDate = model.PostDate,
                    UpdateDate = DateTime.Now,
                    UserID = User.Identity.GetUserId(),
                    PostBody = model.PostBody,
                    ImageName = fileName
                };

                unitOfWork.PostRepository.Insert(post);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            PopulateCategoryDropDownList();
            return View(model);
        }

        // GET: /Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SimpleBlog.Entities.Post post = unitOfWork.PostRepository.GetByID(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var viewModel = new EditPostViewModel
            {
                PostID = post.PostID,
                CategoryID = post.CategoryID,
                PostDate = post.PostDate,
                Summary = post.Summary,
                Title = post.Title,
                PostBody = post.PostBody,
                ImageName = post.ImageName
            };
            PopulateCategoryDropDownList(post.CategoryID);
            return View(viewModel);
        }

        // POST: /Post/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SimpleBlog.Web.Models.EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var post = unitOfWork.PostRepository.GetByID(model.PostID);
                string fileName = null;
                HttpPostedFileBase photo = Request.Files["photo"];
                if (photo.ContentType.Contains("image"))
                {
                    fileName = SaveImage(photo, model.ImageName);
                }
                else
                {
                    fileName = model.ImageName;
                }
                post.CategoryID = model.CategoryID;
                post.Title = model.Title;
                post.Summary = model.Summary;
                post.PostDate = model.PostDate;
                post.UpdateDate = DateTime.Now;
                //post.UserID = User.Identity.GetUserId();
                post.PostBody = model.PostBody;
                post.ImageName = fileName;
                unitOfWork.PostRepository.Update(post);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            PopulateCategoryDropDownList(model.CategoryID);
            return View(model);
        }

        // GET: /Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SimpleBlog.Entities.Post post = unitOfWork.PostRepository.GetByID(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: /Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SimpleBlog.Entities.Post post = unitOfWork.PostRepository.GetByID(id);
            unitOfWork.PostRepository.Delete(post);
            unitOfWork.Save();

            string fullPath = Server.MapPath("~" + @HttpRuntime.Cache["ImagePath"]) + post.ImageName;
            if (System.IO.File.Exists(fullPath))
            {
                try
                {
                    System.IO.File.Delete(fullPath);
                }
                catch (Exception)
                {


                }
            }
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

        private void PopulateCategoryDropDownList(object selectedCategory = null)
        {
            var categories = unitOfWork.CategoryRepository.Get(orderBy: q => q.OrderBy(d => d.Name));
            ViewBag.CategoryID = new SelectList(categories, "CategoryID", "Name", selectedCategory);
        }

        private string SaveImage(HttpPostedFileBase photo, string fileToDelete)
        {
            string path = Server.MapPath("~" + @HttpRuntime.Cache["ImagePath"]);
            var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
            string fileExt = System.IO.Path.GetExtension(photo.FileName).Substring(1);

            if (supportedTypes.Contains(fileExt))
            {
                //write GUID filename
                string fileName = Guid.NewGuid().ToString() + "." + fileExt;

                try
                {
                    photo.SaveAs(path + fileName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if (fileToDelete != "")
                {
                    string fullPath = path + fileToDelete;
                    if (System.IO.File.Exists(fullPath))
                    {
                        try
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                return fileName;
            }
            else
            {
                return null;
            }
        }
    }
}
