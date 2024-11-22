namespace Prog_POE_Part2.Models
{
    public class Invoice
    {
        public int ClaimId { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public double TotalAmount { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
