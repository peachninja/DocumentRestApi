namespace DocumentContainerRestService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _004 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documents", "Created", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Documents", "CheckedOutTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DocumentVersions", "LastUpdated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DocumentVersions", "LastUpdated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Documents", "CheckedOutTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Documents", "Created", c => c.DateTime(nullable: false));
        }
    }
}
