namespace WhereToPlay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MandatoryCourtEmail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courts", "EmailAddress", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courts", "EmailAddress", c => c.String(maxLength: 50));
        }
    }
}
