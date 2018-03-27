using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System.Collections.Generic;

namespace FlooringMastery.DATA
{
    public class ProductTestRepository : IProductRepository
    {

        private Product _product = new Product
        {
            ProductType = "Carpet",
            CostPerSquareFoot = 2.25m,
            LaborCostPerSquareFoot = 2.25m
        };

        public Product GetProduct(string productType)
        {
            List<Product> products = List();
            
            foreach(Product p in products)
            {
                if(p.ProductType == productType)
                {
                    return p;
                }
            }
            return null;
        }

        public List<Product> List()
        {
            List<Product> products = new List<Product>();
            products.Add(_product);
            return products;
        }
    }
}
