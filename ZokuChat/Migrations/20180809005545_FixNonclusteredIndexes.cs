using Microsoft.EntityFrameworkCore.Migrations;

namespace ZokuChat.Migrations
{
    public partial class FixNonclusteredIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropIndex("IX_ContactRequests_AspNetUsers_FromUID_ToUID", "ContactRequests");
			migrationBuilder.DropIndex("IX_ContactRequests_AspNetUsers_ToUID_FromUID", "ContactRequests");
			migrationBuilder.CreateIndex("IX_ContactRequests_RequestorUID_RequestedUID", "ContactRequests", new string[] { "RequestorUID", "RequestedUID" }, unique: false);
			migrationBuilder.CreateIndex("IX_ContactRequests_RequestedUID_RequestorUID", "ContactRequests", new string[] { "RequestedUID", "RequestorUID" }, unique: false);

			migrationBuilder.DropIndex("IX_Contacts_AspNetUsers_UserUID_ContactUID", "Contacts");
			migrationBuilder.CreateIndex("IX_Contacts_UserUID_ContactUID", "Contacts", new string[] { "UserUID", "ContactUID" }, unique: true);
			migrationBuilder.CreateIndex("IX_Contacts_ContactUID_UserUID", "Contacts", new string[] { "ContactUID", "UserUID" }, unique: true);

			migrationBuilder.DropIndex("IX_BlockedUsers_AspNetUsers_BlockerUID_BlockedUID", "BlockedUsers");
			migrationBuilder.DropIndex("IX_BlockedUsers_AspNetUsers_BlockedUID_BlockerUID", "BlockedUsers");
			migrationBuilder.CreateIndex("IX_BlockedUsers_BlockerUID_BlockedUID", "BlockedUsers", new string[] { "BlockerUID", "BlockedUID" }, unique: true);
			migrationBuilder.CreateIndex("IX_BlockedUsers_BlockedUID_BlockerUID", "BlockedUsers", new string[] { "BlockedUID", "BlockerUID" }, unique: true);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateIndex("IX_ContactRequests_AspNetUsers_FromUID_ToUID", "ContactRequests", new string[] { "RequestorUID", "RequestedUID" }, unique: true);
			migrationBuilder.CreateIndex("IX_ContactRequests_AspNetUsers_ToUID_FromUID", "ContactRequests", new string[] { "RequestedUID", "RequestorUID" }, unique: true);
			migrationBuilder.CreateIndex("IX_Contacts_AspNetUsers_UserUID_ContactUID", "Contacts", new string[] { "UserUID", "ContactUID" }, unique: true);
			migrationBuilder.CreateIndex("IX_BlockedUsers_AspNetUsers_BlockerUID_BlockedUID", "BlockedUsers", new string[] { "BlockerUID", "BlockedUID" }, unique: true);
			migrationBuilder.CreateIndex("IX_BlockedUsers_AspNetUsers_BlockedUID_BlockerUID", "BlockedUsers", new string[] { "BlockedUID", "BlockerUID" }, unique: true);
		}
	}
}
