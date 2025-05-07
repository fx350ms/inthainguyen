using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTN.StatisticAndReporting.Dto
{
    public class StatisticSummaryDto
    {
        /// <summary>
        /// Tổng đơn hàng
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// Tổng khách hàng
        /// </summary>
        public int TotalCustomers { get; set; }

        /// <summary>
        /// Tổng công nợ
        /// </summary>
        public decimal TotalDebt { get; set; }

        /// <summary>
        /// Tổng tiền hàng
        /// </summary>
        public decimal TotalOrderAmount { get; set; }

        /// <summary>
        /// Tổng giao dịch
        /// </summary>
        public int TotalTransaction { get; set; }
    }

    public class StatisticSummaryByDateDto
    {
        public DateTime Date { get; set; }
        public int TotalOrders { get; set; }
       // public int TotalCustomers { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public int TotalTransaction { get; set; }
    }
}
