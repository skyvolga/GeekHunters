using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GRS.Web.Data;
using GRS.Web.Data.Models;

namespace GRS.Web.Models
{
    public class CandidateViewModel
    {
        public CandidateViewModel()
        {
            Skills = new List<int>();
        }

        public CandidateViewModel(Candidate candidate)
        {
            Id = candidate.Id;
            FirstName = candidate.FirstName;
            LastName = candidate.LastName;
            Skills = candidate.CandidateSkills
                        .Select(x => x.SkillId)
                        .ToList();

            JoinedSkillNames = string.Join(", ", candidate.CandidateSkills
                            .Select(x => x.Skill.Name));
        }

        public int? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<int> Skills { get; set; }

        [Display(Name ="Skills")]

        public string JoinedSkillNames { get; set; }
    }
}