using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using InTN.Entities;
using InTN.StatisticAndReporting.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace InTN.StatisticAndReporting
{

    public class StatisticReportingAppService : IStatisticReportingAppService
    {

        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Customer> _customerRepository;
        public StatisticReportingAppService(
            IRepository<Order> orderRepository,
            IRepository<Customer> customerRepository
           )
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public async Task<StatisticSummaryDto> GetTotalOrdersCustomersDebtAsync()
        {
            int TotalOrders = await _orderRepository.CountAsync();
            int TotalCustomers = await _customerRepository.CountAsync();
            decimal TotalDebt = await _customerRepository.GetAll()
                .SumAsync(c => c.TotalDebt);
            decimal TotalOrderAmount = await _orderRepository.GetAll()
                .SumAsync(o => o.TotalAmount ?? 0);
            int TotalTransaction = await _orderRepository.GetAll()
                .CountAsync(o => o.PaymentStatus == 1); // Assuming 1 means paid

            return new StatisticSummaryDto
            {
                TotalOrders = TotalOrders,
                TotalCustomers = TotalCustomers,
                TotalDebt = TotalDebt,
                TotalOrderAmount = TotalOrderAmount,
                TotalTransaction = TotalTransaction
            };
        }
        public async Task<List<StatisticSummaryByDateDto>> GetTotalOrdersCustomersDebtByDateAsync()
        {
            // var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-6);
            // var today = DateTime.UtcNow.Date;

            // var statistics = await _orderRepository.GetAll()
            //     .Where(o => o.OrderDate.Date >= sevenDaysAgo && o.OrderDate.Date <= today)
            //     .GroupBy(o => o.OrderDate.Date)
            //     .Select(g => new StatisticSummaryByDateDto
            //     {
            //         Date = g.Key,
            //         TotalOrders = g.Count(),
            //         TotalOrderAmount = g.Sum(o => o.TotalAmount ?? 0),
            //         TotalDebt = g.Sum(o => o.TotalAmount ?? 0) - g.Sum(o => o.TotalDeposit ?? 0),
            //         TotalTransaction = g.Count(o => o.PaymentStatus == 1) // Assuming 1 means paid
            //     })
            //     .OrderBy(s => s.Date)
            //     .ToListAsync();

            // return statistics;
            var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-6);
            var today = DateTime.UtcNow.Date;

            // Tạo danh sách tất cả các ngày trong khoảng thời gian
            var allDates = Enumerable.Range(0, (today - sevenDaysAgo).Days + 1)
                                      .Select(offset => sevenDaysAgo.AddDays(offset))
                                      .ToList();

            // Lấy dữ liệu từ cơ sở dữ liệu
            var statistics = await _orderRepository.GetAll()
                .Where(o => o.OrderDate.Date >= sevenDaysAgo && o.OrderDate.Date <= today)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalOrders = g.Count(),
                    TotalOrderAmount = g.Sum(o => o.TotalAmount ?? 0),
                    TotalDebt = g.Sum(o => o.TotalAmount ?? 0) - g.Sum(o => o.TotalDeposit ?? 0),
                    TotalTransaction = g.Count(o => o.PaymentStatus == 1) // Assuming 1 means paid
                })
                .ToListAsync();

            // Kết hợp dữ liệu từ cơ sở dữ liệu với danh sách tất cả các ngày
            var result = allDates.Select(date => new StatisticSummaryByDateDto
            {
                Date = date,
                TotalOrders = statistics.FirstOrDefault(s => s.Date == date)?.TotalOrders ?? 0,
                TotalOrderAmount = statistics.FirstOrDefault(s => s.Date == date)?.TotalOrderAmount ?? 0,
                TotalDebt = statistics.FirstOrDefault(s => s.Date == date)?.TotalDebt ?? 0,
                TotalTransaction = statistics.FirstOrDefault(s => s.Date == date)?.TotalTransaction ?? 0
            }).OrderBy(s => s.Date).ToList();

            return result;
        }
    }

}
