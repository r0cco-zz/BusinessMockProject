using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringProgram.Models
{
    public class Order
    {
        int OrderNumber { get; set; }
        string CustomerName { get; set; }
        string State { get; set; }
        decimal TaxRate { get; set; }
        string ProductType { get; set; }
        decimal Area { get; set; }
        decimal MaterialCostPerSqFt { get; set; }
        decimal LaborCostPerSqFt { get; set; }
        decimal MaterialCost { get; set; }
        decimal LaborCost { get; set; }
        decimal Tax { get; set; }
        decimal Total { get; set; }
    }
}
