using System;
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
            migrationBuilder.CreateTable(
                name: "SystemConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Valid = table.Column<bool>(type: "boolean", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    GroupName = table.Column<string>(type: "text", nullable: true)
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
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Path = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Icon = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false),
                    AccessCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MenuType = table.Column<int>(type: "integer", nullable: false),
                    Sort = table.Column<int>(type: "integer", nullable: false),
                    Hidden = table.Column<bool>(type: "boolean", nullable: false)
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
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true)
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
                name: "SystemPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    PermissionPath = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemPermissions_SystemPermissions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SystemPermissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SystemRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    NameValue = table.Column<string>(type: "text", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    Icon = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
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
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RealName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
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
                    Sex = table.Column<int>(type: "integer", nullable: false)
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
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    Avatar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionTypeMyProperty = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_SystemPermissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "SystemPermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_SystemRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SystemRoles",
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
                name: "SystemPermissionSystemRole",
                columns: table => new
                {
                    PermissionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPermissionSystemRole", x => new { x.PermissionsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_SystemPermissionSystemRole_SystemPermissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "SystemPermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemPermissionSystemRole_SystemRoles_RolesId",
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
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ActionUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TargetName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Route = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ActionType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SystemUserId = table.Column<Guid>(type: "uuid", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfigs_Key",
                table: "SystemConfigs",
                column: "Key");

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
                name: "IX_SystemPermissions_ParentId",
                table: "SystemPermissions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemPermissionSystemRole_RolesId",
                table: "SystemPermissionSystemRole",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemRoles_Name",
                table: "SystemRoles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SystemRoles_NameValue",
                table: "SystemRoles",
                column: "NameValue");

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
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_IsDeleted",
                table: "SystemUsers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_PhoneNumber",
                table: "SystemUsers",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_UserName",
                table: "SystemUsers",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedTime",
                table: "Users",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsDeleted",
                table: "Users",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "SystemConfigs");

            migrationBuilder.DropTable(
                name: "SystemLogs");

            migrationBuilder.DropTable(
                name: "SystemMenuSystemRole");

            migrationBuilder.DropTable(
                name: "SystemOrganizationSystemUser");

            migrationBuilder.DropTable(
                name: "SystemPermissionSystemRole");

            migrationBuilder.DropTable(
                name: "SystemRoleSystemUser");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SystemMenus");

            migrationBuilder.DropTable(
                name: "SystemOrganizations");

            migrationBuilder.DropTable(
                name: "SystemPermissions");

            migrationBuilder.DropTable(
                name: "SystemRoles");

            migrationBuilder.DropTable(
                name: "SystemUsers");
        }
    }
}
