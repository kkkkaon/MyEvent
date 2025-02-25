using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEvent.Migrations
{
    /// <inheritdoc />
    public partial class EnableCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventHolder",
                columns: table => new
                {
                    EventHolderID = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: false),
                    EventHolderName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    City = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EventHol__ACE2A24FC6F83D0A", x => x.EventHolderID);
                });

            migrationBuilder.CreateTable(
                name: "EventType",
                columns: table => new
                {
                    EventTypeID = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EventTyp__A9216B1FABFF90B6", x => x.EventTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    MemberID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    MemberName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    City = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Member__0CF04B38B532B3D5", x => x.MemberID);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__9B556A5875A298C7", x => x.PaymentID);
                });

            migrationBuilder.CreateTable(
                name: "TicketMethod",
                columns: table => new
                {
                    MethodID = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TicketMe__FC681FB1A621CC57", x => x.MethodID);
                });

            migrationBuilder.CreateTable(
                name: "TicketTypeList",
                columns: table => new
                {
                    TicketTypeID = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TicketTy__516F0395D3F1980E", x => x.TicketTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Venue",
                columns: table => new
                {
                    VenueID = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    VenueName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Region = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    City = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Venue__3C57E5D275B2916E", x => x.VenueID);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    Account = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MemberID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Credenti__B0C3AC476950C594", x => x.Account);
                    table.ForeignKey(
                        name: "FK__Credentia__Membe__5DCAEF64",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditCard",
                columns: table => new
                {
                    CardNo = table.Column<string>(type: "nchar(12)", fixedLength: true, maxLength: 12, nullable: false),
                    ValidYear = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    ValidMonth = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    SecurityNo = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    MemberID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CreditCa__55FF25F1B9F394E2", x => x.CardNo);
                    table.ForeignKey(
                        name: "FK__CreditCar__Membe__628FA481",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "MemberTel",
                columns: table => new
                {
                    MemberID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberTel", x => new { x.MemberID, x.Tel });
                    table.ForeignKey(
                        name: "FK__MemberTel__Membe__5AEE82B9",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    TicketID = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    EventID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TicketTypeID = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ticket__712CC6275F174448", x => x.TicketID);
                    table.ForeignKey(
                        name: "FK__Ticket__TypeID__71D1E811",
                        column: x => x.TicketTypeID,
                        principalTable: "TicketTypeList",
                        principalColumn: "TicketTypeID");
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventID = table.Column<string>(type: "nchar(11)", fixedLength: true, maxLength: 11, nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    VenueID = table.Column<string>(type: "nchar(5)", fixedLength: true, maxLength: 5, nullable: false),
                    EventHolderID = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pic = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Discount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventTypeID = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Event__7944C870358F73FF", x => x.EventID);
                    table.ForeignKey(
                        name: "FK__Event__EventHold__2EDAF651",
                        column: x => x.EventHolderID,
                        principalTable: "EventHolder",
                        principalColumn: "EventHolderID");
                    table.ForeignKey(
                        name: "FK__Event__EventType__534D60F1",
                        column: x => x.EventTypeID,
                        principalTable: "EventType",
                        principalColumn: "EventTypeID");
                    table.ForeignKey(
                        name: "FK__Event__VenueID__2DE6D218",
                        column: x => x.VenueID,
                        principalTable: "Venue",
                        principalColumn: "VenueID");
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    SeatID = table.Column<string>(type: "nchar(7)", fixedLength: true, maxLength: 7, nullable: false),
                    SeatType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Row = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: true),
                    Number = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: true),
                    Status = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    VenueID = table.Column<string>(type: "nchar(5)", fixedLength: true, maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Seat__311713D3EA7FE4C2", x => x.SeatID);
                    table.ForeignKey(
                        name: "FK__Seat__VenueID__6A30C649",
                        column: x => x.VenueID,
                        principalTable: "Venue",
                        principalColumn: "VenueID");
                });

            migrationBuilder.CreateTable(
                name: "EventTag",
                columns: table => new
                {
                    EventID = table.Column<string>(type: "nchar(11)", fixedLength: true, maxLength: 11, nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTag", x => new { x.EventID, x.Tag });
                    table.ForeignKey(
                        name: "FK__EventTag__EventI__5535A963",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "EventID");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<string>(type: "nchar(12)", fixedLength: true, maxLength: 12, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventID = table.Column<string>(type: "nchar(11)", fixedLength: true, maxLength: 11, nullable: false),
                    MemberID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    PaymentID = table.Column<int>(type: "int", nullable: false),
                    MethodID = table.Column<int>(type: "int", nullable: false),
                    CollectionID = table.Column<string>(type: "nchar(7)", fixedLength: true, maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__C3905BAFC90BF40E", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK__Order__EventID__2FCF1A8A",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "EventID");
                    table.ForeignKey(
                        name: "FK__Order__MemberID__30C33EC3",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK__Order__MethodID__32AB8735",
                        column: x => x.MethodID,
                        principalTable: "TicketMethod",
                        principalColumn: "MethodID");
                    table.ForeignKey(
                        name: "FK__Order__PaymentID__31B762FC",
                        column: x => x.PaymentID,
                        principalTable: "Payment",
                        principalColumn: "PaymentID");
                });

            migrationBuilder.CreateTable(
                name: "TicketType",
                columns: table => new
                {
                    EventID = table.Column<string>(type: "nchar(11)", fixedLength: true, maxLength: 11, nullable: false),
                    TicketTypeID = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType", x => new { x.EventID, x.TicketTypeID });
                    table.ForeignKey(
                        name: "FK__TicketTyp__Event__7849DB76",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "EventID");
                    table.ForeignKey(
                        name: "FK__TicketTyp__Ticke__793DFFAF",
                        column: x => x.TicketTypeID,
                        principalTable: "TicketTypeList",
                        principalColumn: "TicketTypeID");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderID = table.Column<string>(type: "nchar(12)", fixedLength: true, maxLength: 12, nullable: false),
                    SeatID = table.Column<string>(type: "nchar(8)", fixedLength: true, maxLength: 8, nullable: false),
                    TicketID = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__E0812A924EAEBB87", x => new { x.OrderID, x.SeatID });
                    table.ForeignKey(
                        name: "FK__OrderDeta__Order__7C4F7684",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK__OrderDeta__SeatI__7D439ABD",
                        column: x => x.SeatID,
                        principalTable: "Seat",
                        principalColumn: "SeatID");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Ticke__7E37BEF6",
                        column: x => x.TicketID,
                        principalTable: "Ticket",
                        principalColumn: "TicketID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_MemberID",
                table: "Credentials",
                column: "MemberID",
                unique: true,
                filter: "[MemberID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCard_MemberID",
                table: "CreditCard",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventHolderID",
                table: "Event",
                column: "EventHolderID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventTypeID",
                table: "Event",
                column: "EventTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_VenueID",
                table: "Event",
                column: "VenueID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_EventID",
                table: "Order",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_MemberID",
                table: "Order",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_MethodID",
                table: "Order",
                column: "MethodID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentID",
                table: "Order",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_SeatID",
                table: "OrderDetail",
                column: "SeatID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_TicketID",
                table: "OrderDetail",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_VenueID",
                table: "Seat",
                column: "VenueID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketTypeID",
                table: "Ticket",
                column: "TicketTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketType_TicketTypeID",
                table: "TicketType",
                column: "TicketTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "CreditCard");

            migrationBuilder.DropTable(
                name: "EventTag");

            migrationBuilder.DropTable(
                name: "MemberTel");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "TicketType");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "TicketMethod");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "TicketTypeList");

            migrationBuilder.DropTable(
                name: "EventHolder");

            migrationBuilder.DropTable(
                name: "EventType");

            migrationBuilder.DropTable(
                name: "Venue");
        }
    }
}
