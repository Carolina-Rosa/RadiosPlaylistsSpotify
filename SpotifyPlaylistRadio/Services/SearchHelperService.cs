using AngleSharp.Text;
using Newtonsoft.Json;
using SpotifyPlaylistRadio.Hubs;
using SpotifyPlaylistRadio.Models;
using System.Text.RegularExpressions;

namespace SpotifyPlaylistRadio.Services
{
    public class SearchHelperService : ISearchHelperService
    {
        private readonly IMessageWriter _messageWriter;

        public SearchHelperService(IMessageWriter messageWrite)
        {
            _messageWriter = messageWrite;
        }

        public async Task<MusicSpotify> CompareSongScrapedWithSearchResults(string songArtist, List<MusicSpotify> searchResults, string radioName)
        {

            songArtist= PrepareStringToCompare(songArtist);

            string[] splitBySeparator = TryToSplitMultipleArtists(songArtist);

            foreach (MusicSpotify spotifyMusic in searchResults)
            {
                 foreach (var artist in spotifyMusic.artists)
                 {
                    artist.name = PrepareStringToCompare(artist.name);
                    
                    await _messageWriter.SendMessageSocket("Comparing artists: " + songArtist+ "  with : " + artist.name, MessageType.Log, radioName);
                    if (artist.name.Equals(songArtist, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return spotifyMusic;
                    }

                    if (splitBySeparator != null)
                    {
                        foreach (var artistFromMultipleArtists in splitBySeparator)
                        {
                            await _messageWriter.SendMessageSocket("Comparing artists: " + artistFromMultipleArtists + "  with : " + artist.name, MessageType.Log, radioName);

                            if (artist.name.Equals(artistFromMultipleArtists, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return spotifyMusic;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public string PrepareSongTitleForSearch(string songTitle)
        {
            return RemoveDiacritics(songTitle.
                Replace("ft. ", String.Empty, StringComparison.InvariantCultureIgnoreCase).
                Replace("feat. ", String.Empty, StringComparison.InvariantCultureIgnoreCase).
                Replace("(", String.Empty).
                Replace(")", String.Empty));
        }

        public string PrepareStringToCompare(string text)
        {
            text = text.Replace("&nbsp;", String.Empty);
            text = RemoveDiacritics(text); 
            text = RemoveDotsAndApostrophes(text);
            text = DealWithAmpersand(text);
            text = RemoveTheIfFirstWord(text);

            return text;
        }

        public string[] TryToSplitMultipleArtists(string Artists)
        {
            Artists = Artists.ToLower();
            if (Artists.Contains(" e "))
            {
                return Artists.Split(" e ");
            }
            else if (Artists.Contains(" com "))
            {
                return Artists.Split(" com ");
            }
            else if (Artists.Contains(" & "))
            {
                return Artists.Split(" & ");
            }
            else if (Artists.Contains(" and "))
            {
                return Artists.Split(" and ");
            }
            else if (Artists.Contains(" feat "))
            {
                return Artists.Split(" feat ");
    
            }
            else if (Artists.Contains(" ft "))
            {
                return Artists.Split(" ft ");
            }
            
            else if (Artists.Contains(" c/ "))
            {
                return Artists.Split(" c/ ");
            }
            else if (Artists.Contains(" c "))
            {
                return Artists.Split(" c ");
            }
            else if (Artists.Contains(" [+] "))
            {
                return Artists.Split(" [+] ");
            }
            else if (Artists.Contains(" X "))
            {
                return Artists.Split(" X ");
            }
            return null;
        }

        public string RemoveDiacritics(string text)
        {
            text = Regex.Replace(text, "[éèëêð]", "e");
            text = Regex.Replace(text, "[ÉÈËÊ]", "E");
            text = Regex.Replace(text, "[àâä]", "a");
            text = Regex.Replace(text, "[ÀÁÂÃÄÅ]", "A");
            text = Regex.Replace(text, "[àáâãäå]", "a");
            text = Regex.Replace(text, "[ÙÚÛÜ]", "U");
            text = Regex.Replace(text, "[ùúûüµ]", "u");
            text = Regex.Replace(text, "[òóôõöø]", "o");
            text = Regex.Replace(text, "[ÒÓÔÕÖØ]", "O");
            text = Regex.Replace(text, "[ìíîï]", "i");
            text = Regex.Replace(text, "[ÌÍÎÏ]", "I");
            text = Regex.Replace(text, "[š]", "s");
            text = Regex.Replace(text, "[Š]", "S");
            text = Regex.Replace(text, "[ñ]", "n");
            text = Regex.Replace(text, "[Ñ]", "N");
            text = Regex.Replace(text, "[ç]", "c");
            text = Regex.Replace(text, "[Ç]", "C");
            text = Regex.Replace(text, "[ÿ]", "y");
            text = Regex.Replace(text, "[Ÿ]", "Y");
            text = Regex.Replace(text, "[ž]", "z");
            text = Regex.Replace(text, "[Ž]", "Z");
            text = Regex.Replace(text, "[Ð]", "D");
            text = Regex.Replace(text, "[œ]", "oe");
            text = Regex.Replace(text, "[Œ]", "Oe");
            text = Regex.Replace(text, "[Æ]", "Ae");
            text = Regex.Replace(text, "[æ]", "ae");
            text = Regex.Replace(text, "[«»\u201C\u201D\u201E\u201F\u2033\u2036]", "\"");
            text = Regex.Replace(text, "[\u2026]", "...");

            return text;
        }

        public string RemoveTheIfFirstWord(string text)
        {
            if (text.StartsWith("the ", StringComparison.InvariantCultureIgnoreCase))
            {
                text = text.ToLower().ReplaceFirst("the ", string.Empty);
            }
            return text;
        }
        
        public string RemoveDotsAndApostrophes(string text)
        {
            return text.Replace(".", string.Empty).Replace("'",string.Empty).Replace("+ ", string.Empty);
        }

        public string DealWithAmpersand(string text) //ampersand = &
        {
            return text.Replace("&amp;", "&").Replace(" e ", " & ", StringComparison.InvariantCultureIgnoreCase).Replace(" and ", " & ", StringComparison.InvariantCultureIgnoreCase).Replace(", ", " & "); ;
        }

    }
}
