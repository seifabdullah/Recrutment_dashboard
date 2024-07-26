using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recrutement_plateforme.Models.OffresModels;

namespace Recrutement_plateforme.Controllers
{
    public class JobsController : Controller
    {
        private readonly OffreDbContext _context;

        public JobsController(OffreDbContext context)
        {
            _context = context;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            var offreDbContext = _context.Jobs.Include(j => j.Recruteur);
            return View(await offreDbContext.ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Recruteur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            ViewData["RecruteurId"] = new SelectList(_context.Recruteurs, "Id", "CompanyName");
            return View();
        }

        // POST: Jobs/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CompetencesRequired,Responsibilities,Salary,PublishedAt,IsAccepted,RecruteurId")] Job job)
        {

            _context.Add(job);
            _context.SaveChanges();

            // Display a toast notification
            TempData["ToastMessage"] = "Job created successfully!";
            TempData["ToastType"] = "success";

            return RedirectToAction(nameof(Index));
            ViewData["RecruteurId"] = new SelectList(_context.Recruteurs, "Id", "CompanyName", job.RecruteurId);
            return View(job);
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["RecruteurId"] = new SelectList(_context.Recruteurs, "Id", "CompanyName", job.RecruteurId);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CompetencesRequired,Responsibilities,Salary,PublishedAt,IsAccepted,RecruteurId")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            
                    _context.Update(job);
                    await _context.SaveChangesAsync();
             
                return RedirectToAction(nameof(Index));
            
            ViewData["RecruteurId"] = new SelectList(_context.Recruteurs, "Id", "CompanyName", job.RecruteurId);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Recruteur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Jobs == null)
            {
                return Problem("Entity set 'OffreDbContext.Jobs'  is null.");
            }
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
          return (_context.Jobs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
