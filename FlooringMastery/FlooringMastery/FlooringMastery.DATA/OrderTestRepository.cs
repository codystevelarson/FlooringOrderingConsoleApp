using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace FlooringMastery.DATA
{
    public class OrderTestRepository : IOrderRepository
    {

        private static Order _order = new Order
        {
            OrderDate = DateTime.Parse("08/07/2020"),
            OrderNumber = 99,
            CustomerName = "Chad Testa",
            State = "OH",
            TaxRate = 6.25m,
            ProductType = "Carpet",
            Area = 200.5m,
            CostPerSquareFoot = 2.25m,
            LaborCostPerSquareFoot = 2.25m,
        };

        private static List<Order> _orders = new List<Order>
        {
            _order
        };

        public void AddOrder(Order order)
        {
            _orders.Add(order);
        }

        public void DeleteOrder(Order order, List<Order> orders)
        {
            foreach(Order o in orders)
            {
                if(o.OrderDate == order.OrderDate && o.OrderNumber == order.OrderNumber)
                {
                    orders.Remove(o);
                    break;
                }
            }
        }

        public List<Order> List(DateTime orderDate)
        { 
            return _orders;
        }

        public Order LoadOrder(int orderNumber, DateTime orderDate)
        {
            foreach(Order o in _orders)
            {
                if(o.OrderNumber == orderNumber && o.OrderDate == orderDate)
                {
                    return o;
                }
            }
            return null;
        }

        public void SaveOrder(Order order)
        {
            foreach(Order o in _orders)
            {
                if (o.OrderNumber == order.OrderNumber && o.OrderDate == order.OrderDate)
                {
                    _orders.Remove(o);
                    _orders.Add(order);
                    break;
                }
            }
        }
    }
}
