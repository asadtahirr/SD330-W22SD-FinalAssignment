using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stack_overload.Data.Migrations
{
    public partial class UpdatedQuestionsAndAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Votes",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Votes",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AnswerUser",
                columns: table => new
                {
                    DownvotedAnswersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DownvotersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerUser", x => new { x.DownvotedAnswersId, x.DownvotersId });
                    table.ForeignKey(
                        name: "FK_AnswerUser_Answers_DownvotedAnswersId",
                        column: x => x.DownvotedAnswersId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerUser_AspNetUsers_DownvotersId",
                        column: x => x.DownvotersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnswerUser1",
                columns: table => new
                {
                    UpvotedAnswersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpvotersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerUser1", x => new { x.UpvotedAnswersId, x.UpvotersId });
                    table.ForeignKey(
                        name: "FK_AnswerUser1_Answers_UpvotedAnswersId",
                        column: x => x.UpvotedAnswersId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerUser1_AspNetUsers_UpvotersId",
                        column: x => x.UpvotersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionUser",
                columns: table => new
                {
                    DownvotedQuestionsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DownvotersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionUser", x => new { x.DownvotedQuestionsId, x.DownvotersId });
                    table.ForeignKey(
                        name: "FK_QuestionUser_AspNetUsers_DownvotersId",
                        column: x => x.DownvotersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionUser_Questions_DownvotedQuestionsId",
                        column: x => x.DownvotedQuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionUser1",
                columns: table => new
                {
                    UpvotedQuestionsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpvotersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionUser1", x => new { x.UpvotedQuestionsId, x.UpvotersId });
                    table.ForeignKey(
                        name: "FK_QuestionUser1_AspNetUsers_UpvotersId",
                        column: x => x.UpvotersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionUser1_Questions_UpvotedQuestionsId",
                        column: x => x.UpvotedQuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUser_DownvotersId",
                table: "AnswerUser",
                column: "DownvotersId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUser1_UpvotersId",
                table: "AnswerUser1",
                column: "UpvotersId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionUser_DownvotersId",
                table: "QuestionUser",
                column: "DownvotersId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionUser1_UpvotersId",
                table: "QuestionUser1",
                column: "UpvotersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerUser");

            migrationBuilder.DropTable(
                name: "AnswerUser1");

            migrationBuilder.DropTable(
                name: "QuestionUser");

            migrationBuilder.DropTable(
                name: "QuestionUser1");

            migrationBuilder.DropColumn(
                name: "Votes",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Votes",
                table: "Answers");
        }
    }
}
