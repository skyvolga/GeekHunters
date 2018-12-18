using System.Collections.Generic;

namespace GRS.Web.Data.Models
{
    public class Skill
    {
        public Skill()
        {
            CandidateSkills = new List<CandidateSkill>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<CandidateSkill> CandidateSkills { get; private set; }
    }
}
