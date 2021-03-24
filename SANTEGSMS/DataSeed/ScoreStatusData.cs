using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class ScoreStatusData
    {
        public static void SeedScoreStatus(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScoreStatus>().HasData(
                new ScoreStatus
                {
                    Id = 1,
                    ScoreStatusName = "Passed"
                },
                new ScoreStatus
                {
                    Id = 2,
                    ScoreStatusName = "Failed"
                },
                new ScoreStatus
                {
                    Id = 3,
                    ScoreStatusName = "Pending"
                }
            );
        }
    }
}
