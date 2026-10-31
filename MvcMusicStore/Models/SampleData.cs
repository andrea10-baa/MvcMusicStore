using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    public class SampleData : DropCreateDatabaseIfModelChanges<MusicStoreEntities>
    {
        protected override void Seed(MusicStoreEntities context)
        {
            var genres = new List<Genre>
            {
                new Genre { Name = "Rock", Description = "Rock music" },
                new Genre { Name = "Jazz", Description = "Jazz music" },
                new Genre { Name = "Country", Description = "Country music" },
                new Genre { Name = "Pop", Description = "Pop music" }
            };
            genres.ForEach(g => context.Genres.Add(g));
            context.SaveChanges();

            var artists = new List<Artist>
            {
                new Artist { Name = "Led Zeppelin" },
                new Artist { Name = "Miles Davis" },
                new Artist { Name = "Johnny Cash" },
                new Artist { Name = "Madonna" }
            };
            artists.ForEach(a => context.Artists.Add(a));
            context.SaveChanges();

            var albums = new List<Album>
            {
                new Album { Title = "Led Zeppelin IV", Price = 10.99m, GenreId = genres[0].GenreId, ArtistId = artists[0].ArtistId, AlbumArtUrl = "/Content/albums/led-zeppelin-iv.jpg" },
                new Album { Title = "Kind of Blue", Price = 12.99m, GenreId = genres[1].GenreId, ArtistId = artists[1].ArtistId, AlbumArtUrl = "/Content/albums/kind-of-blue.jpg" },
                new Album { Title = "At Folsom Prison", Price = 9.99m, GenreId = genres[2].GenreId, ArtistId = artists[2].ArtistId, AlbumArtUrl = "/Content/albums/at-folsom-prison.jpg" },
                new Album { Title = "Ray of Light", Price = 11.99m, GenreId = genres[3].GenreId, ArtistId = artists[3].ArtistId, AlbumArtUrl = "/Content/albums/ray-of-light.jpg" }
            };
            albums.ForEach(a => context.Albums.Add(a));
            context.SaveChanges();
        }
    }
}