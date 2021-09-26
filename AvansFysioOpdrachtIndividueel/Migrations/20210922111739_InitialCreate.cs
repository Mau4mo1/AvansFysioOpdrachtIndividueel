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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PatientNumber = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_Person_Id",
                        column: x => x.Id,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "patientDossiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    IssueDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntakeDoneById = table.Column<int>(type: "int", nullable: true),
                    IntakeSupervisedById = table.Column<int>(type: "int", nullable: true),
                    TherapistId = table.Column<int>(type: "int", nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExtraComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyProperty = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientDossiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_patientDossiers_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_patientDossiers_Person_IntakeDoneById",
                        column: x => x.IntakeDoneById,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_patientDossiers_Person_IntakeSupervisedById",
                        column: x => x.IntakeSupervisedById,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_patientDossiers_Person_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_patientDossiers_IntakeDoneById",
                table: "patientDossiers",
                column: "IntakeDoneById");

            migrationBuilder.CreateIndex(
                name: "IX_patientDossiers_IntakeSupervisedById",
                table: "patientDossiers",
                column: "IntakeSupervisedById");

            migrationBuilder.CreateIndex(
                name: "IX_patientDossiers_PatientId",
                table: "patientDossiers",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_patientDossiers_TherapistId",
                table: "patientDossiers",
                column: "TherapistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "patientDossiers");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
