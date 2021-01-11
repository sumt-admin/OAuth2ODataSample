using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace OAuth2ODataSample
{
	//Reads configuration settings from App.Config.
	//Contains methods relating to obtaining an Access Token via the Client Credential grant type.
	public class B2BGrantOauth
    {

		// Production OAuth server endpoints.
		public readonly static string env = ConfigurationManager.AppSettings.Get("Environment");
        public readonly static string redirect = ConfigurationManager.AppSettings.Get("RedirectUri");
        public readonly static string clientId = ConfigurationManager.AppSettings.Get("ClientId");
        public readonly static string clientSecret = ConfigurationManager.AppSettings.Get("ClientSecret");

        private readonly string AuthorizationUri = env + "/apisecurity/connect/authorize";  // Authorization code endpoint
        private readonly string RefreshUri = env + "/apisecurity/connect/token";  // Get tokens endpoint

        private readonly string CodeQueryString = "?client_id={0}&client_secret={1}&scope=allapis&response_type=code";
        private readonly string AccessBody = "client_id={0}&client_secret={1}&grant_type=client_credentials";

        private readonly string _clientId = null;
        private readonly string _clientSecret = null;
        private readonly string _uri = null;

		public string AccessToken { get; private set; } = null;
		public string RefreshToken { get; private set; } = null;
		public int Expiration { get; private set; }
		public string Error { get; private set; } = null;

		public B2BGrantOauth(string clientId)
        {

            if (string.IsNullOrEmpty(clientId))
            {
				Console.WriteLine("An Error occured:");
				Console.WriteLine("The client ID is missing.");
				Console.ReadKey();
            }

            this._clientId = clientId;
            this._clientSecret = clientSecret;
            this._uri = string.Format(this.AuthorizationUri + this.CodeQueryString, this._clientId, this._clientSecret);
        }

        public string GetAccessToken()
        {
            try
            {
                var accessTokenRequestBody = string.Format(this.AccessBody, this._clientId, this._clientSecret);
                AccessTokens tokens = GetTokens(this.RefreshUri, accessTokenRequestBody);
                this.AccessToken = tokens.AccessToken;
                this.RefreshToken = tokens.RefreshToken;
                this.Expiration = tokens.Expiration;
            }
            catch (WebException)
            {
                this.Error = "GetAccessToken failed likely due to an invalid client ID, client secret, or authorization code";
            }

            return this.AccessToken;
        }

        private static AccessTokens GetTokens(string uri, string body)
        {

            try
            {
                AccessTokens tokens = null;
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = body.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    StreamWriter writer = new StreamWriter(requestStream);
                    writer.Write(body);
                    writer.Close();
                }

                var response = (HttpWebResponse)request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream);
                    string json = reader.ReadToEnd();
                    reader.Close();
                    tokens = JsonConvert.DeserializeObject(json, typeof(AccessTokens)) as AccessTokens;
                }

                return tokens;
            }
            catch (Exception ex)
            {
				Console.WriteLine("An Error occured:");
				Console.WriteLine(ex.Message);
				Console.ReadKey();

				return null;
            }
        }

    }
}
