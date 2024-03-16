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

            InitializePhotos(context);
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
            var admin = userManager.FindByEmailAsync("Admin@task.com").Result;

            if (adminUser == null || admin==null)
            {
                // Створення адміністратора, якщо його не існує
                adminUser = new ApplicationUser
                {
                    UserName = "Admin_1",
                    Email = "Admin@task2.com"
                };
               
                admin = new ApplicationUser
                {
                    UserName = "Admin_2",
                    Email = "Admin@task.com"
                };
                var result = userManager.CreateAsync(adminUser, "Admin1!").Result;
                var result2 = userManager.CreateAsync(admin, "Admin2!").Result;

                if (result.Succeeded&&result2.Succeeded)
                {
                    // Додавання адміністратору ролі Admin
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                    userManager.AddToRoleAsync(admin, "Admin").Wait();

                }
            }
        }


        private static void InitializePhotos(ApplicationDbContext context)
        {
            // Перевірка, чи в базі даних є фотографії
            if (!context.evaluationUsers.Any())
            {
                // Додавання фотографій за дефолтом
                var photo1 = new EvaluationUser
                {
                    Name = "Спокійна людина",
                    Description = "Ви розмірено і своїми силами достинаєте успіху",
                    PhotoData = GetPhotoData("E:\\C#LAB\\aspnet\\TaskNexus – копія\\фото\\rELAX.jpg")
                };
                var photo2 = new EvaluationUser
                {
                    Name = "Трудолюбива людина",
                    Description = "Ви та людина яка поставила ціль і йде до неї як би тяжко не було",
                    PhotoData = GetPhotoData("E:\\C#LAB\\aspnet\\TaskNexus – копія\\фото\\Good.jpg")
                };
                var photo3 = new EvaluationUser
                {
                    Name = "Забудькувата людина",
                    Description = "Ви тут?",
                    PhotoData = GetPhotoData("E:\\C#LAB\\aspnet\\TaskNexus – копія\\фото\\Forful.jpg")
                };

                // Додавання фотографій до контексту бази даних
                context.evaluationUsers.AddRange(photo1, photo2, photo3);
                context.SaveChanges();
            }
        }
        private static byte[] GetPhotoData(string path)
        {
            return File.ReadAllBytes(path);
        }



    }
}
