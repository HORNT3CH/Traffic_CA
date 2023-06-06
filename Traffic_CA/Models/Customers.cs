using System.ComponentModel;

namespace Traffic_CA.Models
{
    public class Customers
    {
        public int Id { get; set; }
        [DisplayName("Customer Name")]
        public string? CustomerName { get; set; }
        public string? Analytical { get; set; }
    }
}
