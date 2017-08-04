using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WasteMVC.Migrations
{
    public partial class ADD_Entitys_WasteType_Waste_Partner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Persons",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Persons",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "WasteTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Deleted_At = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Updated_At = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wastes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cost = table.Column<double>(nullable: false),
                    Created_At = table.Column<DateTime>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Deleted_At = table.Column<DateTime>(nullable: false),
                    SalePrice = table.Column<double>(nullable: false),
                    Updated_At = table.Column<DateTime>(nullable: false),
                    WasteTypeId = table.Column<int>(nullable: true),
                    Weight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wastes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wastes_WasteTypes_WasteTypeId",
                        column: x => x.WasteTypeId,
                        principalTable: "WasteTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created_At = table.Column<DateTime>(nullable: false),
                    Deleted_At = table.Column<DateTime>(nullable: false),
                    Percentage = table.Column<double>(nullable: false),
                    PersonId = table.Column<int>(nullable: true),
                    Updated_At = table.Column<DateTime>(nullable: false),
                    WasteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partners_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partners_Wastes_WasteId",
                        column: x => x.WasteId,
                        principalTable: "Wastes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partners_PersonId",
                table: "Partners",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_WasteId",
                table: "Partners",
                column: "WasteId");

            migrationBuilder.CreateIndex(
                name: "IX_Wastes_WasteTypeId",
                table: "Wastes",
                column: "WasteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteTypes_Description",
                table: "WasteTypes",
                column: "Description",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "Wastes");

            migrationBuilder.DropTable(
                name: "WasteTypes");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Persons",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Persons",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
