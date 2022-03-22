using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkCore.Migrations;

public partial class Init : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AccountExtends",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                RealName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                NickName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                Birthday = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Country = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                Province = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                City = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                County = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                Street = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                AddressDetail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                WXOpenId = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                WXAvatar = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                WXUnionId = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AccountExtends", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ArticleExtends",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Content = table.Column<string>(type: "text", nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ArticleExtends", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                Icon = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Accounts",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Email = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                Password = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                HashSalt = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                LastLoginTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                RetryCount = table.Column<int>(type: "integer", nullable: false),
                Phone = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                PhoneConfirm = table.Column<bool>(type: "boolean", nullable: false),
                EmailConfirm = table.Column<bool>(type: "boolean", nullable: false),
                Avatar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                ExtendId = table.Column<Guid>(type: "uuid", nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Accounts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Accounts_AccountExtends_ExtendId",
                    column: x => x.ExtendId,
                    principalTable: "AccountExtends",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "AccountRole",
            columns: table => new
            {
                AccountsId = table.Column<Guid>(type: "uuid", nullable: false),
                RolesId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AccountRole", x => new { x.AccountsId, x.RolesId });
                table.ForeignKey(
                    name: "FK_AccountRole_Accounts_AccountsId",
                    column: x => x.AccountsId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AccountRole_Roles_RolesId",
                    column: x => x.RolesId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ArticleCatalog",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                Sort = table.Column<short>(type: "smallint", nullable: false),
                Level = table.Column<short>(type: "smallint", nullable: false),
                ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ArticleCatalog", x => x.Id);
                table.ForeignKey(
                    name: "FK_ArticleCatalog_Accounts_AccountId",
                    column: x => x.AccountId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ArticleCatalog_ArticleCatalog_ParentId",
                    column: x => x.ParentId,
                    principalTable: "ArticleCatalog",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "LibraryCatalogs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                Sort = table.Column<short>(type: "smallint", nullable: false),
                Level = table.Column<short>(type: "smallint", nullable: false),
                ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LibraryCatalogs", x => x.Id);
                table.ForeignKey(
                    name: "FK_LibraryCatalogs_Accounts_AccountId",
                    column: x => x.AccountId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_LibraryCatalogs_LibraryCatalogs_ParentId",
                    column: x => x.ParentId,
                    principalTable: "LibraryCatalogs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Articles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Summary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                AuthorName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Tags = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                ArticleType = table.Column<int>(type: "integer", nullable: false),
                AccountId = table.Column<Guid>(type: "uuid", nullable: true),
                ExtendId = table.Column<Guid>(type: "uuid", nullable: true),
                CatalogId = table.Column<Guid>(type: "uuid", nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Articles", x => x.Id);
                table.ForeignKey(
                    name: "FK_Articles_Accounts_AccountId",
                    column: x => x.AccountId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Articles_ArticleCatalog_CatalogId",
                    column: x => x.CatalogId,
                    principalTable: "ArticleCatalog",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Articles_ArticleExtends_ExtendId",
                    column: x => x.ExtendId,
                    principalTable: "ArticleExtends",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Libraries",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Namespace = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                Language = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                IsValid = table.Column<bool>(type: "boolean", nullable: false),
                IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: true),
                CatalogId = table.Column<Guid>(type: "uuid", nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Libraries", x => x.Id);
                table.ForeignKey(
                    name: "FK_Libraries_Accounts_UserId",
                    column: x => x.UserId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Libraries_LibraryCatalogs_CatalogId",
                    column: x => x.CatalogId,
                    principalTable: "LibraryCatalogs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Comments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ArticleId = table.Column<Guid>(type: "uuid", nullable: true),
                AccountId = table.Column<Guid>(type: "uuid", nullable: true),
                Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Comments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Comments_Accounts_AccountId",
                    column: x => x.AccountId,
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Comments_Articles_ArticleId",
                    column: x => x.ArticleId,
                    principalTable: "Articles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "CodeSnippets",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                Content = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                LibraryId = table.Column<Guid>(type: "uuid", nullable: true),
                Language = table.Column<int>(type: "integer", nullable: false),
                CodeType = table.Column<int>(type: "integer", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CodeSnippets", x => x.Id);
                table.ForeignKey(
                    name: "FK_CodeSnippets_Libraries_LibraryId",
                    column: x => x.LibraryId,
                    principalTable: "Libraries",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AccountExtends_City",
            table: "AccountExtends",
            column: "City");

        migrationBuilder.CreateIndex(
            name: "IX_AccountExtends_Country",
            table: "AccountExtends",
            column: "Country");

        migrationBuilder.CreateIndex(
            name: "IX_AccountExtends_Province",
            table: "AccountExtends",
            column: "Province");

        migrationBuilder.CreateIndex(
            name: "IX_AccountExtends_RealName",
            table: "AccountExtends",
            column: "RealName");

        migrationBuilder.CreateIndex(
            name: "IX_AccountRole_RolesId",
            table: "AccountRole",
            column: "RolesId");

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_CreatedTime",
            table: "Accounts",
            column: "CreatedTime");

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_Email",
            table: "Accounts",
            column: "Email");

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_EmailConfirm",
            table: "Accounts",
            column: "EmailConfirm");

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_ExtendId",
            table: "Accounts",
            column: "ExtendId");

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_IsDeleted",
            table: "Accounts",
            column: "IsDeleted");

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_Phone",
            table: "Accounts",
            column: "Phone");

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_PhoneConfirm",
            table: "Accounts",
            column: "PhoneConfirm");

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_Username",
            table: "Accounts",
            column: "Username");

        migrationBuilder.CreateIndex(
            name: "IX_ArticleCatalog_AccountId",
            table: "ArticleCatalog",
            column: "AccountId");

        migrationBuilder.CreateIndex(
            name: "IX_ArticleCatalog_Level",
            table: "ArticleCatalog",
            column: "Level");

        migrationBuilder.CreateIndex(
            name: "IX_ArticleCatalog_Name",
            table: "ArticleCatalog",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_ArticleCatalog_ParentId",
            table: "ArticleCatalog",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_ArticleCatalog_Sort",
            table: "ArticleCatalog",
            column: "Sort");

        migrationBuilder.CreateIndex(
            name: "IX_ArticleCatalog_Type",
            table: "ArticleCatalog",
            column: "Type");

        migrationBuilder.CreateIndex(
            name: "IX_Articles_AccountId",
            table: "Articles",
            column: "AccountId");

        migrationBuilder.CreateIndex(
            name: "IX_Articles_ArticleType",
            table: "Articles",
            column: "ArticleType");

        migrationBuilder.CreateIndex(
            name: "IX_Articles_CatalogId",
            table: "Articles",
            column: "CatalogId");

        migrationBuilder.CreateIndex(
            name: "IX_Articles_CreatedTime",
            table: "Articles",
            column: "CreatedTime");

        migrationBuilder.CreateIndex(
            name: "IX_Articles_ExtendId",
            table: "Articles",
            column: "ExtendId");

        migrationBuilder.CreateIndex(
            name: "IX_Articles_Title",
            table: "Articles",
            column: "Title");

        migrationBuilder.CreateIndex(
            name: "IX_CodeSnippets_CodeType",
            table: "CodeSnippets",
            column: "CodeType");

        migrationBuilder.CreateIndex(
            name: "IX_CodeSnippets_CreatedTime",
            table: "CodeSnippets",
            column: "CreatedTime");

        migrationBuilder.CreateIndex(
            name: "IX_CodeSnippets_Language",
            table: "CodeSnippets",
            column: "Language");

        migrationBuilder.CreateIndex(
            name: "IX_CodeSnippets_LibraryId",
            table: "CodeSnippets",
            column: "LibraryId");

        migrationBuilder.CreateIndex(
            name: "IX_CodeSnippets_Name",
            table: "CodeSnippets",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_CodeSnippets_Status",
            table: "CodeSnippets",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_Comments_AccountId",
            table: "Comments",
            column: "AccountId");

        migrationBuilder.CreateIndex(
            name: "IX_Comments_ArticleId",
            table: "Comments",
            column: "ArticleId");

        migrationBuilder.CreateIndex(
            name: "IX_Comments_CreatedTime",
            table: "Comments",
            column: "CreatedTime");

        migrationBuilder.CreateIndex(
            name: "IX_Libraries_CatalogId",
            table: "Libraries",
            column: "CatalogId");

        migrationBuilder.CreateIndex(
            name: "IX_Libraries_CreatedTime",
            table: "Libraries",
            column: "CreatedTime");

        migrationBuilder.CreateIndex(
            name: "IX_Libraries_IsPublic",
            table: "Libraries",
            column: "IsPublic");

        migrationBuilder.CreateIndex(
            name: "IX_Libraries_IsValid",
            table: "Libraries",
            column: "IsValid");

        migrationBuilder.CreateIndex(
            name: "IX_Libraries_Language",
            table: "Libraries",
            column: "Language");

        migrationBuilder.CreateIndex(
            name: "IX_Libraries_Namespace",
            table: "Libraries",
            column: "Namespace");

        migrationBuilder.CreateIndex(
            name: "IX_Libraries_UserId",
            table: "Libraries",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_LibraryCatalogs_AccountId",
            table: "LibraryCatalogs",
            column: "AccountId");

        migrationBuilder.CreateIndex(
            name: "IX_LibraryCatalogs_Level",
            table: "LibraryCatalogs",
            column: "Level");

        migrationBuilder.CreateIndex(
            name: "IX_LibraryCatalogs_Name",
            table: "LibraryCatalogs",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_LibraryCatalogs_ParentId",
            table: "LibraryCatalogs",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_LibraryCatalogs_Sort",
            table: "LibraryCatalogs",
            column: "Sort");

        migrationBuilder.CreateIndex(
            name: "IX_LibraryCatalogs_Type",
            table: "LibraryCatalogs",
            column: "Type");

        migrationBuilder.CreateIndex(
            name: "IX_Roles_Name",
            table: "Roles",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_Roles_Status",
            table: "Roles",
            column: "Status");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AccountRole");

        migrationBuilder.DropTable(
            name: "CodeSnippets");

        migrationBuilder.DropTable(
            name: "Comments");

        migrationBuilder.DropTable(
            name: "Roles");

        migrationBuilder.DropTable(
            name: "Libraries");

        migrationBuilder.DropTable(
            name: "Articles");

        migrationBuilder.DropTable(
            name: "LibraryCatalogs");

        migrationBuilder.DropTable(
            name: "ArticleCatalog");

        migrationBuilder.DropTable(
            name: "ArticleExtends");

        migrationBuilder.DropTable(
            name: "Accounts");

        migrationBuilder.DropTable(
            name: "AccountExtends");
    }
}
