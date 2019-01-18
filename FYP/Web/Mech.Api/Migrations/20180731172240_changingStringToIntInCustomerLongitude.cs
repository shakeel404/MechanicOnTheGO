using Microsoft.EntityFrameworkCore.Migrations;

namespace Mech.Api.Migrations
{
    public partial class changingStringToIntInCustomerLongitude : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Customers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(double));
        }
    }
}
