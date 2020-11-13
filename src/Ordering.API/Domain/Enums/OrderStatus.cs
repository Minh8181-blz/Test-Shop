using Domain.Base.SeedWork;

namespace Ordering.API.Domain.Enums
{
    public class OrderStatus : Enumeration
    {
        //ref: https://support.bigcommerce.com/s/article/Order-Statuses
        public static OrderStatus Pending = new OrderStatus(1, "pending");
        public static OrderStatus AwaitingPayment = new OrderStatus(2, "awaiting-payment");
        public static OrderStatus AwaitingShipment = new OrderStatus(3, "awaiting-shipment");
        public static OrderStatus Shipped = new OrderStatus(4, "shipped");
        public static OrderStatus Completed = new OrderStatus(5, "completed");
        public static OrderStatus Cancelled = new OrderStatus(6, "cancelled");
        public static OrderStatus Declined = new OrderStatus(7, "declined");
        public static OrderStatus Refunded = new OrderStatus(8, "refunded");

        public OrderStatus(int id, string name) : base(id, name)
        {
        }
    }
}
