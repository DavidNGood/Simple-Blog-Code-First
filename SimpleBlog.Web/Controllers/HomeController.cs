using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SimpleBlog.DAL;
using PagedList;
using SimpleBlog.Entities;

namespace SimpleBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Home/
        public ActionResult Index(int? page)
        {
            var posts = unitOfWork.PostRepository.Get(orderBy: q => q.OrderByDescending(d => d.PostDate));
            int pageNumber = page ?? 1;
            int pageSize = Convert.ToInt16(HttpRuntime.Cache["PageSize"]);

            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Home/Article/5
        public ActionResult Article(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = unitOfWork.PostRepository.GetByID(id);
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

        //GET: /Home/Category/5
        public ActionResult Category(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = unitOfWork.CategoryRepository.GetByID(id.GetValueOrDefault());
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryName = category.Name;

            var posts = unitOfWork.PostRepository.Get(filter: p => p.CategoryID == id.Value);
            int pageNumber = page ?? 1;
            int pageSize = Convert.ToInt16(HttpRuntime.Cache["PageSize"]);

            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}