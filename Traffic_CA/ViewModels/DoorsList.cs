using Traffic_CA.Models;

namespace Traffic_CA.ViewModels
{
    public class DoorsList
    {
        public IEnumerable<Doors>? Doors { get; set; }  
        public Doors? Door { get; set; }
    }
}
