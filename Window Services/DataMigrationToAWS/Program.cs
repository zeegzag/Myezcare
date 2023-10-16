using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace DataMigrationToAWS
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                DataMigrationAws.ShowConsolApp("-----Data Migration Process started----");

                DataMigrationAws dataMigration = new DataMigrationAws();
                dataMigration.ProcessFolder();

                DataMigrationAws.ShowConsolApp("-----Data Migration Process Completed----");
                Console.Read();
                // ProcessFloder();
            }
            catch (Exception ex)
            {
                DataMigrationAws.ShowConsolApp(string.Format("Error: {0}", Common.SerializeObject(ex)));

            }
            
        }

        //Main Folder Where we have to get All Folder
       

    }
}