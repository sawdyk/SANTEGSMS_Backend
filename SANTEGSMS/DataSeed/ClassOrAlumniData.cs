using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class ClassOrAlumniData
    {
        public static void SeedClassOrAlumni(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassAlumni>().HasData(
                new ClassAlumni
                {
                    Id = 1,
                    Category = "Alumni"
                },
                new ClassAlumni
                {
                    Id = 2,
                    Category = "Class"
                }
            );
        }
    }
}
