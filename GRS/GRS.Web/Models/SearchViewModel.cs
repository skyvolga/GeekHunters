using System;
using System.Collections.Generic;

namespace GRS.Web.Models
{
    public class SearchViewModel
    {
        public int? SkillId { get; set; }

        public List<CandidateViewModel> Candidates { get; set; }
    }
}
