![Sumtotal](https://plsadaptive.s3.amazonaws.com/gmedia/jpg/_i_a6a0bc8dba67fd7c5a4a86c08c49ff20_sumtotal_logo.jpg)
# Introduction
This sample code base is intended to demonstrate the authentication and basic functionality of Sumtotal’s User related OData API. It utilizes SumTotal’s OAuth 2.0 authentication to obtain an access token via User Authorization or Client Credentials (B2B) 
## Prerequisites
- SumTotal Admin account (Permission level tied to OAuth Client Configuration)
- [Visual Studio](https://visualstudio.microsoft.com/vs/)
- [Postman](https://www.getpostman.com)
- [Fiddler](https://www.telerik.com/fiddler)
## Swagger
- https://{site-url}/odata/swagger/ui/index
- https://{site-url}/odata/documentation
- See the swagger documentation for full list of supported OData APIs
## Authentication Types
### [Authorization Code](https://www.oauth.com/oauth2-servers/server-side-apps/authorization-code/)
 The authorization code is a temporary code that the client will exchange for an access token. The code itself is obtained from the authorization server where the user gets a chance to see what the information the client is requesting, and approve or deny the request.
### [Client Credentials](https://oauth.net/2/grant-types/client-credentials/)
The Client Credentials grant type is used by clients to obtain an access token outside of the context of a user. This is typically used by clients to access resources about themselves rather than to access a user's resources.
## Functionality
1. Authorization Code
    - Get Users (GET > odata/api/Person)
    - Get Transcripts (GET > odata/api/Transcript)
    - Get Activities (GET > odata/api/Activity)
2. Client Credentials (B2B)
    - Get Users (GET > odata/api/Person)
    - Get Transcripts (GET > odata/api/Transcript)
    - Get Activities (GET > odata/api/Activity)
## Setup Guide
1. OAuth Configuration 
    - Login in as an Admin
    - Administration > Common Objects > Configuration > OAuth Configuration
    - Click 'Add' to create a new OAuth Client
    - Enter desired Client ID
    - PKCE disabled (Enabling this will block API calls)
    - Enter secure Client Secret
    - Select desired scopes 
        - allapis (Access SumTotal Rest APIs)
        - odatapis (Access to OData APIs) (We use this one)
        - offline_access (Responsible for granting a refresh token)
    - Add a redirect URL you wish to use (Used for Authorization Code Grant Type)
    - Submit
## Code Structure
1. Program.cs
    - The start and end of your C# console application.
    - The Main method is where you are able to create objects and execute methods throughout your solution.
    - Reads weather or not you want to use Client Credentials or Authorization code authentication, this is indicated by the 'b2b' key (Boolean) within your App.Config.
    - Contains the relevant functions required to obtain access tokens & refresh tokens.
2. B2BGrantOauth.cs
    - Reads configuration settings from App.Config.
    - Contains methods relating to obtaining an Access Token via the Client Credential grant type.
3. CodeGrantOauth.cs
    - Responsible for rendering a form in order to load SumTotal login screen.
        - From here a user should have login details to proceed.
    - Contains methods relating to obtaining an Access Token via the Authorization Code grant type.
    - Contains methods relating to obtaining a Refresh Token.
4. ODataOperations.cs
    - Contains GET methods relating to user,transcript and activity operations.
        - Get Users (Authorization Code & Client Credentials).
        - Get Transcripts (Authorization Code & Client Credentials).
        - Get Activities (Authorization Code & Client Credentials).
5. App.Config
    - Configuration needed in order to successfully execute the program.
```sh
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7" />
    </startup>
  <appSettings>
    <!--Enter the client id from the portal.-->
    <add key="ClientId" value="AU02_SALES_TEST_d4e91f6c5e134d2b800f09f241e9b0c2"/>
    <!--Enter the client secret from the portal-->
    <add key="ClientSecret" value="b4cfc23ecaa84bc28a9cd9135fdd9d85b5afe9e4ec764e9c8f8096a3b522215c"/>
    <!--Default scope is odataapis-->
    <add key="Scope" value="odataapis"/>
    <!--Enter the URL for the Service-->
    <add key="Environment" value="https://au02sales.sumtotaldevelopment.net"/>
    <!--Enter the redirect uri for service-->
    <add key="RedirectUri" value="https://test.sumtotal.com/oidc"/>
    <!--Indicate if you want a b2b call-->
    <add key="b2b" value="false"/>
  </appSettings>
</configuration>
```
## Example Requests & Responses
##### AUTHENTICATION REQUEST
    - POST https://{site-url}/apisecurity/connect/token HTTP/1.1
        Accept: application/json
        Content-Type: application/x-www-form-urlencoded
        Host: {site-url}
        Content-Length: 73
        Expect: 100-continue
        Connection: Keep-Alive
        
        client_id=b2b_oidc&client_secret=b2b_secret&grant_type=client_credentials
        
##### AUTHENTICATION RESPONSE
    {"access_token":"eyJhbGciOiJSUzI1NiIsImtpZCI6IkEwQjVCMUFCMTUzMjI1MzRDNUIxQUU3QTdEMjZDRkI3NDYzNTIwMzMiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJvTFd4cXhVeUpUVEZzYTU2ZlNiUHQwWTFJRE0ifQ.eyJuYmYiOjE1NzExMzU3ODgsImV4cCI6MTU3MTE0Mjk4OCwiaXNzIjoiaHR0cHM6Ly9hdTA0c2FsZXMuc3VtdG90YWxkZXZlbG9wbWVudC5uZXQvYXBpc2VjdXJpdHkiLCJhdWQiOlsiaHR0cHM6Ly9hdTA0c2FsZXMuc3VtdG90YWxkZXZlbG9wbWVudC5uZXQvYXBpc2VjdXJpdHkvcmVzb3VyY2VzIiwiZXh0YXBpcyJdLCJjbGllbnRfaWQiOiJiMmJfb2lkYyIsImJyb2tlcnNlc3Npb24iOiJkZjZhMzI5Zi02MjRkLTRkN2ItODBkYy1kNDdhM2U1ODVkYzIiLCJzY29wZSI6WyJhbGxhcGlzIl19.VKurjJBPar7K-KjExPJhInA9T5aIpm6NZyjVCaLLN5Dt4QJkQZTZq7p0EhEfzshtqVck2GSba-pxLNkPLbeONkBKTcYQGRKUgzlk787NPnn4_fSHCOxy-LDykIbv6G_zWcT3RW9_DE4ap5t2tmTPPgEHi3huYx_YabYL4WSpSslbs7tttIi1qI2m9NpN3apsT8uMT7izr5PbmrHGGWPhBI-lmwjx2l2Y8mh62ErPm281VSYVSrTkRPPSQHrkySLskYYiGXy0zUZuIa5abveTnTqFH9uxWL1Nt-wuC4AgRhacJTcmdaBynN8mguvQaL64fcNTt1yl9Tnf2T6XFDKogQ","expires_in":7200,"token_type":"Bearer","scope":"allapis"}
    

    
##### GET USERS REQUEST
    
    GET https://{site-url}/odata/api/Person HTTP/1.1
    
    Authorization: Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IkMyNTFGQzY0REExNDkxOTgxREIxQUIzQjVGNTkwNUQxRjlBRkNEQkIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJ3bEg4Wk5vVWtaZ2RzYXM3WDFrRjBmbXZ6YnMifQ.eyJuYmYiOjE1NzExNDQ4OTcsImV4cCI6MTU3MTE0ODQ5NywiaXNzIjoiaHR0cDovL3N0cy1kZXYvYXBpc2VjdXJpdHkiLCJhdWQiOlsiaHR0cDovL3N0cy1kZXYvYXBpc2VjdXJpdHkvcmVzb3VyY2VzIiwiZXh0YXBpcyJdLCJjbGllbnRfaWQiOiJzdW10b3RhbF9wb3dlcmJpIiwic3ViIjoiYWRtaW4iLCJhdXRoX3RpbWUiOjE1NzExNDQ4OTUsImlkcCI6ImxvY2FsIiwibmFtZSI6ImFkbWluIiwidXNlcm5hbWUiOiJhZG1pbiIsInJvbGUiOiJQb3J0YWwgVXNlciIsInRlbmFudCI6InN0cy1kZXYiLCJicm9rZXJzZXNzaW9uIjoiNDk1YTY2MTY5ZmE4NDJlNGEzMmM5YjQ1M2RjYzQ3MGUiLCJjdWx0dXJlIjoiZW4tVVMiLCJsYW5ndWFnZSI6ImVuLXVzIiwiZGF0ZWZvcm1hdCI6Ik1NL2RkL3l5eXkiLCJ0aW1lZm9ybWF0IjoiaGg6bW0gYSIsInVzZXJpZCI6IjEiLCJwZXJzb25wayI6Ii0xIiwiZ3Vlc3RhY2NvdW50IjoiMCIsInVzZXJ0aW1lem9uZWlkIjoiRXVyb3BlL1p1cmljaCIsInByb3Blcm5hbWUiOiJIZWF0aGVyK1Jvc2UiLCJwaG90b3VybCI6IjE5ZTM1MzNiYWY5YTQ3YTc5YjBlNjVlZTU4NTc0ZTM4LnBuZyIsInNjb3BlIjpbInByb2ZpbGUiLCJvcGVuaWQiLCJhbGxhcGlzIl0sImFtciI6WyJwd2QiXX0.KUk9booNAjrwk84cLcVSAOGdAu5sA3duj8xCvJ1B74EbJVRWRCy_O5ZpWUIojLzOOeOXrUGJ8yiwnGOMS2Ih5x-EFERARAo3nXHBHlDxd-nbYoyG5_F3DBf-Zcat9isKd2rW8mvT0b1gJ4PQSb4PGnGG7wd3jCM2_sqelWAn1GnhIKNQPHs2K5Z6_YFT-epj_toiWYfleg0G50Kb2OPL5dtCK7_jNZskdUeGaX7ZDHwi9Px8nOabuUrGNl-NbBDRnOajm8YoyGHeR3-mUtOrX4SO78MnjtqZoV0gRfc6G6nmrXyePFr8PsBiFPR-MCH_5SBtJmzdsftvy5JfbWOdVA
    
    Content-Type: application/x-www-form-urlencoded
    Host: {site-url}

##### GET USERS RESPONSE
##### NOTE: The API Response has been truncated to only return the first 4 records from the response to sample the functionality. in real execution first 1000 records will be returend by default
    {
      {
          "@odata.context": "https://au02sales.sumtotaldevelopment.net/odata/api/$metadata#Person",
          "value": [
              {
                  "@odata.etag": "W/\"YmluYXJ5J0FBQUFBQk8yZmhzPSc=\"",
                  "Active": 1,
                  "ModifiedDate": "2018-01-31T22:47:10+02:00",
                  "PersonGuid": "6b5f2741-4c81-4412-b9eb-1a2fdce259c4",
                  "PrefixCode": "-1",
                  "PrefixText": null,
                  "LastNameNative": null,
                  "FirstNameNative": null,
                  "FriendlyName": null,
                  "SuffixCode": null,
                  "SuffixText": null,
                  "TypeCode": null,
                  "PublishCalendar": true,
                  "NoteText": null,
                  "EthnicityCode": null,
                  "GenderCode": null,
                  "TimezoneId": 50,
                  "CompanyCode": null,
                  "CompanyName": null,
                  "EndDate": null,
                  "BirthDate": null,
                  "LastReviewDate": null,
                  "URL": null,
                  "Deleted": 0,
                  "Description": null,
                  "InternalInd": true,
                  "CurrencyId": 11,
                  "FullName": "Removed User",
                  "SearchKey": "Removed User",
                  "GovermentId": null,
                  "SelfAccount": 0,
                  "GuestAccount": 0,
                  "ImagePath": null,
                  "Lock": 0,
                  "CreatedBy": "DB Installer",
                  "ModifiedBy": "DB Installer",
                  "AgencySubElementCode": null,
                  "WFMUser": true,
                  "LocaleId": 1,
                  "MiddleName": null,
                  "StartDate": null,
                  "UserId": "-33",
                  "RowVersionBytes": "AAAAABO2fhs=",
                  "RowVersionId": 1981221078810230784,
                  "Id": -33,
                  "FirstName": "Removed",
                  "LastName": "User",
                  "CreatedOn": "2018-01-31T22:47:10+02:00",
                  "StatusCode": null
              },
              {
                  "@odata.etag": "W/\"YmluYXJ5J0FBQUFBQk8yZmh3PSc=\"",
                  "Active": 1,
                  "ModifiedDate": "2014-06-11T08:21:31+02:00",
                  "PersonGuid": "4b2736b0-57a3-40f9-ac3a-8bc496ef624f",
                  "PrefixCode": null,
                  "PrefixText": "",
                  "LastNameNative": "",
                  "FirstNameNative": "",
                  "FriendlyName": "",
                  "SuffixCode": null,
                  "SuffixText": null,
                  "TypeCode": 0,
                  "PublishCalendar": true,
                  "NoteText": null,
                  "EthnicityCode": null,
                  "GenderCode": null,
                  "TimezoneId": 50,
                  "CompanyCode": null,
                  "CompanyName": null,
                  "EndDate": null,
                  "BirthDate": "1950-06-02T04:31:36.067+02:00",
                  "LastReviewDate": null,
                  "URL": null,
                  "Deleted": 0,
                  "Description": "",
                  "InternalInd": true,
                  "CurrencyId": 0,
                  "FullName": "extcloudguest Guest",
                  "SearchKey": "extcloudguest Guest extcloudguest Guest 10",
                  "GovermentId": null,
                  "SelfAccount": 0,
                  "GuestAccount": 0,
                  "ImagePath": "",
                  "Lock": 0,
                  "CreatedBy": "systemusr",
                  "ModifiedBy": "systemusr",
                  "AgencySubElementCode": null,
                  "WFMUser": true,
                  "LocaleId": 1,
                  "MiddleName": null,
                  "StartDate": null,
                  "UserId": "10",
                  "RowVersionBytes": "AAAAABO2fhw=",
                  "RowVersionId": 2053278672848158720,
                  "Id": -32,
                  "FirstName": "extcloudguest Guest",
                  "LastName": "extcloudguest Guest",
                  "CreatedOn": "2016-01-13T11:14:44.83+02:00",
                  "StatusCode": null
              },
              {
                  "@odata.etag": "W/\"YmluYXJ5J0FBQUFBQk8yZmgwPSc=\"",
                  "Active": 1,
                  "ModifiedDate": "2011-04-08T17:47:18+02:00",
                  "PersonGuid": "3b17d9cc-95b7-4054-b7a1-f15f65c1e3ed",
                  "PrefixCode": null,
                  "PrefixText": "",
                  "LastNameNative": "",
                  "FirstNameNative": "",
                  "FriendlyName": "",
                  "SuffixCode": null,
                  "SuffixText": null,
                  "TypeCode": 1,
                  "PublishCalendar": true,
                  "NoteText": null,
                  "EthnicityCode": null,
                  "GenderCode": null,
                  "TimezoneId": 50,
                  "CompanyCode": null,
                  "CompanyName": null,
                  "EndDate": null,
                  "BirthDate": "1951-06-02T04:31:36.067+02:00",
                  "LastReviewDate": null,
                  "URL": null,
                  "Deleted": 0,
                  "Description": "",
                  "InternalInd": true,
                  "CurrencyId": 0,
                  "FullName": "Web Service Admin",
                  "SearchKey": "Web Service  Admin 9",
                  "GovermentId": null,
                  "SelfAccount": 0,
                  "GuestAccount": 0,
                  "ImagePath": "",
                  "Lock": 0,
                  "CreatedBy": "admin",
                  "ModifiedBy": "admin",
                  "AgencySubElementCode": null,
                  "WFMUser": true,
                  "LocaleId": 1,
                  "MiddleName": "",
                  "StartDate": null,
                  "UserId": "9",
                  "RowVersionBytes": "AAAAABO2fh0=",
                  "RowVersionId": 2125336266886086656,
                  "Id": -31,
                  "FirstName": "Web Service",
                  "LastName": "Admin",
                  "CreatedOn": "2011-04-08T17:47:18+02:00",
                  "StatusCode": null
              },
              {
                  "@odata.etag": "W/\"YmluYXJ5J0FBQUFBQk8yZmg0PSc=\"",
                  "Active": 1,
                  "ModifiedDate": "2016-05-27T15:10:25+02:00",
                  "PersonGuid": "b4bd4f46-029c-7934-9466-005056b88c36",
                  "PrefixCode": null,
                  "PrefixText": "",
                  "LastNameNative": "",
                  "FirstNameNative": "",
                  "FriendlyName": "",
                  "SuffixCode": null,
                  "SuffixText": null,
                  "TypeCode": null,
                  "PublishCalendar": true,
                  "NoteText": null,
                  "EthnicityCode": null,
                  "GenderCode": null,
                  "TimezoneId": 50,
                  "CompanyCode": null,
                  "CompanyName": null,
                  "EndDate": null,
                  "BirthDate": "1952-06-02T04:31:36.067+02:00",
                  "LastReviewDate": null,
                  "URL": null,
                  "Deleted": 0,
                  "Description": "",
                  "InternalInd": true,
                  "CurrencyId": 0,
                  "FullName": "vendor Guest",
                  "SearchKey": "vendor Guest vendor Guest -30",
                  "GovermentId": null,
                  "SelfAccount": 0,
                  "GuestAccount": 0,
                  "ImagePath": "",
                  "Lock": 0,
                  "CreatedBy": "systemusr",
                  "ModifiedBy": "batchadmin",
                  "AgencySubElementCode": null,
                  "WFMUser": true,
                  "LocaleId": 1,
                  "MiddleName": null,
                  "StartDate": null,
                  "UserId": "-30",
                  "RowVersionBytes": "AAAAABO2fh4=",
                  "RowVersionId": 2197393860924014592,
                  "Id": -30,
                  "FirstName": "vendor Guest",
                  "LastName": "vendor Guest",
                  "CreatedOn": "2008-10-10T00:00:00+02:00",
                  "StatusCode": null
              }
          ],
          "@odata.nextLink": "http://sts-dev/odata/api/Person?$skip=1000"
    }