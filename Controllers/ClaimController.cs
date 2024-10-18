using Microsoft.AspNetCore.Mvc;
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
        public IActionResult SubmitClaim(LecturerTb claim)
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Add(claim);
                _context.SaveChanges();
                return RedirectToAction("ClaimSubmitted");
            }
            return View(claim);
        }

        public IActionResult ClaimSubmitted()
        {
            return View();
        }
    }
}
