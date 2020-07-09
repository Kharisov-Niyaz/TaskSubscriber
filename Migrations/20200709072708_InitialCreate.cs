using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskSubscriber.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "packets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    publishId = table.Column<int>(nullable: false),
                    message = table.Column<string>(nullable: true),
                    sendDate = table.Column<DateTime>(nullable: false),
                    hash = table.Column<string>(nullable: true),
                    receiveDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_packets", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "packets");
        }
    }
}
