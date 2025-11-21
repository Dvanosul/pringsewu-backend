using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pringsewu.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbs000_developer",
                columns: table => new
                {
                    developer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    developer_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    developer_code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    developer_phone = table.Column<string>(type: "text", nullable: false),
                    developer_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    developer_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    developer_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    developer_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    developer_createdby = table.Column<string>(type: "text", nullable: true),
                    developer_updatedby = table.Column<string>(type: "text", nullable: true),
                    developer_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_developer", x => x.developer_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_event",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    event_code = table.Column<string>(type: "text", nullable: false),
                    event_isdefault = table.Column<bool>(type: "boolean", nullable: false),
                    event_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    event_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    event_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    event_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    event_createdby = table.Column<string>(type: "text", nullable: true),
                    event_updatedby = table.Column<string>(type: "text", nullable: true),
                    event_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_event", x => x.event_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_language",
                columns: table => new
                {
                    language_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    language_code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    language_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    language_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    language_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    language_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    language_createdby = table.Column<string>(type: "text", nullable: true),
                    language_updatedby = table.Column<string>(type: "text", nullable: true),
                    language_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_language", x => x.language_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_menu",
                columns: table => new
                {
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    menu_code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    menu_internalcode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    menu_number = table.Column<string>(type: "text", nullable: false),
                    menu_description = table.Column<string>(type: "text", nullable: false),
                    menu_icon = table.Column<string>(type: "text", nullable: false),
                    menu_isexpanded = table.Column<bool>(type: "boolean", nullable: false),
                    menu_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    menu_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    menu_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    menu_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    menu_createdby = table.Column<string>(type: "text", nullable: true),
                    menu_updatedby = table.Column<string>(type: "text", nullable: true),
                    menu_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_menu", x => x.menu_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_page",
                columns: table => new
                {
                    page_id = table.Column<Guid>(type: "uuid", nullable: false),
                    page_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    page_code = table.Column<string>(type: "text", nullable: false),
                    page_description = table.Column<string>(type: "text", nullable: false),
                    page_classname = table.Column<string>(type: "text", nullable: false),
                    page_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    page_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    page_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_createdby = table.Column<string>(type: "text", nullable: true),
                    page_updatedby = table.Column<string>(type: "text", nullable: true),
                    page_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_page", x => x.page_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_profile",
                columns: table => new
                {
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    profile_code = table.Column<string>(type: "text", nullable: false),
                    profile_description = table.Column<string>(type: "text", nullable: false),
                    profile_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    profile_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    profile_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    profile_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    profile_createdby = table.Column<string>(type: "text", nullable: true),
                    profile_updatedby = table.Column<string>(type: "text", nullable: true),
                    profile_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_profile", x => x.profile_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_usertype",
                columns: table => new
                {
                    usertype_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usertype_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    usertype_code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    usertype_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    usertype_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    usertype_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    usertype_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    usertype_createdby = table.Column<string>(type: "text", nullable: true),
                    usertype_updatedby = table.Column<string>(type: "text", nullable: true),
                    usertype_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_usertype", x => x.usertype_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_zone",
                columns: table => new
                {
                    zone_id = table.Column<Guid>(type: "uuid", nullable: false),
                    zone_offset = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    zone_offsetminute = table.Column<int>(type: "integer", nullable: true),
                    zone_label = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    zone_tzcode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    zone_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    zone_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    zone_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    zone_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    zone_createdby = table.Column<string>(type: "text", nullable: true),
                    zone_updatedby = table.Column<string>(type: "text", nullable: true),
                    zone_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_zone", x => x.zone_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs015_citytype",
                columns: table => new
                {
                    citytype_id = table.Column<Guid>(type: "uuid", nullable: false),
                    citytype_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    citytype_name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    citytype_shortname = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    citytype_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    citytype_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    citytype_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    citytype_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    citytype_createdby = table.Column<string>(type: "text", nullable: true),
                    citytype_updatedby = table.Column<string>(type: "text", nullable: true),
                    citytype_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs015_citytype", x => x.citytype_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs015_country",
                columns: table => new
                {
                    country_id = table.Column<Guid>(type: "uuid", nullable: false),
                    country_name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    country_shortname = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    country_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    country_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    country_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    country_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    country_createdby = table.Column<string>(type: "text", nullable: true),
                    country_updatedby = table.Column<string>(type: "text", nullable: true),
                    country_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs015_country", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs015_employee",
                columns: table => new
                {
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    employee_code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    employee_phone = table.Column<string>(type: "text", nullable: false),
                    employee_gendercode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    employee_address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    employee_birthdate = table.Column<DateOnly>(type: "date", nullable: false),
                    employee_hireddate = table.Column<DateOnly>(type: "date", nullable: false),
                    employee_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    employee_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    employee_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    employee_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    employee_createdby = table.Column<string>(type: "text", nullable: true),
                    employee_updatedby = table.Column<string>(type: "text", nullable: true),
                    employee_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs015_employee", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs015_gender",
                columns: table => new
                {
                    gender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    gender_name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    gender_code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    gender_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    gender_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    gender_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    gender_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    gender_createdby = table.Column<string>(type: "text", nullable: true),
                    gender_updatedby = table.Column<string>(type: "text", nullable: true),
                    gender_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs015_gender", x => x.gender_id);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_page_event",
                columns: table => new
                {
                    page_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    page_event_pageid = table.Column<Guid>(type: "uuid", nullable: false),
                    page_event_eventid = table.Column<Guid>(type: "uuid", nullable: false),
                    page_event_hasevent = table.Column<bool>(type: "boolean", nullable: false),
                    page_event_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    page_event_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    page_event_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_event_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_event_createdby = table.Column<string>(type: "text", nullable: true),
                    page_event_updatedby = table.Column<string>(type: "text", nullable: true),
                    page_event_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_page_event", x => x.page_event_id);
                    table.ForeignKey(
                        name: "FK_dbs000_page_event_dbs000_event_page_event_eventid",
                        column: x => x.page_event_eventid,
                        principalTable: "dbs000_event",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbs000_page_event_dbs000_page_page_event_pageid",
                        column: x => x.page_event_pageid,
                        principalTable: "dbs000_page",
                        principalColumn: "page_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_profile_page_event",
                columns: table => new
                {
                    profile_page_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_page_event_pageid = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_page_event_eventid = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_page_event_profileid = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_page_event_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    profile_page_event_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    profile_page_event_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    profile_page_event_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    profile_page_event_createdby = table.Column<string>(type: "text", nullable: true),
                    profile_page_event_updatedby = table.Column<string>(type: "text", nullable: true),
                    profile_page_event_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_profile_page_event", x => x.profile_page_event_id);
                    table.ForeignKey(
                        name: "FK_dbs000_profile_page_event_dbs000_event_profile_page_event_e~",
                        column: x => x.profile_page_event_eventid,
                        principalTable: "dbs000_event",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbs000_profile_page_event_dbs000_page_profile_page_event_pa~",
                        column: x => x.profile_page_event_pageid,
                        principalTable: "dbs000_page",
                        principalColumn: "page_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbs000_profile_page_event_dbs000_profile_profile_page_event~",
                        column: x => x.profile_page_event_profileid,
                        principalTable: "dbs000_profile",
                        principalColumn: "profile_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_role",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    role_code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    role_usertypeid = table.Column<Guid>(type: "uuid", nullable: false),
                    role_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    role_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    role_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    role_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    role_createdby = table.Column<string>(type: "text", nullable: true),
                    role_updatedby = table.Column<string>(type: "text", nullable: true),
                    role_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_role", x => x.role_id);
                    table.ForeignKey(
                        name: "FK_dbs000_role_dbs000_usertype_role_usertypeid",
                        column: x => x.role_usertypeid,
                        principalTable: "dbs000_usertype",
                        principalColumn: "usertype_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_user",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_password = table.Column<string>(type: "text", nullable: true),
                    user_username = table.Column<string>(type: "text", nullable: true),
                    user_salt = table.Column<string>(type: "text", nullable: true),
                    user_languageid = table.Column<Guid>(type: "uuid", nullable: true),
                    user_zoneid = table.Column<Guid>(type: "uuid", nullable: true),
                    user_token = table.Column<string>(type: "text", nullable: true),
                    user_hash = table.Column<string>(type: "text", nullable: true),
                    user_image = table.Column<string>(type: "text", nullable: true),
                    user_issuper = table.Column<bool>(type: "boolean", nullable: false),
                    user_isremember = table.Column<bool>(type: "boolean", nullable: false),
                    user_isverified = table.Column<bool>(type: "boolean", nullable: false),
                    user_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    user_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_createdby = table.Column<string>(type: "text", nullable: true),
                    user_updatedby = table.Column<string>(type: "text", nullable: true),
                    user_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_user", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_dbs000_user_dbs000_language_user_languageid",
                        column: x => x.user_languageid,
                        principalTable: "dbs000_language",
                        principalColumn: "language_id");
                    table.ForeignKey(
                        name: "FK_dbs000_user_dbs000_zone_user_zoneid",
                        column: x => x.user_zoneid,
                        principalTable: "dbs000_zone",
                        principalColumn: "zone_id");
                });

            migrationBuilder.CreateTable(
                name: "dbs015_province",
                columns: table => new
                {
                    province_id = table.Column<Guid>(type: "uuid", nullable: false),
                    province_countryid = table.Column<Guid>(type: "uuid", nullable: false),
                    province_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    province_name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    province_shortname = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    province_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    province_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    province_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    province_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    province_createdby = table.Column<string>(type: "text", nullable: true),
                    province_updatedby = table.Column<string>(type: "text", nullable: true),
                    province_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs015_province", x => x.province_id);
                    table.ForeignKey(
                        name: "FK_dbs015_province_dbs015_country_province_countryid",
                        column: x => x.province_countryid,
                        principalTable: "dbs015_country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_menu_pageevent",
                columns: table => new
                {
                    menu_pageevent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_pageevent_menuid = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_pageevent_pageeventid = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_pageevent_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    menu_pageevent_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    menu_pageevent_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    menu_pageevent_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    menu_pageevent_createdby = table.Column<string>(type: "text", nullable: true),
                    menu_pageevent_updatedby = table.Column<string>(type: "text", nullable: true),
                    menu_pageevent_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_menu_pageevent", x => x.menu_pageevent_id);
                    table.ForeignKey(
                        name: "FK_dbs000_menu_pageevent_dbs000_menu_menu_pageevent_menuid",
                        column: x => x.menu_pageevent_menuid,
                        principalTable: "dbs000_menu",
                        principalColumn: "menu_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbs000_menu_pageevent_dbs000_page_event_menu_pageevent_page~",
                        column: x => x.menu_pageevent_pageeventid,
                        principalTable: "dbs000_page_event",
                        principalColumn: "page_event_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_role_profile",
                columns: table => new
                {
                    role_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_profile_roleid = table.Column<Guid>(type: "uuid", nullable: false),
                    role_profile_profileid = table.Column<Guid>(type: "uuid", nullable: false),
                    role_profile_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    role_profile_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    role_profile_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    role_profile_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    role_profile_createdby = table.Column<string>(type: "text", nullable: true),
                    role_profile_updatedby = table.Column<string>(type: "text", nullable: true),
                    role_profile_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_role_profile", x => x.role_profile_id);
                    table.ForeignKey(
                        name: "FK_dbs000_role_profile_dbs000_profile_role_profile_profileid",
                        column: x => x.role_profile_profileid,
                        principalTable: "dbs000_profile",
                        principalColumn: "profile_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbs000_role_profile_dbs000_role_role_profile_roleid",
                        column: x => x.role_profile_roleid,
                        principalTable: "dbs000_role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dbs000_user_usertype_role",
                columns: table => new
                {
                    user_usertype_role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_usertype_role_userid = table.Column<Guid>(type: "uuid", nullable: true),
                    user_usertype_role_usertypeid = table.Column<Guid>(type: "uuid", nullable: false),
                    user_usertype_role_roleid = table.Column<Guid>(type: "uuid", nullable: true),
                    user_usertype_role_isenabled = table.Column<bool>(type: "boolean", nullable: false),
                    user_usertype_role_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    user_usertype_role_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_usertype_role_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_usertype_role_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_usertype_role_createdby = table.Column<string>(type: "text", nullable: true),
                    user_usertype_role_updatedby = table.Column<string>(type: "text", nullable: true),
                    user_usertype_role_deletedby = table.Column<string>(type: "text", nullable: true),
                    user_usertype_role_discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    user_usertype_role_developerid = table.Column<Guid>(type: "uuid", nullable: true),
                    user_usertype_role_employeeid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs000_user_usertype_role", x => x.user_usertype_role_id);
                    table.ForeignKey(
                        name: "FK_dbs000_user_usertype_role_dbs000_developer_user_usertype_ro~",
                        column: x => x.user_usertype_role_developerid,
                        principalTable: "dbs000_developer",
                        principalColumn: "developer_id");
                    table.ForeignKey(
                        name: "FK_dbs000_user_usertype_role_dbs000_role_user_usertype_role_ro~",
                        column: x => x.user_usertype_role_roleid,
                        principalTable: "dbs000_role",
                        principalColumn: "role_id");
                    table.ForeignKey(
                        name: "FK_dbs000_user_usertype_role_dbs000_user_user_usertype_role_us~",
                        column: x => x.user_usertype_role_userid,
                        principalTable: "dbs000_user",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_dbs000_user_usertype_role_dbs000_usertype_user_usertype_rol~",
                        column: x => x.user_usertype_role_usertypeid,
                        principalTable: "dbs000_usertype",
                        principalColumn: "usertype_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbs000_user_usertype_role_dbs015_employee_user_usertype_rol~",
                        column: x => x.user_usertype_role_employeeid,
                        principalTable: "dbs015_employee",
                        principalColumn: "employee_id");
                });

            migrationBuilder.CreateTable(
                name: "dbs015_city",
                columns: table => new
                {
                    city_id = table.Column<Guid>(type: "uuid", nullable: false),
                    city_provinceid = table.Column<Guid>(type: "uuid", nullable: false),
                    city_citytypeid = table.Column<Guid>(type: "uuid", nullable: false),
                    city_countryid = table.Column<Guid>(type: "uuid", nullable: false),
                    city_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    city_name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    city_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    city_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    city_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    city_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    city_createdby = table.Column<string>(type: "text", nullable: true),
                    city_updatedby = table.Column<string>(type: "text", nullable: true),
                    city_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs015_city", x => x.city_id);
                    table.ForeignKey(
                        name: "FK_dbs015_city_dbs015_citytype_city_citytypeid",
                        column: x => x.city_citytypeid,
                        principalTable: "dbs015_citytype",
                        principalColumn: "citytype_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbs015_city_dbs015_country_city_countryid",
                        column: x => x.city_countryid,
                        principalTable: "dbs015_country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbs015_city_dbs015_province_city_provinceid",
                        column: x => x.city_provinceid,
                        principalTable: "dbs015_province",
                        principalColumn: "province_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dbs015_district",
                columns: table => new
                {
                    district_id = table.Column<Guid>(type: "uuid", nullable: false),
                    district_provinceid = table.Column<Guid>(type: "uuid", nullable: false),
                    district_cityid = table.Column<Guid>(type: "uuid", nullable: false),
                    district_pcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    district_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    district_name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    district_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    district_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    district_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    district_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    district_createdby = table.Column<string>(type: "text", nullable: true),
                    district_updatedby = table.Column<string>(type: "text", nullable: true),
                    district_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs015_district", x => x.district_id);
                    table.ForeignKey(
                        name: "FK_dbs015_district_dbs015_city_district_cityid",
                        column: x => x.district_cityid,
                        principalTable: "dbs015_city",
                        principalColumn: "city_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbs015_district_dbs015_province_district_provinceid",
                        column: x => x.district_provinceid,
                        principalTable: "dbs015_province",
                        principalColumn: "province_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dbs015_subdistrict",
                columns: table => new
                {
                    subdistrict_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subdistrict_provinceid = table.Column<Guid>(type: "uuid", nullable: false),
                    subdistrict_cityid = table.Column<Guid>(type: "uuid", nullable: false),
                    subdistrict_districtid = table.Column<Guid>(type: "uuid", nullable: false),
                    subdistrict_pcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    subdistrict_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    subdistrict_name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    subdistrict_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    subdistrict_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    subdistrict_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    subdistrict_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    subdistrict_createdby = table.Column<string>(type: "text", nullable: true),
                    subdistrict_updatedby = table.Column<string>(type: "text", nullable: true),
                    subdistrict_deletedby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbs015_subdistrict", x => x.subdistrict_id);
                    table.ForeignKey(
                        name: "FK_dbs015_subdistrict_dbs015_city_subdistrict_cityid",
                        column: x => x.subdistrict_cityid,
                        principalTable: "dbs015_city",
                        principalColumn: "city_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbs015_subdistrict_dbs015_district_subdistrict_districtid",
                        column: x => x.subdistrict_districtid,
                        principalTable: "dbs015_district",
                        principalColumn: "district_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbs015_subdistrict_dbs015_province_subdistrict_provinceid",
                        column: x => x.subdistrict_provinceid,
                        principalTable: "dbs015_province",
                        principalColumn: "province_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_developer_developer_isactive_developer_code",
                table: "dbs000_developer",
                columns: new[] { "developer_isactive", "developer_code" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_event_event_isactive_event_code_event_isdefault",
                table: "dbs000_event",
                columns: new[] { "event_isactive", "event_code", "event_isdefault" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_language_language_isactive_language_code",
                table: "dbs000_language",
                columns: new[] { "language_isactive", "language_code" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_menu_menu_isactive_menu_internalcode_menu_code_menu_~",
                table: "dbs000_menu",
                columns: new[] { "menu_isactive", "menu_internalcode", "menu_code", "menu_number", "menu_isexpanded" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_menu_pageevent_menu_pageevent_isactive_menu_pageeven~",
                table: "dbs000_menu_pageevent",
                columns: new[] { "menu_pageevent_isactive", "menu_pageevent_menuid", "menu_pageevent_pageeventid" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_menu_pageevent_menu_pageevent_menuid",
                table: "dbs000_menu_pageevent",
                column: "menu_pageevent_menuid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_menu_pageevent_menu_pageevent_pageeventid",
                table: "dbs000_menu_pageevent",
                column: "menu_pageevent_pageeventid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_page_page_isactive_page_classname_page_code",
                table: "dbs000_page",
                columns: new[] { "page_isactive", "page_classname", "page_code" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_page_event_page_event_eventid",
                table: "dbs000_page_event",
                column: "page_event_eventid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_page_event_page_event_isactive_page_event_eventid_pa~",
                table: "dbs000_page_event",
                columns: new[] { "page_event_isactive", "page_event_eventid", "page_event_pageid", "page_event_hasevent" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_page_event_page_event_pageid",
                table: "dbs000_page_event",
                column: "page_event_pageid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_profile_profile_isactive_profile_code",
                table: "dbs000_profile",
                columns: new[] { "profile_isactive", "profile_code" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_profile_page_event_profile_page_event_eventid",
                table: "dbs000_profile_page_event",
                column: "profile_page_event_eventid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_profile_page_event_profile_page_event_isactive_profi~",
                table: "dbs000_profile_page_event",
                columns: new[] { "profile_page_event_isactive", "profile_page_event_eventid", "profile_page_event_pageid", "profile_page_event_profileid" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_profile_page_event_profile_page_event_pageid",
                table: "dbs000_profile_page_event",
                column: "profile_page_event_pageid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_profile_page_event_profile_page_event_profileid",
                table: "dbs000_profile_page_event",
                column: "profile_page_event_profileid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_role_role_isactive_role_code_role_usertypeid",
                table: "dbs000_role",
                columns: new[] { "role_isactive", "role_code", "role_usertypeid" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_role_role_usertypeid",
                table: "dbs000_role",
                column: "role_usertypeid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_role_profile_role_profile_isactive_role_profile_prof~",
                table: "dbs000_role_profile",
                columns: new[] { "role_profile_isactive", "role_profile_profileid", "role_profile_roleid" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_role_profile_role_profile_profileid",
                table: "dbs000_role_profile",
                column: "role_profile_profileid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_role_profile_role_profile_roleid",
                table: "dbs000_role_profile",
                column: "role_profile_roleid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_user_user_isactive_user_token_user_languageid_user_z~",
                table: "dbs000_user",
                columns: new[] { "user_isactive", "user_token", "user_languageid", "user_zoneid" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_user_user_languageid",
                table: "dbs000_user",
                column: "user_languageid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_user_user_zoneid",
                table: "dbs000_user",
                column: "user_zoneid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_user_usertype_role_user_usertype_role_developerid",
                table: "dbs000_user_usertype_role",
                column: "user_usertype_role_developerid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_user_usertype_role_user_usertype_role_employeeid",
                table: "dbs000_user_usertype_role",
                column: "user_usertype_role_employeeid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_user_usertype_role_user_usertype_role_isactive_user_~",
                table: "dbs000_user_usertype_role",
                columns: new[] { "user_usertype_role_isactive", "user_usertype_role_userid", "user_usertype_role_usertypeid", "user_usertype_role_roleid" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_user_usertype_role_user_usertype_role_roleid",
                table: "dbs000_user_usertype_role",
                column: "user_usertype_role_roleid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_user_usertype_role_user_usertype_role_userid",
                table: "dbs000_user_usertype_role",
                column: "user_usertype_role_userid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_user_usertype_role_user_usertype_role_usertypeid",
                table: "dbs000_user_usertype_role",
                column: "user_usertype_role_usertypeid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_usertype_usertype_isactive_usertype_code",
                table: "dbs000_usertype",
                columns: new[] { "usertype_isactive", "usertype_code" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs000_zone_zone_isactive",
                table: "dbs000_zone",
                column: "zone_isactive");

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_city_city_citytypeid",
                table: "dbs015_city",
                column: "city_citytypeid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_city_city_countryid",
                table: "dbs015_city",
                column: "city_countryid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_city_city_provinceid",
                table: "dbs015_city",
                column: "city_provinceid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_district_district_cityid",
                table: "dbs015_district",
                column: "district_cityid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_district_district_provinceid",
                table: "dbs015_district",
                column: "district_provinceid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_employee_employee_isactive_employee_code",
                table: "dbs015_employee",
                columns: new[] { "employee_isactive", "employee_code" });

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_province_province_countryid",
                table: "dbs015_province",
                column: "province_countryid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_subdistrict_subdistrict_cityid",
                table: "dbs015_subdistrict",
                column: "subdistrict_cityid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_subdistrict_subdistrict_districtid",
                table: "dbs015_subdistrict",
                column: "subdistrict_districtid");

            migrationBuilder.CreateIndex(
                name: "IX_dbs015_subdistrict_subdistrict_provinceid",
                table: "dbs015_subdistrict",
                column: "subdistrict_provinceid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbs000_menu_pageevent");

            migrationBuilder.DropTable(
                name: "dbs000_profile_page_event");

            migrationBuilder.DropTable(
                name: "dbs000_role_profile");

            migrationBuilder.DropTable(
                name: "dbs000_user_usertype_role");

            migrationBuilder.DropTable(
                name: "dbs015_gender");

            migrationBuilder.DropTable(
                name: "dbs015_subdistrict");

            migrationBuilder.DropTable(
                name: "dbs000_menu");

            migrationBuilder.DropTable(
                name: "dbs000_page_event");

            migrationBuilder.DropTable(
                name: "dbs000_profile");

            migrationBuilder.DropTable(
                name: "dbs000_developer");

            migrationBuilder.DropTable(
                name: "dbs000_role");

            migrationBuilder.DropTable(
                name: "dbs000_user");

            migrationBuilder.DropTable(
                name: "dbs015_employee");

            migrationBuilder.DropTable(
                name: "dbs015_district");

            migrationBuilder.DropTable(
                name: "dbs000_event");

            migrationBuilder.DropTable(
                name: "dbs000_page");

            migrationBuilder.DropTable(
                name: "dbs000_usertype");

            migrationBuilder.DropTable(
                name: "dbs000_language");

            migrationBuilder.DropTable(
                name: "dbs000_zone");

            migrationBuilder.DropTable(
                name: "dbs015_city");

            migrationBuilder.DropTable(
                name: "dbs015_citytype");

            migrationBuilder.DropTable(
                name: "dbs015_province");

            migrationBuilder.DropTable(
                name: "dbs015_country");
        }
    }
}
