using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class AppTypesData
    {
        public static void SeedAppTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppTypes>().HasData(
                new AppTypes
                {
                    Id = 1,
                    AppName = "SchoolApp"
                }
            );
        }
    }
}
