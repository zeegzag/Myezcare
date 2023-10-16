using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace DataMigrationToAWS
{
    public class DataMigrationAws
    {
        public readonly string ReferalsFolderPath = ConfigSettings.DTMReferalsFolderPath;
        public readonly int MaxFolderLevel = ConfigSettings.DTMMaxFolderLevel;
        public readonly string DTMAdminUserName = ConfigSettings.DTMAdminUserName;
        public static readonly string LogFileName = ConfigSettings.DTMLogFileName;
        public static readonly string LogPath = ConfigSettings.LogPath;

        public long loggedInUser;

        ICronJobDataProvider _iCronJobDataProvider = new CronJobDataProvider();

        public DataMigrationAws()
        {
        }

        public void ProcessFolder()
        {
            ServiceResponse userresponse = _iCronJobDataProvider.GetAdminUser(DTMAdminUserName);

            if (!userresponse.IsSuccess)
            {
                ShowConsolApp(string.Format("User not configured"));
                return;
            }
            loggedInUser = ((Employee)userresponse.Data).EmployeeID;


            string[] subdirectoryEntries = Directory.GetDirectories(ReferalsFolderPath);

            ShowConsolApp(string.Format("{0} directories found.", subdirectoryEntries.Length));

            List<string> successdir = new List<string>();
            foreach (string subdirectory in subdirectoryEntries)
            {
                ShowConsolApp(string.Format("=============="));
                var directoryName = subdirectory.Split('\\').LastOrDefault();
                try
                {
                    ShowConsolApp(string.Format("Reading {0} directory.", directoryName));
                    ProcessReferralFolder(subdirectory);
                    ShowConsolApp(string.Format("Directory {0} process completed successfully.", directoryName));
                    successdir.Add(subdirectory);
                }
                catch (Exception ex)
                {
                    ShowConsolApp(string.Format("Directory {0} process completed with error.", subdirectory));
                    ShowConsolApp(string.Format("Error: {0}", Common.SerializeObject(ex)));

                }
                ShowConsolApp(string.Format("==============="));
            }
            ShowConsolApp(string.Format("{0} Folders Uploaded success.", string.Join("##", successdir)));
        }

        public void ProcessReferralFolder(string dir)
        {
            #region Get Value from Folder

            var directoryName = dir.Split('\\').LastOrDefault();

            string[] searchValue = directoryName.Split(new string[] { "_", ", " }, new StringSplitOptions());

            DMTSearchReferralListModel searchReferralList = new DMTSearchReferralListModel();

            searchReferralList.AHCCCSID = searchValue[0];
            //searchReferralList.LastName = searchValue[1];
            //string[] firstnameDate = searchValue[2].Split(' ');
            //searchReferralList.FirstName = firstnameDate[0];

            //searchReferralList.Dob = DateTime.ParseExact(firstnameDate[1], "yyyy.MM.dd", CultureInfo.InvariantCulture);// Convert.ToDateTime(searchValue[3], new DateFormat("dd/MM/yyyy");"MM.dd.yyyy");

            ServiceResponse response = _iCronJobDataProvider.GetReferralList(searchReferralList);

            if (!response.IsSuccess)
            {
                ShowConsolApp("Referal Not found in Database.");
                return;
            }
            #endregion

            DMTReferralList referal = (DMTReferralList)response.Data;

            if (referal.UploadStatus)
            {
                ShowConsolApp("Referral files Already Uplodad");
                return;
            }

            #region Read File
            List<string> files = GetAllFileList(dir);
            ShowConsolApp(string.Format("{0} Files found in Directory.", files.Count));
            string awsReferalDocumentDirectoryPath = string.Format("{0}{1}{2}/", ConfigSettings.AmazoneUploadPath,
                                                                   ConfigSettings.ReferralUploadPath, referal.AHCCCSID);

            List<ReferralDocument> referalDocuments = new List<ReferralDocument>();

            foreach (string file in files)
            {
                string filename = Path.GetFileName(file);
                string fileKey = awsReferalDocumentDirectoryPath + file.Replace(dir, "").TrimStart('\\').Replace("\\", "/");
                AmazonFileUpload amazonFileUpload = new AmazonFileUpload();
                amazonFileUpload.UploadFile(ConfigSettings.ZarephathBucket, fileKey, file);

                referalDocuments.Add(new ReferralDocument
                    {
                        FileName = filename,
                        FilePath = fileKey,
                        KindOfDocument = Convert.ToString(DocumentType.DocumentKind.Internal),
                        DocumentTypeID = (int)DocumentType.DocumentTypes.Other,
                        ReferralID = referal.ReferralID
                    });

                ShowConsolApp(string.Format("==> {0} file uploaded", filename));
            }

            _iCronJobDataProvider.SaveReferralDocumentFile(referalDocuments, loggedInUser);

            _iCronJobDataProvider.SaveDMTReferralDocumentUploadStatus(new DMTReferralDocumentUploadStatus
                {
                    AHCCCSID = referal.AHCCCSID,
                    ReferralID = referal.ReferralID,
                    UploadStatus = true
                }, loggedInUser);

            #endregion
        }

        public List<string> GetAllFileList(string dir, int folderLevel = 0)
        {
            if (folderLevel > MaxFolderLevel)
            {
                return null;
            }
            List<string> files = Directory.GetFiles(dir).ToList();

            foreach (var subdir in Directory.GetDirectories(dir))
            {
                List<string> subfolderfilelist = GetAllFileList(subdir, folderLevel++);
                if (subfolderfilelist != null)
                {
                    files.AddRange(subfolderfilelist);
                }
            }
            return files;
        }

        public static void ShowConsolApp(string message)
        {
            Console.WriteLine(message);
            Common.CreateLogFile(message, LogFileName, LogPath);
        }
    }
}
