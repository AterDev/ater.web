﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Http.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:ltree", ",,");

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Path = table.Column<string>(type: "ltree", maxLength: 500, nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Folders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CostScore = table.Column<int>(type: "integer", nullable: false),
                    Sort = table.Column<int>(type: "integer", nullable: false),
                    Days = table.Column<int>(type: "integer", nullable: false),
                    ProductType = table.Column<int>(type: "integer", nullable: false),
                    OriginPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Valid = table.Column<bool>(type: "boolean", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    GroupName = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemMenus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Path = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Icon = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false),
                    AccessCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MenuType = table.Column<int>(type: "integer", nullable: false),
                    Sort = table.Column<int>(type: "integer", nullable: false),
                    Hidden = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemMenus_SystemMenus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SystemMenus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SystemOrganizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemOrganizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemOrganizations_SystemOrganizations_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SystemOrganizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SystemPermissionGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPermissionGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    NameValue = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    Icon = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    RealName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordSalt = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false),
                    LastLoginTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    Avatar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordSalt = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false),
                    LastLoginTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    Avatar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FolderId = table.Column<Guid>(type: "uuid", nullable: true),
                    FileName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Extension = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Md5 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Content = table.Column<byte[]>(type: "bytea", maxLength: 2097152, nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileDatas_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SystemPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    PermissionType = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemPermissions_SystemPermissionGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "SystemPermissionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemMenuSystemRole",
                columns: table => new
                {
                    MenusId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemMenuSystemRole", x => new { x.MenusId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_SystemMenuSystemRole_SystemMenus_MenusId",
                        column: x => x.MenusId,
                        principalTable: "SystemMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemMenuSystemRole_SystemRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "SystemRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemPermissionGroupSystemRole",
                columns: table => new
                {
                    PermissionGroupsId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPermissionGroupSystemRole", x => new { x.PermissionGroupsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_SystemPermissionGroupSystemRole_SystemPermissionGroups_Perm~",
                        column: x => x.PermissionGroupsId,
                        principalTable: "SystemPermissionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemPermissionGroupSystemRole_SystemRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "SystemRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TargetName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Route = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ActionType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    SystemUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemLogs_SystemUsers_SystemUserId",
                        column: x => x.SystemUserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemOrganizationSystemUser",
                columns: table => new
                {
                    SystemOrganizationsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemOrganizationSystemUser", x => new { x.SystemOrganizationsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SystemOrganizationSystemUser_SystemOrganizations_SystemOrga~",
                        column: x => x.SystemOrganizationsId,
                        principalTable: "SystemOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemOrganizationSystemUser_SystemUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemRoleSystemUser",
                columns: table => new
                {
                    SystemRolesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoleSystemUser", x => new { x.SystemRolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SystemRoleSystemUser_SystemRoles_SystemRolesId",
                        column: x => x.SystemRolesId,
                        principalTable: "SystemRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemRoleSystemUser_SystemUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Catalogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Level = table.Column<short>(type: "smallint", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catalogs_Catalogs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Catalogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Catalogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PayNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Content = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    Authors = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TranslateTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TranslateContent = table.Column<string>(type: "character varying(12000)", maxLength: 12000, nullable: true),
                    LanguageType = table.Column<int>(type: "integer", nullable: false),
                    BlogType = table.Column<int>(type: "integer", nullable: false),
                    IsAudit = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    IsOriginal = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<Guid>(type: "uuid", nullable: false),
                    ViewCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_CatalogId",
                table: "Blogs",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_Level",
                table: "Catalogs",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_Name",
                table: "Catalogs",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_ParentId",
                table: "Catalogs",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_UserId",
                table: "Catalogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileDatas_Extension",
                table: "FileDatas",
                column: "Extension");

            migrationBuilder.CreateIndex(
                name: "IX_FileDatas_FileName",
                table: "FileDatas",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_FileDatas_FolderId",
                table: "FileDatas",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_FileDatas_Md5",
                table: "FileDatas",
                column: "Md5");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_Name",
                table: "Folders",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentId",
                table: "Folders",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DiscountCode",
                table: "Orders",
                column: "DiscountCode");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductType",
                table: "Products",
                column: "ProductType");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Sort",
                table: "Products",
                column: "Sort");

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfigs_GroupName",
                table: "SystemConfigs",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfigs_IsSystem",
                table: "SystemConfigs",
                column: "IsSystem");

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfigs_Key",
                table: "SystemConfigs",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfigs_Valid",
                table: "SystemConfigs",
                column: "Valid");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_ActionType",
                table: "SystemLogs",
                column: "ActionType");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_ActionUserName",
                table: "SystemLogs",
                column: "ActionUserName");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_CreatedTime",
                table: "SystemLogs",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_SystemUserId",
                table: "SystemLogs",
                column: "SystemUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemMenus_ParentId",
                table: "SystemMenus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemMenuSystemRole_RolesId",
                table: "SystemMenuSystemRole",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemOrganizations_ParentId",
                table: "SystemOrganizations",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemOrganizationSystemUser_UsersId",
                table: "SystemOrganizationSystemUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemPermissionGroupSystemRole_RolesId",
                table: "SystemPermissionGroupSystemRole",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemPermissions_GroupId",
                table: "SystemPermissions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemPermissions_Name",
                table: "SystemPermissions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SystemPermissions_PermissionType",
                table: "SystemPermissions",
                column: "PermissionType");

            migrationBuilder.CreateIndex(
                name: "IX_SystemRoles_Name",
                table: "SystemRoles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SystemRoles_NameValue",
                table: "SystemRoles",
                column: "NameValue",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemRoleSystemUser_UsersId",
                table: "SystemRoleSystemUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_CreatedTime",
                table: "SystemUsers",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_Email",
                table: "SystemUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_IsDeleted",
                table: "SystemUsers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_PhoneNumber",
                table: "SystemUsers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_UserName",
                table: "SystemUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedTime",
                table: "Users",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsDeleted",
                table: "Users",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "FileDatas");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "SystemConfigs");

            migrationBuilder.DropTable(
                name: "SystemLogs");

            migrationBuilder.DropTable(
                name: "SystemMenuSystemRole");

            migrationBuilder.DropTable(
                name: "SystemOrganizationSystemUser");

            migrationBuilder.DropTable(
                name: "SystemPermissionGroupSystemRole");

            migrationBuilder.DropTable(
                name: "SystemPermissions");

            migrationBuilder.DropTable(
                name: "SystemRoleSystemUser");

            migrationBuilder.DropTable(
                name: "Catalogs");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SystemMenus");

            migrationBuilder.DropTable(
                name: "SystemOrganizations");

            migrationBuilder.DropTable(
                name: "SystemPermissionGroups");

            migrationBuilder.DropTable(
                name: "SystemRoles");

            migrationBuilder.DropTable(
                name: "SystemUsers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
