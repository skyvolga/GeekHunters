namespace GRS.Web.Data.Models
{
    public class CandidateSkill
    {
        public int CandidateId { get; set; }

        public virtual Candidate Candidate { get; set; }

        public int SkillId { get; set; }

        public virtual Skill Skill { get; set; }
    }
}
