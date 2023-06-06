using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Traffic_CA.Models
{
    public class Loads
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}"), DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
        [DisplayName("Load #")]
        public string? LoadNbr { get; set; }
        [DisplayName("Time Slot")]
        public string? TimeSlot { get; set; }
        [DisplayName("Stage Location")]
        public string? StageLocation { get; set; }
        [DisplayName("Carrier Name")]
        public string? CarrierName { get; set; }
        [DisplayName("Load Type")]
        public string? LoadType { get; set; }
        [DisplayName("Customer Name")]
        public string? CustomerName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        [DisplayName("MBOL #")]
        public string? MBOLNbr { get; set; }
        [DisplayName("Stager 1")]
        public string? StagerOne { get; set; }
        [DisplayName("Stager 2")]
        public string? StagerTwo { get; set; }
        [DisplayName("Stager 3")]
        public string? StagerThree { get; set; }
        [DisplayName("Stage Start")]
        public DateTime? StageStart { get; set; }
        [DisplayName("Stage Finish")]
        public DateTime? StageFinish { get; set; }
        [DisplayName("# Cartons")]
        public string? NbrCartons { get; set; }
        public string? Comments { get; set; }
        [DisplayName("Cube")]
        public string? LoadCube { get; set; }
        [DisplayName("# Stops")]
        public string? NbrStops { get; set; }
        public string? Waved { get; set; }
        [DisplayName("Coordinator")]
        public string? CoordinatorName { get; set; }
        [DisplayName("Load Status")]
        public string? LoadStatus { get; set; }
        public string? Labels { get; set; }
        [DisplayName("Loader 1")]
        public string? LoaderOne { get; set; }
        [DisplayName("Loader 2")]
        public string? LoaderTwo { get; set; }
        [DisplayName("Loader 3")]
        public string? LoaderThree { get; set; }
        [DisplayName("Load Start")]
        public DateTime? LoadStart { get; set; }
        [DisplayName("Load Finish")]
        public DateTime? LoadFinish { get; set; }
        [DisplayName("Trailer #")]
        public string? TrailerNumber { get; set; }
        public string? Lot { get; set; }
        public string? Door { get; set; }
        [DisplayName("Arrival Time")]
        public DateTime? ArrivalTime { get; set; }
        [DisplayName("Req Trailer")]
        public string? RequestedTrailer { get; set; }
        [DisplayName("Appt #")]
        public string? AppointmentNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true), DisplayName("Pickup Date")]
        public DateTime? PickupDate { get; set; }
        [DataType(DataType.Currency), DisplayName("Load Value")]
        public decimal? LoadValue { get; set; }
        public DateTime? ConvertedTimeSlots { get; set; }
    }
}
