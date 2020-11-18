using Microsoft.EntityFrameworkCore.Migrations;

namespace Subscriber.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "new_name",
                table: "order_info",
                type: "varchar(255)",
                nullable: true,
                comment: "备注",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "new_name",
                table: "order_info");
        }
    }
}
