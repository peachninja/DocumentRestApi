namespace DocumentContainerRestService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentMetaDatas", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentMetaDatas", "Url");
        }
    }
}
