using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiagnosisModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeAndDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosisModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentPlanModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountOfTreaments = table.Column<int>(type: "int", nullable: false),
                    TimeOfTreatment = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentPlanModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Therapist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Therapist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Therapist_Person_Id",
                        column: x => x.Id,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientDossierModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisCodeId = table.Column<int>(type: "int", nullable: true),
                    IntakeDoneById = table.Column<int>(type: "int", nullable: true),
                    IntakeSupervisedById = table.Column<int>(type: "int", nullable: true),
                    TherapistId = table.Column<int>(type: "int", nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TreatmentPlanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDossierModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientDossierModel_DiagnosisModel_DiagnosisCodeId",
                        column: x => x.DiagnosisCodeId,
                        principalTable: "DiagnosisModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientDossierModel_Person_IntakeDoneById",
                        column: x => x.IntakeDoneById,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientDossierModel_Person_IntakeSupervisedById",
                        column: x => x.IntakeSupervisedById,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientDossierModel_Person_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientDossierModel_TreatmentPlanModel_TreatmentPlanId",
                        column: x => x.TreatmentPlanId,
                        principalTable: "TreatmentPlanModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    StudentNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Student_Therapist_Id",
                        column: x => x.Id,
                        principalTable: "Therapist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BIGNumber = table.Column<int>(type: "int", nullable: false),
                    PersonnelNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teacher_Therapist_Id",
                        column: x => x.Id,
                        principalTable: "Therapist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TimeOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommentMadeById = table.Column<int>(type: "int", nullable: true),
                    CommentVisibleForPatient = table.Column<bool>(type: "bit", nullable: false),
                    PatientDossierModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentModel_PatientDossierModel_PatientDossierModelId",
                        column: x => x.PatientDossierModelId,
                        principalTable: "PatientDossierModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentModel_Person_CommentMadeById",
                        column: x => x.CommentMadeById,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PatientNumber = table.Column<int>(type: "int", nullable: false),
                    TeacherOrStudentNumber = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientDossierId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_PatientDossierModel_PatientDossierId",
                        column: x => x.PatientDossierId,
                        principalTable: "PatientDossierModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patient_Person_Id",
                        column: x => x.Id,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Treatment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VektisType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TreatmentRoomOrTrainingRoom = table.Column<bool>(type: "bit", nullable: false),
                    Complications = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreatmentDoneById = table.Column<int>(type: "int", nullable: true),
                    TreatmentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TreatmentUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientDossierModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treatment_PatientDossierModel_PatientDossierModelId",
                        column: x => x.PatientDossierModelId,
                        principalTable: "PatientDossierModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Treatment_Person_TreatmentDoneById",
                        column: x => x.TreatmentDoneById,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[,]
                {
                    { 4, "meesmake@outlook.com", "Mees Maske" },
                    { 5, "kevin@outlook.com", "Kevin Verhoeven" },
                    { 1, "mauricederidder@outlook.com", "Maurice de Ridder" },
                    { 2, "timdelaater@outlook.com", "Tim de Laater" },
                    { 3, "ricoschouten@outlook.com", "Rico Schouten" }
                });

            migrationBuilder.InsertData(
                table: "Therapist",
                column: "Id",
                values: new object[]
                {
                    4,
                    5,
                    1,
                    2,
                    3
                });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "StudentNumber" },
                values: new object[,]
                {
                    { 4, 321 },
                    { 5, 33421 }
                });

            migrationBuilder.InsertData(
                table: "Teacher",
                columns: new[] { "Id", "BIGNumber", "PersonnelNumber" },
                values: new object[,]
                {
                    { 1, 32, 2 },
                    { 2, 3, 3231 },
                    { 3, 55, 98721 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentModel_CommentMadeById",
                table: "CommentModel",
                column: "CommentMadeById");

            migrationBuilder.CreateIndex(
                name: "IX_CommentModel_PatientDossierModelId",
                table: "CommentModel",
                column: "PatientDossierModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_PatientDossierId",
                table: "Patient",
                column: "PatientDossierId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDossierModel_DiagnosisCodeId",
                table: "PatientDossierModel",
                column: "DiagnosisCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDossierModel_IntakeDoneById",
                table: "PatientDossierModel",
                column: "IntakeDoneById");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDossierModel_IntakeSupervisedById",
                table: "PatientDossierModel",
                column: "IntakeSupervisedById");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDossierModel_TherapistId",
                table: "PatientDossierModel",
                column: "TherapistId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDossierModel_TreatmentPlanId",
                table: "PatientDossierModel",
                column: "TreatmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_Email",
                table: "Person",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Treatment_PatientDossierModelId",
                table: "Treatment",
                column: "PatientDossierModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatment_TreatmentDoneById",
                table: "Treatment",
                column: "TreatmentDoneById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentModel");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Teacher");

            migrationBuilder.DropTable(
                name: "Treatment");

            migrationBuilder.DropTable(
                name: "Therapist");

            migrationBuilder.DropTable(
                name: "PatientDossierModel");

            migrationBuilder.DropTable(
                name: "DiagnosisModel");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "TreatmentPlanModel");
        }
    }
}
