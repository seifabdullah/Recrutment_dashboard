using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recrutement_plateforme.Enum;
using Recrutement_plateforme.Models.OffresModels;
using Microsoft.AspNetCore.Authorization;

namespace Recrutement_plateforme.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly OffreDbContext _context;

        public CandidatesController(OffreDbContext context)
        {
            _context = context;
        }

        // GET: Candidates
        public async Task<IActionResult> Index()
        {
            return _context.Candidates != null
                ? View(await _context.Candidates.ToListAsync())
                : Problem("Entity set 'OffreDbContext.Candidates'  is null.");
        }

        // GET: Candidates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Candidates == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(m => m.Id == id);

            return candidate != null ? View(candidate) : NotFound();
        }

        // GET: Candidates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Candidates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Candidate candidate)
        {

            _context.Add(candidate);
            _context.SaveChanges(); // Save changes synchronously
            return RedirectToAction(nameof(Index));


            
        }

        // GET: Candidates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Candidates == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates.FindAsync(id);

            return candidate != null ? View(candidate) : NotFound();
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Candidates/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Prenom,Email,Password,Phone,LinkedInUrl")] Candidate candidate)
        {
            if (id != candidate.Id)
            {
                return NotFound();
            }

            _context.Update(candidate);
            _context.SaveChanges(); // Save changes 
            return RedirectToAction(nameof(Index));

        }

        // GET: Candidates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Candidates == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(m => m.Id == id);

            return candidate != null ? View(candidate) : NotFound();
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Candidates == null)
            {
                return Problem("Entity set 'OffreDbContext.Candidates'  is null.");
            }

            var candidate = await _context.Candidates.FindAsync(id);

            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Candidates/Login
        [HttpGet]
 
        public IActionResult Login()
        {
            return View();
        }



        // POST: Candidates/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Your authentication logic
            if (IsValidCandidate(email, password))
            {
                var candidate = _context.Candidates.FirstOrDefault(c => c.Email == email);

                if (candidate != null)
                {
                    // Set authentication cookie
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, candidate.Email),
                // Add more claims if needed
            };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        // Customize authentication properties if needed
                        IsPersistent = true, // This will make the cookie persistent
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Set a success message for the candidate
                    TempData["Message"] = "Welcome, " + candidate.Name + "!";

                    // Redirect to the Welcome action with candidateId as a route value
                    return RedirectToAction(nameof(Welcome), new { id = candidate.Id });
                }
            }

            // Authentication failed, return to the login page
            TempData["Message"] = "Invalid email or password";
            return RedirectToAction(nameof(Login));
        }



        [Authorize]
        [HttpGet]
        public IActionResult Welcome()
        {
            var candidate = GetCurrentCandidate();

            if (candidate != null)
            {
                return View(candidate);
            }

            // If the candidate is not found, handle the case accordingly
            return NotFound();
        }

        // GET: Candidates/Register
        [HttpGet]
        [Route("CandidateRegister")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Candidates/Register
        [HttpPost]
        [Route("CandidateRegister")]
        public async Task<IActionResult> Register([Bind("Name,Prenom,Email,Password,Phone,LinkedInUrl")] Candidate candidate)
        {


            _context.Add(candidate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
          
        }

        private bool IsValidCandidate(string email, string password)
        {
            // Simulate a simple check (replace with your actual authentication logic)
            var candidate = _context.Candidates.FirstOrDefault(c => c.Email == email && c.Password == password);
            return candidate != null;
        }
        private bool CandidateExists(int id)
        {
            return _context.Candidates.Any(e => e.Id == id);
        }

        [Authorize]
        public IActionResult Logout()
        {
            // Sign out the current user
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page
            return RedirectToAction(nameof(Login));
        }


        // GET: Candidates/ViewJobs
        [HttpGet]
        [Route("ViewJobs")]
        public IActionResult ViewJobs()
        {
            // Fetch all jobs from the database
            var allJobs = _context.Jobs.Include(j => j.Recruteur).ToList();

            // You can access candidate and its related jobs here
            ViewData["AllJobs"] = allJobs;
            ViewData["Message"] = TempData["Message"];

          
            return View();

        }

        // GET: Candidates/JobStatus

        [Authorize]
        [HttpGet]
        [Route("JobStatus")]
        public IActionResult JobStatus()
        {
            var currentCandidate = GetCurrentCandidate();

            if (currentCandidate == null)
            {
                // If the authentication check fails, redirect to the login page
                return RedirectToAction(nameof(Login));
            }

            // Fetch candidate and their applied jobs with status
            var candidate = _context.Candidates
                .Include(c => c.Applications)
                .ThenInclude(ca => ca.Job)
                .ThenInclude(j => j.Recruteur)
                .FirstOrDefault(c => c.Id == currentCandidate.Id);

            if (candidate == null)
            {
                
                return View("CandidateNotFound");
            }

            return View(candidate.Applications);
        }

        [Authorize]
        [HttpPost]
        [Route("Apply")]
        public IActionResult Apply(int jobId)
        {
            var currentCandidate = GetCurrentCandidate();

            if (currentCandidate != null)
            {
                var job = _context.Jobs.Find(jobId);

                if (job != null)
                {
                    // Check if the candidate has already applied for this job
                    var existingApplication = _context.CandidateJob
                        .FirstOrDefault(ca => ca.CandidateId == currentCandidate.Id && ca.JobId == job.Id);

                    if (existingApplication != null)
                    {
                        TempData["Message"] = "You have already applied for this job.";
                        return RedirectToAction(nameof(ViewJobs));
                    }
                    else
                    {
                        // Create a new CandidateJob entry
                        var application = new CandidateJob
                        {
                            CandidateId = currentCandidate.Id,
                            JobId = job.Id,
                            Status = (int)ApplicationStatus.Pending
                        };

                        _context.Add(application);
                        _context.SaveChanges();

                        // Log that the application is successful
                        Console.WriteLine($"Application successful for JobId: {job.Id} by CandidateId: {currentCandidate.Id}");
                    }

                    return RedirectToAction(nameof(ViewJobs));
                }
                else
                {
                    // Log that the job is not found
                    Console.WriteLine($"Job not found for JobId: {jobId}");
                }
            }
            else
            {
                // Log that the candidate is not found
                Console.WriteLine($"Candidate not found");
            }

            // If the authentication check fails, redirect to the login page
            return RedirectToAction(nameof(Login));
        }

      



  

        // Helper method to get the current candidate from the authentication context
        private Candidate GetCurrentCandidate()
        {
            var candidateEmail = User?.Identity?.Name;

            // Ensure that the email is not null or empty
            if (!string.IsNullOrEmpty(candidateEmail))
            {
                // Retrieve the candidate based on the email
                var candidate = _context.Candidates.FirstOrDefault(c => c.Email == candidateEmail);

                return candidate;
            }

            return null;
        }



        [Authorize]
        [HttpPost]
        [Route("CancelApplication")]
        public IActionResult CancelApplication(int jobId)
        {
            try
            {
                var currentCandidate = GetCurrentCandidate();

                if (currentCandidate != null)
                {
                    var job = _context.Jobs.Find(jobId);

                    if (job != null)
                    {
                        // Find the application
                        var application = _context.CandidateJob
                            .FirstOrDefault(ca => ca.CandidateId == currentCandidate.Id && ca.JobId == job.Id);

                        if (application != null)
                        {
                            // Remove the application
                            _context.CandidateJob.Remove(application);
                            _context.SaveChanges();

                            // Log that the cancellation is successful
                            Console.WriteLine($"Cancellation successful for JobId: {job.Id} by CandidateId: {currentCandidate.Id}");

                            // Redirect to the JobStatus action with a success message
                            TempData["Message"] = "Application canceled successfully.";
                            return RedirectToAction(nameof(JobStatus));
                        }
                        else
                        {
                            // Log that the application is not found
                            Console.WriteLine($"Application not found for JobId: {jobId} by CandidateId: {currentCandidate.Id}");
                        }
                    }
                    else
                    {
                        // Log that the job is not found
                        Console.WriteLine($"Job not found for JobId: {jobId}");
                    }
                }
                else
                {
                    // Log that the candidate is not found
                    Console.WriteLine($"Candidate not found");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");

                // Redirect to the JobStatus action with an error message
                TempData["Message"] = "Failed to cancel the application. An unexpected error occurred. Please try again.";
                return RedirectToAction(nameof(JobStatus));
            }

            // If the cancellation fails, redirect to the JobStatus action with a generic error message
            TempData["Message"] = "Failed to cancel the application. Please try again.";
            return RedirectToAction(nameof(JobStatus));
        }




        // GET: Candidates/CompleteProfile
        [Authorize]
        [HttpGet]
        public IActionResult CompleteProfile()
        {
            var currentCandidate = GetCurrentCandidate();

            if (currentCandidate != null)
            {
                // Check if the candidate already has a complete profile
                if (currentCandidate.CompleteProfile == null)
                {
                    // If not, create a new CompleteProfile
                    var completeProfile = new CompleteProfile
                    {
                        CandidateId = currentCandidate.Id
                    };

                    return View(completeProfile);
                }

                // Candidate already has a complete profile, redirect or handle accordingly
                return RedirectToAction(nameof(Welcome));
            }

            // If the authentication check fails, redirect to the login page
            return RedirectToAction(nameof(Login));
        }

        // GET: Candidates/ViewProfile
        [Authorize]
        [HttpGet]
        public IActionResult ViewProfile()
        {
            var currentCandidate = GetCurrentCandidate();

            if (currentCandidate != null)
            {
                // Retrieve the candidate's complete profile
                var candidateWithProfile = _context.Candidates
                    .Include(c => c.CompleteProfile)
                    .FirstOrDefault(c => c.Id == currentCandidate.Id);

                if (candidateWithProfile != null)
                {
                    return View(candidateWithProfile);
                }
                else
                {
                    // Handle the case where the candidate or complete profile is not found
                    return View("CandidateNotFound");
                }
            }

            // If the authentication check fails, redirect to the login page
            return RedirectToAction(nameof(Login));
        }




        // POST: Candidates/CompleteProfile
        [Authorize]
        [HttpPost]
        public IActionResult CompleteProfile(CompleteProfile completeProfile)
        {
            var currentCandidate = GetCurrentCandidate();

            if (currentCandidate != null)
            {
                try
                {
                    // Check if the candidate already has a complete profile
                    if (currentCandidate.CompleteProfile == null)
                    {
                        // If not, set the CandidateId and add the new CompleteProfile
                        completeProfile.CandidateId = currentCandidate.Id;

                        // Check if a profile with the same CandidateId already exists
                        var existingProfile = _context.Profile
                            .FirstOrDefault(cp => cp.CandidateId == currentCandidate.Id);

                        if (existingProfile == null)
                        {
                            _context.Add(completeProfile);
                            _context.SaveChanges();

                            // Redirect to the welcome page or handle accordingly
                            return RedirectToAction(nameof(Welcome));
                        }
                        else
                        {
                            // If a profile with the same CandidateId exists, update it
                            existingProfile.Experience = completeProfile.Experience;
                            existingProfile.Education = completeProfile.Education;
                            // ... Update other properties as needed
                            _context.SaveChanges();

                            // Redirect to the welcome page or handle accordingly
                            return RedirectToAction(nameof(Welcome));
                        }
                    }
                    else
                    {
                        // If the candidate already has a complete profile, you might want to handle it accordingly
                        // For example, redirect to a different page or show a message
                        TempData["Message"] = "You have already completed your profile.";
                        return RedirectToAction(nameof(Welcome));
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception details for debugging
                    Console.WriteLine($"Error saving changes to the database: {ex}");

                    // You might want to inspect the inner exception for more details
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }

                    throw; // Rethrow the exception to maintain the original behavior
                }
            }

            // If the authentication check fails, redirect to the login page
            return RedirectToAction(nameof(Login));
        }
        // GET: Candidates/ViewJob/5
        [HttpGet]
        public IActionResult ViewJob(int jobId)
        {
            var job = _context.Jobs
                .Include(j => j.Recruteur)
                .FirstOrDefault(j => j.Id == jobId);

            if (job != null)
            {
                return View(job);
            }

            return NotFound();
        }
        // ... (existing code)

        // GET: Candidates/UpdateProfile
        [Authorize]
        [HttpGet]
        public IActionResult UpdateProfile()
        {
            var currentCandidate = GetCurrentCandidate();

            if (currentCandidate != null)
            {
                return View(currentCandidate.CompleteProfile);
            }

            // If the authentication check fails, redirect to the login page
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpdateProfile([FromForm] Candidate updatedCandidate)
        {
            // Check if ModelState is valid
            if (!ModelState.IsValid)
            {
                // If validation fails, return to the view with validation errors
                return View("UpdateProfile", updatedCandidate);
            }

            var currentCandidate = _context.Candidates.Find(updatedCandidate.Id);

            // Check if the candidate with the specified ID is found
            if (currentCandidate != null)
            {
                // Ensure the email is not updated
                updatedCandidate.Email = currentCandidate.Email;

                // Update candidate attributes
                currentCandidate.Name = updatedCandidate.Name;
                currentCandidate.Prenom = updatedCandidate.Prenom;
                currentCandidate.Phone = updatedCandidate.Phone;
                currentCandidate.LinkedInUrl = updatedCandidate.LinkedInUrl;

                // Save changes to the database
                _context.Update(currentCandidate);
                _context.SaveChanges();

                // Redirect to the view profile page or another appropriate action
                return RedirectToAction(nameof(ViewProfile));
            }

            // If the candidate with the specified ID is not found, handle accordingly
            return NotFound();
        }





    }
}




