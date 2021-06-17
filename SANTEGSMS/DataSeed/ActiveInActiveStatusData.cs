using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class ActiveInActiveStatusData
    {
        public static void SeedActiveInActiveStatus(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActiveInActiveStatus>().HasData(
                new ActiveInActiveStatus
                {
                    Id = 1,
                    StatusName = "Active"
                },
                new ActiveInActiveStatus
                {
                    Id = 2,
                    StatusName = "InActive"
                }
            );
        }
    }
}
