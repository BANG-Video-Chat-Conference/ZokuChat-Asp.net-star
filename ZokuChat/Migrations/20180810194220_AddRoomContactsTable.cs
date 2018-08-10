using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZokuChat.Migrations
{
    public partial class AddRoomContactsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoomId = table.Column<int>(nullable: false),
                    ContactUID = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedUID = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedUID = table.Column<string>(maxLength: 450, nullable: false),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomContacts", rc => rc.Id);
					table.ForeignKey("FK_RoomContacts_AspNetUsers_ContactUID", rc => rc.ContactUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_RoomContacts_AspNetUsers_CreatedUID", rc => rc.CreatedUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_RoomContacts_AspNetUsers_ModifiedUID", rc => rc.ModifiedUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_RoomContacts_Rooms_RoomId", rc => rc.RoomId, "Rooms", "Id");
				});

			migrationBuilder.CreateIndex("IX_RoomContacts_RoomId_ContactUID", "RoomContacts", new string[] { "RoomId", "ContactUID" }, unique: true);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomContacts");
        }
    }
}
