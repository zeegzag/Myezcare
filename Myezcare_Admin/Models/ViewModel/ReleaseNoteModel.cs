using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Models.Entity;

namespace Myezcare_Admin.Models.ViewModel
{
    public class AddReleaseNoteModel
    {
        public AddReleaseNoteModel()
        {
            ReleaseNote = new ReleaseNote();
        }
        public ReleaseNote ReleaseNote { get; set; }
    }

    public class SetReleaseNoteListPage
    {
        public SetReleaseNoteListPage()
        {
            SearchReleaseNoteListPage = new SearchReleaseNoteListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public SearchReleaseNoteListPage SearchReleaseNoteListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchReleaseNoteListPage
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }

    public class ListReleaseNoteModel
    {
        public long ReleaseNoteID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }

        public int Row { get; set; }
        public int Count { get; set; }

        public string EncryptedReleaseNoteID { get { return Crypto.Encrypt(Convert.ToString(ReleaseNoteID)); } }
    }
}