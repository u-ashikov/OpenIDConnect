using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IdentityServer.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserSecret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "UserSecrets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Secret = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSecrets_Users_UserId",
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
                    { new Guid("3d4829e7-545e-4c3c-9f54-347610a8d04d"), "df9b3215-20eb-4001-9913-27c938f3e914", "role", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "ProUser" },
                    { new Guid("3df5f983-2cce-434d-817b-2e7903ed76f8"), "18598694-1fad-40ad-9e29-07202cc94cee", "country", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "bel" },
                    { new Guid("6d8d5089-71cb-4ddf-bf35-646a5d779825"), "2b489a25-f895-435f-86a2-1f76afc04fed", "given_name", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "Emma" },
                    { new Guid("9bf6f177-3eb9-420d-a0df-2fd390907a00"), "151cb348-5729-46c1-91f8-e3a85989ba65", "family_name", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "Flagg" },
                    { new Guid("b7454aa0-40bc-42a8-9003-ee95ec0a38d3"), "e8ea482c-8db6-4da9-a307-5e033bf57fcb", "family_name", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "Flagg" },
                    { new Guid("c099c557-e5a7-48cb-ba59-0e0ee6535d2f"), "65a62058-f422-47be-ba81-40d29a44fbeb", "given_name", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "David" },
                    { new Guid("cbb97702-d8ec-4f46-8f8f-99617936af5e"), "b40076f7-4ef8-4043-b01a-e1b88826a86f", "country", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "usa" },
                    { new Guid("f4453de5-4b27-4179-8fc9-eb7502a44661"), "a48a5b58-ce88-4eeb-b00e-e0ec159b675a", "role", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "FreeUser" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                column: "ConcurrencyStamp",
                value: "430afe88-68a1-426e-b7a4-a3d6fa1e0742");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                column: "ConcurrencyStamp",
                value: "21fda86a-8a4d-4fde-b430-9d7daf77798e");

            migrationBuilder.CreateIndex(
                name: "IX_UserSecrets_UserId",
                table: "UserSecrets",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSecrets");

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("3d4829e7-545e-4c3c-9f54-347610a8d04d"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("3df5f983-2cce-434d-817b-2e7903ed76f8"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("6d8d5089-71cb-4ddf-bf35-646a5d779825"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("9bf6f177-3eb9-420d-a0df-2fd390907a00"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("b7454aa0-40bc-42a8-9003-ee95ec0a38d3"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("c099c557-e5a7-48cb-ba59-0e0ee6535d2f"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("cbb97702-d8ec-4f46-8f8f-99617936af5e"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("f4453de5-4b27-4179-8fc9-eb7502a44661"));

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
        }
    }
}
