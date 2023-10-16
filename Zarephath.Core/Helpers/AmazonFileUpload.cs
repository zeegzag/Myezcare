using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Win32;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Helpers
{
    public class AmazonFileUpload
    {

        // Change the accessKeyID and the secretAccessKeyID to your credentials
        // See http://aws.amazon.com/credentials  for more details.
        // You must also sign up for an Amazon S3 account for this to work
        // See http://aws.amazon.com/s3/ for details on creating an Amazon S3 account
        // Change the bucketName and keyName fields to values that match your bucketname and keyname

        private static readonly string accessKeyID = ConfigSettings.AccessKeyID;
        private static readonly string secretAccessKeyID = ConfigSettings.SecretAccessKeyID;
        private static readonly int signUrlExpireTime = ConfigSettings.SignUrlExpireTimeLimit;
        //public string _policyB64;
        //public string _signature;

        private AmazonS3Client Client;

        public AmazonFileUpload()
        {
            Client = new AmazonS3Client(accessKeyID, secretAccessKeyID, new AmazonS3Config
                {
                    ServiceURL = ConfigSettings.AmazonS3Url,
                    RegionEndpoint = Amazon.RegionEndpoint.USEast1
                });
        }


        public AmazonS3Client GetS3Client()
        {
            return Client;
        }

        public string GetMimeType(FileInfo fileInfo)
        {
            string mimeType = "application/unknown";
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(
                fileInfo.Extension.ToLower()
                );
            if (regKey != null)
            {
                object contentType = regKey.GetValue("Content Type");

                if (contentType != null)
                    mimeType = contentType.ToString();
            }

            return mimeType;
        }

        public bool CreateAmazonBucket(string bucketName)
        {
            AmazonS3Client client = GetS3Client();
            ListBucketsResponse response = client.ListBuckets();
            bool found = response.Buckets.Any(bucket => bucket.BucketName == bucketName);
            if (!found)
            {
                PutBucketResponse putresponse = client.PutBucket(new PutBucketRequest { BucketName = bucketName });
                if (putresponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public string UploadFile(string bucketName, string s3_key, string path, bool deleteFileFormLocal = false)
        {
            //S3_KEY is name of file we want upload
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = bucketName;
            request.Key = s3_key;
            request.FilePath = path;
            request.ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256;
            //if (isPdf)
            //    request.ContentType = "application/pdf";
            PutObjectResponse response = Client.PutObject(request);

            if (deleteFileFormLocal)
            {
                Common.DeleteFile(path);
            }
            return s3_key;
        }

        /// <summary>
        /// This will First check if file exist passed in oldkey then it will deleted first and then upload file from given path with key newkey. 
        /// </summary>
        /// <param name="bucketname"></param>
        /// <param name="oldkey"></param>
        /// <param name="newkey"></param>
        /// <param name="path"></param>
        /// <param name="deleteFileFormLocal"></param>
        /// <returns></returns>
        public string UploadFile(string bucketname, string oldkey, string newkey, string path, bool deleteFileFormLocal = false)
        {
            if (!string.IsNullOrEmpty(oldkey) && CheckFileExist(bucketname, oldkey))
            {
                DeleteFile(bucketname, oldkey);
            }
            return UploadFile(bucketname, newkey, path, deleteFileFormLocal);
        }

        public string GetPreSignedUrl(string bucketName, string s3_key)
        {
            GetPreSignedUrlRequest urlRequest = new GetPreSignedUrlRequest();
            urlRequest.BucketName = bucketName;
            urlRequest.Key = (s3_key);
            urlRequest.Expires = DateTime.UtcNow.AddMinutes(signUrlExpireTime);
            return Client.GetPreSignedURL(urlRequest);
        }

        public string DownloadFile(string bucketname, string s3_key, string path)
        {
            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = bucketname;
            request.Key = s3_key;

            GetObjectResponse response = Client.GetObject(request);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                if (File.Exists(path))
                {
                    Common.DeleteFile(path);
                }

                response.WriteResponseStreamToFile(path);
                return path;
            }
            throw new FileNotFoundException();
        }

        public bool DeleteFile(string bucketName, string s3_key)
        {
            //DeleteObjectRequest request = new DeleteObjectRequest()
            //{
            //    BucketName = bucketName,
            //    Key = s3_key
            //};
            DeleteObjectResponse response = Client.DeleteObject(bucketName, s3_key);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public void DeleteAllObjectsInFolder(string bucketName, string folderPath)
        {
            ListObjectsResponse response = Client.ListObjects(bucketName, folderPath);
            foreach (var item in response.S3Objects)
            {
                Client.DeleteObject(bucketName, item.Key);
            }
        }


        public bool CopyFile(string sourceBucketName, string sourceFilePath, string destBucketName, string destFilePath, S3CannedACL cannedACL = null)
        {

            CopyObjectRequest request = new CopyObjectRequest()
            {
                SourceBucket = sourceBucketName,
                SourceKey = sourceFilePath,
                DestinationBucket = destBucketName,
                DestinationKey = destFilePath,
                CannedACL = cannedACL ?? S3CannedACL.Private
            };
            CopyObjectResponse response = Client.CopyObject(request);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Copy File Form source path to destination and delete file from source path.
        /// </summary>
        /// <param name="sourceBucketName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="destBucketName"></param>
        /// <param name="destFilePath"></param>
        /// <returns></returns>
        public bool MoveFile(string sourceBucketName, string sourceFilePath, string destBucketName, string destFilePath, S3CannedACL cannedACL = null)
        {
            if (CopyFile(sourceBucketName, sourceFilePath, destBucketName, destFilePath, cannedACL))
            {
                return DeleteFile(sourceBucketName, sourceFilePath);
            }
            return false;
        }

        public bool CreateNewFolder(string bucketName, string s3_key)
        {
            String S3_KEY = s3_key/*exm: abc/abd/  */;
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = (bucketName);
            request.Key = (S3_KEY);
            request.InputStream = new MemoryStream();
            PutObjectResponse response = Client.PutObject(request);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public Boolean CheckFileExist(string bucketName, string s3key)
        {
            try
            {
                GetObjectResponse r = Client.GetObject(new GetObjectRequest
                {//VarsityTestAc2012/varimage/Schedule.htm
                    BucketName = bucketName,//should be whole path from bucket to file root folder
                    Key = s3key//should be file name
                });
                return true;
            }
            catch (AmazonS3Exception)
            {
                return false;
            }
        }

        public string GetFileName(string url)
        {
            string[] array = url.Split('/');
            foreach (string item in array)
            {
                if (item.Contains("?"))
                    url = item;
            }
            return url;
        }

        public string GetOldKey(string FileName)
        {
            string oldkey = string.Empty;
            if (!string.IsNullOrEmpty(FileName))
            {
                if (FileName.Contains("?"))
                {
                    oldkey = GetFileName(FileName).Split('?')[0];
                }
                else if (FileName.Contains("/"))
                {
                    oldkey = FileName.Substring(FileName.LastIndexOf('/')).Substring(1);
                }
                else
                {
                    oldkey = FileName;
                }
            }
            return oldkey;
        }



        //public bool Createbucket(string strBucketName)
        //{


        //    PutBucketRequest bucketRequest = new PutBucketRequest();
        //    bucketRequest.BucketName = strBucketName;
        //    bucketRequest.BucketRegion = S3Region.US;

        //    // create bucket first, this function is called only once
        //    PutBucketResponse buckerResponjse = Client.PutBucket(bucketRequest);

        //}

        public bool AddFileToBucket(string bucketName, string fileKey, string fileName, System.IO.Stream fileStream, string contentType)
        {
            //upload $file to the bucket
            //return the sucess message and filename - if we hash the filename when we upload, etc
            // simple object put
            PutObjectRequest request = new PutObjectRequest();
            // create request object
            request.BucketName = bucketName;
            request.Key = fileKey;
            request.ContentType = contentType;
            request.Metadata.Add("FileName", fileName);
            request.InputStream = fileStream;
            PutObjectResponse response = Client.PutObject(request);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        /*
        public void addFileToBucket(string bucketName, string fileKey, string fileName, string filePath, System.Net.Mime.MediaTypeNames mediaType)
        {
            //upload $file to the bucket
            //return the sucess message and filename - if we hash the filename when we upload, etc

            try
            {
                AmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKeyID, secretAccessKeyID);
                // simple object put
                PutObjectRequest request = new PutObjectRequest();
                request.WithBucketName(bucketName).WithKey(fileKey).WithContentType(mediaType.ToString()).WithMetaData("FileName", fileName).WithInputStream(new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read));

                //Dim ACLReq As New SetACLRequest

                //Dim oGrantee As New S3Grantee()
                //oGrantee.URI = "http://acs.amazonaws.com/groups/global/AllUsers"

                //' Create a new Access grant for anonymous users.
                //Dim myGrant As New S3Grant

                //myGrant.Grantee = New S3Grantee
                //myGrant.Grantee.URI = "http://acs.amazonaws.com/groups/global/AllUsers"
                //myGrant.Permission = New S3Permission
                //myGrant.Permission = S3Permission.READ

                //Dim oACL As New S3AccessControlList
                //oACL.AddGrant(New S3Grantee(), S3Permission.READ)
                //oACL.AddGrant(oGrantee, S3Permission.READ)

                //ACLReq.WithACL(oACL)
                //ACLReq.CannedACL = S3CannedACL.PublicRead
                //ACLReq.WithKey(strFileID).WithBucketName(bucketName)

                //client.SetACL(ACLReq)

                using (S3Response response = client.PutObject(request))
                {
                }
                // put a more complex object with some metadata and http headers.
                //Dim titledRequest As New PutObjectRequest()
                //titledRequest.WithMetaData(strFileID, "the title").WithContentBody("this object has a title").WithBucketName(bucketName).WithKey(keyName)

                //Using responseWithMetadata As S3Response = client.PutObject(titledRequest)
                //    'responseWithMetadata.
                //    ' Do something with the response
                //End Using
                //End Using
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    //.Console.WriteLine("Please check the provided AWS Credentials.")
                    //.Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3")
                    //.Else()
                    //.Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message)
                }
                throw amazonS3Exception;
            }
        }
*/

        //public void GetFileFromBucket(string bucketName, string fileKey, ref string fileName, ref string fileURL)
        //{
        //    try
        //    {

        //        GetObjectRequest request = new GetObjectRequest { BucketName = (bucketName), Key = (fileKey) };

        //        // get filename if stored
        //        using (GetObjectResponse response = Client.GetObject(request))
        //        {
        //            if (string.IsNullOrEmpty(fileName = response.Metadata["FileName"]))
        //            {
        //                fileName = "Resume";
        //            }
        //        }

        //        // get file URL
        //        GetPreSignedUrlRequest urlRequest = new GetPreSignedUrlRequest();
        //        urlRequest.BucketName=(bucketName);
        //        urlRequest.WithKey(fileKey).WithExpires(DateTime.Now.AddYears(10));
        //        fileURL = client.GetPreSignedURL(urlRequest);
        //    }
        //    catch (AmazonS3Exception amazonS3Exception)
        //    {
        //        throw amazonS3Exception;
        //    }
        //}

        //public void GetFileFromBucketAsAttachment(string bucketName, string fileKey, string preferredFileName, ref string fileName, ref string fileURL, ref System.Net.Mail.Attachment fileAttachment)
        //{
        //    try
        //    {
        //        AmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKeyID, secretAccessKeyID);
        //        //string AttachFile = "";
        //        GetObjectRequest request = new GetObjectRequest().WithBucketName(bucketName).WithKey(fileKey);

        //        // get file URL
        //        GetPreSignedUrlRequest urlRequest = new GetPreSignedUrlRequest();
        //        urlRequest.WithBucketName(bucketName).WithKey(fileKey).WithExpires(DateTime.Now.AddYears(10));
        //        fileURL = client.GetPreSignedURL(urlRequest);

        //        // get filename if stored
        //        GetObjectResponse response = client.GetObject(request);

        //        if (string.IsNullOrEmpty(fileName = response.Metadata["FileName"]))
        //        {
        //            fileName = "Resume";
        //        }

        //        if (string.IsNullOrEmpty(preferredFileName))
        //        {
        //            preferredFileName = fileName;
        //        }

        //        //AttachFile = HttpContext.Current.Server.MapCustomPath(ConfigurationManager.AppSettings("AmazonFileWorkPath")) + "\" + preferredFileName
        //        //HttpContext.Current.Session("AmazonFileWorkPath") = AttachFile
        //        //'Dim loFileStream As New FileStream("D://" & preferredFileName, FileMode.Create, FileAccess.Write)

        //        //Dim laBytes(32768) As Byte
        //        //Dim fs As FileStream = New FileStream(AttachFile, FileMode.Create, FileAccess.Write)

        //        //Dim bytesRead As Int32 = 1
        //        //Do While bytesRead > 0
        //        //    bytesRead = response.ResponseStream.Read(laBytes, 0, laBytes.Length)
        //        //    fs.Write(laBytes, 0, bytesRead)
        //        //Loop
        //        //fs.Flush()
        //        //fs.Close()
        //        // get file as an attachment
        //        fileAttachment = new System.Net.Mail.Attachment(response.ResponseStream, preferredFileName, response.ContentType);
        //        //fileAttachment = New Mail.Attachment(AttachFile)

        //        //response.Dispose()
        //    }
        //    catch (AmazonS3Exception amazonS3Exception)
        //    {
        //        throw amazonS3Exception;
        //    }
        //}

        //public void deleteFileFromBucket(string bucketName, string keyName, string filename = null)
        //{
        //    try
        //    {
        //        AmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKeyID, secretAccessKeyID);
        //        DeleteObjectRequest request = new DeleteObjectRequest();
        //        request.WithBucketName(bucketName).WithKey(keyName);

        //        using (DeleteObjectResponse response = client.DeleteObject(request))
        //        {
        //            System.Net.WebHeaderCollection headers = response.Headers;
        //            foreach (string key in headers.Keys)
        //            {
        //                //Console.WriteLine("Response Header: {0}, Value: {1}", key, headers.[Get](key))
        //            }
        //        }
        //    }
        //    catch (AmazonS3Exception amazonS3Exception)
        //    {
        //        if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
        //        {
        //            //Console.WriteLine("Please check the provided AWS Credentials.")
        //            //Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3")
        //        }
        //        else
        //        {
        //            //Console.WriteLine("An error occurred with the message '{0}' when deleting an object", amazonS3Exception.Message)
        //        }
        //    }
        //}


        private static string ConstructPolicy(string acl)
        {
            StringBuilder policy = new StringBuilder();
            DateTime expDate = DateTime.Now.AddYears(10);

            policy.AppendLine("{ \"expiration\": \"" + expDate.ToString("s") + ".000Z\"");
            policy.AppendLine(", \"conditions\": [");
            policy.AppendLine(" {\"acl\": \"" + acl + "\" }");
            policy.AppendLine(" , {\"bucket\": \"" + ConfigSettings.ZarephathBucket + "\" }");
            policy.AppendLine(" , {\"success_action_status\": \"" + Constants.SuccessAction + "\"}");
            policy.AppendLine(" , [\"starts-with\", \"$key\", \"\"]");

            //policy.AppendLine(" , [\"starts-with\", \"$x-amz-meta-width\", \"\"]");
            //policy.AppendLine(" , [\"starts-with\", \"$x-amz-meta-height\", \"\"]");
            //policy.AppendLine(" , [\"starts-with\", \"$x-amz-meta-author\", \"\"]");

            policy.AppendLine("]");
            policy.AppendLine("}");

            return policy.ToString();
        }

        public static string CreateSignature(ref string policyB64, string acl)
        {
            //Policy and signature
            string policy = ConstructPolicy(acl);

            //Step 1. Encode the policy using UTF-8.
            byte[] pb = new byte[policy.Length];
            pb = Encoding.UTF8.GetBytes(policy);

            //Step 2. Encode those UTF-8 bytes using Base64.
            policyB64 = Convert.ToBase64String(pb);

            //Step 3. Sign the policy with your Secret Access Key using HMAC SHA-1.
            System.Security.Cryptography.HMACSHA1 hmac = new System.Security.Cryptography.HMACSHA1();
            hmac.Key = Encoding.UTF8.GetBytes(ConfigSettings.SecretAccessKeyID);

            byte[] signb = hmac.ComputeHash(Encoding.UTF8.GetBytes(policyB64));

            //Step 4. Encode the SHA-1 signature using Base64.
            return Convert.ToBase64String(signb);
        }

        public static AmazonSettingModel GetAmazonModelForClientSideUpload(long userID, string folderPath, string acl)
        {
            AmazonSettingModel model = new AmazonSettingModel();
            string policy = "";
            model.ACL = acl;//; ConfigSettings.PrivateAcl;
            model.AccessKey = ConfigSettings.AccessKeyID;
            //model.SecretKey = ConfigSettings.SecretAccessKeyID;
            model.Signature = CreateSignature(ref policy, acl);
            model.Policy = policy;

            model.SuccessAction = Constants.SuccessAction;
            model.URL = ConfigSettings.AmazonS3Url + ConfigSettings.ZarephathBucket;
            model.UserID = userID;
            model.Folder = folderPath;
            return model;
        }


    }
}
