using System.Collections.Generic;

namespace GRS.Web.Data.Models
{
    public class Candidate
    {
        public Candidate()
        {
            CandidateSkills = new List<CandidateSkill>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual List<CandidateSkill> CandidateSkills { get; private set; }
    }
}
