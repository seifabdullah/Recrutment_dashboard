using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recrutement_plateforme.Models.OffresModels;

namespace Recrutement_plateforme.Controllers
{
    public class CompleteProfilesController : Controller
    {
        private readonly OffreDbContext _context;

        public CompleteProfilesController(OffreDbContext context)
        {
            _context = context;
        }

        // GET: CompleteProfiles
        public async Task<IActionResult> Index()
        {
            var offreDbContext = _context.Profile.Include(c => c.Candidate);
            return View(await offreDbContext.ToListAsync());
        }

        // GET: CompleteProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Profile == null)
            {
                return NotFound();
            }

            var completeProfile = await _context.Profile
                .Include(c => c.Candidate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (completeProfile == null)
            {
                return NotFound();
            }

            return View(completeProfile);
        }

        // GET: CompleteProfiles/Create
        public IActionResult Create()
        {
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "Id", "Email");
            return View();
        }

        // POST: CompleteProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateId,Id,Experience,Education,Skills,Competencies,Languages,Hobbies")] CompleteProfile completeProfile)
        {
          
                _context.Add(completeProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        }

        // GET: CompleteProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Profile == null)
            {
                return NotFound();
            }

            var completeProfile = await _context.Profile.FindAsync(id);
            if (completeProfile == null)
            {
                return NotFound();
            }
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "Id", "Email", completeProfile.CandidateId);
            return View(completeProfile);
        }

        // POST: CompleteProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidateId,Id,Experience,Education,Skills,Competencies,Languages,Hobbies")] CompleteProfile completeProfile)
        {
            if (id != completeProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(completeProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompleteProfileExists(completeProfile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "Id", "Email", completeProfile.CandidateId);
            return View(completeProfile);
        }

        // GET: CompleteProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profile == null)
            {
                return NotFound();
            }

            var completeProfile = await _context.Profile
                .Include(c => c.Candidate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (completeProfile == null)
            {
                return NotFound();
            }

            return View(completeProfile);
        }

        // POST: CompleteProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profile == null)
            {
                return Problem("Entity set 'OffreDbContext.Profile'  is null.");
            }
            var completeProfile = await _context.Profile.FindAsync(id);
            if (completeProfile != null)
            {
                _context.Profile.Remove(completeProfile);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompleteProfileExists(int id)
        {
          return (_context.Profile?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
