using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Classware.Infrastructure.Migrations
{
    public partial class IsAnsweredColumnAddedToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnswered",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnswered",
                table: "Messages");
        }
    }
}
