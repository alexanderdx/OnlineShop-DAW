using OnlineShopDAW.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineShopDAW.Startup))]
namespace OnlineShopDAW
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            CreateApplicationRoles();
        }

        private void CreateApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("administrator"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "administrator";
                roleManager.Create(role);

                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "admin@gmail.com";
                user.Email = "admin@gmail.com";
                user.FirstName = "admin";
                user.LastName = "admin";
                var adminCreated = UserManager.Create(user, "admin123");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "administrator");
                }
            }

            if (!roleManager.RoleExists("collaborator"))
            {
                var role = new IdentityRole();
                role.Name = "collaborator";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("registered"))
            {
                var role = new IdentityRole();
                role.Name = "registered";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("unregistered"))
            {
                var role = new IdentityRole();
                role.Name = "unregistered";
                roleManager.Create(role);
            }
        }
    }
}
