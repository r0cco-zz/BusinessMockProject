using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringProgram.Models
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string State { get; set; }
        public decimal TaxRate { get; set; }
        public ProductTypes ProductType { get; set; }
        public decimal Area { get; set; }
        //public decimal MaterialCostPerSqFt { get; set; }
        //public decimal LaborCostPerSqFt { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public int OrderDate { get; set; }
    }
}
