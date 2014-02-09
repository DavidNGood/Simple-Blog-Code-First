using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SimpleBlog.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace SimpleBlog.DAL
{
    public class SimpleBlogInitializer : CreateDatabaseIfNotExists<SimpleBlogContext>
    {
        protected override void Seed(SimpleBlogContext context)
        {
            Setting s1 = new Setting() { Name = "BlogTitle", Value = "Sample Blog Title" };
            context.Settings.Add(s1);

            Setting s2 = new Setting() { Name = "ImagePath", Value = "/Images/Uploads/" };
            context.Settings.Add(s2);

            Setting s3 = new Setting() { Name = "PageSize", Value = "5" };
            context.Settings.Add(s3);

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)); 

              var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
   
              string name = "Admin";
              string password = "pass123";
              string role = "Administrators";
 
  
              //Create Role Admin if it does not exist
              if (!RoleManager.RoleExists(name))
              {
                  var roleresult = RoleManager.Create(new IdentityRole(role));
             }
   
              //Create User=Admin with password=123456
              var user = new ApplicationUser();
              user.UserName = name;
              user.DisplayName = "Blog Editor";
              var adminresult = UserManager.Create(user, password);
  
              //Add User Admin to Role Admin
              if (adminresult.Succeeded)
              {
                  var result = UserManager.AddToRole(user.Id, role);
             }

            base.Seed(context);
        }
    }
}
