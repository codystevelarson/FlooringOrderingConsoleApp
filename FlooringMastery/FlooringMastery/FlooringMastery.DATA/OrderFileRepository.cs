using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlooringMastery.DATA
{
    public class OrderFileRepository : IOrderRepository
    {
        private string _filePath;

        public OrderFileRepository(string filePath)
        {
            _filePath = filePath;
        }


        public void AddOrder(Order order)
        {
            //If no orders for that date have been made yet, create a file for that date
            if (!File.Exists($"{_filePath}_{order.OrderDate:MMddyyyy}.txt"))
            {
                CreateOrderFile(new List<Order>(), order.OrderDate);
            }

            //If no orders in the file order number is set to 1, else set to max order number +1 
            List<Order> orders = List(order.OrderDate);

            if (orders.Count() == 0)
            {
                order.OrderNumber = 1;
            }
            else
            {
                order.OrderNumber = List(order.OrderDate).Max(o => o.OrderNumber) + 1;
            }

            //Append the order list file with the AddOrder order
            using (StreamWriter sw = new StreamWriter($"{_filePath}_{order.OrderDate:MMddyyyy}.txt", true))
            {
                string line = CreateDsvForOrder(order);
                sw.WriteLine(line);
            }
        }

        public void DeleteOrder(Order order, List<Order> orders)
        {
            //Remove the order that match the order provided 
            foreach (Order o in orders)
            {
                if (o.OrderNumber == order.OrderNumber)
                {
                    orders.Remove(o);
                    break;
                }
            }

            //delete and recreate order list file
            CreateOrderFile(orders, order.OrderDate);
        }

        public List<Order> List(DateTime orderDate)
        {
            List<Order> orders = new List<Order>();

            //If file doesnt exist for order return an empty list
            bool fileExists = File.Exists($"{_filePath}_{orderDate:MMddyyyy}.txt");
            if (!fileExists)
            {
                return orders;
            }

            //Read each line after the header, assign order params, add order to list, repeat until the line being read is null
            using (StreamReader sr = new StreamReader($"{_filePath}_{orderDate:MMddyyyy}.txt"))
            {
                sr.ReadLine();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    Order order = new Order();
                    string[] columns = line.Split('|');

                    order.OrderDate = orderDate;
                    order.OrderNumber = int.Parse(columns[0]);
                    order.CustomerName = columns[1];
                    order.State = columns[2];
                    order.TaxRate = decimal.Parse(columns[3]);
                    order.ProductType = columns[4];
                    order.Area = decimal.Parse(columns[5]);
                    order.CostPerSquareFoot = decimal.Parse(columns[6]);
                    order.LaborCostPerSquareFoot = decimal.Parse(columns[7]);

                    orders.Add(order);
                }
            }
            return orders;
        }

        public Order LoadOrder(int orderNumber, DateTime orderDate)
        {
            //Get order list file by order date
            List<Order> orders = List(orderDate);

            //if any orders in the list match the order number given return that order, else return null.
            if (orders.Any(order => order.OrderNumber == orderNumber))
            {
                return orders.Single(o => o.OrderNumber == orderNumber);
            }
            else
            {
                return null;
            }
        }

        public void SaveOrder(Order order)
        {
            //get the order list for the order you want to save
            List<Order> orders = List(order.OrderDate);

            //Remove the matching order from the list and add the order parameter to the list
            foreach (Order o in orders)
            {
                if (o.OrderNumber == order.OrderNumber)
                {
                    orders.Remove(o);
                    orders.Add(order);
                    break;
                }
            }

            //Update the order listfile
            CreateOrderFile(orders, order.OrderDate);
        }


        private string CreateDsvForOrder(Order order)
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}", order.OrderNumber, order.CustomerName, order.State, order.TaxRate, order.ProductType, order.Area, order.CostPerSquareFoot, order.LaborCostPerSquareFoot, order.MaterialCost, order.LaborCost, order.Tax, order.Total);
        }

        private void CreateOrderFile(List<Order> orders, DateTime orderDate)
        {
            if (File.Exists($"{_filePath}_{orderDate:MMddyyyy}.txt"))
                File.Delete($"{_filePath}_{orderDate:MMddyyyy}.txt");

            using (StreamWriter sr = new StreamWriter($"{_filePath}_{orderDate:MMddyyyy}.txt"))
            {
                sr.WriteLine("OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");

                foreach (Order order in orders)
                {
                    sr.WriteLine(CreateDsvForOrder(order));
                }
            }
        }
    }
}
