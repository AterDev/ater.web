using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkCore.Migrations;

public partial class updateThirdNewsFileds : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "AuthorAvatar",
            table: "ThirdNews",
            type: "character varying(300)",
            maxLength: 300,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "AuthorName",
            table: "ThirdNews",
            type: "character varying(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "IdentityId",
            table: "ThirdNews",
            type: "character varying(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "Type",
            table: "ThirdNews",
            type: "integer",
            nullable: false,
            defaultValue: 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AuthorAvatar",
            table: "ThirdNews");

        migrationBuilder.DropColumn(
            name: "AuthorName",
            table: "ThirdNews");

        migrationBuilder.DropColumn(
            name: "IdentityId",
            table: "ThirdNews");

        migrationBuilder.DropColumn(
            name: "Type",
            table: "ThirdNews");
    }
}
