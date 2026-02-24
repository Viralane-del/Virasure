using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirasureYouAI.Migrations
{
    public partial class mig14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Messages");

            migrationBuilder.CreateTable(
                name: "ClaudeAIMessages",
                columns: table => new
                {
                    ClaudeAIMessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveNameSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaudeAIMessages", x => x.ClaudeAIMessageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaudeAIMessages");

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
