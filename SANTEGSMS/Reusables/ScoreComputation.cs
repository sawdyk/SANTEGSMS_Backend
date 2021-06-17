using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Reusables
{
    public static class ScoreComputation
    {
        public static decimal computeScore(decimal scoreObtained, decimal scoreObtainable, decimal scoreConfigPercentage)
        {
            decimal totalScore = (scoreObtained / scoreObtainable) * scoreConfigPercentage;

            return totalScore;
        }
    }
}
