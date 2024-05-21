using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MektepTagamAPI.Migrations
{
    public partial class APIAvtoMigNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "CardCodes");

            migrationBuilder.DropTable(
                name: "CashRegisterShift");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.CreateTable(
                name: "CardCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AspNetUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardCodes_AspNetUsers_AspNetUserId",
                        column: x => x.AspNetUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CardCodes_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CashRegisterShift",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AspNetUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfClose = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfOpen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: true),
                    IsOpened = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashRegisterShift", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CashRegisterShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DishId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    DateOfCreatedTransaction = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_CardCodes_CardCodeId",
                        column: x => x.CardCodeId,
                        principalTable: "CardCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_CashRegisterShift_CashRegisterShiftId",
                        column: x => x.CashRegisterShiftId,
                        principalTable: "CashRegisterShift",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardCodes_AspNetUserId",
                table: "CardCodes",
                column: "AspNetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CardCodes_OrganizationId",
                table: "CardCodes",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_OrganizationId",
                table: "Dishes",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CardCodeId",
                table: "Transactions",
                column: "CardCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CashRegisterShiftId",
                table: "Transactions",
                column: "CashRegisterShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DishId",
                table: "Transactions",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OrganizationId",
                table: "Transactions",
                column: "OrganizationId");
        }
    }
}
