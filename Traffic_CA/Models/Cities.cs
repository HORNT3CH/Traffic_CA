using System.ComponentModel;

namespace Traffic_CA.Models
{
    public class Cities
    {
        public int Id { get; set; }
        [DisplayName("City Name")]
        public String? CityName { get; set; }
    }
}
