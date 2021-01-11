using System;
using System.Configuration;

namespace OAuth2ODataSample
{
	//The start and end of your C# console application.
	//The Main method is where you are able to create objects and execute methods throughout your solution.
	//Reads weather or not you want to use Client Credentials or Authorization code authentication, this is indicated by the 'b2b' key (Boolean) within your App.Config.
	//Contains the relevant functions required to obtain access tokens & refresh tokens.
	static class Program
	{
		// The application ID that you were given when you
		// registered your application. This is for a 
		// desktop app so there's not client secret.

		private static string _clientId = ConfigurationManager.AppSettings.Get("ClientId");

		// If _storedRefreshToken is null, CodeGrantFlow goes
		// through the entire process of getting the user credentials
		// and permissions. If _storedRefreshToken contains the refresh
		// token, CodeGrantFlow returns the new access and refresh tokens.

		private static string _storedRefreshToken = null;

		private static DateTime _tokenExpiration;

		[STAThread]
		private static void Main(string[] args)
		{
			CodeGrantOauth _tokens = null;
			B2BGrantOauth _b2bTokens = null;

			try
			{
				bool b2b = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("b2b"));

				ODataOperations userOperations = new ODataOperations();

				if (!b2b)
				{
					_tokens = GetOauthTokens_AuthCode(_storedRefreshToken, _clientId);

					PrintTokens(_tokens.AccessToken, _tokens.RefreshToken, _tokens.Expiration);
					userOperations.GetODataUsers(_tokens.AccessToken);

					//uncomment below to run user context api calls for Transcript or Activity
					//userOperations.GetODataTranscripts(_tokens.AccessToken);
					//userOperations.GetODataActivity(_tokens.AccessToken);
				}
				else
				{
					_b2bTokens = GetToken(_clientId);

					PrintTokens(_b2bTokens.AccessToken, _b2bTokens.RefreshToken, _b2bTokens.Expiration);
					userOperations.GetODataUsers(_b2bTokens.AccessToken);

					//uncomment below to run B2B api calls for Transcript or Activity
					//userOperations.GetODataTranscripts(_tokens.AccessToken);
					//userOperations.GetODataActivity(_tokens.AccessToken);
				}
				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine("\n" + e.Message);
			}
		}

		public static void PrintTokens(string accessToken, string refreshToken, int tokenExpiry)
		{
			Console.BackgroundColor = ConsoleColor.Green;
			Console.WriteLine("access token:");
			Console.ResetColor();
			Console.WriteLine(accessToken);
			if (!String.IsNullOrEmpty(refreshToken))
			{
				Console.BackgroundColor = ConsoleColor.Green;
				Console.WriteLine("refresh token:");
				Console.ResetColor();
				Console.WriteLine(refreshToken);
			}
			Console.BackgroundColor = ConsoleColor.Green;
			Console.WriteLine("token expires");
			Console.ResetColor();
			Console.WriteLine(tokenExpiry);
		}

		private static B2BGrantOauth GetToken(string clientId)
		{
			B2BGrantOauth auth = new B2BGrantOauth(clientId);

			auth.GetAccessToken();

			return auth;
		}

		private static CodeGrantOauth GetOauthTokens_AuthCode(string refreshToken, string clientId)
		{
			CodeGrantOauth auth = new CodeGrantOauth(clientId);

			if (string.IsNullOrEmpty(refreshToken))
			{
				auth.GetAccessToken();
			}
			else
			{
				auth.RefreshAccessToken(refreshToken);

				// Refresh tokens can become invalid for several reasons
				// such as the user's password changed.

				if (!string.IsNullOrEmpty(auth.Error))
				{
					auth = GetOauthTokens_AuthCode(null, clientId);
				}
			}

			if (!string.IsNullOrEmpty(auth.Error))
			{
				Console.WriteLine("An Error occured:");
				Console.WriteLine(auth.Error);
				Console.ReadKey();
			}
			else
			{
				_storedRefreshToken = auth.RefreshToken;
				_tokenExpiration = DateTime.Now.AddSeconds(auth.Expiration);
			}

			return auth;
		}
	}
}
