using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class GenderData
    {
        public static void SeedGenderTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>().HasData(
                new Gender
                {
                    Id = 1,
                    GenderName = "Male"
                },
                new Gender
                {
                    Id = 2,
                    GenderName = "Female"
                }
            );
        }
    }
}
