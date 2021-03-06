﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRS.Web.Common;
using GRS.Web.Data;
using GRS.Web.Data.Models;
using GRS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GRS.Web.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Candidate
        public async Task<IActionResult> Index()
        {
            var candidates = await _context.Candidates
                                        .OrderBy(x => x.LastName + ", " + x.FirstName)
                                        .Include(x => x.CandidateSkills)
                                            .ThenInclude(x => x.Skill)
                                        .ToListAsync();

            var viewModel = new SearchViewModel
            {
                Candidates = candidates.Select(x => new CandidateViewModel(x)).ToList()
            };
            await PrepareViewBagAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(SearchViewModel viewModel)
        {
            var candidates = await _context.Candidates
                                        .Where(x => viewModel.SkillId == null ||
                                             x.CandidateSkills.Any(y => y.SkillId == viewModel.SkillId))
                                        .OrderBy(x => x.LastName + ", " + x.FirstName)
                                        .Include(x => x.CandidateSkills)
                                            .ThenInclude(x => x.Skill)
                                        .ToListAsync();
            viewModel.Candidates = candidates.Select(x => new CandidateViewModel(x)).ToList();
            await PrepareViewBagAsync();
            return View(nameof(Index), viewModel);
        }

        // GET: Candidate/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await GetCandidateAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            var viewModel = new CandidateViewModel(model);
            return View(viewModel);
        }

        // GET: Candidate/Create
        public async Task<IActionResult> Create()
        {
            await PrepareViewBagAsync();
            return View();
        }

        // POST: Candidate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CandidateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await UpdateCandidateAsync(viewModel);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Candidate/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await GetCandidateAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            var viewModel = new CandidateViewModel(model);
            await PrepareViewBagAsync();
            return View(viewModel);
        }

        // POST: Candidate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CandidateViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateCandidateAsync(viewModel);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateExists(viewModel.Id))
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

            await PrepareViewBagAsync();
            return View(viewModel);
        }

        // GET: Candidate/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await GetCandidateAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.Candidates.Remove(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CandidateExists(int? id)
        {
            return _context.Candidates.Any(e => e.Id == id);
        }

        public async Task<Candidate> UpdateCandidateAsync(
        CandidateViewModel candidateViewModel
        )
        {
            var candidate = await GetCandidateAsync(candidateViewModel.Id) ?? new Candidate();

            candidate.FirstName = candidateViewModel.FirstName;
            candidate.LastName = candidateViewModel.LastName;

            _context.AddOrUpdate(candidate);

            _context.UpdateEntities(
                candidate.CandidateSkills,
                candidateViewModel.Skills
                    .Select(y => new CandidateSkill
                    {
                        Candidate = candidate,
                        SkillId = y
                    }).ToList(),
                 x => x.SkillId);

            return candidate;
        }

        private Task<Candidate> GetCandidateAsync(int? id)
        {
            return _context.Candidates
                            .Include(x => x.CandidateSkills)
                                .ThenInclude(x => x.Skill)
                            .SingleOrDefaultAsync(m => m.Id == id);
        }

        private async Task PrepareViewBagAsync()
        {
            ViewBag.AllSkills =
                        await _context.Skills
                            .OrderBy(x => x.Name)
                            .Select(x =>
                            new SelectListItem
                            {
                                Text = x.Name,
                                Value = x.Id.ToString()
                            })
                         .ToListAsync();
        }
    }
}
