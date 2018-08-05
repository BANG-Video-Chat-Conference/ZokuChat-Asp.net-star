using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZokuChat.Migrations
{
    public partial class AddBlockedUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlockedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlockerUID = table.Column<string>(maxLength: 450, nullable: false),
                    BlockedUID = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedUID = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedUID = table.Column<string>(maxLength: 450, nullable: false),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedUsers", b => b.Id);
					table.ForeignKey("FK_BlockedUsers_AspNetUsers_BlockerUID", b => b.BlockerUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_BlockedUsers_AspNetUsers_BlockedUID", b => b.BlockedUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_BlockedUsers_AspNetUsers_CreatedUID", b => b.CreatedUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_BlockedUsers_AspNetUsers_ModifiedUID", b => b.ModifiedUID, "AspNetUsers", "Id");
				});

			migrationBuilder.CreateIndex("IX_BlockedUsers_AspNetUsers_BlockerUID_BlockedUID", "BlockedUsers", new string[] { "BlockerUID", "BlockedUID" }, unique: true);
			migrationBuilder.CreateIndex("IX_BlockedUsers_AspNetUsers_BlockedUID_BlockerUID", "BlockedUsers", new string[] { "BlockedUID", "BlockerUID" }, unique: true);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockedUsers");
        }
    }
}
