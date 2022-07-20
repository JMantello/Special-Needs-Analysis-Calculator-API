using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Special_Needs_Analysis_Calculator.Data.Migrations
{
    public partial class AddedSessions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Email = table.Column<string>(type: "text", nullable: false),
                    SessionToken = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Email);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}
