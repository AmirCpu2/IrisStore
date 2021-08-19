using Iris.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DataLayer
{
    class ApplicationDbInitialzer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {

            #region Add User
            context.Users.Add(
                new ApplicationUser
                {
                    FirstName = "مدیر سیستم",
                    LastName = "",
                    Email = "Admin@email.com",
                    UserName = "Admin",
                    PasswordHash = "demo@email.com",
                    EmailConfirmed = true,
                });

            context.Roles.Add(
                new CustomRole
                {
                    Name = "Admin"
                });

            context.SaveChanges();

            //Get User
            var user = context.Users.Single(q => q.UserName == "Admin");

            //Get And Set Role
            var role = context.Roles.Single(q => q.Name == "Admin");
            role.Users.Add(new CustomUserRole() { UserId = user.Id, RoleId = role.Id});
            context.Roles.AddOrUpdate(role);
            #endregion

            #region ItemType
            
            #endregion
            base.Seed(context);
        }
    }
}
