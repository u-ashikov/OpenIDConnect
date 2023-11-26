using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IdentityServer.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedActivationCodeAndExpiration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("1c766551-7716-4741-9955-b8a8914c39a9"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("4baf61d0-d6f1-446f-b4eb-38a43af06be9"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("8f2557ff-a3d4-43b6-8e9f-4fab396d476f"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("9ec94538-29a8-4b03-8cc9-aca97548a895"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("9edb9c91-745d-4602-8c5b-19538d969600"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("d8222c4e-fb18-4116-99fc-00f9e8470ddc"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("dd239b7d-106d-4bf5-b9a4-2b516c6e45dc"));

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("e91e3480-33dc-4e4d-893e-a644c9ab18f7"));

            migrationBuilder.AddColumn<string>(
                name: "ActivationCode",
                table: "Users",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivationCodeExpirationDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                columns: new[] { "ActivationCode", "ActivationCodeExpirationDate", "ConcurrencyStamp" },
                values: new object[] { null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "51f2557c-5182-445d-b9ed-c216d2d7a2a5" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                columns: new[] { "ActivationCode", "ActivationCodeExpirationDate", "ConcurrencyStamp" },
                values: new object[] { null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "009de95b-32d7-4a35-8ec0-8958089967c7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ActivationCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ActivationCodeExpirationDate",
                table: "Users");

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "ConcurrencyStamp", "Type", "UserId", "Value" },
                values: new object[,]
                {
                    { new Guid("1c766551-7716-4741-9955-b8a8914c39a9"), "4c8854e6-5e04-4177-acc7-5446cc14d6de", "given_name", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "Emma" },
                    { new Guid("4baf61d0-d6f1-446f-b4eb-38a43af06be9"), "3818df3e-1118-456f-8130-3f5264f0537f", "family_name", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "Flagg" },
                    { new Guid("8f2557ff-a3d4-43b6-8e9f-4fab396d476f"), "8e1cb145-a9d5-4080-b47a-16b6850d0e70", "role", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "FreeUser" },
                    { new Guid("9ec94538-29a8-4b03-8cc9-aca97548a895"), "1b23cdc9-56c9-46e8-a873-9c76a50334a1", "role", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "ProUser" },
                    { new Guid("9edb9c91-745d-4602-8c5b-19538d969600"), "accaaf16-230e-48c9-99f9-32982901d8a0", "country", new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"), "bel" },
                    { new Guid("d8222c4e-fb18-4116-99fc-00f9e8470ddc"), "5a193b1d-d67f-4a97-8002-430375697e03", "country", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "usa" },
                    { new Guid("dd239b7d-106d-4bf5-b9a4-2b516c6e45dc"), "71827c3d-46a4-4aa1-956b-7718fc20fad0", "family_name", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "Flagg" },
                    { new Guid("e91e3480-33dc-4e4d-893e-a644c9ab18f7"), "02b7718d-946b-4349-9507-80149bcc32fb", "given_name", new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"), "David" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("13229d33-99e0-41b3-b18d-4f72127e3971"),
                column: "ConcurrencyStamp",
                value: "4389ced4-d4d1-47d5-8312-b3866afa658e");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("96053525-f4a5-47ee-855e-0ea77fa6c55a"),
                column: "ConcurrencyStamp",
                value: "b31cc52b-9a4e-40aa-b32b-5b328a910943");
        }
    }
}
