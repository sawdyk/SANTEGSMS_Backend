using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DataSeed
{
    public static class ReportCardCommentConfigData
    {
        public static void SeedReportCardConfig(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportCardCommentConfig>().HasData(
                new ReportCardCommentConfig
                {
                    Id = 1,
                    CommentBy = "Examiner"
                },
                new ReportCardCommentConfig
                {
                    Id = 2,
                    CommentBy = "Class Teacher"
                },
                new ReportCardCommentConfig
                {
                    Id = 3,
                    CommentBy = "Head Teacher"
                },
                 new ReportCardCommentConfig
                 {
                     Id = 4,
                     CommentBy = "Principal"
                 }
            );
        }
    }
}