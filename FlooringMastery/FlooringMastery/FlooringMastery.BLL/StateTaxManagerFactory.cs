using FlooringMastery.DATA;
using FlooringMastery.Models;
using System;
using System.Configuration;

namespace FlooringMastery.BLL
{
    public class StateTaxManagerFactory
    {
        public static StateTaxManager Create()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();

            switch(mode)
            {
                case "Test":
                    return new StateTaxManager(new StateTaxTestRepository());
                case "FileTest":
                    return new StateTaxManager(new StateTaxFileTestRepository(Settings.FilePathTaxesTest));
                case "Prod":
                    return new StateTaxManager(new StateTaxFileRepository(Settings.FilePathTaxes));
                default:
                    throw new Exception("Mode value in app config is not valid");
            }
        }
    }
}
