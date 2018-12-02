namespace RecomendaPlays.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _011218 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Musicas",
                c => new
                    {
                        MusicaId = c.Int(nullable: false, identity: true),
                        IdUsuario = c.String(),
                        QualCluster = c.Int(nullable: false),
                        Speechiness = c.Single(nullable: false),
                        Liveness = c.Single(nullable: false),
                        Energy = c.Single(nullable: false),
                        Danceability = c.Single(nullable: false),
                        Titulo = c.String(),
                        Cantor = c.String(),
                    })
                .PrimaryKey(t => t.MusicaId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Musicas");
        }
    }
}
