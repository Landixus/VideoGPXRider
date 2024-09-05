using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi.DTO;
using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.SimpleJSONLib;
using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi
{
    public class OpenWeatherApiClient
    {
        private readonly string _apiKey;

        public OpenWeatherApiClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        private Uri GenerateRequestUrl(string queryString, LanguageKey languageKey) => new Uri($"http://api.openweathermap.org/data/2.5/weather?only_current=true&appid={_apiKey}&q={queryString}&lang={languageKey.ToString().ToLower()}");

        public async Task<WeatherInformation> QueryAsync(string queryString, LanguageKey languageKey = LanguageKey.EN)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(GenerateRequestUrl(queryString, languageKey)))
            {
                webRequest.SendWebRequest();

                while (!webRequest.isDone) { }
                string jsonResponse = null;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                    case UnityWebRequest.Result.ProtocolError:
                        throw new Exception(webRequest.error);
                    case UnityWebRequest.Result.Success:
                        jsonResponse = webRequest.downloadHandler.text;
                        break;
                }

                if (jsonResponse == null)
                    throw new ArgumentNullException("JSON Response from OpenWeatherMap api is null!");

                return new WeatherInformation(JSON.Parse(jsonResponse));
            }
        }

        [Serializable]
        public enum LanguageKey
        {
            AF,
            AL,
            AR,
            AZ,
            BG,
            CA,
            CZ,
            DA,
            DE,
            EL,
            EN,
            EU,
            FA,
            FI,
            FR,
            GL,
            HE,
            HI,
            HR,
            HU,
            ID,
            IT,
            JA,
            KR,
            LA,
            LT,
            MK,
            NO,
            NL,
            PL,
            PT,
            PT_BR,
            RO,
            RU,
            SV,
            SK,
            SL,
            ES,
            SR,
            TH,
            TR,
            UA,
            VI,
            ZH_CN,
            ZH_TW,
            ZU
        }
    }
}
