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

        //Contains code for the submission of claims
        [HttpPost]
        public IActionResult SubmitClaim(LecturerTb lecturerClaim)
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Add(lecturerClaim);//Adding data to LecturerTb
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
                _context.Claims.Add(claim);//Saving claim as a new claim
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
        public async Task<IActionResult> VerifyClaim()//Contains code for the verification of claims
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
            //Code which allows the lecturer to approve a claim
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
            //code which allows a lecturer to reject a claim
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.Status = "Rejected";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("VerifyClaim");
        }

        [HttpGet]
        public async Task<IActionResult> TrackClaim()
        {
            //fetching data from Claims table and ordering it
            var allClaims = await _context.Claims
                .Include(c => c.Lecturer)
                .OrderByDescending(c => c.SubmissionDate)
                .ToListAsync();
            return View(allClaims);
        }
    }
}


//----------------------------------------------End Of File--------------------------------------------\\