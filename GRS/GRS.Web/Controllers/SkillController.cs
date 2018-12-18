using System;
using System.Linq;
using System.Threading.Tasks;
using GRS.Web.Common;
using GRS.Web.Data;
using GRS.Web.Data.Models;
using GRS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GRS.Web.Controllers
{
    public class SkillController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SkillController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Skill
        public async Task<IActionResult> Index()
        {
            var skills = await _context.Skills
                                    .OrderBy(x => x.Name)
                                    .ToListAsync();
            var viewModel = skills.Select(x => new SkillViewModel(x)).ToList();
            return View(viewModel);
        }

        // GET: Skill/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await GetSkillAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            var viewModel = new SkillViewModel(model);
            return View(viewModel);
        }

        // GET: Skill/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Skill/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SkillViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await UpdateSkillAsync(_context, viewModel);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Skill/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await GetSkillAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            var viewModel = new SkillViewModel(model);
            return View(viewModel);
        }

        // POST: Skill/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SkillViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateSkillAsync(_context, viewModel);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkillExists(viewModel.Id))
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

            return View(viewModel);
        }

        // GET: Skill/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await GetSkillAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.Skills.Remove(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool SkillExists(int? id)
        {
            return _context.Skills.Any(e => e.Id == id);
        }

        public async Task<Skill> UpdateSkillAsync(
        ApplicationDbContext context,
        SkillViewModel skillViewModel
        )
        {
            var skill = (await context.Skills.FindAsync(skillViewModel?.Id)) ?? new Skill();

            skill.Name = skillViewModel.Name;

            context.AddOrUpdate(skill);

            return skill;
        }

        private Task<Skill> GetSkillAsync(int? id)
        {
            return _context.Skills
                            .SingleOrDefaultAsync(m => m.Id == id);
        }
    }
}
