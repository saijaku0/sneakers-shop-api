using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class SalesSnapshot : Entity
    {
        public DateOnly Date { get; private set; }
        public decimal TotalRevenue { get; private set; }
        public int TotalOrders { get; private set; }
        public decimal TotalPayouts { get; private set; }
        public decimal AverageOrderValue { get; private set; }

        private SalesSnapshot() { }

        private SalesSnapshot(
            DateOnly date,
            decimal totalRevenue,
            int totalOrders,
            decimal totalPayouts,
            decimal averageOrderValue) : base(Guid.NewGuid())
        {
            Date = date;
            TotalRevenue = totalRevenue;
            TotalOrders = totalOrders;
            TotalPayouts = totalPayouts;
            AverageOrderValue = averageOrderValue;
        }

        public static SalesSnapshot Create(
            DateOnly date,
            IEnumerable<Order> orders,
            IEnumerable<DropperPayout> payouts)
        {
            var filteredOrders = orders
                .Where(o => o.Status == OrderStatus.Paid || o.Status == OrderStatus.Delivered)
                .ToList();

            var filteredPayouts = payouts
                .Where(p => p.Status == PayoutStatus.Completed)
                .ToList();

            var totalRevenue = CalculateRevenue(filteredOrders);
            var totalOrders = filteredOrders.Count;
            var totalPayouts = CalculatePayouts(filteredPayouts);
            var avg = CalculateAverageOrderValue(totalRevenue, totalOrders);

            return new SalesSnapshot(
                date,
                totalRevenue,
                totalOrders,
                totalPayouts,
                avg);
        }

        private static decimal CalculateRevenue(IEnumerable<Order> orders)
        {
            return orders.Sum(o => o.TotalOrderPrice);
        }

        private static decimal CalculatePayouts(IEnumerable<DropperPayout> payouts)
        {
            return payouts.Sum(p => p.Amount);
        }

        private static decimal CalculateAverageOrderValue(decimal revenue, int ordersCount)
        {
            if (ordersCount == 0)
                return 0;

            return revenue / ordersCount;
        }

        public decimal CalculateGrowth(SalesSnapshot previous)
        {
            if (previous == null || previous.TotalRevenue == 0)
                return 0;

            return (TotalRevenue - previous.TotalRevenue) / previous.TotalRevenue;
        }
    }
}
