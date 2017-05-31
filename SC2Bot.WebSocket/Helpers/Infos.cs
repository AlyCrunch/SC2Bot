using AligulacSC2;
using System;

namespace SC2Bot.WebSocket.Helpers
{
    public class Infos
    {
        private static string _botApi = string.Empty;
        public static string BotAPI
        {
            get
            {
                if (_botApi == string.Empty)
                    _botApi = Environment.GetEnvironmentVariable("APIBotSC2");
                return _botApi;
            }
        }

        private static Aligulac _aligulac;
        public static Aligulac Aligulac
        {
            get
            {
                if (_aligulac == null)
                    _aligulac = new Aligulac(Environment.GetEnvironmentVariable("APIBotAligulacSC2"));
                return _aligulac;
            }
        }

        private static string _imgurAPI = string.Empty;
        public static string ImgurAPI
        {
            get
            {
                if (_imgurAPI == null || _imgurAPI == string.Empty)
                    _imgurAPI = Environment.GetEnvironmentVariable("APIImgur");
                return _imgurAPI;
            }
        }
    }
}
