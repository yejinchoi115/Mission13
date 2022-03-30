using System;
using System.ComponentModel.DataAnnotations;

namespace Mission13.Models
{
    public class Team
    {
        [Key]
        [Required]
        public int TeamID { get; set; }

        public string TeamName { get; set; }

        public int CaptainID { get; set; }
    }
}
