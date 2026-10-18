using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalWeb.Api.EntityFramework;

namespace PersonalWeb.Api
{
    public static class DataSeeder
    {
        public static void SeedAdminUser(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PersonalWebApiContext>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var adminEmail = config["Seed:AdminEmail"] ?? "admin@personalweb.local";
            var adminPassword = config["Seed:AdminPassword"] ?? "Change_Me_123!";

            var id = Guid.NewGuid();
            var hash = BCrypt.Net.BCrypt.HashPassword(adminPassword);
            var now = DateTimeOffset.UtcNow;

            context.Database.ExecuteSqlRaw(
                @"INSERT INTO ""Users"" (""Id"", ""Email"", ""PasswordHash"", ""Role"", ""CreatedAt"")
                  VALUES ({0}, {1}, {2}, 'admin', {3})
                  ON CONFLICT (""Email"") DO NOTHING",
                id, adminEmail, hash, now);
        }
    }
}
