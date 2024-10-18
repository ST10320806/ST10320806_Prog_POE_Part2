using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog_POE_Part2.Data;
using Prog_POE_Part2.Models;

namespace ST10320806Prog2.Controllers
{
    public class ClaimController : Controller
    {
        private readonly AppDbContext _context;

        public ClaimController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitClaim(LecturerTb lecturerClaim)
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Add(lecturerClaim);
                _context.SaveChanges();
                var claim = new ClaimTb
                {
                    LecturerId = lecturerClaim.LecturerId,
                    Status = "Pending",
                    SubmissionDate = DateTime.Now,
                    HoursWorked = lecturerClaim.HoursWorked,
                    HourlyRate = lecturerClaim.HourlyRate,
                    ClaimNotes = lecturerClaim.ClaimNotes
                };
                _context.Claims.Add(claim);
                _context.SaveChanges();
                return RedirectToAction("ClaimSubmitted");
            }
            return View(lecturerClaim);
        }

        public IActionResult ClaimSubmitted()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> VerifyClaim()
        {
            var pendingClaims = await _context.Claims
                .Where(c => c.Status == "Pending")
                .Include(c => c.Lecturer)
                .ToListAsync();
            return View(pendingClaims);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.Status = "Approved";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("VerifyClaim");
        }

        [HttpPost]
        public async Task<IActionResult> RejectClaim(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.Status = "Rejected";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("VerifyClaim");
        }
    }
}
