using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace FlooringMastery.DATA
{
    public class ProductFileRepository : IProductRepository
    {
        private string _filePath;

        public ProductFileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public Product GetProduct(string productType)
        {
            List<Product> products = List();

            foreach (Product p in products)
            {
                if (p.ProductType == productType)
                {
                    return p;
                }
            }
            return null;
        }

        public List<Product> List()
        {
            List<Product> products = new List<Product>();
            using (StreamReader sr = new StreamReader(_filePath))
            {
                //Skip header
                sr.ReadLine();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Product product = new Product();
                    string[] columns = line.Split(',');

                    product.ProductType = columns[0];
                    product.CostPerSquareFoot = decimal.Parse(columns[1]);
                    product.LaborCostPerSquareFoot = decimal.Parse(columns[2]);

                    products.Add(product);
                }
            }
            return products;
        }
    }
}
