using FlooringMastery.UI.Workflows;
using System;

namespace FlooringMastery.UI
{
    public class MainMenu
    {
        public static void Start()
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Flooring Program");
                Console.WriteLine(ConsoleIO.DividingBar);
                Console.WriteLine("1. Display Orders");
                Console.WriteLine("2. Add an Order");
                Console.WriteLine("3. Edit an Order");
                Console.WriteLine("4. Remove an Order");

                Console.WriteLine("\nQ - Quit");
                Console.WriteLine(ConsoleIO.DividingBar);
                string userInput = ConsoleIO.GetStringInputFromUser("Enter a menu option: ").ToUpper();

                switch (userInput)
                {
                    case "1":
                    case "D":
                    case "DISPLAY":
                    case "DISPLAY ORDERS":
                        OrderLookupWorkflow lookupFlow = new OrderLookupWorkflow();
                        lookupFlow.Execute();
                        break;
                    case "2":
                    case "A":
                    case "ADD":
                    case "ADD ORDER":
                    case "ADD AN ORDER":
                        AddOrderWorkflow addOrderFlow = new AddOrderWorkflow();
                        addOrderFlow.Execute();
                        break;
                    case "3":
                    case "E":
                    case "EDIT":
                    case "EDIT ORDER":
                    case "EDIT AN ORDER":
                        EditOrderWorkflow editOrderFlow = new EditOrderWorkflow();
                        editOrderFlow.Execute();
                        break;
                    case "4":
                    case "R":
                    case "REMOVE":
                    case "REMOVE ORDER":
                    case "REMOVE AN ORDER":
                        DeleteOrderWorkflow deleteOrderFlow = new DeleteOrderWorkflow();
                        deleteOrderFlow.Execute();
                        break;
                    case "ALL":
                        OrderLookupAllWorkflow lookupAllFlow = new OrderLookupAllWorkflow();
                        lookupAllFlow.Execute();
                        break;
                    case "Q":
                        return;
                }           
            }
        }
    }
}
