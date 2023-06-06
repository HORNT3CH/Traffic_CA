using System.ComponentModel;

namespace Traffic_CA.Models
{
    public class Docklot
    {
        public int Id { get; set; }
        [DisplayName("Carrier Name")]
        public string? CarrierName { get; set; }
        [DisplayName("Trailer Number")]
        public string? TrailerNumber { get; set; }
        public string? Dimension { get; set; }
        [DisplayName("Trailer Comments")]
        public string? TrailerComments { get; set; }
        public string? LoadNbr { get; set; }
        [DisplayName("Time Stamp")]
        public DateTime DocklotDate { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
        public string? MBOLNbr { get; set; }
    }
}
