using Traffic_CA.Models;

namespace Traffic_CA.ViewModels
{
    public class TimeSlotsList
    {
        public IEnumerable<TimeSlots>? TimeSlots { get; set; }
        public TimeSlots? TimeSlot { get; set; }
    }
}
