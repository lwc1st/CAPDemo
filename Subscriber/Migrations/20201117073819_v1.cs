using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Subscriber.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "order_info",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "自增ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    order_code = table.Column<Guid>(type: "char(36)", nullable: true, comment: "订单编码", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_code = table.Column<Guid>(type: "char(36)", nullable: true, comment: "用户编码", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_info", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_info");
        }
    }
}
