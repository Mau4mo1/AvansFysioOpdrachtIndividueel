using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AvansFysioOpdrachtIndividueel.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "PatientDossierModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntakeDoneById = table.Column<int>(type: "int", nullable: true),
                    IntakeSupervisedById = table.Column<int>(type: "int", nullable: true),
                    TherapistId = table.Column<int>(type: "int", nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExtraComments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDossierModel", x => x.Id);
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
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PatientNumber = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_Patient_PatientDossierId",
                table: "Patient",
                column: "PatientDossierId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "PatientDossierModel");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
