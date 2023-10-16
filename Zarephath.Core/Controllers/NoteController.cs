using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class NoteController : BaseController
    {
        private INoteDataProvider _noteDataProvider;

        #region Note

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned)]
        public ActionResult Index(string id, string id1 = "")
        {
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _noteDataProvider = new NoteDataProvider();
            var model = (SetNoteListModel)_noteDataProvider.SetNoteListPage(referralID, SessionHelper.LoggedInID, SessionHelper.LoggedInID).Data;
            if (id1 == "1")
            {
                model.IsPartial = true;
            }
            return View(model);
        }


        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned)]
        public ActionResult ReferralNoteIndex(string id, string id1 = "")
        {
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _noteDataProvider = new NoteDataProvider();
            var model = (SetNoteListModel)_noteDataProvider.SetNoteListPage(referralID,assigneeID:Constants.NotStaff).Data;
            if (id1 == "1")
            {
                model.IsPartial = true;
            }
            return View("Index", model);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned)]
        public JsonResult SetAddNote(string id, long noteID)
        {
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _noteDataProvider = new NoteDataProvider();
            return JsonSerializer(_noteDataProvider.SetAddNote(referralID, SessionHelper.LoggedInID, noteID));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned)]
        public JsonResult GetDtrDetails(string term, string type, int pageSize)
        {
            _noteDataProvider = new NoteDataProvider();
            ServiceResponse response = _noteDataProvider.GetDtrDetails(term, type, pageSize);
            return JsonSerializer(response.Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned)]
        public JsonResult ExportNoteList(SearchNoteListModel searchNoteModel, string sortIndex = "", string sortDirection = "")
        {
            _noteDataProvider = new NoteDataProvider();
            return JsonSerializer(_noteDataProvider.ExportNoteList(searchNoteModel, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned)]
        public JsonResult GetNoteList(SearchNoteListModel searchNoteModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _noteDataProvider = new NoteDataProvider();
            if (!Common.HasPermission(Constants.Permission_NoteList_ViewAll) && Common.HasPermission(Constants.Permission_NoteList_ViewAssigned))
                searchNoteModel.AssigneeID = SessionHelper.LoggedInID;
            return JsonSerializer(_noteDataProvider.GetNoteList(searchNoteModel, pageIndex, pageSize, sortIndex, sortDirection));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned)]
        public JsonResult DeleteNote(SearchNoteListModel searchNoteModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _noteDataProvider = new NoteDataProvider();
            var response = _noteDataProvider.DeleteNote(searchNoteModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned)]
        public JsonResult SaveNote(Note note, string referralID, PosDropdownModel selectedServiceCodeDetail, List<DXCodeMappingList> dxCodes)
        {
            _noteDataProvider = new NoteDataProvider();
            long id = !string.IsNullOrEmpty(referralID) ? Convert.ToInt64(Crypto.Decrypt(referralID)) : 0;
            return JsonSerializer(_noteDataProvider.SaveNote(note, id, selectedServiceCodeDetail, dxCodes, SessionHelper.LoggedInID, SessionHelper.LastName + ", " + SessionHelper.FirstName));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned)]
        public JsonResult SaveMultiNote(Note note, string referralID, PosDropdownModel selectedServiceCodeDetail, List<DXCodeMappingList> dxCodes, List<Note> tempNoteList)
        {
            _noteDataProvider = new NoteDataProvider();
            long id = !string.IsNullOrEmpty(referralID) ? Convert.ToInt64(Crypto.Decrypt(referralID)) : 0;
            return JsonSerializer(_noteDataProvider.SaveMultiNote(note, id, selectedServiceCodeDetail, dxCodes, tempNoteList, SessionHelper.LoggedInID, SessionHelper.LastName + ", " + SessionHelper.FirstName));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned)]
        public JsonResult GetServiceCodes(string searchText, int pageSize, string searchParam)
        {
            _noteDataProvider = new NoteDataProvider();
            var model = Common.DeserializeObject<ServiceCodeSearchParam>(searchParam);
            long id = !string.IsNullOrEmpty(model.encReferralID) ? Convert.ToInt64(Crypto.Decrypt(model.encReferralID)) : 0;
            return Json(_noteDataProvider.GetServiceCodes(id, SessionHelper.LoggedInID, model.serviceDate, model.serviceCodeTypeID, pageSize, searchText));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned + "," + Constants.AnonymousLoginPermission)]
        public JsonResult GetReferralInfo(string searchText, int pageSize)
        {
            _noteDataProvider = new NoteDataProvider();
            return Json(_noteDataProvider.GetReferralInfo(pageSize, searchText));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult GetPosCodes(string encReferralID, DateTime serviceDate, int serviceCodeID, long noteID, long payorID)
        {
            _noteDataProvider = new NoteDataProvider();
            long id = !string.IsNullOrEmpty(encReferralID) ? Convert.ToInt64(Crypto.Decrypt(encReferralID)) : 0;
            return Json(_noteDataProvider.GetPosCodes(id, serviceDate, serviceCodeID, noteID, payorID));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteList_ViewAll + "," + Constants.Permission_NoteList_ViewAssigned)]
        public ActionResult GetAutoCreateServiceInformation(string encReferralID, Note tempNote)
        {
            _noteDataProvider = new NoteDataProvider();
            long id = !string.IsNullOrEmpty(encReferralID) ? Convert.ToInt64(Crypto.Decrypt(encReferralID)) : 0;
            return JsonSerializer(_noteDataProvider.GetAutoCreateServiceInformation(id, tempNote));
        }


        #endregion

        #region Client Notes

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_NoteReview_ViewAll + "," + Constants.Permission_NoteReview_ViewAssigned)]
        public ActionResult ClientNotes(string id, string id1 = "")
        {
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _noteDataProvider = new NoteDataProvider();
            var model = (SetNoteListModel)_noteDataProvider.SetNoteListPage(referralID, assigneeID: Constants.NotStaff).Data;
            if (id1 == "1")
            {
                model.IsPartial = true;
            }
            return View(model);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_NoteReview_ViewAll + "," + Constants.Permission_NoteReview_ViewAssigned)]
        public JsonResult GetNoteClientList(SearchNoteListModel searchNoteModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _noteDataProvider = new NoteDataProvider();
            if (!Common.HasPermission(Constants.Permission_NoteReview_ViewAll) && Common.HasPermission(Constants.Permission_NoteReview_ViewAssigned))
                searchNoteModel.AssigneeID = SessionHelper.LoggedInID;
            return JsonSerializer(_noteDataProvider.GetNoteClientList(searchNoteModel, pageIndex, pageSize, sortIndex, sortDirection));
        }


        #endregion

        #region Group Note

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_GroupNote)]
        public ActionResult AddGroupNote()
        {
            _noteDataProvider = new NoteDataProvider();
            var model = (GroupNoteModel)_noteDataProvider.SetAddGroupNote(SessionHelper.LoggedInID).Data;
            return View("AddGroupNote", model);

        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_GroupNote)]
        public JsonResult SearchClientForNote(SaerchGroupNoteClient searchGroupNoteClient, List<long> ignoreClientID)
        {
            _noteDataProvider = new NoteDataProvider();
            return JsonSerializer(_noteDataProvider.SearchClientForNote(searchGroupNoteClient, ignoreClientID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult GetGroupPageServiceCodes(string searchText, int pageSize, string searchParam)
        {
            _noteDataProvider = new NoteDataProvider();
            var model = Common.DeserializeObject<ServiceCodeSearchParam>(searchParam);
            //long id = !string.IsNullOrEmpty(model.encReferralID) ? Convert.ToInt64(Crypto.Decrypt(model.encReferralID)) : 0;
            return Json(_noteDataProvider.GetGroupNoteServiceCodes(model.PayorID, model.serviceDate, model.serviceCodeTypeID, pageSize, searchText));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_GroupNote)]
        public JsonResult SaveGroupNote(List<SaveGroupNoteModel> saveGroupNoteModel)
        {
            _noteDataProvider = new NoteDataProvider();
            return Json(_noteDataProvider.SaveGroupNote(saveGroupNoteModel, SessionHelper.LoggedInID, SessionHelper.LastName + ", " + SessionHelper.FirstName));
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_GroupNote)]
        public JsonResult GetServiceCodeList(string searchText, int pageSize, string searchParam)
        {
            _noteDataProvider = new NoteDataProvider();
            var model = Common.DeserializeObject<ServiceCodeSearchParam>(searchParam);
            return Json(_noteDataProvider.GetServiceCodeList(searchText, SessionHelper.LoggedInID, pageSize, model.serviceCodeTypeID));
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_NoteList)]
        public JsonResult ValidateServiceCodeDetails(ValidateServiceCodeModel model)
        {
            _noteDataProvider = new NoteDataProvider();
            return Json(_noteDataProvider.ValidateServiceCodeDetails(model));
        }


        #endregion

        #region Note Service Code Change Mapping


        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_ChangeServiceCode)]
        public ActionResult ChangeServiceCode()
        {
            _noteDataProvider = new NoteDataProvider();
            var model = (ChangeServiceCodeModel)_noteDataProvider.ChangeServiceCode(SessionHelper.LoggedInID).Data;
            return View(model);

        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ChangeServiceCode)]
        public JsonResult SearchNoteForChangeServiceCode(SearchNote searchNote)
        {
            _noteDataProvider = new NoteDataProvider();
            return JsonSerializer(_noteDataProvider.SearchNoteForChangeServiceCode(searchNote));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ChangeServiceCode)]
        public JsonResult ValidateChangeServiceCode(SearchNote searchNote)
        {
            _noteDataProvider = new NoteDataProvider();
            return JsonSerializer(_noteDataProvider.ValidateChangeServiceCode(searchNote));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ChangeServiceCode)]
        public JsonResult ReplaceServiceCode(string noteIds, SearchNote searchNote)
        {
            _noteDataProvider = new NoteDataProvider();
            return JsonSerializer(_noteDataProvider.ReplaceServiceCode(noteIds, searchNote, SessionHelper.LoggedInID));
        }

        


        #endregion

    }
}
