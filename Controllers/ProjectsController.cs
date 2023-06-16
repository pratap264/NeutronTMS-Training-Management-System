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
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SelectList _statusList;
        private readonly SelectList _modesList;
        private readonly SelectList _departmentList;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
            _statusList = new SelectList(Enum.GetValues(typeof(Enums.Status)).Cast<Enums.Status>());
            _departmentList = new SelectList(Enum.GetValues(typeof(Enums.Department)).Cast<Enums.Department>());
            _modesList = new SelectList(Enum.GetValues(typeof(Enums.Modes)).Cast<Enums.Modes>());
        }

        // GET: Projects
        public async Task<IActionResult> Index(string SearchString)
        {
            var applicationDbContext = _context.Projects.Include(p => p.CreatedBy);


            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Project'  is null.");
            }
            if (!String.IsNullOrEmpty(SearchString))
            {
                applicationDbContext = _context.Projects.Where(s => s.Title!.Contains(SearchString) || s.Department!.Contains(SearchString)).Include(p => p.CreatedBy);
            }

            return View(await applicationDbContext.ToListAsync());
        }



        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects
                .Include(p => p.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projects == null)
            {
                return NotFound();
            }

            return View(projects);
        }

        // GET: Projects/Create
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            // ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["departmentList"] = _departmentList;
            ViewData["modesList"] = _modesList;
            ViewData["CreatedById"] = _currentUserID;
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Department,Mode,Status,StartDate,EndDate,CreatedById")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projects);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["departmentList"] = _departmentList;
            ViewData["modesList"] = _modesList;
            ViewData["CreatedById"] = _currentUserID;
            // ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", projects.CreatedById);
            return View(projects);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects.FindAsync(id);
            if (projects == null)
            {
                return NotFound();
            }
            // ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", projects.CreatedById);
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["departmentList"] = _departmentList;
            ViewData["modesList"] = _modesList;
            ViewData["CreatedById"] = _currentUserID;
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Department,Mode,Status,StartDate,EndDate,CreatedById")] Projects projects)
        {
            if (id != projects.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projects);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectsExists(projects.Id))
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
            // ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", projects.CreatedById);
            ClaimsPrincipal currentUser = this.User;
            var _currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["statusList"] = _statusList;
            ViewData["departmentList"] = _departmentList;
            ViewData["modesList"] = _modesList;
            ViewData["CreatedById"] = _currentUserID;
            return View(projects);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects
                .Include(p => p.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projects == null)
            {
                return NotFound();
            }

            return View(projects);
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
            }
            var projects = await _context.Projects.FindAsync(id);
            if (projects != null)
            {
                _context.Projects.Remove(projects);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectsExists(int id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
