using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog_POE_Part2.Data;
using Prog_POE_Part2.Models;
/// <summary>
/// Jesse Weeder
/// ST10320806
/// Module PROG6221
/// </summary>

/// <references>
/// Errors and bugs fixed by Claude AI
/// https://stackoverflow.com/questions/64250102/access-localdb-data-in-a-view
/// https://getbootstrap.com/docs/4.0/components/badge/
/// https://www.w3schools.com/html/html_tables.asp
/// https://stackoverflow.com/questions/13384474/simple-multiplication-with-jquery
/// </references>

namespace ST10320806Prog2.Controllers
{
    public class ClaimController : Controller
    {
        private readonly AppDbContext _context;

        public ClaimController(AppDbContext context)
        {
            _context = context;
        }
        //------------------------------------------------------------------------------------------------
        public IActionResult SubmitClaim()
        {
            return View();
        }
        //------------------------------------------------------------------------------------------------
        //Contains code for the submission of claims
        [HttpPost]
        public IActionResult SubmitClaim(LecturerTb lecturerClaim)
        {
            if (ModelState.IsValid)
            {
                // Saving
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

                // Validating claim
                if (claim.HoursWorked > ClaimValidation.MaxHoursWorked)
                {
                    claim.Status = "Rejected";
                    claim.ClaimNotes = "Hours worked exceed the maximum allowed limit.";
                }
                else if (claim.HourlyRate < ClaimValidation.MinHourlyRate || claim.HourlyRate > ClaimValidation.MaxHourlyRate)
                {
                    claim.Status = "Rejected";
                    claim.ClaimNotes = "Hourly rate is outside the allowable range.";
                }
                else
                {
                    claim.Status = "Approved";
                    claim.ClaimNotes = "Claim approved automatically.";
                }

                // Saving claim to the database
                _context.Claims.Add(claim);
                _context.SaveChanges();

                return RedirectToAction("ClaimSubmitted");
            }

            return View(lecturerClaim);
        }
        //------------------------------------------------------------------------------------------------
        public IActionResult ClaimSubmitted()
        {
            return View();
        }
        //------------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> VerifyClaim()//Contains code for the verification of claims
        {
            var pendingClaims = await _context.Claims
                .Where(c => c.Status == "Pending")
                .Include(c => c.Lecturer)
                .ToListAsync();
            return View(pendingClaims);
        }
        //------------------------------------------------------------------------------------------------
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
        //------------------------------------------------------------------------------------------------
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
        //------------------------------------------------------------------------------------------------
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
//------------------------------------------------------------------------------------------------//        
        public async Task<IActionResult> AutoApproveClaim(int id) //Contains code for the automatic approval of claims
        {
            var claim = await _context.Claims.Include(c => c.Lecturer).FirstOrDefaultAsync(c => c.ClaimId == id);

            if (claim == null)// error handling if no claim is found
            {
                return NotFound("Claim not found.");
            }

            // Validating the code nased off of the criteria in the ClaimValidation Model
            if (claim.HoursWorked > ClaimValidation.MaxHoursWorked)
            {
                claim.Status = "Rejected";
                claim.ClaimNotes = "Hours worked exceed the maximum allowed limit.";
            }
            else if (claim.HourlyRate < ClaimValidation.MinHourlyRate || claim.HourlyRate > ClaimValidation.MaxHourlyRate)
            {
                claim.Status = "Rejected";
                claim.ClaimNotes = "Hourly rate is outside the allowable range.";
            }
            else
            {
                claim.Status = "Approved";
                claim.ClaimNotes = "Claim approved automatically.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("VerifyClaim");
        }
//------------------------------------------------------------------------------------------------//
        public IActionResult HR()
        {
            return View();
        }
//------------------------------------------------------------------------------------------------//
        [HttpGet]
        public async Task<IActionResult> GenerateInvoice()//Code for the generation of an Invoice for HR
        {
            var approvedClaims = await _context.Claims
                .Where(c => c.Status == "Approved")// only getting data with "Approved" status
                .OrderByDescending(c => c.SubmissionDate)
                .ToListAsync();

            return View("HR",approvedClaims);
        }
    }
}


//----------------------------------------------End Of File--------------------------------------------\\