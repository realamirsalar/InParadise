using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InParadise.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AmenityTBL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VillaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amenities_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "Description", "Name", "VillaId" },
                values: new object[,]
                {
                    { 1, null, "استخر خصوصی", 1 },
                    { 2, null, "فر آشپزخانه", 1 },
                    { 3, null, "بالکن خصوصی", 1 },
                    { 4, null, "یک تخت بزرگ به همراه یک مبل تختخواب شو", 1 },
                    { 5, null, "استخز اختصاصی با سقوط آزاد", 2 },
                    { 6, null, "آشپز خانه مجهز", 2 },
                    { 7, null, "بالکن اختصاصی", 2 },
                    { 8, null, "تخت دو نفره", 2 },
                    { 9, null, "استخر خصوصی", 3 },
                    { 10, null, "جکوزی", 3 },
                    { 11, null, "بالکن اختصاصی", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_VillaId",
                table: "Amenities",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amenities");
        }
    }
}
