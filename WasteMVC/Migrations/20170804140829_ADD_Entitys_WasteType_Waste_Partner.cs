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
                name: "WasteType",
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
                    table.PrimaryKey("PK_WasteType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Waste",
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
                    table.PrimaryKey("PK_Waste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Waste_WasteType_WasteTypeId",
                        column: x => x.WasteTypeId,
                        principalTable: "WasteType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Partner",
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
                    table.PrimaryKey("PK_Partner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partner_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partner_Waste_WasteId",
                        column: x => x.WasteId,
                        principalTable: "Waste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partner_PersonId",
                table: "Partner",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_WasteId",
                table: "Partner",
                column: "WasteId");

            migrationBuilder.CreateIndex(
                name: "IX_Waste_WasteTypeId",
                table: "Waste",
                column: "WasteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WasteType_Description",
                table: "WasteType",
                column: "Description",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partner");

            migrationBuilder.DropTable(
                name: "Waste");

            migrationBuilder.DropTable(
                name: "WasteType");

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
