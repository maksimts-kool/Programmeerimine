using Microsoft.AspNetCore.Identity;
using Tund12.Models;

namespace Tund12.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            // Loo rollid
            string[] roles = { "Admin", "Opetaja", "Opilane" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Loo admin kasutaja
            var adminEmail = "maksimtsitkool@gmail.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "Maksim Tsikvasvili"
                };
                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Loo test-õpetaja
            var teacherEmail = "mari@keelekool.ee";
            if (await userManager.FindByEmailAsync(teacherEmail) == null)
            {
                var teacher = new ApplicationUser
                {
                    UserName = teacherEmail,
                    Email = teacherEmail,
                    EmailConfirmed = true,
                    FullName = "Mari Maasikas"
                };
                var result = await userManager.CreateAsync(teacher, "Opetaja123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(teacher, "Opetaja");

                    // Lisa õpetaja profiil
                    var teacherProfile = new Teacher
                    {
                        Nimi = "Mari Maasikas",
                        Kvalifikatsioon = "Magistrikraad germaani keeltes",
                        FotoPath = "mari.jpg",
                        ApplicationUserId = teacher.Id
                    };
                    context.Teachers.Add(teacherProfile);
                    await context.SaveChangesAsync();
                }
            }

            // Lisa DbSeeder.cs faili lõppu
            var studentEmail = "maksimutuberzapasnojkanal@gmail.com";
            if (await userManager.FindByEmailAsync(studentEmail) == null)
            {
                var student = new ApplicationUser
                {
                    UserName = studentEmail,
                    Email = studentEmail,
                    EmailConfirmed = true,
                    FullName = "Juhan Juurikas"
                };
                var result = await userManager.CreateAsync(student, "Opilane123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(student, "Opilane");
                }
            }

            // Lisa näidiskursused
            if (!context.Courses.Any())
            {
                var courses = new List<Course>
                {
                    new Course {
                        Nimetus = "Saksa keel algajatele",
                        Keel = "Saksa",
                        Tase = "A1",
                        Kirjeldus = "Põhikursus saksa keele õppimiseks"
                    },
                    new Course {
                        Nimetus = "Inglise keel edasijõudnutele",
                        Keel = "Inglise",
                        Tase = "B2",
                        Kirjeldus = "Edasijõudnute kursus"
                    }
                };
                context.Courses.AddRange(courses);
                await context.SaveChangesAsync();
            }
        }
    }
}