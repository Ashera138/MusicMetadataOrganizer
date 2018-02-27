namespace MusicMetadataUpdater_v2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SysIOFiles",
                c => new
                    {
                        Filepath = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Directory = c.String(),
                        Extension = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        LastAccessTime = c.DateTime(nullable: false),
                        Length = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Filepath);
            
            CreateTable(
                "dbo.TagLibFiles",
                c => new
                    {
                        Filepath = c.String(nullable: false, maxLength: 128),
                        BitRate = c.Int(nullable: false),
                        MediaType = c.String(),
                        Artist = c.String(),
                        Album = c.String(),
                        Genres = c.String(),
                        Lyrics = c.String(),
                        Title = c.String(),
                        Rating = c.Byte(nullable: false),
                        IsCover = c.Boolean(nullable: false),
                        IsLive = c.Boolean(nullable: false),
                        Duration = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Filepath);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TagLibFiles");
            DropTable("dbo.SysIOFiles");
        }
    }
}
