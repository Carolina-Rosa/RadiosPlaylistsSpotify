using System.Collections;
using System.ComponentModel;
using System.Xml.Linq;
using SpotifyPlaylistRadio;
using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Services;

namespace Antena3ToSpotifyPlaylistTests
{
    public class SongTitles : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]{"O Meu Para Sempre (ft. Amaura e Heber Marques)", "O Meu Para Sempre Amaura e Heber Marques"},
            new object[]{"Line of Fire (feat. Sharon Ven Etten)", "Line of Fire Sharon Ven Etten"},
            new object[]{"Waterfall (Ft. RAYE)", "Waterfall RAYE"},
            new object[]{"Impress�es Digitais", "Impressoes Digitais" }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class ArtistsWithDiacritics : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]{"Kendrick Lamar", "kendrick lamar"},
            new object[]{"ROSAL�A", "rosalia"},
            new object[]{"J�RA", "jura"},
            new object[]{"Mallu Magalh�es", "mallu magalhaes"},
            new object[]{"IN�S APENAS", "ines apenas"},
            new object[]{"�", "a"},
            new object[]{ "�ter", "aeter"},
            new object[]{ "H�sak�ri�", "husakorie"},
            new object[]{"�neheart", "oneheart"}
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class ArtistsWithAmpersand : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]{"Xutos &amp; Pontap�s", "xutos & pontap�s"}, 
            new object[]{"Xutos e Pontap�s", "xutos & pontap�s"},
            new object[]{"Edward Sharpe and The Magnetic Zeros", "edward sharpe & the magnetic zeros"},
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class ArtistsWithDotsAndApostrophes : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]{"Lena D'�gua", "lena d�gua"}, 
            new object[]{"EU.CLIDES", "euclides"}, 
            new object[]{"Florence + The Machine", "Florence The Machine"}, 
            new object[]{ "You can't win, Charlie Brown", "You cant win Charlie Brown"}, 

            // new object[]{"Peixe: avi�o", "peixe : avi�o"}, 
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class ArtistsThatStartWithThe : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]{"The Chemical Brothers", "chemical brothers"}, 
            new object[]{"Thea Gilmore", "thea gilmore"}, 
            new object[]{"Brothers", "brothers"}, 
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ArtistsThatMayBe2 : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]{"Benjamim e Barnaby Keen", new string[] { "Benjamim", "Barnaby Keen" } },
            new object[]{"Xutos e Pontap�s", new string[] { "Xutos", "Pontap�s" }},
            new object[]{"X-press 2 feat David Byrne", new string[] { "X-press 2", "David Byrne" } },
            new object[]{ "Expresso Transatl�ntico & Conan Os�ris", new string[] { "Expresso Transatl�ntico", "Conan Os�ris" } },
            new object[]{ "Mundo Segundo ft Bezegol", new string[] { "Mundo Segundo","Bezegol" } },
            new object[]{"GNR", null },
            new object[]{"Z� Simples", null },
            new object[]{"EU.CLIDES", null },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class General : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]{"Benjamim e Barnaby Keen", "Benjamim", "benjamim"},
            new object[]{"Xutos e Pontap�s", "Xutos & Pontap�s", "xutos & pontapes"},
            new object[]{"X-press 2 feat. David Byrne", "X-press 2", "x-press 2"},
            new object[]{ "Expresso Transatl�ntico &amp; Conan Os�ris", "Expresso Transatl�ntico", "Expresso Transatlantico"},
            new object[]{"GNR", "GNR", "GNR"},
            new object[]{"Z� Simples", "Z� Simples", "Ze Simples" },
            new object[]{"EU.CLIDES", "EU.CLIDES", "euclides" },
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Antena3ToSpotifyTests
    {
        [Theory]
        [ClassData(typeof(SongTitles))]
        public void TestPrepareSongTitleForSearch(string song, string expectedText)
        {
            SearchHelperService shs = new();

            string textResult = shs.PrepareSongTitleForSearch(song);

            CheckText(expectedText, textResult);
        }
        
        [Theory]
        [ClassData(typeof(ArtistsThatMayBe2))]
        public void TestTryToSplitMultipleArtists(string artists, string[] expectedText)
        {
            SearchHelperService shs = new();

            string[] textResult = shs.TryToSplitMultipleArtists(artists);

            CheckListOfArtists(expectedText, textResult);
        }
        
        [Theory]
        [ClassData(typeof(ArtistsWithDiacritics))]
        public void TestRemoveDiacritics(string artist, string expectedText)
        {
            SearchHelperService shs = new();

            string textResult = shs.RemoveDiacritics(artist);

            CheckText(expectedText, textResult);
        }

        [Theory]
        [ClassData(typeof(ArtistsWithAmpersand))]
        public void TestDealWithAmpersand(string artist, string expectedText)
        {
            SearchHelperService shs = new();

            string textResult = shs.DealWithAmpersand(artist);

            CheckText(expectedText, textResult);
        }
        
        [Theory]
        [ClassData(typeof(ArtistsWithDotsAndApostrophes))]
        public void TestRemoveDotsAndApostrophes(string artist, string expectedText)
        {
            SearchHelperService shs = new();

            string textResult = shs.RemoveDotsAndApostrophes(artist);

            CheckText(expectedText, textResult);
        }
        
        [Theory]
        [ClassData(typeof(ArtistsThatStartWithThe))]
        public void TestRemoveTheIfFirstWord(string artist, string expectedText)
        {
            SearchHelperService shs = new();

            string textResult = shs.RemoveTheIfFirstWord(artist);

            CheckText(expectedText, textResult);
        }

        [Theory]
        [ClassData(typeof(General))]
        public void TestCompareSongScrapedWithSearchResults(string artistScraped, string artistSearch, string expectedText)
        {
            SearchHelperService shs = new();

            MusicSpotify textResult = shs.CompareSongScrapedWithSearchResults(artistScraped, new() { new MusicSpotify() { artists = new List<Artist>() { new Artist() { name = artistSearch } } } });

            CheckText(expectedText, textResult.artists[0].name);
        }

        private static void CheckText(string expectedText, string realText)
        {
            Assert.Equal(expectedText, realText, true);
        }
        
        private static void CheckListOfArtists(string[] expectedList, string[] realList)
        {
            Assert.Equal(expectedList, realList);
        }
    }
}