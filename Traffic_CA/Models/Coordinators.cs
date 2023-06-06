using System.ComponentModel;

namespace Traffic_CA.Models
{
    public class Coordinators
    {
        public int Id { get; set; }
        [DisplayName("Coordinator Name")]
        public string? CoordinatorName { get; set; }
    }
}
