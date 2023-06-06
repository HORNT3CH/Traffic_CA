using Traffic_CA.Models;

namespace Traffic_CA.ViewModels
{
    public class LotsList
    {
        public IEnumerable<Lots>? Lots { get; set; }
        public Lots? Lot { get; set; }
    }
}
