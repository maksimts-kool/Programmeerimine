using Microsoft.AspNetCore.Identity;
using Tund12.Data;
using Tund12.Models;

namespace Tund12.Data;

public static class SeedData
{
    public static async Task InitialiseAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // ── 1. Rollid ──────────────────────────────────────────────────
        string[] roles = ["Admin", "Opetaja", "Opilane"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // ── 2. Administraator ──────────────────────────────────────────
        await EnsureUserAsync(
            userManager,
            email: "admin@kool.ee",
            password: "Admin123!",
            role: "Admin"
        );

        // ── 3. Test-õpetaja: konto + profiil ──────────────────────────
        var opetajaUser = await EnsureUserAsync(
            userManager,
            email: "mari@kool.ee",
            password: "Opetaja123!",
            role: "Opetaja"
        );

        if (opetajaUser != null && !db.Opetajad.Any(o => o.ApplicationUserId == opetajaUser.Id))
        {
            db.Opetajad.Add(new Opetaja
            {
                Nimi = "Mari Speek",
                Kvalifikatsioon = "Magistrikraad inglise filoloogias, CELTA sertifikaat",
                FotoPath = null,
                ApplicationUserId = opetajaUser.Id
            });
        }

        // ── 4. Test-õpilane ───────────────────────────────────────────
        await EnsureUserAsync(
            userManager,
            email: "opilane@kool.ee",
            password: "Opilane123!",
            role: "Opilane"
        );

        // ── 5. Keelekursused ──────────────────────────────────────────
        if (!db.Keelekursused.Any())
        {
            db.Keelekursused.AddRange(
                new Keelekursus
                {
                    Nimetus = "Inglise keel algajatele",
                    Keel = "Inglise",
                    Tase = KursuseTase.A1,
                    Kirjeldus = "Põhisõnavara ja elementaarne suhtlusoskus."
                },
                new Keelekursus
                {
                    Nimetus = "Saksa keel kesktasemele",
                    Keel = "Saksa",
                    Tase = KursuseTase.B1,
                    Kirjeldus = "Grammatika süvendamine, ilukirjanduse lugemine."
                },
                new Keelekursus
                {
                    Nimetus = "Prantsuse keel edasijõudnutele",
                    Keel = "Prantsuse",
                    Tase = KursuseTase.C1,
                    Kirjeldus = "Akadeemiline ja ärisuhtlus prantsuse keeles."
                },
                new Keelekursus
                {
                    Nimetus = "Soome keel algajatele",
                    Keel = "Soome",
                    Tase = KursuseTase.A2,
                    Kirjeldus = "Igapäevane suhtlus ja põhifraasid."
                }
            );
        }

        await db.SaveChangesAsync();

        // ── 6. Koolitused (vajavad opetaja ID) ────────────────────────
        var opetajaUserId = opetajaUser?.Id;
        var opetaja = opetajaUserId != null
            ? db.Opetajad.FirstOrDefault(o => o.ApplicationUserId == opetajaUserId)
            : null;
        if (opetaja != null && !db.Koolitused.Any())
        {
            var kursus1 = db.Keelekursused.First(k => k.Keel == "Inglise");
            var kursus2 = db.Keelekursused.First(k => k.Keel == "Saksa");

            db.Koolitused.AddRange(
                new Koolitus
                {
                    KeelekursusId = kursus1.Id,
                    OpetajaId = opetaja.Id,
                    AlgusKuupaev = DateTime.Today.AddDays(7),
                    LoppKuupaev = DateTime.Today.AddDays(7 + 60),
                    Hind = 350.00m,
                    MaxOsalejaid = 10
                },
                new Koolitus
                {
                    KeelekursusId = kursus2.Id,
                    OpetajaId = opetaja.Id,
                    AlgusKuupaev = DateTime.Today.AddDays(14),
                    LoppKuupaev = DateTime.Today.AddDays(14 + 90),
                    Hind = 420.00m,
                    MaxOsalejaid = 8
                }
            );

            await db.SaveChangesAsync();
        }
    }

    // Abi-meetod: loo kasutaja kui ta puudub ja määra roll
    private static async Task<ApplicationUser?> EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string email, string password, string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded) return null;
        }

        if (!await userManager.IsInRoleAsync(user, role))
            await userManager.AddToRoleAsync(user, role);

        return user;
    }
}
