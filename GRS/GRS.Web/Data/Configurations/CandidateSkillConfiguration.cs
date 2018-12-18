using System;
using GRS.Web.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GRS.Web.Data.Configurations
{
    public class CandidateSkillConfiguration : IEntityTypeConfiguration<CandidateSkill>
    {
        public void Configure(EntityTypeBuilder<CandidateSkill> builder)
        {
            builder
               .HasKey(p => new { p.CandidateId, p.SkillId });

            builder.HasOne(x => x.Candidate)
                   .WithMany(x => x.CandidateSkills)
                   .HasForeignKey(x => x.CandidateId);

            builder.HasOne(x => x.Skill)
                   .WithMany(x => x.CandidateSkills)
                   .HasForeignKey(x => x.SkillId);
        }
    }
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder
            .Property(b => b.FirstName)
            .IsRequired();

            builder
            .Property(b => b.LastName)
            .IsRequired();
        }
    }
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder
            .Property(b => b.Name)
            .IsRequired();
        }
    }
}
