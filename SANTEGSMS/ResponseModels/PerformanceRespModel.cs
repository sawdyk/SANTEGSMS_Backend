using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    public class PerformanceRespModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public PerformanceAnalysisSchoolnfo PerformanceAnalysisSchoolnfo { get; set; }
        public IList<PerformanceAnalysisInfo> PerformanceAnalysisInfo { get; set; }
    }

    public class PerformanceAnalysisSchoolnfo
    {
        public string SchoolName { get; set; }
        public string CampusName { get; set; }
        public string CampusAddress { get; set; }
        public string Session { get; set; }
        public string Term { get; set; }
        public string DateGenerated { get; set; }
    }

    public class PerformanceSummaryInfo
    {
        public long Male { get; set; }
        public long Female { get; set; }
        public long Total { get; set; }
    }

    public class PerformanceAnalysisInfo
    {
        public string Class { get; set; }
        public PerformanceSummaryInfo Registered { get; set; }
        public PerformanceSummaryInfo Present { get; set; }
        public PerformanceSummaryInfo Absent { get; set; }
        public PerformanceSummaryInfo Mandatory { get; set; }
        public PerformanceSummaryInfo Others { get; set; }
        public decimal PercentagePass { get; set; }
    }
}
