using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalWeb.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEducation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ToDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Degree = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Institution = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    City = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Country = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    CreateBy = table.Column<string>(type: "character varying(216)", maxLength: 216, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(216)", maxLength: 216, nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDraft = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Education");
        }
    }
}
