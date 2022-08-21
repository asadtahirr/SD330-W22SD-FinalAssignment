using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stack_overload.Data.Migrations
{
    public partial class AddedMarkAsCorrectToQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MarkedAsCorrect",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkedAsCorrect",
                table: "Answers");
        }
    }
}
