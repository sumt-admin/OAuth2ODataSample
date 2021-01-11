using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace OAuth2ODataSample
{
	//Current list of all OData Entities supported by the API
	//   "Activity",
	//   "ActivityDomain",
	//   "ActivityLabel",
	//   "ActivityLink",
	//   "ActivityLocation",
	//   "ActivityMetadata",
	//   "ContentType",
	//   "DomainFacility",
	//   "EmployeeOptional1",
	//   "EmployeeOptional2",
	//   "EmployeeOptional3",
	//   "EvaluationAnswer",
	//   "EvaluationQuestion",
	//   "Facility",
	//   "Instructor",
	//   "Job",
	//   "Location",
	//   "Manager",
	//   "MediaType",
	//   "Organization",
	//   "Person",
	//   "PersonJob",
	//   "PersonOrganization",
	//   "Registration",
	//   "Role",
	//   "RolePermission",
	//   "Transcript",
	//   "UserLogin",
	//   "UserLoginHistory",
	//   "UserRequiredActivity"

	//Contains GET methods relating to user, transcript and activity operations.
	//Get Users(Authorization Code & Client Credentials).
	//Get Transcripts(Authorization Code & Client Credentials).
	//Get Activities(Authorization Code & Client Credentials).
    public class ODataOperations
    {
		readonly string env = ConfigurationManager.AppSettings.Get("Environment");

        public void GetODataUsers(string accessToken)
        {
            try
            {
                Console.WriteLine("Get Persons OData Call");

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(env + "/odata/api/Person");
                httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    Console.WriteLine(responseText); 
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

		public void GetODataTranscripts(string accessToken)
		{
			try
			{
				Console.WriteLine("Get Transcripts OData Call");

				var httpWebRequest = (HttpWebRequest)WebRequest.Create(env + "/odata/api/Transcript");
				httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.Method = "GET";

				var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					var responseText = streamReader.ReadToEnd();
					Console.WriteLine(responseText);
				}
			}
			catch (WebException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void GetODataActivity(string accessToken)
		{
			try
			{
				Console.WriteLine("Get Activities OData Call");

				var httpWebRequest = (HttpWebRequest)WebRequest.Create(env + "/odata/api/Activity");
				httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.Method = "GET";

				var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					var responseText = streamReader.ReadToEnd();
					Console.WriteLine(responseText);
				}
			}
			catch (WebException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
