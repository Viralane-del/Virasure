using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirasureYouAI.Migrations
{
    public partial class mig18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AICategory",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AICategory",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Messages");
        }
    }
}
