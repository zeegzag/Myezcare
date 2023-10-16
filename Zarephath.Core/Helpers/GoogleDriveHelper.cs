using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Helpers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Net;
    using Zarephath.Core.Infrastructure;
    using Zarephath.Core.Models;
    using Zarephath.Core.Models.ViewModel;

    public class GoogleDriveHelper
    {
        private static T ParseJson<T>(string json) where T : class, new()
        {
            var jobject = JObject.Parse(json);
            return JsonConvert.DeserializeObject<T>(jobject.ToString());
        }


        public string GetAuthenticationUrl(long OrganizationId)
        {
            var organizationIDBytes = System.Text.Encoding.UTF8.GetBytes("orgid=" + OrganizationId.ToString());
            var organizationIDEncoded = System.Convert.ToBase64String(organizationIDBytes);


            string oAuthUrl = ConfigSettings.GoogleDriveAuthUri + "?";

            // Determines whether the Google OAuth 2.0 endpoint returns an authorization code. Web server applications should use code.
            oAuthUrl += "response_type=code";

            //Identifies the client that is making the request. 
            //The value passed in this parameter must exactly match the value shown in the "Google Cloud Console".
            oAuthUrl += "&client_id=" + ConfigSettings.GoogleDriveClientID;

            // Determines where the response is sent. 
            // The value of this parameter must exactly match one of the values registered in the Google Cloud Console 
            // (including the http or https scheme, case, and trailing '/').
            oAuthUrl += "&redirect_uri=" + ConfigSettings.GoogleDriveAuthCallback; //+ " ? drive=" + driveId.ToString();

            // Space-delimited set of permissions that the application requests.
            oAuthUrl += "&scope=https://www.googleapis.com/auth/drive.file https://www.googleapis.com/auth/drive.readonly https://www.googleapis.com/auth/drive";
            //oAuthUrl += "&scope=https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.file https://www.googleapis.com/auth/drive.readonly https://www.googleapis.com/auth/drive.metadata.readonly https://www.googleapis.com/auth/drive.appdata https://www.googleapis.com/auth/drive.metadata https://www.googleapis.com/auth/drive.photos.readonly";

            // https://www.googleapis.com/auth/drive.metadata.readonly

            //https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.appdata https://www.googleapis.com/auth/drive.file https://www.googleapis.com/auth/drive.metadata.readonly https://www.googleapis.com/auth/drive.readonly";
            //https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.file https://www.googleapis.com/auth/drive.readonly https://www.googleapis.com/auth/drive.metadata.readonly https://www.googleapis.com/auth/drive.appdata https://www.googleapis.com/auth/drive.metadata https://www.googleapis.com/auth/drive.photos.readonly


            // If your application needs to refresh access tokens when the user is not present at the browser, then use offline
            oAuthUrl += "&access_type=offline";

            // If the value is force, then the user sees a consent page even if they previously gave consent to your application for a given set of scopes
            oAuthUrl += "&approval_prompt=auto";

            // state = I am using it for set of params
            oAuthUrl += "&state=" + organizationIDEncoded;

            return oAuthUrl;
        }

        public GoogleToken GetAccessToken(long OrganizationId, string authCode)
        {
            try
            {
                // encode drive ID information
                var driveInfoBytes = System.Text.Encoding.UTF8.GetBytes("orgid=" + OrganizationId.ToString());
                var driveInfoEncoded = System.Convert.ToBase64String(driveInfoBytes);

                string requestUri = "";
                string postData = "";

                requestUri = ConfigSettings.GoogleDriveTokenUri + "?";

                postData += "code=" + authCode;
                postData += "&client_id=" + ConfigSettings.GoogleDriveClientID;
                postData += "&client_secret=" + ConfigSettings.GoogleDriveClientSecret;
                postData += "&redirect_uri=" + ConfigSettings.GoogleDriveAuthCallback;
                postData += "&grant_type=authorization_code";
                //postData += "&state=" + driveInfoEncoded;

                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Method = WebRequestMethods.Http.Post;
                request.KeepAlive = true;
                request.ContentType = "application/x-www-form-urlencoded";

                var data = System.Text.Encoding.ASCII.GetBytes(postData);

                request.ContentLength = data.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }

                var response = request.GetResponse();
                var reader = new System.IO.StreamReader(response.GetResponseStream());
                //var responseString = reader.ReadToEnd();

                var json = reader.ReadToEnd();
                return ParseJson<GoogleToken>(json);
            }
            catch { }

            return null;
        }

        private GoogleToken RefreshAccessToken(string refreshToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    string requestUri = "";
                    string postData = "";

                    requestUri = ConfigSettings.GoogleDriveTokenUri + "?";

                    postData += "refresh_token=" + refreshToken;
                    postData += "&client_id=" + ConfigSettings.GoogleDriveClientID;
                    postData += "&client_secret=" + ConfigSettings.GoogleDriveClientSecret;
                    postData += "&grant_type=refresh_token";

                    // get access token from google
                    var request = (HttpWebRequest)WebRequest.Create(requestUri);
                    request.Method = WebRequestMethods.Http.Post;
                    request.KeepAlive = true;
                    request.ContentType = "application/x-www-form-urlencoded";

                    var data = System.Text.Encoding.ASCII.GetBytes(postData);

                    request.ContentLength = data.Length;
                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(data, 0, data.Length);
                    }

                    var response = request.GetResponse();
                    var reader = new System.IO.StreamReader(response.GetResponseStream());
                    //var responseString = reader.ReadToEnd();

                    var json = reader.ReadToEnd();
                    return ParseJson<GoogleToken>(json);
                }
            }
            catch { }

            return null;
        }


        public ServiceResponse SaveFile(string refreshToken, byte[] localFileContents, string contentType, string destinationFolderId = "", string fileName = "", string fileExtension = "")
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                Guid guid = Guid.NewGuid();
                string fName = guid + "-" + DateTime.Now.ToString(ConfigSettings.DateFormatForSaveFile);

                string actualDestinationPath = destinationFolderId;

                if (string.IsNullOrEmpty(fileExtension))
                    fileExtension = fileName.Split('.').Last();

                string fullFileName = string.Format("{0}{1}.{2}", actualDestinationPath, fName, fileExtension);


                string googleResponse = "";

                if (!string.IsNullOrEmpty(destinationFolderId))
                {
                    // upload file with specific folder
                    googleResponse = UploadFile(refreshToken, fullFileName, localFileContents, contentType, destinationFolderId);
                }
                else
                {
                    // upload file at root folder
                    googleResponse = UploadFile(refreshToken, fullFileName, localFileContents, contentType);
                }

                string fileUrl = "";
                if (!string.IsNullOrEmpty(googleResponse))
                {
                    dynamic googleResponseObj = JsonConvert.DeserializeObject(googleResponse);
                    fileUrl = (string)googleResponseObj.embedLink;
                }

                UploadedFileModel fileModel = new UploadedFileModel
                {
                    FileOriginalName = fileName,
                    TempFileName = string.Format("{0}.{1}", fName, fileExtension),
                    GoogleFileJson = googleResponse
                };

                fileModel.TempFilePath = fileUrl; // destinationFolderId + fileModel.TempFileName;
                response.IsSuccess = true;
                response.Data = fileModel;
                return response;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;// Resource.ExceptionMessage;
                response.IsSuccess = false;
            }
            return response;
        }

        private string UploadFile(string refreshToken, string fileName, byte[] localFileContents, string contentType, string remoteFolderId)
        {
            string result = string.Empty;

            // get refreshed auth token
            var token = this.RefreshAccessToken(refreshToken);


            ///////////////////// FOR Metadata /////////////////////

            List<string> _postData = new List<string>();

            _postData.Add("{");
            _postData.Add("\"title\": \"" + fileName + "\",");
            _postData.Add("\"description\": \"Uploaded with EZCare\",");
            _postData.Add("\"parents\": [{\"id\":\"" + remoteFolderId + "\"}],");
            _postData.Add("\"mimeType\": \"" + contentType + "\"");
            _postData.Add("}");
            string postData = string.Join(" ", _postData.ToArray());
            byte[] MetaDataByteArray = Encoding.UTF8.GetBytes(postData);

            string boundry = "foo_bar_baz";

            ////////////////////////////////////////////////////////

            string requestUri = "";

            requestUri = requestUri = ConfigSettings.GoogleDriveContentUri + "?uploadType=multipart";

            // get access token from google
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = WebRequestMethods.Http.Post;
            request.KeepAlive = true;
            request.ContentType = "multipart/related; boundary=\"" + boundry + "\"";

            request.Headers["Authorization"] = "Bearer " + token.AccessToken;

            // Wrighting Meta Data
            string headerJson = string.Format("--{0}\r\nContent-Type: {1}\r\n\r\n",
                                                boundry,
                                                "application/json; charset=UTF-8");
            string headerFile = string.Format("\r\n--{0}\r\nContent-Type: {1}\r\n\r\n",
                                                boundry,
                                                contentType);

            string footer = "\r\n--" + boundry + "--\r\n";

            int headerLenght = headerJson.Length + headerFile.Length + footer.Length;
            request.ContentLength = MetaDataByteArray.Length + localFileContents.Length + headerLenght;
            var dataStream = request.GetRequestStream();

            // write the MetaData ContentType
            dataStream.Write(Encoding.UTF8.GetBytes(headerJson), 0, Encoding.UTF8.GetByteCount(headerJson));

            // write the MetaData
            dataStream.Write(MetaDataByteArray, 0, MetaDataByteArray.Length);

            // write the File ContentType
            dataStream.Write(Encoding.UTF8.GetBytes(headerFile), 0, Encoding.UTF8.GetByteCount(headerFile));

            // write the file
            dataStream.Write(localFileContents, 0, localFileContents.Length);

            // Add the end of the request.  Start with a newline

            dataStream.Write(Encoding.UTF8.GetBytes(footer), 0, Encoding.UTF8.GetByteCount(footer));
            dataStream.Close();

            // Check if file is there
            using (WebResponse response = request.GetResponse())
            {
                // Get the stream containing content returned by the server.
                using (dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    using (var reader = new System.IO.StreamReader(dataStream))
                    {
                        // Read the content.
                        result = reader.ReadToEnd();

                        // Display the content.
                        //Console.WriteLine(responseFromServer);
                    }
                }
            }

            return result;
        }

        private string UploadFile(string refreshToken, string fileName, byte[] localFileContents, string contentType)
        {
            string result = string.Empty;

            // get refreshed auth token
            var token = this.RefreshAccessToken(refreshToken);


            ///////////////////// FOR Metadata /////////////////////

            List<string> _postData = new List<string>();

            _postData.Add("{");
            _postData.Add("\"title\": \"" + fileName + "\",");
            _postData.Add("\"description\": \"Uploaded with EZCare\",");
            _postData.Add("\"mimeType\": \"" + contentType + "\"");
            _postData.Add("}");
            string postData = string.Join(" ", _postData.ToArray());
            byte[] MetaDataByteArray = Encoding.UTF8.GetBytes(postData);

            string boundry = "foo_bar_baz";

            ////////////////////////////////////////////////////////

            string requestUri = "";

            requestUri = requestUri = ConfigSettings.GoogleDriveContentUri + "?uploadType=multipart";

            // get access token from google
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = WebRequestMethods.Http.Post;
            request.KeepAlive = true;
            request.ContentType = "multipart/related; boundary=\"" + boundry + "\"";

            request.Headers["Authorization"] = "Bearer " + token.AccessToken;

            // Wrighting Meta Data
            string headerJson = string.Format("--{0}\r\nContent-Type: {1}\r\n\r\n",
                                                boundry,
                                                "application/json; charset=UTF-8");
            string headerFile = string.Format("\r\n--{0}\r\nContent-Type: {1}\r\n\r\n",
                                                boundry,
                                                contentType);

            string footer = "\r\n--" + boundry + "--\r\n";

            int headerLenght = headerJson.Length + headerFile.Length + footer.Length;
            request.ContentLength = MetaDataByteArray.Length + localFileContents.Length + headerLenght;
            var dataStream = request.GetRequestStream();

            // write the MetaData ContentType
            dataStream.Write(Encoding.UTF8.GetBytes(headerJson), 0, Encoding.UTF8.GetByteCount(headerJson));

            // write the MetaData
            dataStream.Write(MetaDataByteArray, 0, MetaDataByteArray.Length);

            // write the File ContentType
            dataStream.Write(Encoding.UTF8.GetBytes(headerFile), 0, Encoding.UTF8.GetByteCount(headerFile));

            // write the file
            dataStream.Write(localFileContents, 0, localFileContents.Length);

            // Add the end of the request.  Start with a newline

            dataStream.Write(Encoding.UTF8.GetBytes(footer), 0, Encoding.UTF8.GetByteCount(footer));
            dataStream.Close();

            // Check if file is there
            using (WebResponse response = request.GetResponse())
            {
                // Get the stream containing content returned by the server.
                using (dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    using (var reader = new System.IO.StreamReader(dataStream))
                    {
                        // Read the content.
                        result = reader.ReadToEnd();

                        // Display the content.
                        //Console.WriteLine(responseFromServer);
                    }
                }
            }

            return result;
        }


        public ServiceResponse DeleteFile(string refreshToken, string fileId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var googleResponse = RemoveFile(refreshToken, fileId);

                response.IsSuccess = true;
                response.Data = Zarephath.Core.Resources.Resource.FileDeleteSuccess;
                return response;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;// Resource.ExceptionMessage;
                response.IsSuccess = false;
            }
            return response;
        }

        private string RemoveFile(string refreshToken, string fileId)
        {
            string result = "ERROR";

            // get refreshed auth token
            var token = this.RefreshAccessToken(refreshToken);


            ////////////////////////////////////////////////////////

            string requestUri = "";

            requestUri = requestUri = ConfigSettings.GoogleDriveDeleteUri + "/" + fileId;

            // get access token from google
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "DELETE";
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";

            request.Headers["Authorization"] = "Bearer " + token.AccessToken;

            /*
            // Wrighting Meta Data
            string headerJson = string.Format("--{0}\r\nContent-Type: {1}\r\n\r\n",
                                                boundry,
                                                "application/json; charset=UTF-8");
            string headerFile = string.Format("\r\n--{0}\r\nContent-Type: {1}\r\n\r\n",
                                                boundry,
                                                contentType);

            string footer = "\r\n--" + boundry + "--\r\n";

            int headerLenght = headerJson.Length + headerFile.Length + footer.Length;
            request.ContentLength = MetaDataByteArray.Length + localFileContents.Length + headerLenght;
            var dataStream = request.GetRequestStream();

            // write the MetaData ContentType
            dataStream.Write(Encoding.UTF8.GetBytes(headerJson), 0, Encoding.UTF8.GetByteCount(headerJson));

            // write the MetaData
            dataStream.Write(MetaDataByteArray, 0, MetaDataByteArray.Length);

            // write the File ContentType
            dataStream.Write(Encoding.UTF8.GetBytes(headerFile), 0, Encoding.UTF8.GetByteCount(headerFile));

            // write the file
            dataStream.Write(localFileContents, 0, localFileContents.Length);

            // Add the end of the request.  Start with a newline

            dataStream.Write(Encoding.UTF8.GetBytes(footer), 0, Encoding.UTF8.GetByteCount(footer));
            dataStream.Close();
            */

            // Check if file is there
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                    result = "Success";

                // or check this one
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }

            return result;
        }

        public ServiceResponse GetFiles(string refreshToken)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var googleResponse = GetFilesList(refreshToken);

                response.IsSuccess = true;
                response.Data = googleResponse;
                return response;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;// Resource.ExceptionMessage;
                response.IsSuccess = false;
            }
            return response;
        }

        private string GetFilesList(string refreshToken)
        {
            string result = "ERROR";

            // get refreshed auth token
            var token = this.RefreshAccessToken(refreshToken);


            ////////////////////////////////////////////////////////

            string requestUri = "";

            requestUri = requestUri = ConfigSettings.GoogleDriveFilesListUri;

            // get access token from google
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "GET";
            request.KeepAlive = true;
            //request.ContentType = "application/x-www-form-urlencoded";

            request.Headers["Authorization"] = "Bearer " + token.AccessToken;

            // Check if file is there
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                    result = "Success";

                // or check this one
                result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }

            return result;
        }

        public ServiceResponse GetFileInfo(string refreshToken, string id)
        {
            ServiceResponse response = new ServiceResponse();
            response.IsSuccess = false;

            try
            {
                // get refreshed auth token
                var token = this.RefreshAccessToken(refreshToken);

                ////////////////////////////////////////////////////////

                string requestUri = "";

                requestUri = requestUri = ConfigSettings.GoogleDriveFilesListUri + "/" + id + "?fields=id,name,webViewLink,webContentLink,thumbnailLink,originalFilename";

                // get access token from google
                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Method = "GET";
                request.KeepAlive = true;

                request.Headers["Authorization"] = "Bearer " + token.AccessToken;

                string googleResponse = "";

                // Check if file is there
                using (var webResponse = (HttpWebResponse)request.GetResponse())
                {
                    //if (webResponse.StatusCode == HttpStatusCode.OK)
                    //    result = "Success";

                    // or check this one
                    googleResponse = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                }


                string fileUrl = "";
                string fileNameAtDrive = "";
                if (!string.IsNullOrEmpty(googleResponse))
                {
                    dynamic googleResponseObj = JsonConvert.DeserializeObject(googleResponse);
                    fileUrl = (string)googleResponseObj.webViewLink;
                    fileNameAtDrive = (string)googleResponseObj.originalFilename;
                }

                UploadedFileModel fileModel = new UploadedFileModel
                {
                    FileOriginalName = fileNameAtDrive,
                    //TempFileName = string.Format("{0}.{1}", fName, fileExtension),
                    GoogleFileJson = googleResponse
                };

                fileModel.TempFilePath = fileUrl; // destinationFolderId + fileModel.TempFileName;
                response.IsSuccess = true;
                response.Data = fileModel;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;// Resource.ExceptionMessage;
                response.IsSuccess = false;
            }
            return response;
        }
    }
}
