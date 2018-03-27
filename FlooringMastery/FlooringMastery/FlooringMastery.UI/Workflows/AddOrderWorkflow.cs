using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using System;

namespace FlooringMastery.UI.Workflows
{
    public class AddOrderWorkflow
    {
        public void Execute()
        {
            Console.Clear();
            ConsoleIO.HeadingLable("Add an Order");
            Order newOrder = new Order();
            newOrder.OrderDate = ConsoleIO.GetDateFromUser("Enter Order Date (ex. MM/DD/YYYY): ");
            newOrder.CustomerName = ConsoleIO.GetStringInputFromUser("Customer Name: ");

            StateTaxManager stateTaxManager = StateTaxManagerFactory.Create();
            bool validState = false;
            while(!validState)
            {
                StateLookupResponse response = stateTaxManager.GetStateTax(ConsoleIO.GetStringInputFromUser("State (ex. MN, or Minnesota): "));
                if(response.Success)
                {
                    newOrder.State = response.StateTax.StateAbbreviation;
                    newOrder.TaxRate = response.StateTax.TaxRate;
                    validState = true;
                }
                else
                {
                    ConsoleIO.RedMessage($"An Error Occured: {response.Message}");
                }
            }

            ProductManager productManager = ProductManagerFactory.Create();
            ProductListResponse productList = productManager.GetProductList();
            if(productList.Success)
            {
                ConsoleIO.HeadingLable("Product List");

                foreach (Product p in productList.Products)
                {
                    ConsoleIO.DisplayProducts(p);
                }
            }
            else
            {
                ConsoleIO.RedMessage($"An Error Occured: {productList.Message}");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            bool validProduct = false;
            while (!validProduct)
            {
                ProductLookupResponse response = productManager.GetProduct(ConsoleIO.GetStringInputFromUser("Product Name: "));
                if(response.Success)
                {
                    newOrder.ProductType = response.Product.ProductType;
                    newOrder.CostPerSquareFoot = response.Product.CostPerSquareFoot;
                    newOrder.LaborCostPerSquareFoot = response.Product.LaborCostPerSquareFoot;
                    validProduct = true;
                }
                else
                {
                    ConsoleIO.RedMessage($"An Error Occured: {response.Message}");
                }
            }

            newOrder.Area = ConsoleIO.GetAreaFromUser("Area: ");
            ConsoleIO.HeadingLable($"{newOrder.CustomerName}'s New Order");
            ConsoleIO.DisplayOrderInformation(newOrder, false);

            OrderManager orderManager = OrderManagerFactory.Create();

            bool submit = ConsoleIO.GetYesNoAnswerFromUser("Would you like to submit this order?");
            if(submit)
            {
                
                AddOrderResponse addResponse = orderManager.AddOrder(newOrder);

                if(addResponse.Success)
                {
                    Console.Clear();
                    ConsoleIO.YellowMessage("Order Submitted!");
                    ConsoleIO.DisplayOrderInformation(addResponse.Order, true);
                }
                else
                {
                    ConsoleIO.RedMessage($"An Error Occured: {addResponse.Message}");
                }
            }
            else
            {
                ConsoleIO.RedMessage("Add order cancelled.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
