using SpotifyPlaylistRadio.Models.ReadDataFromRadio;

namespace SpotifyPlaylistRadio.Models
{
    public class ReadDataFactory
    {
        public IDataFromRadio readDataFromRadio(string request)
        {            
            IDataFromRadio dataFromRadio = null;
            if (DataFormat.XML.ToString().Equals(request)){
                dataFromRadio = new RadioInfo();
            }
            else if (DataFormat.XML2.ToString().Equals(request))
            {
                dataFromRadio = new Music();
            }
            else if (DataFormat.JSONRadioNovaEra.ToString().Equals(request))
            {
                dataFromRadio = new JSONNovaEra();
            }
            else if (DataFormat.ListAntena3.ToString().Equals(request))
            {
                dataFromRadio = new ListAntena3();
            }

            return dataFromRadio;
        }
    }

    enum DataFormat
    {
        XML,
        XML2,
        JSONRadioNovaEra,
        ListAntena3
    }
}
