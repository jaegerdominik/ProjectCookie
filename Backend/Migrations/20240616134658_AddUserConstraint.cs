using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCookie.Migrations
{
    /// <inheritdoc />
    public partial class AddUserConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FK_User",
                schema: "cookie",
                table: "Score",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Score_FK_User",
                schema: "cookie",
                table: "Score",
                column: "FK_User");

            migrationBuilder.AddForeignKey(
                name: "FK_Score_User_FK_User",
                schema: "cookie",
                table: "Score",
                column: "FK_User",
                principalSchema: "cookie",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Score_User_FK_User",
                schema: "cookie",
                table: "Score");

            migrationBuilder.DropIndex(
                name: "IX_Score_FK_User",
                schema: "cookie",
                table: "Score");

            migrationBuilder.DropColumn(
                name: "FK_User",
                schema: "cookie",
                table: "Score");
        }
    }
}
