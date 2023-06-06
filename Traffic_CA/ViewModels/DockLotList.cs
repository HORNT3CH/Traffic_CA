using Traffic_CA.Models;

namespace Traffic_CA.ViewModels
{
    public class DockLotList
    {
        public IEnumerable<Docklot>? Docklot { get; set; }
        public Docklot? Docklots { get; set; }
    }
}
