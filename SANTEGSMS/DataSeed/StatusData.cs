using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class StatusData
    {
        public static void SeedStatus(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().HasData(
                new Status
                {
                    Id = 1,
                    StatusName = "Approved"
                },
                new Status
                {
                    Id = 2,
                    StatusName = "Pending"
                },
                new Status
                {
                    Id = 3,
                    StatusName = "Declined"
                }
            );
        }
    }
}
