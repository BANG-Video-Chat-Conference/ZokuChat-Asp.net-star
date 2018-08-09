using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZokuChat.Migrations
{
    public partial class AddRoomsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 80, nullable: false),
					Description = table.Column<string>(maxLength: 300, nullable: true),
					CreatorUID = table.Column<string>(maxLength: 450, nullable: false),
					CreatedUID = table.Column<string>(maxLength: 450, nullable: false),
					CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedUID = table.Column<string>(maxLength: 450, nullable: false),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", r => r.Id);
					table.ForeignKey("FK_Rooms_AspNetUsers_CreatorUID", r => r.CreatorUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_Rooms_AspNetUsers_CreatedUID", r => r.CreatedUID, "AspNetUsers", "Id");
					table.ForeignKey("FK_Rooms_AspNetUsers_ModifiedUID", r => r.ModifiedUID, "AspNetUsers", "Id");
				});

			migrationBuilder.CreateIndex("IX_Rooms_CreatorUID_IsDeleted", "Rooms", new string[] { "CreatorUID", "IsDeleted" }, unique: false);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
