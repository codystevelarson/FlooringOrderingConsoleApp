using FlooringMastery.BLL;
using FlooringMastery.Models.Responses;
using System;

namespace FlooringMastery.UI.Workflows
{
    public class OrderLookupWorkflow
    {
        public void Execute()
        {
            Console.Clear();
            ConsoleIO.HeadingLable("Order Lookup");
            int orderNumber = ConsoleIO.GetIntInputFromUser("Order Number: ");
            DateTime orderDate = ConsoleIO.GetDateFromUser("Order Date (ex. MM/DD/YYYY): ", true);

            OrderManager manager = OrderManagerFactory.Create();
            OrderLookupResponse response = manager.LookupOrder(orderNumber, orderDate);
            if(response.Success)
            {
                ConsoleIO.DisplayOrderInformation(response.Order, true);
            }
            else
            {
                ConsoleIO.RedMessage($"An Error Occured: {response.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
