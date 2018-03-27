using FlooringMastery.BLL;
using FlooringMastery.DATA;
using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using FlooringMastery.Models.Responses;
using NUnit.Framework;
using System;
using System.Linq;

namespace FlooringMastery.Tests
{
    [TestFixture]
    class TestManagerTests
    {
        private IOrderRepository orderRepo = new OrderTestRepository();
        private IStateTaxRepository taxRepo = new StateTaxTestRepository();
        public IProductRepository productRepo = new ProductTestRepository();

        //Test repo not seeded so delete order and edit order should be tested seperately 
        [Test]
        public void CanGetOrders()
        {
            DateTime date = DateTime.Parse("08/07/2020");
            OrderManager manager = new OrderManager(orderRepo);
            OrderLookupAllResponse response = manager.LookupAllOrders(date);

            Order actual = new Order();

            if (response.Orders.Count() == 0)
            {
                actual = null;
            }
            else
            {
                foreach (Order o in response.Orders)
                {
                    actual = o;
                }
            }

            Assert.IsNotNull(response.Orders);
            Assert.AreEqual(true, response.Success);
        }

        [TestCase("M", false)]
        [TestCase("MN", true)]
        [TestCase("Minnesota", true)]
        [TestCase("MINNESOTA", false)]
        public void CanGetStateTaxObject(string state, bool expected)
        {
            StateTaxManager manager = new StateTaxManager(taxRepo);
            StateLookupResponse response = manager.GetStateTax(state);

            Assert.AreEqual(expected, response.Success);
        }

        [TestCase("Shag Carpet", false)]
        [TestCase("Carpet", true)]
        [TestCase("CARPET", false)]
        public void CanGetProductObject(string productType, bool expected)
        {
            ProductManager manager = new ProductManager(productRepo);
            ProductLookupResponse response = manager.GetProduct(productType);

            Assert.AreEqual(expected, response.Success);
        }

        [TestCase("08/07/2015", "Chad Testa", "OH", "Carpet", 200, false)]
        [TestCase("08/07/2020","Andrew", "OH", "Carpet", 200, true)]
        [TestCase("08/07/2020", "Chad Testa", "MN", "Carpet", 200, false)]
        [TestCase("08/07/2020", "Chad Testa", "OH", "Marble", 200, false)]
        [TestCase("08/07/2020", "Chad Testa", "OH", "Carpet", 21, false)]
        [TestCase("08/07/2020", "Chad Testa", "OH", "Carpet", 200.5, false)]
        public void CanAddOrder(string orderDate, string name, string state, string productType, decimal area, bool expected)
        {
            OrderManager orderManager = new OrderManager(orderRepo);
            Order newOrder = new Order
            {
                OrderNumber = 99,
                OrderDate = DateTime.Parse(orderDate),
                CustomerName = name,
                State = state,
                TaxRate = 6.25m,
                ProductType = productType,
                Area = area, 
                CostPerSquareFoot = 2.25m,
                LaborCostPerSquareFoot = 2.25m
            };

            AddOrderResponse addResponse = orderManager.AddOrder(newOrder);

            Assert.AreEqual(expected, addResponse.Success);
        }

        [TestCase("Andrew", "OH", "Carpet", 200, true)]
        [TestCase("Chad Testa", "MN", "Carpet", 200, false)]
        [TestCase("Chad Testa", "OH", "Tile", 200, true)]
        [TestCase("Chad Testa", "OH", "Marble", 200, false)]
        [TestCase("Chad Testa", "OH", "Carpet", 21, false)]
        [TestCase("Chad Testa", "OH", "Carpet", 200.5, false)]
        public void CanEditOrder(string name, string state, string productType, decimal area, bool expected)
        {

            OrderManager orderManager = new OrderManager(orderRepo);
            Order editOrder = new Order
            {
                OrderNumber = 99,
                OrderDate = DateTime.Parse("08/07/2020"),
                CustomerName = name,
                State = state,
                TaxRate = 6.25m,
                ProductType = productType,
                Area = area,
                CostPerSquareFoot = 2.25m,
                LaborCostPerSquareFoot = 2.25m
            };
            OrderLookupResponse ogOrder = orderManager.LookupOrder(editOrder.OrderNumber, editOrder.OrderDate);
            EditOrderResponse editResponse = orderManager.EditOrder(ogOrder.Order, editOrder);

            Assert.AreEqual(expected, editResponse.Success);
        }

        [TestCase(100, "08/07/2020", true)]
        [TestCase(200, "08/07/2020", false)]
        [TestCase(100, "08/07/2021", false)]
        public void CanDeleteOrder(int orderNumber, string date, bool expected)
        {
            DateTime orderDate = DateTime.Parse(date);
            Order _order = new Order
            {
                OrderDate = DateTime.Parse("08/07/2020"),
                OrderNumber = 100,
                CustomerName = "Chuck Testa",
                State = "OH",
                TaxRate = 6.25m,
                ProductType = "Carpet",
                Area = 100.5m,
                CostPerSquareFoot = 2.25m,
                LaborCostPerSquareFoot = 2.25m,
            };

            OrderManager orderManager = new OrderManager(orderRepo);
            AddOrderResponse addOrderResponse = orderManager.AddOrder(_order);


            OrderLookupResponse orderLookup = orderManager.LookupOrder(orderNumber, orderDate);
            if(orderLookup.Success)
            {
                DeleteOrderResponse deleteOrderResponse = orderManager.DeleteOrder(orderLookup.Order);
                Assert.AreEqual(expected, deleteOrderResponse.Success);
            }
            else
            {
                Assert.AreEqual(expected, orderLookup.Success);
            }

        }
    }
}
