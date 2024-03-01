using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskNexus.Models.ApplicationUser;

namespace TaskNexus.DAL.DB
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Виконати міграцію бази даних
            context.Database.Migrate();

            // Ініціалізувати ролі
            InitializeRoles(roleManager);

            // Ініціалізувати користувачів
            InitializeUsers(userManager);
        }

        private static void InitializeRoles(RoleManager<IdentityRole> roleManager)
        {
            // Список ролей, які потрібно створити
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                // Перевірка, чи існує роль
                var roleExists = roleManager.RoleExistsAsync(roleName).Result;

                if (!roleExists)
                {
                    // Створення ролі, якщо вона не існує
                    var role = new IdentityRole
                    {
                        Name = roleName
                    };
                    var result = roleManager.CreateAsync(role).Result;
                }
            }
        }

        private static void InitializeUsers(UserManager<ApplicationUser> userManager)
        {
            // Перевірка, чи існує адміністратор
            var adminUser = userManager.FindByEmailAsync("admin@example.com").Result;

            if (adminUser == null)
            {
                // Створення адміністратора, якщо його не існує
                adminUser = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com"
                };
                var result = userManager.CreateAsync(adminUser, "AdminPassword123!").Result;
                if (result.Succeeded)
                {
                    // Додавання адміністратору ролі Admin
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
            }
        }
    }
}
