using Traffic_CA.Models;

namespace Traffic_CA.ViewModels
{
    public class CarriersList
    {
        public IEnumerable<Carriers>? Carriers { get; set; }
        public Carriers? Carrier { get; set; }
    }
}
