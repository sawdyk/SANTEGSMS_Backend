using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class SchoolTypeData
    {
        public static void SeedSchoolTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolType>().HasData(
                new SchoolType
                {
                    Id = 1,
                    SchoolTypeName = "Nursery and Primary"
                },
                new SchoolType
                {
                    Id = 2,
                    SchoolTypeName = "Secondary"
                }
            );
        }
    }
}
