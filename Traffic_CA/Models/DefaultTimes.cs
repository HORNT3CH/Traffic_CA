using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace Traffic_CA.Models
{
    public class DefaultTimes
    {
        [Key]
        public int Id { get; set; }
        public string? StartTime { get; set; }
        public int Standard { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}"), DisplayName("Order Date")]
        public DateTime SlotDate { get; set; }
        public int Scheduled { get; set; }
        public int OverRide { get; set; }
        public int ReceivingStandard { get; set; }
        public int ReceivingScheduled { get; set; }
        public int ReceivingOverRide { get; set; }
    }
}
