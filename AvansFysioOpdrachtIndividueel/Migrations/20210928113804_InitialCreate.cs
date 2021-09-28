using Microsoft.EntityFrameworkCore.Migrations;

namespace AvansFysioOpdrachtIndividueel.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Person_Id",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_patientDossiers_Patient_PatientId",
                table: "patientDossiers");

            migrationBuilder.DropForeignKey(
                name: "FK_patientDossiers_Person_IntakeDoneById",
                table: "patientDossiers");

            migrationBuilder.DropForeignKey(
                name: "FK_patientDossiers_Person_IntakeSupervisedById",
                table: "patientDossiers");

            migrationBuilder.DropForeignKey(
                name: "FK_patientDossiers_Person_TherapistId",
                table: "patientDossiers");

            migrationBuilder.DropIndex(
                name: "IX_patientDossiers_PatientId",
                table: "patientDossiers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "patientDossiers");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "PatientDossier");

            migrationBuilder.AddColumn<int>(
                name: "PatientDossierId",
                table: "Patient",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PatientDossier",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "PatientDossier",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientDossier",
                table: "PatientDossier",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_PatientDossierId",
                table: "Patient",
                column: "PatientDossierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_PatientDossier_Id",
                table: "Patient",
                column: "Id",
                principalTable: "PatientDossier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_patientDossiers_PatientDossierId",
                table: "Patient",
                column: "PatientDossierId",
                principalTable: "patientDossiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_patientDossiers_PatientDossier_IntakeDoneById",
                table: "patientDossiers",
                column: "IntakeDoneById",
                principalTable: "PatientDossier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_patientDossiers_PatientDossier_IntakeSupervisedById",
                table: "patientDossiers",
                column: "IntakeSupervisedById",
                principalTable: "PatientDossier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_patientDossiers_PatientDossier_TherapistId",
                table: "patientDossiers",
                column: "TherapistId",
                principalTable: "PatientDossier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patient_PatientDossier_Id",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_patientDossiers_PatientDossierId",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_patientDossiers_PatientDossier_IntakeDoneById",
                table: "patientDossiers");

            migrationBuilder.DropForeignKey(
                name: "FK_patientDossiers_PatientDossier_IntakeSupervisedById",
                table: "patientDossiers");

            migrationBuilder.DropForeignKey(
                name: "FK_patientDossiers_PatientDossier_TherapistId",
                table: "patientDossiers");

            migrationBuilder.DropIndex(
                name: "IX_Patient_PatientDossierId",
                table: "Patient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientDossier",
                table: "PatientDossier");

            migrationBuilder.DropColumn(
                name: "PatientDossierId",
                table: "Patient");

            migrationBuilder.RenameTable(
                name: "PatientDossier",
                newName: "Person");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "patientDossiers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_patientDossiers_PatientId",
                table: "patientDossiers",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Person_Id",
                table: "Patient",
                column: "Id",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_patientDossiers_Patient_PatientId",
                table: "patientDossiers",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_patientDossiers_Person_IntakeDoneById",
                table: "patientDossiers",
                column: "IntakeDoneById",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_patientDossiers_Person_IntakeSupervisedById",
                table: "patientDossiers",
                column: "IntakeSupervisedById",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_patientDossiers_Person_TherapistId",
                table: "patientDossiers",
                column: "TherapistId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
