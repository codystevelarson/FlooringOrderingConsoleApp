using System.Collections.Generic;

namespace FlooringMastery.Models.Interfaces
{
    public interface IProductRepository
    {
        Product GetProduct(string productType);
        List<Product> List();
    }
}
