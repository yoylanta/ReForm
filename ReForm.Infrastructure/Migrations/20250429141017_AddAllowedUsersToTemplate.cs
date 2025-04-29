using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReForm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAllowedUsersToTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemplateFormAllowedUsers",
                columns: table => new
                {
                    TemplateFormId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateFormAllowedUsers", x => new { x.TemplateFormId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TemplateFormAllowedUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateFormAllowedUsers_TemplateForm_TemplateFormId",
                        column: x => x.TemplateFormId,
                        principalTable: "TemplateForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f94b49fb-eb3c-4868-b195-e3fe4518d089");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "6a300d94-fa4b-4c7e-b181-d56d308befba");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateFormAllowedUsers_UserId",
                table: "TemplateFormAllowedUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateFormAllowedUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "aa007c5a-7d70-4957-84b0-54b9fcffc02f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "0a6ca66c-403e-4650-8daa-1bf69f860b82");
        }
    }
}
