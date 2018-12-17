namespace DocumentContainerRestService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _006 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentMetaDatas", "Document_Id", c => c.Int());
            AddColumn("dbo.DocumentMetaDatas", "DocumentVersion_Id", c => c.Int());
            CreateIndex("dbo.DocumentMetaDatas", "Document_Id");
            CreateIndex("dbo.DocumentMetaDatas", "DocumentVersion_Id");
            AddForeignKey("dbo.DocumentMetaDatas", "Document_Id", "dbo.Documents", "Id");
            AddForeignKey("dbo.DocumentMetaDatas", "DocumentVersion_Id", "dbo.DocumentVersions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentMetaDatas", "DocumentVersion_Id", "dbo.DocumentVersions");
            DropForeignKey("dbo.DocumentMetaDatas", "Document_Id", "dbo.Documents");
            DropIndex("dbo.DocumentMetaDatas", new[] { "DocumentVersion_Id" });
            DropIndex("dbo.DocumentMetaDatas", new[] { "Document_Id" });
            DropColumn("dbo.DocumentMetaDatas", "DocumentVersion_Id");
            DropColumn("dbo.DocumentMetaDatas", "Document_Id");
        }
    }
}
