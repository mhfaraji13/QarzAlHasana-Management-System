using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QarzAlHasana.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectionReasonToMembershipPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "MembershipPayments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "MembershipPayments");
        }
    }
}
