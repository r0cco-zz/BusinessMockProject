using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringProgram.Models
{
    public class Response
    {
        bool Success { get; set; }
        string Message { get; set; }
        Order OrderInfo { get; set; }
    }
}
