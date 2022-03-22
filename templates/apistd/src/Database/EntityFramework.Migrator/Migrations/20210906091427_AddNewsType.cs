using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkCore.Migrations;

public partial class AddNewsType : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "NewsType",
            table: "ThirdNews",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_ThirdNews_IdentityId",
            table: "ThirdNews",
            column: "IdentityId");

        migrationBuilder.CreateIndex(
            name: "IX_ThirdNews_NewsType",
            table: "ThirdNews",
            column: "NewsType");

        migrationBuilder.CreateIndex(
            name: "IX_ThirdNews_Type",
            table: "ThirdNews",
            column: "Type");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_ThirdNews_IdentityId",
            table: "ThirdNews");

        migrationBuilder.DropIndex(
            name: "IX_ThirdNews_NewsType",
            table: "ThirdNews");

        migrationBuilder.DropIndex(
            name: "IX_ThirdNews_Type",
            table: "ThirdNews");

        migrationBuilder.DropColumn(
            name: "NewsType",
            table: "ThirdNews");
    }
}
