using FlooringMastery.DATA;
using FlooringMastery.Models;
using System;
using System.Configuration;

namespace FlooringMastery.BLL
{
    public class ProductManagerFactory
    {
        public static ProductManager Create()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();

            switch(mode)
            {
                case "Test":
                    return new ProductManager(new ProductTestRepository());
                case "FileTest":
                    return new ProductManager(new ProductFileTestRepository(Settings.FilePathProductsTest));
                case "Prod":
                    return new ProductManager(new ProductFileRepository(Settings.FilePathProducts));
                default:
                    throw new Exception("Mode value in app config is not valid");
            }
        }
    }
}
