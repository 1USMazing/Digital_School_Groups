namespace Proiect_DSG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                        CategoryDescription = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false),
                        GroupCreator = c.String(nullable: false),
                        GroupDescription = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false, identity: true),
                        ActivityName = c.String(nullable: false),
                        ActivityDescription = c.String(),
                        Date = c.DateTime(nullable: false),
                        CalendarId = c.Int(nullable: false),
                        Group_GroupId = c.Int(),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Calendars", t => t.CalendarId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_GroupId)
                .Index(t => t.CalendarId)
                .Index(t => t.Group_GroupId);
            
            CreateTable(
                "dbo.Calendars",
                c => new
                    {
                        CalendarId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.CalendarId);
            
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        MembershipId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        PostData = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MembershipId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserType = c.String(nullable: false),
                        UserFirstName = c.String(nullable: false),
                        UserLastName = c.String(nullable: false),
                        RegisteredDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        PostData = c.DateTime(nullable: false),
                        GroupId = c.Int(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.GroupId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Posts", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Memberships", "UserId", "dbo.Users");
            DropForeignKey("dbo.Memberships", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Activities", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.Activities", "CalendarId", "dbo.Calendars");
            DropIndex("dbo.Posts", new[] { "User_UserId" });
            DropIndex("dbo.Posts", new[] { "GroupId" });
            DropIndex("dbo.Memberships", new[] { "GroupId" });
            DropIndex("dbo.Memberships", new[] { "UserId" });
            DropIndex("dbo.Activities", new[] { "Group_GroupId" });
            DropIndex("dbo.Activities", new[] { "CalendarId" });
            DropIndex("dbo.Groups", new[] { "CategoryId" });
            DropTable("dbo.Posts");
            DropTable("dbo.Users");
            DropTable("dbo.Memberships");
            DropTable("dbo.Calendars");
            DropTable("dbo.Activities");
            DropTable("dbo.Groups");
            DropTable("dbo.Categories");
        }
    }
}
