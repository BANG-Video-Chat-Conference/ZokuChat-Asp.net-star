using Microsoft.EntityFrameworkCore.Migrations;

namespace ZokuChat.Migrations
{
    public partial class ChangedContactRequestColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToUID",
                table: "ContactRequests",
                newName: "RequestorUID");

            migrationBuilder.RenameColumn(
                name: "FromUID",
                table: "ContactRequests",
                newName: "RequestedUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestorUID",
                table: "ContactRequests",
                newName: "ToUID");

            migrationBuilder.RenameColumn(
                name: "RequestedUID",
                table: "ContactRequests",
                newName: "FromUID");
        }
    }
}
