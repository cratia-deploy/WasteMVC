using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WasteMVC.Migrations
{
    public partial class Add_Cost2_SalePrice2_Decrease__Waste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Cost2",
                table: "Wastes",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Decrease",
                table: "Wastes",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SalePrice2",
                table: "Wastes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost2",
                table: "Wastes");

            migrationBuilder.DropColumn(
                name: "Decrease",
                table: "Wastes");

            migrationBuilder.DropColumn(
                name: "SalePrice2",
                table: "Wastes");
        }
    }
}
