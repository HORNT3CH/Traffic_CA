using System.ComponentModel;

namespace Traffic_CA.Models
{
	public class Carriers
	{
		public int Id { get; set; }
		[DisplayName("Carrier Name")]
		public string? CarrierName { get; set; }
		[DisplayName("Carrier STCC")]
		public string? CarrierSTCC { get; set; }

	}
}
