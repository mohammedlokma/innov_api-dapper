using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace innov_api.Migrations
{
    public partial class deleteTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableName",
                table: "Verbs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "Verbs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
