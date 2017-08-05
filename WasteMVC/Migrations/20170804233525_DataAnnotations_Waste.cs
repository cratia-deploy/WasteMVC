using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WasteMVC.Migrations
{
    public partial class DataAnnotations_Waste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wastes_WasteTypes_WasteTypeId",
                table: "Wastes");

            migrationBuilder.AlterColumn<int>(
                name: "WasteTypeId",
                table: "Wastes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "SalePrice",
                table: "Wastes",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "Wastes",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AddForeignKey(
                name: "FK_Wastes_WasteTypes_WasteTypeId",
                table: "Wastes",
                column: "WasteTypeId",
                principalTable: "WasteTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wastes_WasteTypes_WasteTypeId",
                table: "Wastes");

            migrationBuilder.AlterColumn<int>(
                name: "WasteTypeId",
                table: "Wastes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "SalePrice",
                table: "Wastes",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "Wastes",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wastes_WasteTypes_WasteTypeId",
                table: "Wastes",
                column: "WasteTypeId",
                principalTable: "WasteTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
