﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamifyX.Migrations
{
    /// <inheritdoc />
    public partial class AddExamIdToQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "ExamId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExamId1",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExamId2",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExamId1",
                table: "Questions",
                column: "ExamId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Exams_ExamId1",
                table: "Questions",
                column: "ExamId1",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Exams_ExamId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ExamId1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ExamId1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ExamId2",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "ExamId",
                table: "Questions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId");
        }
    }
}
