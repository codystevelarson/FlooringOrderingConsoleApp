using FlooringMastery.DATA;
using FlooringMastery.Models;
using System;
using System.Configuration;

namespace FlooringMastery.BLL
{
    public class OrderManagerFactory
    {
        public static OrderManager Create()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();

            switch(mode)
            {
                case "Test":
                    return new OrderManager(new OrderTestRepository());
                case "FileTest":
                    return new OrderManager(new OrderFileTestRepository(Settings.FilePathOrdersTest));
                case "Prod":
                    return new OrderManager(new OrderFileRepository(Settings.FilePathOrders));
                default:
                    throw new Exception("Mode value in app config is not valid");
            }
        }
    }
}
