using System;
using GRS.Web.Data.Models;

namespace GRS.Web.Models
{
    public class SkillViewModel
    {
        public SkillViewModel()
        {
        }

        public SkillViewModel(Skill skill)
        {
            Id = skill.Id;
            Name = skill.Name;
        }

        public int? Id { get; set; }

        public string Name { get; set; }
    }
}