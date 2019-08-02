using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZEQP.Entities.Test.Migrations
{
    public partial class IntAndLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogInts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogInts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogLongs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogLongs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostInts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostInts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostInts_BlogInts_BlogId",
                        column: x => x.BlogId,
                        principalTable: "BlogInts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostLongs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: false),
                    BlogId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLongs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLongs_BlogLongs_BlogId1",
                        column: x => x.BlogId1,
                        principalTable: "BlogLongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostInts_BlogId",
                table: "PostInts",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLongs_BlogId1",
                table: "PostLongs",
                column: "BlogId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostInts");

            migrationBuilder.DropTable(
                name: "PostLongs");

            migrationBuilder.DropTable(
                name: "BlogInts");

            migrationBuilder.DropTable(
                name: "BlogLongs");
        }
    }
}
