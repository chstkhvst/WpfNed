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
            // Извлекаем все контракты для указанного года
            var contracts = db.Contract
                .Where(c => c.SignDate.Year == year)
                .Select(c => new
                {
                    c.SignDate,
                    c.Total,
                    c.ReservationId
                })
                .ToList();  // Загружаем данные в память

            // Получаем список всех резерваций, которые входят в резервации контрактов
            var reservationIds = contracts.Select(c => c.ReservationId).ToList();  // Загружаем все ReservationId в память

            var reservations = db.Reservation
                .Where(r => reservationIds.Contains(r.Id))  // Теперь мы используем список идентификаторов, загруженных в память
                .Select(r => new { r.Id, r.ObjectId })
                .ToList();  // Загружаем данные в память

            // Получаем типы сделок для объектов
            var objectIds = reservations.Select(r => r.ObjectId).ToList();  // Загружаем все ObjectId в память

            var dealTypes = db.Object
                .Where(o => objectIds.Contains(o.Id))  // Проверяем объекты по загруженным идентификаторам
                .Select(o => new { o.Id, o.DealTypeId })
                .ToList();  // Загружаем данные в память

            // Инициализируем итоговый словарь
            var monthlyProfit = new Dictionary<int, decimal>();
            // Обрабатываем контракты
            foreach (var contract in contracts)
            {
                var reservation = reservations.FirstOrDefault(r => r.Id == contract.ReservationId);
                if (reservation != null)
                {
                    var dealType = dealTypes.FirstOrDefault(d => d.Id == reservation.ObjectId);

                    // Если DealTypeId == 2, добавляем сумму к текущему месяцу и всем последующим
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
                        // Если DealTypeId != 2, добавляем сумму только к текущему месяцу
                        if (!monthlyProfit.ContainsKey(contract.SignDate.Month))
                        {
                            monthlyProfit[contract.SignDate.Month] = 0;
                        }
                        monthlyProfit[contract.SignDate.Month] += contract.Total;
                    }
                }
            }

            // Убедимся, что для всех месяцев (1-12) есть данные (даже если прибыль = 0)
            var result = Enumerable.Range(1, 12).ToDictionary(m => m, m => monthlyProfit.ContainsKey(m) ? monthlyProfit[m] : 0m);

            return result;
        }

    }
}
