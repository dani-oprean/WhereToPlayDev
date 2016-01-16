namespace WhereToPlay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2AddPasswordSalt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserPasswordSalt", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "UserPasswordSalt");
        }
    }
}
