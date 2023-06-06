using System.ComponentModel;

namespace Traffic_CA.Models
{
    public class States
    {
        public int Id { get; set; }
        [DisplayName("State Abbreviation")]
        public string? StateName { get; set; }
    }
}
