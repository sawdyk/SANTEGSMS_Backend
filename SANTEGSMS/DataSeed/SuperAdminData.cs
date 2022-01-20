using Microsoft.EntityFrameworkCore;
using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using SANTEGSMS.Helpers;
using SANTEGSMS.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class SuperAdminData
    {
        public static void SeedSystemSuperAdmin(this ModelBuilder modelBuilder)
        {
            var paswordHasher = new PasswordHasher();
            //the salt
            string salt = paswordHasher.getSalt();
            //get deafault password
            string password = DefaultPasswordReUsable.DefaultPassword();
            //Hash the password and salt
            string passwordHash = paswordHasher.hashedPassword(password, salt);

            modelBuilder.Entity<SuperAdmin>().HasData(
                new SuperAdmin
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Super Admin",
                    LastName = "Super Admin",
                    Email = "Oluagbe1@gmail.com",
                    PhoneNumber = "08024174777",
                    Salt = salt,
                    PasswordHash = passwordHash,
                    DateCreated = DateTime.Now
                }
            );
        }
    }
}
