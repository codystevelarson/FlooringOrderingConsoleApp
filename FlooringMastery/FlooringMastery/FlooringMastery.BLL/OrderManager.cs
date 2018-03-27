using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlooringMastery.BLL
{
    public class OrderManager
    {
        private IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public OrderLookupResponse LookupOrder(int orderNumber, DateTime orderDate)
        {
            OrderLookupResponse response = new OrderLookupResponse();
            response.Order = _orderRepository.LoadOrder(orderNumber, orderDate);

            if (response.Order == null)
            {
                response.Success = false;
                response.Message = $"Could not find orders matching \nOrder Number: {orderNumber} & Order Date: {orderDate:MM/dd/yyyy}";
            }
            else
            {
                response.Success = true;
            }
            return response;

        }

        public OrderLookupAllResponse LookupAllOrders(DateTime orderDate)
        {
            OrderLookupAllResponse response = new OrderLookupAllResponse();
            response.Orders = _orderRepository.List(orderDate);

            if (response.Orders.Count() == 0)
            {
                response.Success = false;
                response.Message = "Repository not found!";
                return response;
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public DeleteOrderResponse DeleteOrder(Order order)
        {
            DeleteOrderResponse response = new DeleteOrderResponse();

            List<Order> orders = _orderRepository.List(order.OrderDate);
            _orderRepository.DeleteOrder(order, orders);
            
            if (orders.Any(o => o == order))
            {
                response.Success = false;
                response.Message = $"Failed to delete Order: {order.OrderNumber} - {order.OrderDate:MM/dd/yyyy}";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        //Takes in an original order, makes a copy, assigns user edits from "editOrder" to the copy and returns copy to be saved
        public EditOrderResponse EditOrder(Order originalOrder, Order editOrder)
        {
            Order order = new Order(originalOrder);
            EditOrderResponse response = new EditOrderResponse();

            //Assign order properties if not blank or different than the original order
            if (order.CustomerName != editOrder.CustomerName && editOrder.CustomerName != "")
            {
                order.CustomerName = editOrder.CustomerName;
            }
            if (order.State != editOrder.State && editOrder.State != null)
            {
                order.State = editOrder.State;
                order.TaxRate = editOrder.TaxRate;
            }
            if (order.Area != editOrder.Area && editOrder.Area != decimal.MinValue)
            {
                order.Area = editOrder.Area;
            }
            if (order.ProductType != editOrder.ProductType && editOrder.ProductType != null)
            {
                order.ProductType = editOrder.ProductType;
                order.CostPerSquareFoot = editOrder.CostPerSquareFoot;
                order.LaborCostPerSquareFoot = editOrder.LaborCostPerSquareFoot;
            }

            //Validation or the updated order
            if (order.OrderDate < DateTime.Now)
            {
                response.Success = false;
                response.Message = "Order date MUST be in the future!";
                return response;
            }
            if (order.Area < 100)
            {
                response.Success = false;
                response.Message = "Order area MUST be greater than 100sqft!";
                return response;
            }
            ProductManager productManager = ProductManagerFactory.Create();
            ProductLookupResponse productResponse = productManager.GetProduct(order.ProductType);
            if (!productResponse.Success)
            {
                response.Success = false;
                response.Message = $"Product type: {order.ProductType} does not exist!";
                return response;
            }
            StateTaxManager stateTaxManager = StateTaxManagerFactory.Create();
            StateLookupResponse taxResponse = stateTaxManager.GetStateTax(order.State);
            if (!taxResponse.Success)
            {
                response.Success = false;
                response.Message = $"No state tax data found for: {order.State}";
                return response;
            }

            //lookup order and check for changes
            OrderLookupResponse lookupResponse = LookupOrder(order.OrderNumber, order.OrderDate);
            if (lookupResponse.Success)
            {

                if (order.AreEqualOrders(lookupResponse.Order, order))
                {
                    response.Success = false;
                    response.Message = $"No changes entered for Order: {order.OrderNumber} - {order.OrderDate:MM/dd/yyyy}.";
                }
                else
                {
                    response.Success = true;
                    response.Order = order;
                }
            }
            else
            {
                response.Success = false;
                response.Message = $"{lookupResponse.Message}\nOrder Number and Order Date cannot be changed!";
            }
            
            return response;
        }

        public AddOrderResponse AddOrder(Order order)
        {
            
            AddOrderResponse response = new AddOrderResponse();

            //Validation
            if (order.OrderDate < DateTime.Now)
            {
                response.Success = false;
                response.Message = "Order date MUST be in the future!";
                return response;
            }
            if (order.Area < 100)
            {
                response.Success = false;
                response.Message = "Order area MUST be greater than 100sqft!";
                return response;
            }
            ProductManager productManager = ProductManagerFactory.Create();
            ProductLookupResponse productResponse = productManager.GetProduct(order.ProductType);
            if (!productResponse.Success)
            {
                response.Success = false;
                response.Message = $"Product type: {order.ProductType} does not exist!";
                return response;
            }
            StateTaxManager stateTaxManager = StateTaxManagerFactory.Create();
            StateLookupResponse taxResponse = stateTaxManager.GetStateTax(order.State);
            if (!taxResponse.Success)
            {
                response.Success = false;
                response.Message = $"No state tax data found for: {order.State}";
                return response;
            }

            bool duplicateOrder = _orderRepository.List(order.OrderDate)
                                .Any(o =>
                                     o.CustomerName == order.CustomerName
                                     && o.State == order.State
                                     && o.ProductType == order.ProductType
                                     && o.Area == order.Area
                                     );
            if (duplicateOrder)
            {
                response.Success = false;
                response.Message = "Order entered already exists!";
            }
            else
            {
                _orderRepository.AddOrder(order);
                response.Success = true;
                response.Order = order;
            }
            return response;
        }

        public SaveOrderResponse SaveOrder(Order order)
        {
            SaveOrderResponse response = new SaveOrderResponse();

            //Validation
            if (order.Area < 100)
            {
                response.Success = false;
                response.Message = "Order area MUST be greater than 100sqft!";
                return response;
            }
            ProductManager productManager = ProductManagerFactory.Create();
            ProductLookupResponse productResponse = productManager.GetProduct(order.ProductType);
            if (!productResponse.Success)
            {
                response.Success = false;
                response.Message = $"Product type: {order.ProductType} does not exist!";
                return response;
            }
            StateTaxManager stateTaxManager = StateTaxManagerFactory.Create();
            StateLookupResponse taxResponse = stateTaxManager.GetStateTax(order.State);
            if (!taxResponse.Success)
            {
                response.Success = false;
                response.Message = $"No state tax data found for: {order.State}";
                return response;
            }

            //Save order
            _orderRepository.SaveOrder(order);
            if(!order.AreEqualOrders(order, _orderRepository.LoadOrder(order.OrderNumber,order.OrderDate)))
            {
                response.Success = false;
                response.Message = "Failed to save order!";
            }
            else
            {
                response.Success = true;
            }
            return response;
        }
    }
}
