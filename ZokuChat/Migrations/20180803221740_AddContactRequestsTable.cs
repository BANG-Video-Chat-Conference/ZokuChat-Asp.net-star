using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZokuChat.Migrations
{
    public partial class AddContactRequestsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FromUID = table.Column<string>(maxLength: 450, nullable: false),
                    ToUID = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedUID = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedUID = table.Column<string>(maxLength: 450, nullable: false),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: false),
                    IsConfirmed = table.Column<bool>(nullable: false),
                    IsCancelled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactRequests", x => x.Id);
					table.ForeignKey("FK_ContactRequests_AspNetUsers_FromUID", c => c.FromUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_ContactRequests_AspNetUsers_ToUID", c => c.ToUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_ContactRequests_AspNetUsers_CreatedUID", c => c.CreatedUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_ContactRequests_AspNetUsers_ModifiedUID", c => c.ModifiedUID, "AspNetUsers", "Id");
				});

			migrationBuilder.CreateIndex("IX_ContactRequests_AspNetUsers_FromUID_ToUID", "ContactRequests", new string[] { "FromUID", "ToUID" }, unique: true);
			migrationBuilder.CreateIndex("IX_ContactRequests_AspNetUsers_ToUID_FromUID", "ContactRequests", new string[] { "ToUID", "FromUID" }, unique: true);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactRequests");
        }
    }
}
