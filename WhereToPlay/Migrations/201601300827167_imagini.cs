namespace WhereToPlay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imagini : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courts", "FileName1", c => c.String());
            AddColumn("dbo.Courts", "Content1", c => c.Binary());
            AddColumn("dbo.Courts", "FileName2", c => c.String());
            AddColumn("dbo.Courts", "Content2", c => c.Binary());
            AddColumn("dbo.Courts", "FileName3", c => c.String());
            AddColumn("dbo.Courts", "Content3", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courts", "Content3");
            DropColumn("dbo.Courts", "FileName3");
            DropColumn("dbo.Courts", "Content2");
            DropColumn("dbo.Courts", "FileName2");
            DropColumn("dbo.Courts", "Content1");
            DropColumn("dbo.Courts", "FileName1");
        }
    }
}
