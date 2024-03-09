using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamifyX.Migrations
{
    /// <inheritdoc />
    public partial class FixQuestionForeignKey : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// Remove any foreign keys that shouldn't be there
			migrationBuilder.DropForeignKey(name: "FK_Questions_Exams_ExamId1", table: "Questions");

			// Remove the columns that shouldn't be there
			migrationBuilder.DropColumn(name: "ExamId1", table: "Questions");
			migrationBuilder.DropColumn(name: "ExamId2", table: "Questions");

			// Reapply the correct foreign key
			migrationBuilder.AddForeignKey(
				name: "FK_Questions_Exams_ExamId",
				table: "Questions",
				column: "ExamId",
				principalTable: "Exams",
				principalColumn: "ExamId",
				onDelete: ReferentialAction.Restrict); // No cascading delete
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// If the ExamId1 and ExamId2 columns were erroneous and should not exist,
			// we don't re-create them in the Down method. Instead, we would just re-create
			// any foreign key constraints that were originally in place (if they were correct).

			migrationBuilder.DropForeignKey(
				name: "FK_Questions_Exams_ExamId",
				table: "Questions");

			// Re-add the original foreign key if it was removed as part of the clean-up,
			// using the original delete behavior it had.
			migrationBuilder.AddForeignKey(
				name: "FK_Questions_Exams_ExamId",
				table: "Questions",
				column: "ExamId",
				principalTable: "Exams",
				principalColumn: "ExamId",
				onDelete: ReferentialAction.Cascade); // Use the original DeleteBehavior
		}
	}
}
