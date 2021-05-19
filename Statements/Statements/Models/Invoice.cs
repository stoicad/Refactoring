using System.Collections.Generic;

namespace Statements.Models
{
    public class Invoice
    {
        public string CustomerName { get; set; }
        public List<Performance> Performances { get; set; }
    }
}