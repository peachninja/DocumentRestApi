namespace DocumentContainerRestService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _002 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentMetaDatas", "CaseId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentMetaDatas", "CaseId");
        }
    }
}
