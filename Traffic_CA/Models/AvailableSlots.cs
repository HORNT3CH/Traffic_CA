namespace Traffic_CA.Models
{
    public class AvailableSlots
    {
        public DateTime SlotDate { get; set; }
        public string? StartTime { get; set; }
        public int Standard { get; set; }
        public int Scheduled { get; set; }
    }
}
