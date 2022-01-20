using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class SchoolResourcesData
    {
        public static void SeedSchoolResources(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolResources>().HasData(
                new SchoolResources
                {
                    Id = 1,
                    ResourceName = "Subject Bulk Upload Template",
                    ResourceLink = "http://161.97.77.250:8080/santegfilesrepository/schooldocuments/others/SubjectBulkUpload.xlsx"
                },
                new SchoolResources
                {
                    Id = 2,
                    ResourceName = "Student Bulk Upload Template",
                    ResourceLink = "http://161.97.77.250/santegfilesrepository/schooldocuments/others/StudentBulkUpload.xlsx"
                }
            );
        }
    }
}
