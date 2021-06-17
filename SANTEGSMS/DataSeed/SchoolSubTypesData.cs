using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class SchoolSubTypesData
    {
        public static void SeedSchoolSubTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolSubTypes>().HasData(
                new SchoolSubTypes
                {
                    Id = 1,
                    SchoolTypeId = 2,
                    SubTypeName = "Junior"
                },
                new SchoolSubTypes
                {
                    Id = 2,
                    SchoolTypeId = 2,
                    SubTypeName = "Senior"
                },
                new SchoolSubTypes
                {
                    Id = 3,
                    SchoolTypeId = 1,
                    SubTypeName = "Primary"
                },
                new SchoolSubTypes
                {
                    Id = 4,
                    SchoolTypeId = 1,
                    SubTypeName = "Nursery"
                }

            );
        }
    }
}