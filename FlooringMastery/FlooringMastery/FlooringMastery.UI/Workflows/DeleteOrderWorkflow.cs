using FlooringMastery.BLL;
using FlooringMastery.Models.Responses;
using System;

namespace FlooringMastery.UI.Workflows
{
    public class DeleteOrderWorkflow
    {
        public void Execute()
        {
            Console.Clear();
            ConsoleIO.HeadingLable("Delete an Order");
            int orderNumber = ConsoleIO.GetIntInputFromUser("Order Number: ");
            DateTime orderDate = ConsoleIO.GetDateFromUser("Order Date (MM/DD/YYYY): ", true);

            OrderManager orderManager = OrderManagerFactory.Create();
            OrderLookupResponse lookupResponse = orderManager.LookupOrder(orderNumber, orderDate);

            if(lookupResponse.Success)
            {
                ConsoleIO.DisplayOrderInformation(lookupResponse.Order, true);
                bool deleteIt = ConsoleIO.GetYesNoAnswerFromUser("Are you sure you want to delete this order?");
                if(deleteIt)
                {
                    DeleteOrderResponse deleteResponse = orderManager.DeleteOrder(lookupResponse.Order);
                    if(deleteResponse.Success)
                    {
                        ConsoleIO.YellowMessage("Order Deleted!");
                    }
                    else
                    {
                        ConsoleIO.RedMessage($"An Error Occured: {deleteResponse.Message}");
                    }
                }
                else
                {
                    ConsoleIO.RedMessage("Delete order cancelled.");
                }
            }
            else
            {
                ConsoleIO.RedMessage($"An Error Occured: {lookupResponse.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
