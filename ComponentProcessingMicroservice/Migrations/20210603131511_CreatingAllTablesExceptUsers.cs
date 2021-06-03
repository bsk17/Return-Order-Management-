using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComponentProcessingMicroservice.Migrations
{
    public partial class CreatingAllTablesExceptUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditCards",
                columns: table => new
                {
                    CreditCardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CreditCardNumber = table.Column<string>(nullable: false),
                    CreditCardLimit = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCards", x => x.CreditCardId);
                });

            migrationBuilder.CreateTable(
                name: "DefectiveComponents",
                columns: table => new
                {
                    DefectiveComponentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentType = table.Column<string>(nullable: false),
                    ComponentName = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefectiveComponents", x => x.DefectiveComponentId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessRequests",
                columns: table => new
                {
                    ProcessRequestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    IsPriority = table.Column<bool>(nullable: false),
                    CreditCardNumber = table.Column<string>(nullable: true),
                    DefectiveComponentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessRequests", x => x.ProcessRequestId);
                    table.ForeignKey(
                        name: "FK_ProcessRequests_DefectiveComponents_DefectiveComponentId",
                        column: x => x.DefectiveComponentId,
                        principalTable: "DefectiveComponents",
                        principalColumn: "DefectiveComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessResponses",
                columns: table => new
                {
                    ProcessResponsetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessingCharge = table.Column<decimal>(nullable: false),
                    PackageAndDeliveryCharge = table.Column<decimal>(nullable: false),
                    DateOfDelivery = table.Column<DateTime>(nullable: false),
                    ProcessRequestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessResponses", x => x.ProcessResponsetId);
                    table.ForeignKey(
                        name: "FK_ProcessResponses_ProcessRequests_ProcessRequestId",
                        column: x => x.ProcessRequestId,
                        principalTable: "ProcessRequests",
                        principalColumn: "ProcessRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessRequests_DefectiveComponentId",
                table: "ProcessRequests",
                column: "DefectiveComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessResponses_ProcessRequestId",
                table: "ProcessResponses",
                column: "ProcessRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditCards");

            migrationBuilder.DropTable(
                name: "ProcessResponses");

            migrationBuilder.DropTable(
                name: "ProcessRequests");

            migrationBuilder.DropTable(
                name: "DefectiveComponents");
        }
    }
}
