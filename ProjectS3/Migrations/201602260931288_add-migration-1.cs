namespace ProjectS3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmigration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DiaChi", c => c.String());
            AddColumn("dbo.AspNetUsers", "SoDienThoai", c => c.String());
            AddColumn("dbo.AspNetUsers", "HoTen", c => c.String());
            AddColumn("dbo.AspNetUsers", "avatar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "avatar");
            DropColumn("dbo.AspNetUsers", "HoTen");
            DropColumn("dbo.AspNetUsers", "SoDienThoai");
            DropColumn("dbo.AspNetUsers", "DiaChi");
        }
    }
}
