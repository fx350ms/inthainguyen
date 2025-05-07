using InTN.StatisticAndReporting.Dto;
using System.Collections.Generic;

namespace InTN.Web.Models.Home
{
    public class HomePageModel
    {
        public StatisticSummaryDto StatisticSummary { get; set; }
        public List<StatisticSummaryByDateDto> StatisticSummaryByDate { get; set; }
    }
}
