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
    public class TrainingController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly SelectList _statusList;
        private readonly SelectList _modesList;
        private readonly SelectList _departmentList;

        private readonly SelectList _projectList;


        public TrainingController(ApplicationDbContext context)
        {
            _context = context;
            _statusList = new SelectList(Enum.GetValues(typeof(Enums.Status)).Cast<Enums.Status>());
            _departmentList = new SelectList(Enum.GetValues(typeof(Enums.Department)).Cast<Enums.Department>());
            _modesList = new SelectList(Enum.GetValues(typeof(Enums.Modes)).Cast<Enums.Modes>());
            _projectList = new SelectList(_context.Projects, "Id", "Title");
        }

        // GET: Training
        public async Task<IActionResult> Index(string SearchString)
        {
            var applicationDbContext = _context.Trainings.Include(t => t.CreatedBy).Include(t => t.Project);
            
            // return View(await applicationDbContext.ToListAsync());
            
            if (_context.Trainings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Project'  is null.");
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                applicationDbContext = _context.Trainings.Where(s => s.Title!.Contains(SearchString)|| s.Project.Title!.Contains(SearchString)).Include(a => a.CreatedBy).Include(a => a.Project);
            }

            // if (!String.IsNullOrEmpty(SearchString))
            // {
            //     applicationDbContext = _context.Assessment.Where(s => s.Title!.Contains(SearchString) ||  s.Training!.Title.Contains(SearchString)).Include(a => a.CreatedBy).Include(a => a.Training);
            // }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Training/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Trainings == null)
            {
                return NotFound();
            }

            var trainings = await _context.Trainings
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainings == null)
            {
                return NotFound();
            }

            return View(trainings);
        }

        // GET: Training/Create
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["departmentList"] = _departmentList;
            ViewData["modesList"] = _modesList;
            ViewData["CreatedById"] = _currentUserID;
            ViewData["ProjectId"] = _projectList;
            return View();
        }

        // POST: Training/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ProjectId,Department,Mode,TrainerName,Status,Venue,ScheduledAt,CreatedAt,CreatedById")] Trainings trainings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            ViewData["statusList"] = _statusList;
            ViewData["departmentList"] = _departmentList;
            ViewData["modesList"] = _modesList;
            ViewData["CreatedById"] = _currentUserID;
            ViewData["ProjectId"] = _projectList;
            // ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", trainings.CreatedById);
            return View(trainings);
        }

        // GET: Training/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trainings == null)
            {
                return NotFound();
            }

            var trainings = await _context.Trainings.FindAsync(id);
            if (trainings == null)
            {
                return NotFound();
            }
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            ViewData["CreatedById"] = _currentUserID;
            
            ViewData["statusList"] = _statusList;
            ViewData["departmentList"] = _departmentList;
            ViewData["modesList"] = _modesList;
           
            ViewData["ProjectId"] = _projectList;
            // ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", trainings.CreatedById);
            return View(trainings);
        }

        // POST: Training/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ProjectId,Department,Mode,TrainerName,Status,Venue,ScheduledAt,CreatedAt,CreatedById")] Trainings trainings)
        {
            if (id != trainings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingsExists(trainings.Id))
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

            ViewData["CreatedById"] = _currentUserID;
            
            ViewData["statusList"] = _statusList;
            ViewData["departmentList"] = _departmentList;
            ViewData["modesList"] = _modesList;
            ViewData["ProjectId"] = _projectList;

            // ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", trainings.CreatedById);
            return View(trainings);
        }

        // GET: Training/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Trainings == null)
            {
                return NotFound();
            }

            var trainings = await _context.Trainings
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainings == null)
            {
                return NotFound();
            }

            return View(trainings);
        }

        // POST: Training/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Trainings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Trainings'  is null.");
            }
            var trainings = await _context.Trainings.FindAsync(id);
            if (trainings != null)
            {
                _context.Trainings.Remove(trainings);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingsExists(int id)
        {
          return (_context.Trainings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
