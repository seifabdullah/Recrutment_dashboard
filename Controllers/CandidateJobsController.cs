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
    public class CandidateJobsController : Controller
    {
        private readonly OffreDbContext _context;

        public CandidateJobsController(OffreDbContext context)
        {
            _context = context;
        }

        // GET: CandidateJobs
        public async Task<IActionResult> Index()
        {
            var offreDbContext = _context.CandidateJob.Include(c => c.Candidate).Include(c => c.Job);
            return View(await offreDbContext.ToListAsync());
        }

        // GET: CandidateJobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CandidateJob == null)
            {
                return NotFound();
            }

            var candidateJob = await _context.CandidateJob
                .Include(c => c.Candidate)
                .Include(c => c.Job)
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidateJob == null)
            {
                return NotFound();
            }

            return View(candidateJob);
        }

        // GET: CandidateJobs/Create
        public IActionResult Create()
        {
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "Id", "Email");
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "CompetencesRequired");
            return View();
        }

        // POST: CandidateJobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateId,JobId,Status")] CandidateJob candidateJob)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candidateJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "Id", "Email", candidateJob.CandidateId);
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "CompetencesRequired", candidateJob.JobId);
            return View(candidateJob);
        }

        // GET: CandidateJobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CandidateJob == null)
            {
                return NotFound();
            }

            var candidateJob = await _context.CandidateJob.FindAsync(id);
            if (candidateJob == null)
            {
                return NotFound();
            }
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "Id", "Email", candidateJob.CandidateId);
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "CompetencesRequired", candidateJob.JobId);
            return View(candidateJob);
        }

        // POST: CandidateJobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidateId,JobId,Status")] CandidateJob candidateJob)
        {
            if (id != candidateJob.CandidateId)
            {
                return NotFound();
            }

                         
                
                    _context.Update(candidateJob);
                    await _context.SaveChangesAsync();
                
             
                return RedirectToAction(nameof(Index));
            
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "Id", "Email", candidateJob.CandidateId);
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "CompetencesRequired", candidateJob.JobId);
            return View(candidateJob);
        }

        // GET: CandidateJobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CandidateJob == null)
            {
                return NotFound();
            }

            var candidateJob = await _context.CandidateJob
                .Include(c => c.Candidate)
                .Include(c => c.Job)
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidateJob == null)
            {
                return NotFound();
            }

            return View(candidateJob);
        }

        // POST: CandidateJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CandidateJob == null)
            {
                return Problem("Entity set 'OffreDbContext.CandidateJob'  is null.");
            }
            var candidateJob = await _context.CandidateJob.FindAsync(id);
            if (candidateJob != null)
            {
                _context.CandidateJob.Remove(candidateJob);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateJobExists(int id)
        {
          return (_context.CandidateJob?.Any(e => e.CandidateId == id)).GetValueOrDefault();
        }
    }
}
