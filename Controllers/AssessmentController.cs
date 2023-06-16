using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NeutronTMS.Data;
using NeutronTMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace NeutronTMS.Controllers
{
    public class AssessmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SelectList _statusList;
        // private readonly SelectList _trainingList;
        public AssessmentController(ApplicationDbContext context)
        {
            _context = context;
            _statusList = new SelectList(Enum.GetValues(typeof(Enums.Status)).Cast<Enums.Status>());
            // _trainingList = new SelectList(_context.Trainings, "Id", "Title");
        }

        // GET: Assessment
        public async Task<IActionResult> Index(string SearchString)
        {
            var applicationDbContext = _context.Assessment.Include(a => a.CreatedBy).Include(a => a.Training);
            // return View(await applicationDbContext.ToListAsync());
            
            if (_context.Assessment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Project'  is null.");
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                applicationDbContext = _context.Assessment.Where(s => s.Title!.Contains(SearchString) ||  s.Training!.Title.Contains(SearchString)).Include(a => a.CreatedBy).Include(a => a.Training);
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Assessment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Assessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.Assessment
                .Include(a => a.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessment == null)
            {
                return NotFound();
            }

            return View(assessment);
        }

        // GET: Assessment/Create
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
             ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["CreatedById"] = _currentUserID;
            // ViewData["TrainingId"] = _trainingList;
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Title");

            return View();

        }

        // POST: Assessment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,TrainingId,Description,DocumentLink,SubmissionLink,Status,StartDate,EndDate,CreatedById")] Assessment assessment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assessment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["CreatedById"] = _currentUserID;
            // ViewData["TrainingId"] = _trainingList;
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Title",assessment.TrainingId);
            return View(assessment);

        }

        // GET: Assessment/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Assessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.Assessment.FindAsync(id);
            if (assessment == null)
            {
                return NotFound();
            }
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["CreatedById"] = _currentUserID;
             ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Title",assessment.TrainingId);
            // ViewData["TrainingId"] = _trainingList;

            return View(assessment);
        }

        // POST: Assessment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,TrainingId,Description,DocumentLink,SubmissionLink,Status,StartDate,EndDate,CreatedById")] Assessment assessment)
        {
            if (id != assessment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assessment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssessmentExists(assessment.Id))
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
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["CreatedById"] = _currentUserID;
            // ViewData["TrainingId"] = _trainingList;
             ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Title",assessment.TrainingId);
            return View(assessment);
        }

        // GET: Assessment/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Assessment == null)
            {
                return NotFound();
            }

            var assessment = await _context.Assessment
                .Include(a => a.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessment == null)
            {
                return NotFound();
            }

            return View(assessment);
        }

        // POST: Assessment/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Assessment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Assessment'  is null.");
            }
            var assessment = await _context.Assessment.FindAsync(id);
            if (assessment != null)
            {
                _context.Assessment.Remove(assessment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssessmentExists(int id)
        {
            return (_context.Assessment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
