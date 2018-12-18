using System;
using System.Collections.Generic;
using System.Linq;
using GRS.Web.Data;
using GRS.Web.Data.Models;

namespace GRS.Web.Models
{
    public class CandidateViewModel
    {
        public CandidateViewModel()
        {
            Skills = new List<SkillViewModel>();
        }

        public CandidateViewModel(Candidate candidate)
        {
            Id = candidate.Id;
            FirstName = candidate.FirstName;
            LastName = candidate.LastName;
            Skills = candidate.CandidateSkills
            .Select(x => new SkillViewModel(x.Skill))
                .ToList();
        }

        public int? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<SkillViewModel> Skills { get; set; }
    }
}