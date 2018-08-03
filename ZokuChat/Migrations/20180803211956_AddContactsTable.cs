using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZokuChat.Migrations
{
    public partial class AddContactsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserUID = table.Column<string>(maxLength: 450, nullable: true),
                    ContactUID = table.Column<string>(maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
					table.ForeignKey("FK_Contacts_AspNetUsers_UserUID", c => c.UserUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_Contacts_AspNetUsers_ContactUID", c => c.ContactUID, "AspNetUsers", "Id");
				});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
