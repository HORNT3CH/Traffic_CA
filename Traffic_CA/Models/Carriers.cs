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
		[DisplayName("Carrier Contact")]
		public string? CarrierContact { get; set; }
		[DisplayName("Carrier Phone")]
		public string? CarrierPhone { get; set; }
		[DisplayName("Carrier Fax")]
		public string? CarrierFax { get; set; }
		[DisplayName("Carrier Email")]
		public string? CarrierEmail { get; set; }
	}
}
