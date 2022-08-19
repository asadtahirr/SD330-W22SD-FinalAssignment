using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using stack_overload.Models;

namespace stack_overload.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Answer>()
                .HasOne<User>(e => e.CreatedBy)
                .WithMany(e => e.Answers)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity("AnswerUser", b =>
            {
                b.HasOne("stack_overload.Models.Answer", null)
                    .WithMany()
                    .HasForeignKey("DownvotedAnswersId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("stack_overload.Models.User", null)
                    .WithMany()
                    .HasForeignKey("DownvotersId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });

            builder.Entity("AnswerUser1", b =>
            {
                b.HasOne("stack_overload.Models.Answer", null)
                    .WithMany()
                    .HasForeignKey("UpvotedAnswersId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("stack_overload.Models.User", null)
                    .WithMany()
                    .HasForeignKey("UpvotersId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });

            builder.Entity("QuestionUser", b =>
            {
                b.HasOne("stack_overload.Models.Question", null)
                    .WithMany()
                    .HasForeignKey("DownvotedQuestionsId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("stack_overload.Models.User", null)
                    .WithMany()
                    .HasForeignKey("DownvotersId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });

            builder.Entity("QuestionUser1", b =>
            {
                b.HasOne("stack_overload.Models.Question", null)
                    .WithMany()
                    .HasForeignKey("UpvotedQuestionsId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("stack_overload.Models.User", null)
                    .WithMany()
                    .HasForeignKey("UpvotersId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });
        }
    }
}