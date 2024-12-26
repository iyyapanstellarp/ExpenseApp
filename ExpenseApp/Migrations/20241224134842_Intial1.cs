using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseApp.Migrations
{
    /// <inheritdoc />
    public partial class Intial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EXPexpensemaster",
                columns: table => new
                {
                    ExpenseName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpenseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPexpensemaster", x => x.ExpenseName);
                });

            migrationBuilder.CreateTable(
                name: "EXPmembermaster",
                columns: table => new
                {
                    Groupname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Membersname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Contributionamaount = table.Column<decimal>(type: "money", nullable: false),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPmembermaster", x => new { x.Groupname, x.Membersname });
                });

            migrationBuilder.CreateTable(
                name: "EXPusermaster",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Mobilenumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPusermaster", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "EXPgroupmaster",
                columns: table => new
                {
                    GroupName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Budget = table.Column<decimal>(type: "money", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    GroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPgroupmaster", x => x.GroupName);
                    table.ForeignKey(
                        name: "FK_EXPgroupmaster_EXPusermaster_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "EXPusermaster",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EXPexpense",
                columns: table => new
                {
                    ExpenseName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Spent = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPexpense", x => new { x.ExpenseName, x.GroupName });
                    table.ForeignKey(
                        name: "FK_EXPexpense_EXPexpensemaster_ExpenseName",
                        column: x => x.ExpenseName,
                        principalTable: "EXPexpensemaster",
                        principalColumn: "ExpenseName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EXPexpense_EXPgroupmaster_GroupName",
                        column: x => x.GroupName,
                        principalTable: "EXPgroupmaster",
                        principalColumn: "GroupName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EXPexpense_GroupName",
                table: "EXPexpense",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_EXPgroupmaster_CreatedBy",
                table: "EXPgroupmaster",
                column: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EXPexpense");

            migrationBuilder.DropTable(
                name: "EXPmembermaster");

            migrationBuilder.DropTable(
                name: "EXPexpensemaster");

            migrationBuilder.DropTable(
                name: "EXPgroupmaster");

            migrationBuilder.DropTable(
                name: "EXPusermaster");
        }
    }
}
