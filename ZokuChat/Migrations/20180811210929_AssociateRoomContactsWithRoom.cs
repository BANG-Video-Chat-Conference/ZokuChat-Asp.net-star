using Microsoft.EntityFrameworkCore.Migrations;

namespace ZokuChat.Migrations
{
    public partial class AssociateRoomContactsWithRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RoomContacts_RoomId",
                table: "RoomContacts",
                column: "RoomId");

			migrationBuilder.DropForeignKey(
				name: "FK_RoomContacts_Rooms_RoomId",
				table: "RoomContacts");

			migrationBuilder.AddForeignKey(
                name: "FK_RoomContacts_Rooms_RoomId",
                table: "RoomContacts",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomContacts_Rooms_RoomId",
                table: "RoomContacts");

			migrationBuilder.AddForeignKey(
				name: "FK_RoomContacts_Rooms_RoomId",
				table: "RoomContacts",
				column: "RoomId",
				principalTable: "Rooms",
				principalColumn: "Id");

			migrationBuilder.DropIndex(
                name: "IX_RoomContacts_RoomId",
                table: "RoomContacts");
        }
    }
}
