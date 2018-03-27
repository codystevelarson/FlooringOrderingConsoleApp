using FlooringMastery.DATA;
using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FlooringMastery.Tests
{
    [TestFixture]
    class TestRepositoryTests
    {
        IOrderRepository repo = new OrderTestRepository();

        [Test]
        public void CanListOrders()
        {
            List<Order> orders = repo.List(DateTime.Parse("08/07/2020"));

            Assert.IsNotNull(orders);
        }

        [TestCase(99,"08/07/2020", false)]
        [TestCase(500, "08/07/2020", true)]
        [TestCase(99, "08/07/2022", true)]
        public void CanLoadOrder(int orderNumber, string orderDate, bool expected)
        {
            DateTime date = DateTime.Parse(orderDate);
            Order order = repo.LoadOrder(orderNumber, date);
            bool nullOrder = false;
            if(order == null)
            {
                nullOrder = true;
            }
            Assert.AreEqual(expected, nullOrder);
        }

        [Test]
        public void CanAddOrderToRepo()
        {
            Order order = new Order
            {
                OrderDate = DateTime.Parse("08/07/2020"),
                OrderNumber = 400,
                CustomerName = "Tom Testa",
                State = "OH",
                TaxRate = 6.25m,
                ProductType = "Carpet",
                Area = 100.5m,
                CostPerSquareFoot = 2.25m,
                LaborCostPerSquareFoot = 2.25m,
            };
            repo.AddOrder(order);
            Order checkOrder = repo.LoadOrder(order.OrderNumber, order.OrderDate);
            Assert.IsNotNull(checkOrder);
        }

        [Test]
        public void CanSaveOrderToRepo()
        {
            Order order = new Order
            {
                OrderDate = DateTime.Parse("08/07/2020"),
                OrderNumber = 666,
                CustomerName = "Sam Saver",
                State = "OH",
                TaxRate = 6.25m,
                ProductType = "Carpet",
                Area = 100.5m,
                CostPerSquareFoot = 2.25m,
                LaborCostPerSquareFoot = 2.25m,
            };
            repo.AddOrder(order);

            Order editOrder = new Order
            {
                OrderDate = DateTime.Parse("08/07/2020"),
                OrderNumber = 666,
                CustomerName = "Oren Overwrite",
                State = "OH",
                TaxRate = 6.25m,
                ProductType = "Carpet",
                Area = 100.5m,
                CostPerSquareFoot = 2.25m,
                LaborCostPerSquareFoot = 2.25m,
            };

            repo.SaveOrder(editOrder);
            Order checkOrder = repo.LoadOrder(editOrder.OrderNumber, editOrder.OrderDate);
            Assert.AreEqual(editOrder.CustomerName, checkOrder.CustomerName);
        }

        [Test]
        public void CanDeleteOrderFromRepo()
        {
            Order order = new Order
            {
                OrderDate = DateTime.Parse("08/07/2020"),
                OrderNumber = 777,
                CustomerName = "Derek Delete",
                State = "OH",
                TaxRate = 6.25m,
                ProductType = "Carpet",
                Area = 100.5m,
                CostPerSquareFoot = 2.25m,
                LaborCostPerSquareFoot = 2.25m,
            };
            repo.AddOrder(order);

            List<Order> orders = repo.List(order.OrderDate);

            repo.DeleteOrder(order, orders);

            Order checkDelete = repo.LoadOrder(order.OrderNumber, order.OrderDate);
            Assert.IsNull(checkDelete);
        }
    }
}
