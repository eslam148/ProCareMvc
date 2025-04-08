using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProCareMvc.Database.Migrations
{
    /// <inheritdoc />
    public partial class farah : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageProfileUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageProfileUrl",
                table: "AspNetUsers");
        }
    }
}
