using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReForm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateFormGeneralSettingsFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TemplateForm",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "TemplateForm",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "TemplateForm",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateFormTags",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "integer", nullable: false),
                    TemplateFormsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateFormTags", x => new { x.TagsId, x.TemplateFormsId });
                    table.ForeignKey(
                        name: "FK_TemplateFormTags_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateFormTags_TemplateForm_TemplateFormsId",
                        column: x => x.TemplateFormsId,
                        principalTable: "TemplateForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "8859dc4a-8c0f-432e-a03c-a9129dd4c1d4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3ff817ca-6962-4e57-bf5e-ad07acb4cee8");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateForm_TopicId",
                table: "TemplateForm",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateFormTags_TemplateFormsId",
                table: "TemplateFormTags",
                column: "TemplateFormsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateForm_Topic_TopicId",
                table: "TemplateForm",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateForm_Topic_TopicId",
                table: "TemplateForm");

            migrationBuilder.DropTable(
                name: "TemplateFormTags");

            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_TemplateForm_TopicId",
                table: "TemplateForm");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TemplateForm");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "TemplateForm");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "TemplateForm");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "9aeaa643-2ce3-46d3-a76f-f72cf7585a0e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "06da49de-6c91-4d41-9783-5e3156b74bee");
        }
    }
}
