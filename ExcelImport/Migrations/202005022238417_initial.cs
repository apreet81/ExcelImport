namespace ExcelImport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImportedDatas",
                c => new
                    {
                        ImportedDataId = c.Int(nullable: false, identity: true),
                        Id_Document = c.String(),
                        Phone = c.String(),
                        Alternative_Id = c.String(),
                        Driving_License = c.String(),
                        First_Name = c.String(),
                        Last_Name = c.String(),
                        Sex = c.String(),
                        Education = c.String(),
                        Marital_Status = c.String(),
                        Children = c.String(),
                    })
                .PrimaryKey(t => t.ImportedDataId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ImportedDatas");
        }
    }
}
