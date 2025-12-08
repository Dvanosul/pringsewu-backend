using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pringsewu.Migrations
{
    /// <inheritdoc />
    public partial class AddEventAndGalleryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbs015_donation_event",
                columns: table => new
                {
                    donation_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    donation_event_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    donation_event_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    donation_event_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    donation_event_imgurl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    donation_event_startdate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    donation_event_enddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    donation_event_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    donation_event_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    donation_event_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    donation_event_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    donation_event_createdby = table.Column<string>(type: "text", nullable: true),
                    donation_event_updatedby = table.Column<string>(type: "text", nullable: true),
                    donation_event_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs015_donation_event", x => x.donation_event_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs015_donation_gallery",
                columns: table => new
                {
                    donation_gallery_id = table.Column<Guid>(type: "uuid", nullable: false),
                    donation_gallery_eventid = table.Column<Guid>(type: "uuid", nullable: false),
                    donation_gallery_imgurl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    donation_gallery_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    donation_gallery_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    donation_gallery_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    donation_gallery_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    donation_gallery_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    donation_gallery_createdby = table.Column<string>(type: "text", nullable: true),
                    donation_gallery_updatedby = table.Column<string>(type: "text", nullable: true),
                    donation_gallery_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs015_donation_gallery", x => x.donation_gallery_id);
                    table.ForeignKey(
                        name: "FK_dbs015_donation_gallery_dbs015_donation_event_donation_gall~",
                        column: x => x.donation_gallery_eventid,
                        principalTable: "dbs015_donation_event",
                        principalColumn: "donation_event_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_donation_gallery_donation_gallery_eventid",
                table: "dbs015_donation_gallery",
                column: "donation_gallery_eventid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbs015_donation_gallery");

            migrationBuilder.DropTable(
                name: "dbs015_donation_event");
        }
    }
}
