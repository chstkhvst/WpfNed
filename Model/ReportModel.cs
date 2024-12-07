using System;
using System.Collections.Generic;
using System.Linq;
using WpfNed.EF;

namespace WpfNed.Model
{
    public class ReportModel
    {
        private Model1 db = new Model1();

        public Dictionary<int, decimal> GetMonthlyProfit(int year)
        {
            var contracts = db.Contract
                .Where(c => c.SignDate.Year == year)
                .Select(c => new
                {
                    c.SignDate,
                    c.Total,
                    c.ReservationId
                })
                .ToList(); 
            var reservationIds = contracts.Select(c => c.ReservationId).ToList(); 

            var reservations = db.Reservation
                .Where(r => reservationIds.Contains(r.Id)) 
                .Select(r => new { r.Id, r.ObjectId })
                .ToList(); 

            var objectIds = reservations.Select(r => r.ObjectId).ToList();  

            var dealTypes = db.Object
                .Where(o => objectIds.Contains(o.Id))
                .Select(o => new { o.Id, o.DealTypeId })
                .ToList();  

            var monthlyProfit = new Dictionary<int, decimal>();
            foreach (var contract in contracts)
            {
                var reservation = reservations.FirstOrDefault(r => r.Id == contract.ReservationId);
                if (reservation != null)
                {
                    var dealType = dealTypes.FirstOrDefault(d => d.Id == reservation.ObjectId);
                    if (dealType != null && dealType.DealTypeId == 1)
                    {
                        for (int month = contract.SignDate.Month; month <= 12; month++)
                        {
                            if (!monthlyProfit.ContainsKey(month))
                            {
                                monthlyProfit[month] = 0;
                            }
                            monthlyProfit[month] += contract.Total;
                        }
                    }
                    else
                    {
                        if (!monthlyProfit.ContainsKey(contract.SignDate.Month))
                        {
                            monthlyProfit[contract.SignDate.Month] = 0;
                        }
                        monthlyProfit[contract.SignDate.Month] += contract.Total;
                    }
                }
            }
            var result = Enumerable.Range(1, 12).ToDictionary(m => m, m => monthlyProfit.ContainsKey(m) ? monthlyProfit[m] : 0m);

            return result;
        }
        public Dictionary<string, int> GetProfitByType(int year)
        {
            var contracts = db.Contract
                .Where(c => c.SignDate.Year == year)
                .Select(c => new { c.Total, c.Reservation.Object.TypeId })
                .ToList();

            var profitByType = contracts
                .GroupBy(c => c.TypeId == 1 ? "Квартира" : "Дом")
                .ToDictionary(g => g.Key, g => g.Sum(c => c.Total));

            return profitByType;
        }


    }
}
