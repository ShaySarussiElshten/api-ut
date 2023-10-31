using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingGarageManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class add581 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingLots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsOccupied = table.Column<bool>(type: "INTEGER", nullable: false),
                    OccupiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingLots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LicensePlateId = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    VehicleType = table.Column<int>(type: "INTEGER", maxLength: 20, nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Length = table.Column<int>(type: "INTEGER", nullable: false),
                    ParkingLotId = table.Column<int>(type: "INTEGER", nullable: false),
                    TicketType = table.Column<int>(type: "INTEGER", maxLength: 20, nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_ParkingLots_ParkingLotId",
                        column: x => x.ParkingLotId,
                        principalTable: "ParkingLots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ParkingLotId",
                table: "Vehicles",
                column: "ParkingLotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "ParkingLots");
        }
    }
}
