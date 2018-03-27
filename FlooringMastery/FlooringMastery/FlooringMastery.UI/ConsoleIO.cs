using FlooringMastery.Models;
using System;

namespace FlooringMastery.UI
{
    public class ConsoleIO
    {
        public const string DividingBar = "==================================================";
        public const string OrderBar = "**************************************";
        public const string ProductBar = "--------------------------------------";


        public static void HeadingLable(string prompt)
        {
            Console.WriteLine(DividingBar);
            Console.WriteLine(prompt);
            Console.WriteLine(DividingBar);
        }

        public static DateTime GetDateFromUser(string prompt)
        {
            DateTime date = new DateTime();
            while(true)
            {
                Console.Write(prompt);
                string orderDate = Console.ReadLine();
                if (!DateTime.TryParse(orderDate, out date))
                {
                    RedMessage("Error: Date format must match (MM/DD/YYYY)");
                }
                else if(date < DateTime.Now)
                {
                    RedMessage("Error: Order Date must be in the future.");
                }
                else
                {
                    return date;
                }
            }
        }

        public static DateTime GetDateFromUser(string prompt, bool canBeInPast)
        {
            DateTime date = new DateTime();
            while (true)
            {
                Console.Write(prompt);
                string orderDate = Console.ReadLine();
                if (!DateTime.TryParse(orderDate, out date))
                {
                    RedMessage("Error: Date format must match (MM/DD/YYYY)");
                }
                else if (!canBeInPast && date < DateTime.Now)
                {
                    RedMessage("Error: Order Date must be in the future.");
                }
                else
                {
                    return date;
                }
            }
        }


        public static void DisplayOrderInformation(Order order, bool showOrderNumber)
        {
            Console.WriteLine(OrderBar);
            if (showOrderNumber)
            {
                Console.WriteLine($"Order Number: {order.OrderNumber}");
            }
            Console.WriteLine($"Order Date: {order.OrderDate:MM/dd/yyyy}");
            Console.WriteLine($"Customer Name: {order.CustomerName}");
            Console.WriteLine($"State: {order.State}");
            Console.WriteLine($"Product: {order.ProductType}");
            Console.WriteLine($"Area: {order.Area}");
            Console.WriteLine($"Materials: {order.MaterialCost:c}");
            Console.WriteLine($"Labor: {order.LaborCost:c}");
            Console.WriteLine($"Tax: {order.Tax:c}");
            Console.WriteLine($"Total: {order.Total:c}");
            Console.WriteLine(OrderBar);

        }

        public static void DisplayProducts(Product p)
        {
            Console.WriteLine($"{p.ProductType}:");
            Console.WriteLine($"    -Cost/sqft.: {p.CostPerSquareFoot:c}");
            Console.WriteLine($"    -Labor Cost/sqft.: {p.CostPerSquareFoot:c}");
            Console.WriteLine(ProductBar);
        }


        public static string GetStringInputFromUser(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    RedMessage("Must enter a valid input.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    return input;
                }
            }
        }

        public static string GetStringInputFromUser(string prompt, bool canBeNull)
        {       
            Console.Write(prompt);
            string input = Console.ReadLine();
            return input;            
        }


        public static int GetIntInputFromUser(string prompt)
        {
            int output = int.MinValue;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out output))
                {
                    RedMessage("Must enter a valid number.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    return output;
                }
            }
        }


        public static decimal GetAreaFromUser(string prompt)
        {
            int output;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!int.TryParse(input, out output))
                {
                    RedMessage("You must enter a valid number.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else if (output < 100m)
                {
                    RedMessage("All orders must be at least 100sqft.");
                }
                else
                {
                    decimal d = (decimal)output;
                    return output;
                }
            }
        }

        public static decimal GetAreaFromUser(string prompt, bool canBeNull)
        {
            decimal output;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (input == "")
                {
                    return decimal.MinValue;
                }

                if (!decimal.TryParse(input, out output))
                {
                    RedMessage("You must enter a valid number.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else if (output < 100m)
                {
                    RedMessage("All orders must be at least 100sqft.");
                }
                else
                {
                    return output;
                }
            }
        }


        public static bool GetYesNoAnswerFromUser(string prompt)
        {
            while (true)
            {
                Console.Write(prompt + " (Y/N?): ");
                string input = Console.ReadLine().ToUpper();

                if (string.IsNullOrEmpty(input))
                {
                    RedMessage("You must enter Y/N text.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                if (input != "Y" && input != "N")
                {
                    RedMessage("You must enter Y/N text.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }
                else
                {
                    if (input == "Y")
                        return true;
                    else
                        return false;
                }
            }
        }


        public static void RedMessage(string redText)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(redText);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void YellowMessage(string yellowText)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(yellowText);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
