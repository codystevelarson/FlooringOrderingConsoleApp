using System.Collections.Generic;

namespace FlooringMastery.Models.Responses
{
    public class ProductListResponse : Response
    {
        public List<Product> Products { get; set; }
    }
}
