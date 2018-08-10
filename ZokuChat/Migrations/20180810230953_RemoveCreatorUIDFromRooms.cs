using Microsoft.EntityFrameworkCore.Migrations;

namespace ZokuChat.Migrations
{
    public partial class RemoveCreatorUIDFromRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropForeignKey("FK_Rooms_AspNetUsers_CreatorUID", "Rooms");
			migrationBuilder.DropIndex("IX_Rooms_CreatorUID_IsDeleted", "Rooms");
			migrationBuilder.CreateIndex("IX_Rooms_CreatedUID_IsDeleted", "Rooms", new string[] { "CreatedUID", "IsDeleted" }, unique: false);

			migrationBuilder.DropColumn(
                name: "CreatorUID",
                table: "Rooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<string>(
                name: "CreatorUID",
                table: "Rooms",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

			migrationBuilder.AddForeignKey("FK_Rooms_AspNetUsers_CreatorUID", "Rooms", "CreatorUID", "AspNetUsers", principalColumn: "Id");
			migrationBuilder.CreateIndex("IX_Rooms_CreatorUID_IsDeleted", "Rooms", new string[] { "CreatorUID", "IsDeleted" }, unique: false);
		}
    }
}
