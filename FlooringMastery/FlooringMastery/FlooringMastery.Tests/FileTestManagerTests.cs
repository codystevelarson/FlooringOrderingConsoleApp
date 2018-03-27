using FlooringMastery.BLL;
using FlooringMastery.DATA;
using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using FlooringMastery.Models.Responses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace FlooringMastery.Tests
{
    [TestFixture]
    public class FileTestManagerTests
    {
        private static string _filePath = @"C:\Repos\cody-larson-individual-work\FlooringMastery\FlooringMastery\FlooringMastery.DATA\Repositories\Test\OrderTestRepositoryTEST.txt";
        private static string _originalData = @"C:\Repos\cody-larson-individual-work\FlooringMastery\FlooringMastery\FlooringMastery.DATA\Repositories\Test\OrderTestRepository.txt";

        [SetUp]
        public void Setup()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            File.Copy(_originalData, _filePath);
        }

        private IOrderRepository repo = new OrderFileTestRepository(_filePath);

        [TestCase("CA", false)]
        [TestCase("California", false)]
        [TestCase("pa", false)]
        [TestCase("pennsylvania", false)]
        [TestCase("Pennsylvania", true)]
        [TestCase("PA", true)]
        public void CanGetStateTaxFromFile(string stateName, bool expected)
        {
            StateTaxManager taxManager = StateTaxManagerFactory.Create();
            StateLookupResponse response = taxManager.GetStateTax(stateName);

            Assert.AreEqual(expected, response.Success);

        }

        [TestCase("Marble", false)]
        [TestCase("CARPET", false)]
        [TestCase("Carpet", true)]
        [TestCase("Laminate", true)]
        [TestCase("Tile", true)]
        [TestCase("Wood", true)]
        public void CanGetProductFromFile(string productType, bool expected)
        {
            ProductManager productManager = ProductManagerFactory.Create();
            ProductLookupResponse response = productManager.GetProduct(productType);

            Assert.AreEqual(expected, response.Success);
        }

        [Test]
        public void CanListOrdersFromFile()
        {
            DateTime date = DateTime.Parse("08/07/2020");
            List<Order> orders = repo.List(date);

            Assert.IsNotNull(orders);
        }

        [TestCase(4, "08/07/2020", true)]
        [TestCase(10, "08/07/2020", false)]
        public void CanLoadOrderFromFile(int orderNumber, string orderDate, bool expected)
        {
            bool isNotNull = false;
            DateTime date = DateTime.Parse(orderDate);
            Order lookupOrder = repo.LoadOrder(orderNumber, date);
            if(lookupOrder != null)
            {
                isNotNull = true;
            }
            Assert.AreEqual(expected, isNotNull);
        }

        //Create new order props

        [TestCase("08/07/2020", "Wise", "OH", 6.25, "Wood", 100, 5.15, 4.75)]
        public void CanAddOrderToTestFile(string orderDate, string customerName, string state, decimal taxRate, string productType, decimal area, decimal costPer, decimal laborPer)
        {
            DateTime date = DateTime.Parse(orderDate);
            Order newOrder = new Order
            {
                OrderDate = date,
                CustomerName = customerName,
                State = state,
                TaxRate = taxRate,
                ProductType = productType,
                Area = area,
                CostPerSquareFoot = costPer,
                LaborCostPerSquareFoot = laborPer,
            };

            repo.AddOrder(newOrder);
            Order checkOrder = repo.LoadOrder(newOrder.OrderNumber, newOrder.OrderDate);
            Assert.IsNotNull(checkOrder);
        }

        [Test]
        public void CanSaveOrderToFile()
        {
            Order order = new Order
            {
                OrderDate = DateTime.Parse("08/07/2020"),
                OrderNumber = 5,
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
                OrderNumber = 5,
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
        public void CanDeleteOrderFromFile()
        {
            Order order = new Order
            {
                OrderDate = DateTime.Parse("08/07/2020"),
                OrderNumber = 5,
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
