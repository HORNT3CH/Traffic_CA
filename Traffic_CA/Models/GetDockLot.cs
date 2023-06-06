#nullable disable
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Traffic_CA.Models
{
    public class GetDockLot
    {
        public int Id { get; set; } 
        public string Location { get; set; }
        public string CarrierName { get; set; }
        public string TrailerNumber { get; set; }
        public string Status { get; set; }
        public string TrailerComments { get; set; }
        public string MBOLNbr { get; set; }
        public DateTime DocklotDate { get; set; }
    }
}
