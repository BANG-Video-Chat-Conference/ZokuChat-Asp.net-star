using Microsoft.EntityFrameworkCore.Migrations;

namespace ZokuChat.Migrations
{
    public partial class AddCreatedModifiedFKToMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddForeignKey(
				name: "FK_Messages_AspNetUsers_CreatedUID",
				table: "Messages",
				column: "CreatedUID",
				principalTable: "AspNetUsers",
				principalColumn: "Id");
			migrationBuilder.AddForeignKey(
				name: "FK_Messages_AspNetUsers_ModifiedUID",
				table: "Messages",
				column: "ModifiedUID",
				principalTable: "AspNetUsers",
				principalColumn: "Id");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropForeignKey(name: "FK_Messages_AspNetUsers_CreatedUID", table: "Messages");
			migrationBuilder.DropForeignKey(name: "FK_Messages_AspNetUsers_ModifiedUID", table: "Messages");
		}
    }
}
