using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using SimpleBlog.Entities;
using SimpleBlog.DAL;
using System.Web.Caching;

namespace SimpleBlog.Web.Helpers
{
    public class SettingsCache
    {
        public static void LoadSettings()
        {
            var unitOfWork = new UnitOfWork();
            var settings = unitOfWork.SettingRepository.Get(orderBy: q => q.OrderBy(d => d.Name));
            foreach (var item in settings)
            {
                HttpRuntime.Cache.Insert(item.Name, item.Value);
            }
        }

        public static void LoadCategories()
        {
            var unitOfWork = new UnitOfWork();
            var categories = unitOfWork.CategoryRepository.Get(orderBy: q => q.OrderBy(d => d.Name)).ToList();
            HttpRuntime.Cache.Insert("Categories", categories);
        }
    }

    public static class CategoryHelper
    {
        public static IEnumerable<Category> GetCategories()
        {
            //var unitOfWork = new UnitOfWork();
            //return unitOfWork.CategoryRepository.Get(orderBy: q => q.OrderBy(d => d.Name));

            return HttpRuntime.Cache["Categories"] as List<Category>;
        }
    }
}










