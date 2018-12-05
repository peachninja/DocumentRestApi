namespace DocumentContainerRestService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentMetaDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ForeginKey = c.String(),
                        Text = c.String(),
                        ContentType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DocumentMetaDatas");
        }
    }
}
