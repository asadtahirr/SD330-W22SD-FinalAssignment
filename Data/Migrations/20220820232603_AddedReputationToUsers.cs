using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stack_overload.Data.Migrations
{
    public partial class AddedReputationToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Reputation",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reputation",
                table: "AspNetUsers");
        }
    }
}
