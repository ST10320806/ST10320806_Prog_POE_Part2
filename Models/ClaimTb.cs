using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prog_POE_Part2.Models
{
    public class ClaimTb
    {
        [Key]
        public int ClaimId { get; set; }

        [Required]
        public int LecturerId { get; set; }

        [ForeignKey("LecturerId")]
        public LecturerTb Lecturer { get; set; }

        [Required]
        public string Status { get; set; } 

        [Required]
        public DateTime SubmissionDate { get; set; }

        [Required]
        public double HoursWorked { get; set; }

        [Required]
        public double HourlyRate { get; set; }

        public string ClaimNotes { get; set; }

        [NotMapped]
        public double Amount => HoursWorked * HourlyRate; //Calculating total hours
    }
}