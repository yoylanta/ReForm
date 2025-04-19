using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReForm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReFormEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemplateForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateForm_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilledForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    TemplateFormId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilledForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilledForm_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilledForm_TemplateForm_TemplateFormId",
                        column: x => x.TemplateFormId,
                        principalTable: "TemplateForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Options = table.Column<string>(type: "text", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    TemplateFormId = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateQuestion_TemplateForm_TemplateFormId",
                        column: x => x.TemplateFormId,
                        principalTable: "TemplateForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilledQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Options = table.Column<string>(type: "text", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    FilledFormId = table.Column<int>(type: "integer", nullable: false),
                    TemplateQuestionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilledQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilledQuestion_FilledForm_FilledFormId",
                        column: x => x.FilledFormId,
                        principalTable: "FilledForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilledQuestion_TemplateQuestion_TemplateQuestionId",
                        column: x => x.TemplateQuestionId,
                        principalTable: "TemplateQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Response = table.Column<string>(type: "text", nullable: false),
                    FilledQuestionId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answer_FilledQuestion_FilledQuestionId",
                        column: x => x.FilledQuestionId,
                        principalTable: "FilledQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e48e13eb-8b40-46ba-a04e-76cd22e14dd9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1ad50aac-0c0a-4f1c-b3c3-59de9aba528f");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_FilledQuestionId",
                table: "Answer",
                column: "FilledQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserId",
                table: "Answer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FilledForm_TemplateFormId",
                table: "FilledForm",
                column: "TemplateFormId");

            migrationBuilder.CreateIndex(
                name: "IX_FilledForm_UserId",
                table: "FilledForm",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FilledQuestion_FilledFormId",
                table: "FilledQuestion",
                column: "FilledFormId");

            migrationBuilder.CreateIndex(
                name: "IX_FilledQuestion_TemplateQuestionId",
                table: "FilledQuestion",
                column: "TemplateQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateForm_UserId",
                table: "TemplateForm",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateQuestion_TemplateFormId",
                table: "TemplateQuestion",
                column: "TemplateFormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "FilledQuestion");

            migrationBuilder.DropTable(
                name: "FilledForm");

            migrationBuilder.DropTable(
                name: "TemplateQuestion");

            migrationBuilder.DropTable(
                name: "TemplateForm");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6855867d-0da5-4bcd-9afe-1c2111412abd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "4840d23a-784b-4e77-a8f5-e4d9c9af0f1f");
        }
    }
}
