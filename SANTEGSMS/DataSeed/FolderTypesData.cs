using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class FolderTypesData
    {
        public static void SeedFolderTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FolderTypes>().HasData(
          
                new FolderTypes { Id = 1, AppId = 1, FolderName = "Assignments" },
                new FolderTypes { Id = 2, AppId = 1, FolderName = "LessonNotes" },
                new FolderTypes { Id = 3, AppId = 1, FolderName = "SchoolLogos" },
                new FolderTypes { Id = 4, AppId = 1, FolderName = "Signatures" },
                new FolderTypes { Id = 5, AppId = 1, FolderName = "StudentPassports" },
                new FolderTypes { Id = 6, AppId = 1, FolderName = "SubjectNotes" }

            );
        }
    }
}
