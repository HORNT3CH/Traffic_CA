using System.ComponentModel;

namespace Traffic_CA.Models
{
	public class Associate
	{
		public int Id { get; set; }
		[DisplayName("Associate Name")]
		public string? AssociateName { get; set; }
		[DisplayName("Associate Initials")]
		public string? AssociateInitials { get; set; }
		[DisplayName("Shift Number")]
		public string? ShiftNumber { get; set; }
		public string? Department { get; set; }
		[DisplayName("Associate Username")]
		public string? AssociateUserName { get; set; }
	}
}
