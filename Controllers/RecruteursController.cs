using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recrutement_plateforme.Models.OffresModels;
using Microsoft.AspNetCore.Authorization;
using Recrutement_plateforme.Enum;

namespace Recrutement_plateforme.Controllers
{
  
    public class RecruteursController : Controller
    {
        private readonly OffreDbContext _context;

        public RecruteursController(OffreDbContext context)
        {
            _context = context;
        }

        // GET: Recruteurs
        public async Task<IActionResult> Index()
        {
              return _context.Recruteurs != null ? 
                          View(await _context.Recruteurs.ToListAsync()) :
                          Problem("Entity set 'OffreDbContext.Recruteurs'  is null.");
        }

        // GET: Recruteurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recruteurs == null)
            {
                return NotFound();
            }

            var recruteur = await _context.Recruteurs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recruteur == null)
            {
                return NotFound();
            }

            return View(recruteur);
        }

        // GET: Recruteurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recruteurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyName")] Recruteur recruteur)
        {
          
            
                _context.Add(recruteur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            
        }

        // GET: Recruteurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recruteurs == null)
            {
                return NotFound();
            }

            var recruteur = await _context.Recruteurs.FindAsync(id);
            if (recruteur == null)
            {
                return NotFound();
            }
            return View(recruteur);
        }

        // POST: Recruteurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,Email")] Recruteur recruteur)
        {
            if (id != recruteur.Id)
            {
                return NotFound();
            }

            // Retrieve the existing recruteur from the database
            var existingRecruteur = await _context.Recruteurs.FindAsync(id);

            if (existingRecruteur == null)
            {
                return NotFound();
            }

            // Update the properties of the existing recruteur with the new values
            existingRecruteur.CompanyName = recruteur.CompanyName;

            // Check if the provided email is not null or empty before updating
            if (!string.IsNullOrWhiteSpace(recruteur.Email))
            {
                existingRecruteur.Email = recruteur.Email;
            }

            // Save the changes to the database
            _context.Update(existingRecruteur);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Recruteurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recruteurs == null)
            {
                return NotFound();
            }

            var recruteur = await _context.Recruteurs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recruteur == null)
            {
                return NotFound();
            }

            return View(recruteur);
        }

        // POST: Recruteurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recruteurs == null)
            {
                return Problem("Entity set 'OffreDbContext.Recruteurs'  is null.");
            }
            var recruteur = await _context.Recruteurs.FindAsync(id);
            if (recruteur != null)
            {
                _context.Recruteurs.Remove(recruteur);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecruteurExists(int id)
        {
          return (_context.Recruteurs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: Recruteurs/Login
        [HttpGet]
        [Route("RecruteurLogin")]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Recruteurs/Login
        [HttpPost]
        [Route("RecruteurLogin")]
        public IActionResult Login(string email, string password)
        {
            // Your authentication logic
            if (IsValidRecruteur(email, password))
            {
                // Get the recruteur entity
                var recruteur = _context.Recruteurs.FirstOrDefault(r => r.Email == email);

                // Set the company name as a claim for the user
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, recruteur.Email),
            new Claim("CompanyName", recruteur.CompanyName)
        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                TempData["Message"] = "Login successful!";
                return RedirectToAction(nameof(Welcome));
            }

            TempData["Message"] = "Invalid email or password";
            return View(nameof(Login));
        }



        // GET: Recruteurs/Welcome
        [HttpGet]
        [Route("WelcomeRec")]
        public IActionResult Welcome()
        {

            return View();

        }

        // GET: Recruteur/Register
        [HttpGet]
        [Route("RecruteurRegister")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Recruteur/Register
        [HttpPost]
        [Route("RecruteurRegister")]
        public async Task<IActionResult> Register([Bind("CompanyName,Name,Prenom,Email,Password")] Recruteur recruteur)
        {


            _context.Add(recruteur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
          
        }
        // User Validation
        private bool IsValidRecruteur(string email, string password)
        {
            // Find a recruiter with the provided email
            var recruiter = _context.Recruteurs.FirstOrDefault(r => r.Email == email);

            // Check if the recruiter exists and the password matches
            return recruiter != null && recruiter.Password == password;
        }

        // GET: Recruteurs/Logout
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            // Perform any necessary logout logic (e.g., clear authentication tokens, session data)

            // Redirect to the login page
            return RedirectToAction(nameof(Login));
        }
        // GET: Recruteurs/ViewJobs
        [Authorize]
        [HttpGet]
        [Route("Recruteurs/ViewJobs")]
        public IActionResult ViewJobs()
        {

            // Get the email of the currently logged-in recruteur
            string? recruteurEmail = User.Identity.Name;

            // Find the recruteur based on the email
            var recruteur = _context.Recruteurs.FirstOrDefault(r => r.Email == recruteurEmail);

            if (recruteur != null)
            {
                // Fetch jobs for the recruteur's company
                var companyJobs = _context.Jobs
                    .Where(j => j.Recruteur.CompanyName == recruteur.CompanyName)
                    .Include(j => j.Recruteur)
                    .ToList();

                return View(companyJobs);
            }

            // If recruteur is not found, you can handle the case accordingly
            return NotFound();
        }

        [Authorize]
        [HttpGet]
        [Route("ViewJobApplications")]
        public IActionResult ViewJobApplications()
        {
            var recruteur = GetCurrentRecruteur();

            if (recruteur != null)
            {
                // Fetch job applications related to Recruteur
                var jobApplications = _context.CandidateJobs
                    .Where(ca => ca.Job.RecruteurId == recruteur.Id)
                    .Include(ca => ca.Candidate)
                    .Include(ca => ca.Job)
                    .ToList();

                return View(jobApplications);
            }

            // If the authentication check fails, redirect to the login page
            return RedirectToAction(nameof(Login));
        }

        // RecruteursController.cs

        [Authorize]
        [HttpPost]
        [Route("Recruteurs/ChangeApplicationStatus")]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeApplicationStatus([FromBody] List<ApplicationChangeRequest> changeRequests)
        {
            try
            {
                // Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    // Return a BadRequest response with model state errors
                    return BadRequest(ModelState);
                }

                var currentRecruteur = GetCurrentRecruteur();

                if (currentRecruteur != null)
                {
                    var candidateIds = changeRequests.Select(request => request.CandidateId).ToList();

                    // Update application statuses in the database
                    var applicationsToUpdate = _context.CandidateJobs
                        .Where(ca => candidateIds.Contains(ca.CandidateId) && ca.Job.RecruteurId == currentRecruteur.Id)
                        .ToList();

                    foreach (var application in applicationsToUpdate)
                    {
                        var changeRequest = changeRequests.FirstOrDefault(request => request.CandidateId == application.CandidateId);
                        if (changeRequest != null)
                        {
                            application.Status = changeRequest.Status;
                        }
                    }

                    _context.SaveChanges();

                    return Ok(new { message = "Application statuses changed successfully." });
                }

                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }


        public class ApplicationChangeRequest
        {
            public int CandidateId { get; set; }
            public ApplicationStatus Status { get; set; }
        }




        private Recruteur? GetCurrentRecruteur()
        {
            var recruteurEmail = User?.Identity?.Name;

            return !string.IsNullOrEmpty(recruteurEmail)
                ? _context.Recruteurs.FirstOrDefault(r => r.Email == recruteurEmail)
                : null;
        }




        // Add this action in your Recruteurs controller
        public IActionResult ViewCandidateDetails(int candidateId)
        {
            // Retrieve candidate details from the database based on the candidateId
            var candidate = _context.Candidates
        .Include(c => c.CompleteProfile)  // Eager loading of CompleteProfile
        .FirstOrDefault(c => c.Id == candidateId);

            if (candidate == null)
            {
                // If candidate is not found, handle accordingly (e.g., return NotFound())
                return NotFound();
            }

            // Pass the candidate details to the view
            return View("ViewCandidateDetails", candidate);
        }

    }
}
