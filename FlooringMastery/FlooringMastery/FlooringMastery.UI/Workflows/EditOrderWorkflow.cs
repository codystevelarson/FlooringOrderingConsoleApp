using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using System;

namespace FlooringMastery.UI.Workflows
{
    public class EditOrderWorkflow
    {
        public void Execute()
        {
            Console.Clear();
            ConsoleIO.HeadingLable("Edit an Order");

            //Get an order with an Order Number and Order Date
            int orderNumber = ConsoleIO.GetIntInputFromUser("Order Number: ");
            DateTime orderDate = ConsoleIO.GetDateFromUser("Order Date (MM/DD/YYYY): ", true);

            //Lookup Order and Begin editing or Error out
            OrderManager orderManager = OrderManagerFactory.Create();
            OrderLookupResponse orderLookup = orderManager.LookupOrder(orderNumber, orderDate);
            if(orderLookup.Success)
            {
                //Create a new order for the user to fill in params
                Order editOrder = new Order();

                //Display the order that will be edited
                ConsoleIO.HeadingLable($"Editing Order: {orderLookup.Order.OrderNumber} - {orderLookup.Order.OrderDate:MM/dd/yyyy}");

                //Get new order name from user, display the old name
                editOrder.CustomerName = ConsoleIO.GetStringInputFromUser($"Customer Name ({orderLookup.Order.CustomerName}): ", true);

                //Create a state tax object to get correct input from user for new order state params
                StateTaxManager stateTaxManager = StateTaxManagerFactory.Create();
                bool validInput = false;
                while (!validInput)
                {
                    //Get a state tax object that matches the user input unless input is ""
                    StateLookupResponse stateTaxResponse = stateTaxManager.GetStateTax(ConsoleIO.GetStringInputFromUser($"State (ex. MN, or Minnesota)({orderLookup.Order.State}): ", true));
                    if (!stateTaxResponse.Success && stateTaxResponse.Message == "")
                    {
                        validInput = true;
                    }
                    else if (stateTaxResponse.Success)
                    {
                        //If tax object exists assign new order state and tax params
                        editOrder.State = stateTaxResponse.StateTax.StateAbbreviation;
                        editOrder.TaxRate = stateTaxResponse.StateTax.TaxRate;
                        validInput = true;
                    }
                    else 
                    {
                        ConsoleIO.RedMessage($"An Error Occured: {stateTaxResponse.Message}");
                    }
                }

                //Create a product manager to get a product list
                ProductManager productManager = ProductManagerFactory.Create();
                ProductListResponse productList = productManager.GetProductList();

                //Print out product list or error out
                if (productList.Success)
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

                //Get valid user input for new order product type 
                bool validProduct = false;
                while (!validProduct)
                {
                    ProductLookupResponse productResponse = productManager.GetProduct(ConsoleIO.GetStringInputFromUser($"Product Name ({orderLookup.Order.ProductType}): ", true));

                    //User enters nothing and nothing is assigned
                    if (!productResponse.Success && productResponse.Message == "")
                    {
                        validProduct = true;
                    }
                    else if (productResponse.Success)
                    {
                        //Valid entry assigns new order product params 
                        editOrder.ProductType = productResponse.Product.ProductType;
                        editOrder.CostPerSquareFoot = productResponse.Product.CostPerSquareFoot;
                        editOrder.LaborCostPerSquareFoot = productResponse.Product.LaborCostPerSquareFoot;
                        validProduct = true;
                    }
                    else
                    {
                        ConsoleIO.RedMessage($"An Error Occured: {productResponse.Message}");
                    }
                }

                //Get valid area for new order from user
                editOrder.Area = ConsoleIO.GetAreaFromUser($"Area ({orderLookup.Order.Area}): ", true);

                //Assign old order params to new order params
                EditOrderResponse editResponse = orderManager.EditOrder(orderLookup.Order, editOrder);

                //If the edit is successful ask to save order, otherwise error out
                if (editResponse.Success)
                {
                    //Display 
                    ConsoleIO.HeadingLable("Updated Order");
                    ConsoleIO.DisplayOrderInformation(editResponse.Order, true);
                    bool saveEdit = ConsoleIO.GetYesNoAnswerFromUser("Would you like to submit and save the updated order information?");
                    if (saveEdit)
                    {
                        SaveOrderResponse saveResponse = orderManager.SaveOrder(editResponse.Order);
                        if (saveResponse.Success)
                        {
                            ConsoleIO.YellowMessage($"\nUpdated Order: {editResponse.Order.OrderNumber} - {editResponse.Order.OrderDate:MM/dd/yyyy}");
                        }
                        else
                        {
                            ConsoleIO.RedMessage($"An Error Occured: {saveResponse.Message}");
                        }
                    }
                }
                else
                {
                    ConsoleIO.RedMessage($"An Error Occured: {editResponse.Message}");
                }
            }
            else
            {
                ConsoleIO.RedMessage(orderLookup.Message);
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
