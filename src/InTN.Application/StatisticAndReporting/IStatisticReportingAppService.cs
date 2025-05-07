using InTN.StatisticAndReporting.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTN.StatisticAndReporting
{
    public interface IStatisticReportingAppService
    {
        /// <summary>
        /// Get the total number of orders, customers, and debt
        /// </summary>
        /// <returns></returns>
        Task<StatisticSummaryDto> GetTotalOrdersCustomersDebtAsync();
        /// <summary>
        /// Get the total number of orders, customers, and debt by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<List<StatisticSummaryByDateDto>> GetTotalOrdersCustomersDebtByDateAsync();
    }

}
