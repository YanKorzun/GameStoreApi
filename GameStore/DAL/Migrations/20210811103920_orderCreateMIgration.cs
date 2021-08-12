using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.DAL.Migrations
{
    public partial class orderCreateMIgration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Order");
        }
    }
}
