using System.Linq;
using Amazon.Runtime.Internal.Util;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using System.Net;
using Amazon;
using Microsoft.Win32;


namespace DataMigrationToAWS.AWS_Core
{
    public class AmazonFileUpload
    {
        // Change the accessKeyID and the secretAccessKeyID to your credentials
        // See http://aws.amazon.com/credentials  for more details.
        // You must also sign up for an Amazon S3 account for this to work
        // See http://aws.amazon.com/s3/ for details on creating an Amazon S3 account
        // Change the bucketName and keyName fields to values that match your bucketname and keyname

        static string accessKeyID = ConfigurationManager.AppSettings["accessKeyID"];
        static string secretAccessKeyID = ConfigurationManager.AppSettings["secretAccessKeyID"];


        public AmazonS3 GetS3Client()
        {
            AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client(
                    accessKeyID,
                    secretAccessKeyID
                    );
            return s3Client;
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


        public void CreateAmazonBucket(string bucketName)
        {
          
                AmazonS3 client = GetS3Client();
                ListBucketsResponse response = client.ListBuckets();

                bool found = response.Bucket.Any(bucket => bucket.BucketName == bucketName);               
                switch (found)
                {
                    case false:
                        client.PutBucket(new PutBucketRequest().WithBucketName(bucketName));
                        break;
                }
        }



        public void UploadFile(AmazonS3 client, string bucketName, string s3_key, string path, bool isPdf)
        {
            //S3_KEY is name of file we want upload
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(bucketName);
            request.WithKey(s3_key);
            request.WithFilePath(path);
            if (isPdf)
                request.ContentType = "application/pdf";
            client.PutObject(request);
        }
        public string GetUrlFromBucket(AmazonS3 client, string bucketName, string s3_key, ref string fileURL)
        {
            GetObjectRequest request = new GetObjectRequest().WithBucketName(bucketName).WithKey(s3_key);
            GetPreSignedUrlRequest urlRequest = new GetPreSignedUrlRequest();
            urlRequest.WithBucketName(bucketName).WithKey(s3_key).WithExpires(DateTime.Now.AddYears(10));
            fileURL = client.GetPreSignedURL(urlRequest);
            return fileURL;
        }
       
        
        public void DeleteFile(AmazonS3 Client, string bucketName, string s3_key)
        {
            DeleteObjectRequest request = new DeleteObjectRequest()
            {
                BucketName = bucketName,
                Key = s3_key
            };
            S3Response response = Client.DeleteObject(request);
        }

        public static void CreateNewFolder(AmazonS3 client, string bucketName, string s3_key)
        {
            String S3_KEY = s3_key/*exm: abc/abd/  */;
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(bucketName);
            request.WithKey(S3_KEY);
            request.InputStream = new MemoryStream();
            client.PutObject(request);
        }

        public Boolean CheckFileExist(AmazonS3 s3Client,string bucketName,string s3key)
        {

            MemoryStream file = new MemoryStream();
            try
            {
                GetObjectResponse r = s3Client.GetObject(new GetObjectRequest()
                {//VarsityTestAc2012/varimage/Schedule.htm
                    BucketName = bucketName,//should be whole path from bucket to file root folder
                    Key = s3key//should be file name
                });
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }

        public string getFileName(string url)
        {
            string[] array = url.Split('/');
            foreach (string item in array)
            {
                if(item.Contains("?"))
                    url = item;
               
            }
            return url;
        }
        public string getOldKey(string FileName)
        {
            string oldkey = string.Empty;
            if (!string.IsNullOrEmpty(FileName))
            {
                if (FileName.Contains("?"))
                {
                    oldkey = getFileName(FileName).Split('?')[0];

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

        public string AmazonUpload(AmazonS3 client, string bucketname, string oldkey, string newkey, string path, ref string fileURL, bool isPDF=false)
        {
            if (!string.IsNullOrEmpty(oldkey) && CheckFileExist(client, bucketname, oldkey))
            {
                DeleteFile(client, bucketname, oldkey);
            }
            UploadFile(client, bucketname, newkey, path,isPDF);
            return GetUrlFromBucket(client, bucketname, newkey, ref fileURL);

        }












      
        public static void Createbucket(string strBucketName)
        {
            try
            {
                Amazon.S3.AmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKeyID, secretAccessKeyID);

                PutBucketRequest bucketRequest = new PutBucketRequest();
                bucketRequest.BucketName = strBucketName;
                bucketRequest.BucketRegion = S3Region.US;

                // create bucket first, this function is called only once
                PutBucketResponse buckerResponjse = client.PutBucket(bucketRequest);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void addFileToBucket(string bucketName, string fileKey, string fileName,System.IO.Stream fileStream, string contentType)
        {
            //upload $file to the bucket
            //return the sucess message and filename - if we hash the filename when we upload, etc

            try
            {
                AmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKeyID, secretAccessKeyID);
                // simple object put
                PutObjectRequest request = new PutObjectRequest();
                // create request object
                request.WithBucketName(bucketName).WithKey(fileKey).WithContentType(contentType).WithMetaData("FileName", fileName).WithInputStream(fileStream);

                // send request for file upload
                using (S3Response response = client.PutObject(request))
                {

                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw amazonS3Exception;
            }

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
        public void GetFileFromBucket(string bucketName, string fileKey, ref string fileName, ref string fileURL)
        {
            try
            {
                AmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKeyID, secretAccessKeyID);

                GetObjectRequest request = new GetObjectRequest().WithBucketName(bucketName).WithKey(fileKey);

                // get filename if stored
                using (GetObjectResponse response = client.GetObject(request))
                {
                    if (string.IsNullOrEmpty(fileName = response.Metadata["FileName"]))
                    {
                        fileName = "Resume";
                    }
                }

                // get file URL
                GetPreSignedUrlRequest urlRequest = new GetPreSignedUrlRequest();
                urlRequest.WithBucketName(bucketName).WithKey(fileKey).WithExpires(DateTime.Now.AddYears(10));
                fileURL = client.GetPreSignedURL(urlRequest);

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw amazonS3Exception;
            }
        }
        public void GetFileFromBucketAsAttachment(string bucketName, string fileKey, string preferredFileName, ref string fileName, ref string fileURL, ref System.Net.Mail.Attachment fileAttachment)
        {
            try
            {
                AmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKeyID, secretAccessKeyID);
                //string AttachFile = "";
                GetObjectRequest request = new GetObjectRequest().WithBucketName(bucketName).WithKey(fileKey);

                // get file URL
                GetPreSignedUrlRequest urlRequest = new GetPreSignedUrlRequest();
                urlRequest.WithBucketName(bucketName).WithKey(fileKey).WithExpires(DateTime.Now.AddYears(10));
                fileURL = client.GetPreSignedURL(urlRequest);

                // get filename if stored
                GetObjectResponse response = client.GetObject(request);

                if (string.IsNullOrEmpty(fileName = response.Metadata["FileName"]))
                {
                    fileName = "Resume";
                }

                if (string.IsNullOrEmpty(preferredFileName))
                {
                    preferredFileName = fileName;
                }

                //AttachFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings("AmazonFileWorkPath")) + "\" + preferredFileName
                //HttpContext.Current.Session("AmazonFileWorkPath") = AttachFile
                //'Dim loFileStream As New FileStream("D://" & preferredFileName, FileMode.Create, FileAccess.Write)

                //Dim laBytes(32768) As Byte
                //Dim fs As FileStream = New FileStream(AttachFile, FileMode.Create, FileAccess.Write)

                //Dim bytesRead As Int32 = 1
                //Do While bytesRead > 0
                //    bytesRead = response.ResponseStream.Read(laBytes, 0, laBytes.Length)
                //    fs.Write(laBytes, 0, bytesRead)
                //Loop
                //fs.Flush()
                //fs.Close()
                // get file as an attachment
                fileAttachment = new System.Net.Mail.Attachment(response.ResponseStream, preferredFileName, response.ContentType);
                //fileAttachment = New Mail.Attachment(AttachFile)

                //response.Dispose()

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw amazonS3Exception;
            }
        }
        public void deleteFileFromBucket(string bucketName, string keyName, string filename)
        {
            try
            {
                AmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(accessKeyID, secretAccessKeyID);
                DeleteObjectRequest request = new DeleteObjectRequest();
                request.WithBucketName(bucketName).WithKey(keyName);

                using (DeleteObjectResponse response = client.DeleteObject(request))
                {
                    System.Net.WebHeaderCollection headers = response.Headers;
                    foreach (string key in headers.Keys)
                    {
                        //Console.WriteLine("Response Header: {0}, Value: {1}", key, headers.[Get](key))
                    }
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Logger.Error(amazonS3Exception);
                if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    //Console.WriteLine("Please check the provided AWS Credentials.")
                    //Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3")
                }
                else
                {
                    //Console.WriteLine("An error occurred with the message '{0}' when deleting an object", amazonS3Exception.Message)
                }
            }
        }

    }
}