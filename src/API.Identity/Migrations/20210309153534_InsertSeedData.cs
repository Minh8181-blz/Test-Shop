using API.Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace API.Identity.Migrations
{
    public partial class InsertSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var customerRole = new AppRole
            {
                Name = "Customer",
                NormalizedName = "CUSTOMER",
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };

            var adminRole = new AppRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };

            //migrationBuilder.InsertData(
            //    table: "AppRole",
            //    schema: "ms_identity",
            //    columns: new[] { "Name", "NormalizedName", "ConcurrencyStamp" },
            //    values: new object[] {
            //        new { customerRole.Name, customerRole.NormalizedName, customerRole.ConcurrencyStamp },
            //        new { adminRole.Name, adminRole.NormalizedName, adminRole.ConcurrencyStamp }
            //    });

            migrationBuilder.InsertData(
                table: "AppRole",
                schema: "ms_identity",
                columns: new[] { "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { customerRole.Name, customerRole.NormalizedName, customerRole.ConcurrencyStamp });

            migrationBuilder.InsertData(
                table: "AppRole",
                schema: "ms_identity",
                columns: new[] { "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { adminRole.Name, adminRole.NormalizedName, adminRole.ConcurrencyStamp });

            var admin = new AppUser
            {
                UserName = "masteradmin",
                NormalizedUserName = "MASTERADMIN",
                Email = "Admin@Admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                PhoneNumber = "0392126698",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
            };

            var passHash = new PasswordHasher<AppUser>();
            admin.PasswordHash = passHash.HashPassword(admin, "password");

            migrationBuilder.InsertData(
                table: "AppUser",
                schema: "ms_identity",
                columns: new[] { "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash",
                "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount" },
                values: new object[] {
                    admin.UserName, admin.NormalizedUserName, admin.Email, admin.NormalizedEmail, admin.EmailConfirmed, admin.PasswordHash,
                    admin.SecurityStamp, admin.ConcurrencyStamp, admin.PhoneNumber, admin.PhoneNumberConfirmed, admin.TwoFactorEnabled, admin.LockoutEnabled, admin.AccessFailedCount
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRole",
                schema: "ms_identity",
                keyColumn: "Name",
                keyValues: new object[] { "Customer", "Administrator" });

            migrationBuilder.DeleteData(
                table: "AppUser",
                 schema: "ms_identity",
                 keyColumn: "Email",
                 keyValue: "Admin@Admin.com");
        }
    }
}
