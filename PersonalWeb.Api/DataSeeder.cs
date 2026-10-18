using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalWeb.Api.EntityFramework;
using PersonalWeb.Api.Models;

namespace PersonalWeb.Api
{
    public static class DataSeeder
    {
        public static void SeedAdminUser(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PersonalWebApiContext>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            context.Database.Migrate();

            var adminEmail = config["Seed:AdminEmail"] ?? "admin@personalweb.local";
            var adminPassword = config["Seed:AdminPassword"] ?? "Change_Me_123!";

            if (!context.Users.Any(u => u.Email == adminEmail))
            {
                context.Users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Email = adminEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                    Role = "admin",
                    CreatedAt = DateTimeOffset.UtcNow
                });
                context.SaveChanges();
            }
        }
    }
}
