using System.Collections.Generic;

namespace FlooringMastery.Models.Responses
{
    public class OrderLookupAllResponse : Response
    {
        public List<Order> Orders { get; set; }
    }
}
