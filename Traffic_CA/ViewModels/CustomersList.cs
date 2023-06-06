using Traffic_CA.Models;

namespace Traffic_CA.ViewModels
{
    public class CustomersList
    {
        public IEnumerable<Customers>? Customers { get; set; }
        public Customers? Customer { get; set; }
    }
}
