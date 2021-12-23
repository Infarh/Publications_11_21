using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Publications.DAL.Migrations
{
    public partial class Places : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Annotation",
                table: "Publications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "Publications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Publications_PlaceId",
                table: "Publications",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_Places_PlaceId",
                table: "Publications",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Places_PlaceId",
                table: "Publications");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Publications_PlaceId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Annotation",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Publications");
        }
    }
}
