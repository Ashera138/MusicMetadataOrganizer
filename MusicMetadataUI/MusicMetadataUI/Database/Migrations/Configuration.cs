using MusicMetadataUpdater_v2._0;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;

namespace MusicMetadataUI.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MusicMetadataUpdater_v2._0.FileDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FileDb context)
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [MetadataFiles]");
            context.Database.ExecuteSqlCommand("DELETE FROM [SystemFiles] DBCC CHECKIDENT ('dbo.SystemFiles',RESEED, 0)");

            var directory = @"C:\Users\Ashie\Desktop\musicTest\";
            DirectoryInfo di = new DirectoryInfo(directory);

            List<SystemFile> files = new List<SystemFile>();

            var fileInfos = di.GetFiles();

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo != null)
                {
                    files.Add(new SystemFile(fileInfo.FullName));
                }
            }

            foreach (SystemFile file in files)
            {
                AddOrUpdate(context, file);
            }
        }

        private void AddOrUpdate(FileDb context, SystemFile file)
        {
            file.SystemFileId = -1;
            file.MetadataFile.MetadataFileId = -1;
            context.SystemFiles.AddOrUpdate(file);
            context.MetadataFiles.AddOrUpdate(file.MetadataFile);
        }
    }
}
