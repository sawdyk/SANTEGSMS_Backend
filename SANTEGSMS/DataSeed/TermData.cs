using Microsoft.EntityFrameworkCore;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SANTEGSMS.DataSeed
{
    public static class TermData
    {
        public static void SeedTerms(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Terms>().HasData(
                new Terms
                {
                    Id = 1,
                    TermName = "1st Term"
                },
               new Terms
               {
                   Id = 2,
                   TermName = "2nd Term"
               },
               new Terms
               {
                   Id = 3,
                   TermName = "3rd Term"
               }
            );
        }
    }
}
