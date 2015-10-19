using System.Collections.Generic;

namespace FlooringProgram.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Order> OrderList { get; set; }
        public Order Order { get; set; }
    }
}
