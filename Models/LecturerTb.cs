using System.ComponentModel.DataAnnotations;

namespace Prog_POE_Part2.Models
{
    public class LecturerTb
    {
        [Key]
        public int LecturerId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double HourlyRate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double HoursWorked { get; set; }

        [Required]
        public string ClaimNotes { get; set; }
    }
}
