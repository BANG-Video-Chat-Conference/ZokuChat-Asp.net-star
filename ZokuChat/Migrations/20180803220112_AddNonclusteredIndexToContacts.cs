using Microsoft.EntityFrameworkCore.Migrations;

namespace ZokuChat.Migrations
{
    public partial class AddNonclusteredIndexToContacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateIndex("IX_Contacts_AspNetUsers_UserUID_ContactUID", "Contacts", new string[] { "UserUID", "ContactUID" }, unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropIndex("IX_Contacts_AspNetUsers_UserUID_ContactUID", "Contacts");
        }
    }
}
