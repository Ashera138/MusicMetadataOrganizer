namespace MusicMetadataUpdater_v2._0.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MusicMetadataUpdater_v2._0.FileDb>
    {
        public Configuration()
        {
            // Consider changing to false
            AutomaticMigrationsEnabled = true;
            ContextKey = "MusicMetadataUpdater_v2._0.FileDb";
        }

        protected override void Seed(MusicMetadataUpdater_v2._0.FileDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
