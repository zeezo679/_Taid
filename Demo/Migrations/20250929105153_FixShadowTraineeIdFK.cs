using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class FixShadowTraineeIdFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropIndex(
                name: "IX_Trainees_UserId",
                table: "Trainees");


            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "crsResults",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Trainees_UserId",
                table: "Trainees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_crsResults_UserId",
                table: "crsResults",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_crsResults_Trainees_UserId",
                table: "crsResults",
                column: "UserId",
                principalTable: "Trainees",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_crsResults_Trainees_UserId",
                table: "crsResults");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Trainees_UserId",
                table: "Trainees");

            migrationBuilder.DropIndex(
                name: "IX_crsResults_UserId",
                table: "crsResults");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "crsResults",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "TraineeId",
                table: "crsResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_UserId",
                table: "Trainees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_crsResults_TraineeId",
                table: "crsResults",
                column: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_crsResults_Trainees_TraineeId",
                table: "crsResults",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
