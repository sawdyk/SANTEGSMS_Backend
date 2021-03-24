using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class AttendancePeriodData
    {
        public static void SeedAttendancePeriod(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendancePeriod>().HasData(
                new AttendancePeriod
                {
                    Id = 1,
                    AttendancePeriodName = "Morning"
                },
                new AttendancePeriod
                {
                    Id = 2,
                    AttendancePeriodName = "Afternoon"
                },
                new AttendancePeriod
                {
                    Id = 3,
                    AttendancePeriodName = "Both"
                }
            );
        }
    }
}
