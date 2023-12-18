using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IdentityServer.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("06215b69-cefc-429b-a4a9-2ad1f9ea173b"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("272b96ea-7c86-45cb-86b4-0590468c37b5"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("333b956e-aa1b-4cab-b4d7-7721d2f57c48"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("46c4fe53-2b0a-47d8-a6f1-5a30eab0288c"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("64f9efb5-845c-4ce7-8d3e-6bb5f47eaa42"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("7032d6fb-a7fa-4b11-99f7-f120ad7c8e8a"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("bdaee683-8365-44c9-a028-418be79143cd"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("d1cfd535-01b2-46a0-a5fd-7cba6ffcd9b0"));

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Provider = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ProviderIdentityKey = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "ConcurrencyStamp", "Type", "UserId", "Value" },
                values: new object[,]
                {
                    { new Guid("05ad4f14-104e-45a4-acc3-c7825fd7f206"), "797c13bb-fd96-4b76-93df-e5e751b66478", "given_name", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "David" },
                    { new Guid("1f158567-76a3-4064-9177-614c2670a45e"), "bb29d951-92e6-45ae-9c3b-3482f8e9fdf7", "family_name", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "Flagg" },
                    { new Guid("3a30219a-63f2-488c-b575-1924b2a8ffb3"), "30ac9d70-3720-4252-beec-90fb9290baf2", "country", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "usa" },
                    { new Guid("450046bc-8c06-4d96-879e-2e7d50f1151b"), "0252bbe2-b4de-4c8f-85f5-729d88030380", "given_name", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "Emma" },
                    { new Guid("451195a9-c212-4b21-96e5-a1909e9f8a19"), "6afb669f-8cac-4d2d-87ff-ef929a724e2e", "role", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "ProUser" },
                    { new Guid("c4f71c46-1c7f-435b-9621-0e8d43d06783"), "e038e7d1-12d7-40d3-a20b-32a21e85cd08", "country", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "bel" },
                    { new Guid("c8dc7d20-6768-40ce-bffe-0e9b5cce69fa"), "e00cdbd6-3276-4c34-a92f-264366833834", "role", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "FreeUser" },
                    { new Guid("f7f81a17-8d2b-42aa-8f26-31a57c015e98"), "7c9023c0-17e0-40f6-83ab-26dd6c6cbba4", "family_name", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "Flagg" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                column: "ConcurrencyStamp",
                value: "0f2bcf31-c6a0-4f2e-9d6a-2df9f51658fb");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                column: "ConcurrencyStamp",
                value: "2a9e7326-7755-495d-a506-85b8734d839c");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("05ad4f14-104e-45a4-acc3-c7825fd7f206"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("1f158567-76a3-4064-9177-614c2670a45e"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("3a30219a-63f2-488c-b575-1924b2a8ffb3"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("450046bc-8c06-4d96-879e-2e7d50f1151b"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("451195a9-c212-4b21-96e5-a1909e9f8a19"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("c4f71c46-1c7f-435b-9621-0e8d43d06783"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("c8dc7d20-6768-40ce-bffe-0e9b5cce69fa"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("f7f81a17-8d2b-42aa-8f26-31a57c015e98"));

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "ConcurrencyStamp", "Type", "UserId", "Value" },
                values: new object[,]
                {
                    { new Guid("06215b69-cefc-429b-a4a9-2ad1f9ea173b"), "d7cba8a6-c748-4b6d-8249-99bd82fce552", "given_name", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "Emma" },
                    { new Guid("272b96ea-7c86-45cb-86b4-0590468c37b5"), "1bf45ca8-c7eb-4b11-92cd-9cde88052c1e", "given_name", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "David" },
                    { new Guid("333b956e-aa1b-4cab-b4d7-7721d2f57c48"), "d713bda9-69e3-4379-9854-0c76a3584867", "role", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "FreeUser" },
                    { new Guid("46c4fe53-2b0a-47d8-a6f1-5a30eab0288c"), "5ba11d3d-62cb-42b4-9d7e-61409ae0c156", "family_name", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "Flagg" },
                    { new Guid("64f9efb5-845c-4ce7-8d3e-6bb5f47eaa42"), "f60861fc-1806-4d3b-a85e-4a66aea81b6f", "country", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "usa" },
                    { new Guid("7032d6fb-a7fa-4b11-99f7-f120ad7c8e8a"), "ffc35121-3371-4d46-bf3b-5b74003993ab", "family_name", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "Flagg" },
                    { new Guid("bdaee683-8365-44c9-a028-418be79143cd"), "e1f443ab-c82e-41dc-89ef-8d8f2cc2fe83", "country", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "bel" },
                    { new Guid("d1cfd535-01b2-46a0-a5fd-7cba6ffcd9b0"), "3554d14d-cfcf-4103-a715-66f945f2eb71", "role", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "ProUser" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                column: "ConcurrencyStamp",
                value: "51f2557c-5182-445d-b9ed-c216d2d7a2a5");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                column: "ConcurrencyStamp",
                value: "009de95b-32d7-4a35-8ec0-8958089967c7");
        }
    }
}
