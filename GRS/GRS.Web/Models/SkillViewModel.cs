using System;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Display(Name = "Skill Name")]
        public string Name { get; set; }
    }
}