using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using System;
using System.Linq;

namespace FlooringMastery.UI.Workflows
{
    public class OrderLookupAllWorkflow
    {

        public void Execute()
        {
            Console.Clear();
            ConsoleIO.HeadingLable("Order Lookup (ALL BY DATE)");
            DateTime orderDate = ConsoleIO.GetDateFromUser("Order Date (ex. MM/DD/YYYY): ");

            OrderManager manager = OrderManagerFactory.Create();
            OrderLookupAllResponse response = manager.LookupAllOrders(orderDate);

            Console.Clear();

            if (response.Success)
            {
                ConsoleIO.HeadingLable($"All Orders for {orderDate:MM/dd/yyyy}");
                foreach (Order o in response.Orders.OrderBy(o => o.OrderNumber))
                {
                    ConsoleIO.DisplayOrderInformation(o, true);
                }
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
