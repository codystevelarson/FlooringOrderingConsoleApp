using System;
using System.Collections.Generic;

namespace FlooringMastery.Models.Interfaces
{
    public interface IOrderRepository
    {
        Order LoadOrder(int orderNumber, DateTime orderDate);
        List<Order> List(DateTime orderDate);
        void SaveOrder(Order order);
        void AddOrder(Order order);
        void DeleteOrder(Order order, List<Order> orders);
    }
}
