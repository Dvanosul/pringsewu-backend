using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pringsewu.Migrations
{
    /// <inheritdoc />
    public partial class AddCategorytoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dbs015_donation_gallery_dbs015_donation_event_donation_gall~",
                table: "dbs015_donation_gallery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dbs015_donation_gallery",
                table: "dbs015_donation_gallery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dbs015_donation_event",
                table: "dbs015_donation_event");

            migrationBuilder.RenameTable(
                name: "dbs015_donation_gallery",
                newName: "dbs015_donationgallery");

            migrationBuilder.RenameTable(
                name: "dbs015_donation_event",
                newName: "dbs015_donationevent");

            migrationBuilder.RenameColumn(
                name: "donation_gallery_updateddate",
                table: "dbs015_donationgallery",
                newName: "donationgallery_updateddate");

            migrationBuilder.RenameColumn(
                name: "donation_gallery_updatedby",
                table: "dbs015_donationgallery",
                newName: "donationgallery_updatedby");

            migrationBuilder.RenameColumn(
                name: "donation_gallery_isactive",
                table: "dbs015_donationgallery",
                newName: "donationgallery_isactive");

            migrationBuilder.RenameColumn(
                name: "donation_gallery_imgurl",
                table: "dbs015_donationgallery",
                newName: "donationgallery_imgurl");

            migrationBuilder.RenameColumn(
                name: "donation_gallery_eventid",
                table: "dbs015_donationgallery",
                newName: "donationgallery_eventid");

            migrationBuilder.RenameColumn(
                name: "donation_gallery_description",
                table: "dbs015_donationgallery",
                newName: "donationgallery_description");

            migrationBuilder.RenameColumn(
                name: "donation_gallery_deleteddate",
                table: "dbs015_donationgallery",
                newName: "donationgallery_deleteddate");

            migrationBuilder.RenameColumn(
                name: "donation_gallery_deletedby",
                table: "dbs015_donationgallery",
                newName: "donationgallery_deletedby");

            migrationBuilder.RenameColumn(
                name: "donation_gallery_createddate",
                table: "dbs015_donationgallery",
                newName: "donationgallery_createddate");

            migrationBuilder.RenameColumn(
                name: "donation_gallery_createdby",
                table: "dbs015_donationgallery",
                newName: "donationgallery_createdby");

            migrationBuilder.RenameColumn(
                name: "donation_gallery_id",
                table: "dbs015_donationgallery",
                newName: "donationgallery_id");

            migrationBuilder.RenameIndex(
                name: "IX_dbs015_donation_gallery_donation_gallery_eventid",
                table: "dbs015_donationgallery",
                newName: "IX_dbs015_donationgallery_donationgallery_eventid");

            migrationBuilder.RenameColumn(
                name: "donation_event_updateddate",
                table: "dbs015_donationevent",
                newName: "donationevent_updateddate");

            migrationBuilder.RenameColumn(
                name: "donation_event_updatedby",
                table: "dbs015_donationevent",
                newName: "donationevent_updatedby");

            migrationBuilder.RenameColumn(
                name: "donation_event_startdate",
                table: "dbs015_donationevent",
                newName: "donationevent_startdate");

            migrationBuilder.RenameColumn(
                name: "donation_event_name",
                table: "dbs015_donationevent",
                newName: "donationevent_name");

            migrationBuilder.RenameColumn(
                name: "donation_event_isactive",
                table: "dbs015_donationevent",
                newName: "donationevent_isactive");

            migrationBuilder.RenameColumn(
                name: "donation_event_imgurl",
                table: "dbs015_donationevent",
                newName: "donationevent_imgurl");

            migrationBuilder.RenameColumn(
                name: "donation_event_enddate",
                table: "dbs015_donationevent",
                newName: "donationevent_enddate");

            migrationBuilder.RenameColumn(
                name: "donation_event_description",
                table: "dbs015_donationevent",
                newName: "donationevent_description");

            migrationBuilder.RenameColumn(
                name: "donation_event_deleteddate",
                table: "dbs015_donationevent",
                newName: "donationevent_deleteddate");

            migrationBuilder.RenameColumn(
                name: "donation_event_deletedby",
                table: "dbs015_donationevent",
                newName: "donation_vent_deletedby");

            migrationBuilder.RenameColumn(
                name: "donation_event_createddate",
                table: "dbs015_donationevent",
                newName: "donationevent_createddate");

            migrationBuilder.RenameColumn(
                name: "donation_event_createdby",
                table: "dbs015_donationevent",
                newName: "donationevent_createdby");

            migrationBuilder.RenameColumn(
                name: "donation_event_code",
                table: "dbs015_donationevent",
                newName: "donationevent_code");

            migrationBuilder.RenameColumn(
                name: "donation_event_id",
                table: "dbs015_donationevent",
                newName: "donationevent_id");

            migrationBuilder.AddColumn<Guid>(
                name: "donationevent_categoryid",
                table: "dbs015_donationevent",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_dbs015_donationgallery",
                table: "dbs015_donationgallery",
                column: "donationgallery_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dbs015_donationevent",
                table: "dbs015_donationevent",
                column: "donationevent_id");

            migrationBuilder.CreateTable(
                name: "dbs015_category",
                columns: table => new
                {
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    category_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    category_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    category_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    category_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    category_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    category_createdby = table.Column<string>(type: "text", nullable: true),
                    category_updatedby = table.Column<string>(type: "text", nullable: true),
                    category_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs015_category", x => x.category_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_donationevent_donationevent_categoryid",
                table: "dbs015_donationevent",
                column: "donationevent_categoryid");

            migrationBuilder.AddForeignKey(
                name: "FK_dbs015_donationevent_dbs015_category_donationevent_category~",
                table: "dbs015_donationevent",
                column: "donationevent_categoryid",
                principalTable: "dbs015_category",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dbs015_donationgallery_dbs015_donationevent_donationgallery~",
                table: "dbs015_donationgallery",
                column: "donationgallery_eventid",
                principalTable: "dbs015_donationevent",
                principalColumn: "donationevent_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dbs015_donationevent_dbs015_category_donationevent_category~",
                table: "dbs015_donationevent");

            migrationBuilder.DropForeignKey(
                name: "FK_dbs015_donationgallery_dbs015_donationevent_donationgallery~",
                table: "dbs015_donationgallery");

            migrationBuilder.DropTable(
                name: "dbs015_category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dbs015_donationgallery",
                table: "dbs015_donationgallery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dbs015_donationevent",
                table: "dbs015_donationevent");

            migrationBuilder.DropIndex(
                name: "IX_dbs015_donationevent_donationevent_categoryid",
                table: "dbs015_donationevent");

            migrationBuilder.DropColumn(
                name: "donationevent_categoryid",
                table: "dbs015_donationevent");

            migrationBuilder.RenameTable(
                name: "dbs015_donationgallery",
                newName: "dbs015_donation_gallery");

            migrationBuilder.RenameTable(
                name: "dbs015_donationevent",
                newName: "dbs015_donation_event");

            migrationBuilder.RenameColumn(
                name: "donationgallery_updateddate",
                table: "dbs015_donation_gallery",
                newName: "donation_gallery_updateddate");

            migrationBuilder.RenameColumn(
                name: "donationgallery_updatedby",
                table: "dbs015_donation_gallery",
                newName: "donation_gallery_updatedby");

            migrationBuilder.RenameColumn(
                name: "donationgallery_isactive",
                table: "dbs015_donation_gallery",
                newName: "donation_gallery_isactive");

            migrationBuilder.RenameColumn(
                name: "donationgallery_imgurl",
                table: "dbs015_donation_gallery",
                newName: "donation_gallery_imgurl");

            migrationBuilder.RenameColumn(
                name: "donationgallery_eventid",
                table: "dbs015_donation_gallery",
                newName: "donation_gallery_eventid");

            migrationBuilder.RenameColumn(
                name: "donationgallery_description",
                table: "dbs015_donation_gallery",
                newName: "donation_gallery_description");

            migrationBuilder.RenameColumn(
                name: "donationgallery_deleteddate",
                table: "dbs015_donation_gallery",
                newName: "donation_gallery_deleteddate");

            migrationBuilder.RenameColumn(
                name: "donationgallery_deletedby",
                table: "dbs015_donation_gallery",
                newName: "donation_gallery_deletedby");

            migrationBuilder.RenameColumn(
                name: "donationgallery_createddate",
                table: "dbs015_donation_gallery",
                newName: "donation_gallery_createddate");

            migrationBuilder.RenameColumn(
                name: "donationgallery_createdby",
                table: "dbs015_donation_gallery",
                newName: "donation_gallery_createdby");

            migrationBuilder.RenameColumn(
                name: "donationgallery_id",
                table: "dbs015_donation_gallery",
                newName: "donation_gallery_id");

            migrationBuilder.RenameIndex(
                name: "IX_dbs015_donationgallery_donationgallery_eventid",
                table: "dbs015_donation_gallery",
                newName: "IX_dbs015_donation_gallery_donation_gallery_eventid");

            migrationBuilder.RenameColumn(
                name: "donationevent_updateddate",
                table: "dbs015_donation_event",
                newName: "donation_event_updateddate");

            migrationBuilder.RenameColumn(
                name: "donationevent_updatedby",
                table: "dbs015_donation_event",
                newName: "donation_event_updatedby");

            migrationBuilder.RenameColumn(
                name: "donationevent_startdate",
                table: "dbs015_donation_event",
                newName: "donation_event_startdate");

            migrationBuilder.RenameColumn(
                name: "donationevent_name",
                table: "dbs015_donation_event",
                newName: "donation_event_name");

            migrationBuilder.RenameColumn(
                name: "donationevent_isactive",
                table: "dbs015_donation_event",
                newName: "donation_event_isactive");

            migrationBuilder.RenameColumn(
                name: "donationevent_imgurl",
                table: "dbs015_donation_event",
                newName: "donation_event_imgurl");

            migrationBuilder.RenameColumn(
                name: "donationevent_enddate",
                table: "dbs015_donation_event",
                newName: "donation_event_enddate");

            migrationBuilder.RenameColumn(
                name: "donationevent_description",
                table: "dbs015_donation_event",
                newName: "donation_event_description");

            migrationBuilder.RenameColumn(
                name: "donationevent_deleteddate",
                table: "dbs015_donation_event",
                newName: "donation_event_deleteddate");

            migrationBuilder.RenameColumn(
                name: "donationevent_createddate",
                table: "dbs015_donation_event",
                newName: "donation_event_createddate");

            migrationBuilder.RenameColumn(
                name: "donationevent_createdby",
                table: "dbs015_donation_event",
                newName: "donation_event_createdby");

            migrationBuilder.RenameColumn(
                name: "donationevent_code",
                table: "dbs015_donation_event",
                newName: "donation_event_code");

            migrationBuilder.RenameColumn(
                name: "donation_vent_deletedby",
                table: "dbs015_donation_event",
                newName: "donation_event_deletedby");

            migrationBuilder.RenameColumn(
                name: "donationevent_id",
                table: "dbs015_donation_event",
                newName: "donation_event_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dbs015_donation_gallery",
                table: "dbs015_donation_gallery",
                column: "donation_gallery_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dbs015_donation_event",
                table: "dbs015_donation_event",
                column: "donation_event_id");

            migrationBuilder.AddForeignKey(
                name: "FK_dbs015_donation_gallery_dbs015_donation_event_donation_gall~",
                table: "dbs015_donation_gallery",
                column: "donation_gallery_eventid",
                principalTable: "dbs015_donation_event",
                principalColumn: "donation_event_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
