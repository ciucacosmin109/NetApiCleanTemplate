using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetApiCleanTemplate.Infrastructure.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemoEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemoString = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DemoParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoEntities_DemoEntities_DemoParentId",
                        column: x => x.DemoParentId,
                        principalTable: "DemoEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemoEntities_DemoParentId",
                table: "DemoEntities",
                column: "DemoParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemoEntities");
        }
    }
}

