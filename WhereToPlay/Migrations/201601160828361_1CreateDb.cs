namespace WhereToPlay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1CreateDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        IDAddress = c.Int(nullable: false, identity: true),
                        AddressStreet = c.String(nullable: false, maxLength: 50),
                        AddressNumber = c.String(maxLength: 50),
                        AddressCity = c.String(nullable: false, maxLength: 50),
                        AddressCounty = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDAddress);
            
            CreateTable(
                "dbo.Courts",
                c => new
                    {
                        IDCourt = c.Int(nullable: false, identity: true),
                        CourtName = c.String(nullable: false, maxLength: 50),
                        SportID = c.Int(nullable: false),
                        AddressID = c.Int(nullable: false),
                        Length = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        PhoneNumber = c.String(maxLength: 50),
                        EmailAddress = c.String(maxLength: 50),
                        CreateUserID = c.Int(nullable: false),
                        SessionPrice = c.Int(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDCourt)
                .ForeignKey("dbo.Addresses", t => t.AddressID)
                .ForeignKey("dbo.Sports", t => t.SportID)
                .ForeignKey("dbo.Users", t => t.CreateUserID)
                .Index(t => t.SportID)
                .Index(t => t.AddressID)
                .Index(t => t.CreateUserID);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        IDReservation = c.Int(nullable: false, identity: true),
                        ReservationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserID = c.Int(nullable: false),
                        CourtID = c.Int(nullable: false),
                        ReservationTimeID = c.Int(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDReservation)
                .ForeignKey("dbo.Courts", t => t.CourtID)
                .ForeignKey("dbo.ReservationTimes", t => t.ReservationTimeID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.CourtID)
                .Index(t => t.ReservationTimeID);
            
            CreateTable(
                "dbo.ReservationTimes",
                c => new
                    {
                        IDReservationTime = c.Int(nullable: false, identity: true),
                        Hours = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.IDReservationTime);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        IDUser = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50),
                        UserPhone = c.String(maxLength: 50),
                        UserEmail = c.String(nullable: false, maxLength: 50),
                        UserPassword = c.String(nullable: false),
                        UserFullName = c.String(maxLength: 50),
                        UserGroupID = c.Int(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDUser)
                .ForeignKey("dbo.UserGroups", t => t.UserGroupID)
                .Index(t => t.UserGroupID);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        IDUserGroup = c.Int(nullable: false, identity: true),
                        UserGroupName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDUserGroup);
            
            CreateTable(
                "dbo.PageAccesses",
                c => new
                    {
                        IDPageAccess = c.Int(nullable: false, identity: true),
                        UserGroupID = c.Int(nullable: false),
                        PageName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDPageAccess)
                .ForeignKey("dbo.UserGroups", t => t.UserGroupID)
                .Index(t => t.UserGroupID);
            
            CreateTable(
                "dbo.Sports",
                c => new
                    {
                        IDSport = c.Int(nullable: false, identity: true),
                        SportName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IDSport);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courts", "CreateUserID", "dbo.Users");
            DropForeignKey("dbo.Courts", "SportID", "dbo.Sports");
            DropForeignKey("dbo.Reservations", "UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "UserGroupID", "dbo.UserGroups");
            DropForeignKey("dbo.PageAccesses", "UserGroupID", "dbo.UserGroups");
            DropForeignKey("dbo.Reservations", "ReservationTimeID", "dbo.ReservationTimes");
            DropForeignKey("dbo.Reservations", "CourtID", "dbo.Courts");
            DropForeignKey("dbo.Courts", "AddressID", "dbo.Addresses");
            DropIndex("dbo.PageAccesses", new[] { "UserGroupID" });
            DropIndex("dbo.Users", new[] { "UserGroupID" });
            DropIndex("dbo.Reservations", new[] { "ReservationTimeID" });
            DropIndex("dbo.Reservations", new[] { "CourtID" });
            DropIndex("dbo.Reservations", new[] { "UserID" });
            DropIndex("dbo.Courts", new[] { "CreateUserID" });
            DropIndex("dbo.Courts", new[] { "AddressID" });
            DropIndex("dbo.Courts", new[] { "SportID" });
            DropTable("dbo.Sports");
            DropTable("dbo.PageAccesses");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Users");
            DropTable("dbo.ReservationTimes");
            DropTable("dbo.Reservations");
            DropTable("dbo.Courts");
            DropTable("dbo.Addresses");
        }
    }
}
