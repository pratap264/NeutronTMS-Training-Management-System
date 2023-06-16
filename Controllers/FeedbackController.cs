using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NeutronTMS.Data;
using NeutronTMS.Models;

namespace NeutronTMS.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SelectList _statusList;
        // private readonly SelectList _trainingList;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
            _statusList = new SelectList(Enum.GetValues(typeof(Enums.Status)).Cast<Enums.Status>());
            // _trainingList = new SelectList(_context.Trainings, "Id", "Title");
        }

        // GET: Feedback
        public async Task<IActionResult> Index(string SearchString)
        {
            var applicationDbContext = _context.Feedback.Include(f => f.CreatedBy).Include(f => f.Training);
            if (_context.Assessment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Project'  is null.");
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                applicationDbContext = _context.Feedback.Where(s => s.Title!.Contains(SearchString) ||  s.Training!.Title.Contains(SearchString)).Include(a => a.CreatedBy).Include(a => a.Training);
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Feedback/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Feedback == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback
                .Include(f => f.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedback/Create
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            // ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["CreatedById"] = _currentUserID;
            // ViewData["TrainingId"] = _trainingList;
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Title");
            return View();
        }

        // POST: Feedback/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,TrainingId,FeedbackLink,Status,EndDate,CreatedById")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", feedback.CreatedById);
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["CreatedById"] = _currentUserID;
            // ViewData["TrainingId"] = _trainingList;
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Title",feedback.TrainingId);
            return View(feedback);
        }

        // GET: Feedback/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Feedback == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            // ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", feedback.CreatedById);
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["CreatedById"] = _currentUserID;
            // ViewData["TrainingId"] = _trainingList;
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Title",feedback.TrainingId);
            return View(feedback);
        }

        // POST: Feedback/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,TrainingId,FeedbackLink,Status,EndDate,CreatedById")] Feedback feedback)
        {
            if (id != feedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.Id))
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
            // ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", feedback.CreatedById);
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["CreatedById"] = _currentUserID;
            // ViewData["TrainingId"] = _trainingList;
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Title",feedback.TrainingId);
            return View(feedback);
        }

        // GET: Feedback/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Feedback == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedback
                .Include(f => f.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedback/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Feedback == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Feedback'  is null.");
            }
            var feedback = await _context.Feedback.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedback.Remove(feedback);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(int id)
        {
            return (_context.Feedback?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
