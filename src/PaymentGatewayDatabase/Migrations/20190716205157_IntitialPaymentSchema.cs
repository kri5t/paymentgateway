using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGatewayDatabase.Migrations
{
    public partial class IntitialPaymentSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Uid = table.Column<Guid>(nullable: false),
                    ObfuscatedCardNumber = table.Column<string>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    BankTransactionUid = table.Column<Guid>(nullable: false),
                    CreatedDateUtc = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
