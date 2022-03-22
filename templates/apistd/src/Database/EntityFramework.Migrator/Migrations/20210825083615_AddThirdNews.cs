using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkCore.Migrations;

public partial class AddThirdNews : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Articles_Accounts_AccountId",
            table: "Articles");

        migrationBuilder.AlterColumn<Guid>(
            name: "AccountId",
            table: "Articles",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldNullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsPrivate",
            table: "Articles",
            type: "boolean",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "ThirdNews",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                Url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                ThumbnailUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                Provider = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                DatePublished = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Content = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true),
                Category = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ThirdNews", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ThirdNews_Category",
            table: "ThirdNews",
            column: "Category");

        migrationBuilder.CreateIndex(
            name: "IX_ThirdNews_DatePublished",
            table: "ThirdNews",
            column: "DatePublished");

        migrationBuilder.CreateIndex(
            name: "IX_ThirdNews_Provider",
            table: "ThirdNews",
            column: "Provider");

        migrationBuilder.CreateIndex(
            name: "IX_ThirdNews_Title",
            table: "ThirdNews",
            column: "Title");

        migrationBuilder.AddForeignKey(
            name: "FK_Articles_Accounts_AccountId",
            table: "Articles",
            column: "AccountId",
            principalTable: "Accounts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Articles_Accounts_AccountId",
            table: "Articles");

        migrationBuilder.DropTable(
            name: "ThirdNews");

        migrationBuilder.DropColumn(
            name: "IsPrivate",
            table: "Articles");

        migrationBuilder.AlterColumn<Guid>(
            name: "AccountId",
            table: "Articles",
            type: "uuid",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AddForeignKey(
            name: "FK_Articles_Accounts_AccountId",
            table: "Articles",
            column: "AccountId",
            principalTable: "Accounts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
