using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NdcProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NDC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DosageForm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PackageSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NdcProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NPI = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RxAdmins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RxAdmins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriorAuthorizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorizationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    NdcProductId = table.Column<int>(type: "int", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriorAuthorizations_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriorAuthorizations_NdcProducts_NdcProductId",
                        column: x => x.NdcProductId,
                        principalTable: "NdcProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriorAuthorizations_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RxClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    NdcProductId = table.Column<int>(type: "int", nullable: false),
                    DateOfService = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AmountBilled = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RxClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RxClaims_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RxClaims_NdcProducts_NdcProductId",
                        column: x => x.NdcProductId,
                        principalTable: "NdcProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RxClaims_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClaimSubmitRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RxClaimId = table.Column<int>(type: "int", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubmissionStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimSubmitRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimSubmitRecords_RxClaims_RxClaimId",
                        column: x => x.RxClaimId,
                        principalTable: "RxClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinanceTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RxClaimId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinanceTransactions_RxClaims_RxClaimId",
                        column: x => x.RxClaimId,
                        principalTable: "RxClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OperationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OperationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RxClaimId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationRecords_RxClaims_RxClaimId",
                        column: x => x.RxClaimId,
                        principalTable: "RxClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RxDURRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RxClaimId = table.Column<int>(type: "int", nullable: false),
                    AlertCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AlertDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AlertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RxDURRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RxDURRecords_RxClaims_RxClaimId",
                        column: x => x.RxClaimId,
                        principalTable: "RxClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimSubmitRecords_RxClaimId",
                table: "ClaimSubmitRecords",
                column: "RxClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_FinanceTransactions_RxClaimId",
                table: "FinanceTransactions",
                column: "RxClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_FinanceTransactions_TransactionNumber",
                table: "FinanceTransactions",
                column: "TransactionNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_MemberNumber",
                table: "Members",
                column: "MemberNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NdcProducts_NDC",
                table: "NdcProducts",
                column: "NDC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationRecords_RxClaimId",
                table: "OperationRecords",
                column: "RxClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorAuthorizations_AuthorizationNumber",
                table: "PriorAuthorizations",
                column: "AuthorizationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriorAuthorizations_MemberId",
                table: "PriorAuthorizations",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorAuthorizations_NdcProductId",
                table: "PriorAuthorizations",
                column: "NdcProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorAuthorizations_ProviderId",
                table: "PriorAuthorizations",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_NPI",
                table: "Providers",
                column: "NPI");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_ProviderNumber",
                table: "Providers",
                column: "ProviderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RxClaims_ClaimNumber",
                table: "RxClaims",
                column: "ClaimNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RxClaims_MemberId",
                table: "RxClaims",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_RxClaims_NdcProductId",
                table: "RxClaims",
                column: "NdcProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RxClaims_ProviderId",
                table: "RxClaims",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_RxDURRecords_RxClaimId",
                table: "RxDURRecords",
                column: "RxClaimId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimSubmitRecords");

            migrationBuilder.DropTable(
                name: "FinanceTransactions");

            migrationBuilder.DropTable(
                name: "OperationRecords");

            migrationBuilder.DropTable(
                name: "PriorAuthorizations");

            migrationBuilder.DropTable(
                name: "RxAdmins");

            migrationBuilder.DropTable(
                name: "RxDURRecords");

            migrationBuilder.DropTable(
                name: "RxClaims");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "NdcProducts");

            migrationBuilder.DropTable(
                name: "Providers");
        }
    }
}
