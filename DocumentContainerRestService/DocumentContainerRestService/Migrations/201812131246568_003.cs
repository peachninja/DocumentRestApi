namespace DocumentContainerRestService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _003 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CaseId = c.Guid(nullable: false),
                        Title = c.String(),
                        Status = c.Short(nullable: false),
                        Owner = c.String(),
                        VersionStatus = c.Int(nullable: false),
                        Category = c.Int(nullable: false),
                        Guid = c.Guid(nullable: false),
                        Text = c.String(),
                        ContentType = c.String(),
                        Created = c.DateTime(nullable: false),
                        CheckedOut = c.String(),
                        CheckedOutTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentVersions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocumentId = c.Guid(nullable: false),
                        VersionNumber = c.Int(nullable: false),
                        DocumentPath = c.String(),
                        FileName = c.String(),
                        FileExtension = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(),
                        Size = c.Int(nullable: false),
                        Index = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.DocumentMetaDatas", "FilePath", c => c.String());
            DropColumn("dbo.DocumentMetaDatas", "ForeginKey");
            DropColumn("dbo.DocumentMetaDatas", "CaseId");
            DropColumn("dbo.DocumentMetaDatas", "ContentType");
            DropColumn("dbo.DocumentMetaDatas", "Url");
            DropColumn("dbo.DocumentMetaDatas", "Version");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocumentMetaDatas", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.DocumentMetaDatas", "Url", c => c.String());
            AddColumn("dbo.DocumentMetaDatas", "ContentType", c => c.String());
            AddColumn("dbo.DocumentMetaDatas", "CaseId", c => c.Guid(nullable: false));
            AddColumn("dbo.DocumentMetaDatas", "ForeginKey", c => c.String());
            DropColumn("dbo.DocumentMetaDatas", "FilePath");
            DropTable("dbo.DocumentVersions");
            DropTable("dbo.Documents");
        }
    }
}
