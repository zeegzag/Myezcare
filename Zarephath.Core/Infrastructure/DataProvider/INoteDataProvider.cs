using System;
using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface INoteDataProvider
    {
        #region Note
        ServiceResponse SetNoteListPage(long referralID, long? loggedInUserID = null, long? assigneeID = null);
        ServiceResponse SetAddNote(long referralID, long loggedInUserID, long noteID = 0);
        ServiceResponse GetDtrDetails(string term, string type, int pageSize);
        ServiceResponse GetNoteList(SearchNoteListModel searchNoteModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "");
        ServiceResponse ExportNoteList(SearchNoteListModel searchNoteModel, string sortIndex = "", string sortDirection = "");
        ServiceResponse DeleteNote(SearchNoteListModel searchNoteModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse SaveNote(Note note, long referralID, PosDropdownModel selectedServiceCodeDetail, List<DXCodeMappingList> dxCodes = null, long loggedInUserID = 0, string loggedInName = null);
        ServiceResponse SaveMultiNote(Note note, long referralID, PosDropdownModel selectedServiceCodeDetail, List<DXCodeMappingList> dxCodes = null, List<Note> tempNoteList = null, long loggedInUserID = 0, string loggedInName = null);
        List<ServiceCodes> GetServiceCodes(long referralID, long loggedInId, DateTime serviceDate, int serviceCodeTypeID, int pageSize, string searchText = null);
        List<ReferralDetailForNote> GetReferralInfo(int pageSize, string searchText = null);
        ServiceResponse GetPosCodes(long referralID, DateTime? serviceDate, int serviceCodeID, long noteID, long payorID);
        ServiceResponse GetAutoCreateServiceInformation(long referralID, Note tempNote);

        ServiceResponse SaveGeneralNote(long referralID, string message, string source, long loggedInUserID, string spokeTo, string Relation, string kindOfNote, string NoteAttachment = null, string MonthlySummaryIds = null);
        #endregion

        ServiceResponse GetNoteClientList(SearchNoteListModel searchNoteModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "");


        #region Group Note

        ServiceResponse SetAddGroupNote(long loggedInUserID);
        ServiceResponse SearchClientForNote(SaerchGroupNoteClient searchGroupNoteClient, List<long> ignoreClientID);

        List<ServiceCodes> GetGroupNoteServiceCodes(long payorID, DateTime serviceDate, int serviceCodeTypeID,
                                                    int pageSize, string searchText = null);

        ServiceResponse SaveGroupNote(List<SaveGroupNoteModel> saveGroupNoteModel, long loggedInUserID, string loggedInName = null);

        List<ServiceCodes> GetServiceCodeList(string searchText, long loggedInId, int pageSize, int serviceCodeTypeID);
        ServiceResponse ValidateServiceCodeDetails(ValidateServiceCodeModel model);

        #endregion


        #region Note Service Code Change Mapping

        ServiceResponse ChangeServiceCode(long loggedInUserID);
        ServiceResponse SearchNoteForChangeServiceCode(SearchNote saerchNote);
        ServiceResponse ValidateChangeServiceCode(SearchNote searchNote);
        ServiceResponse ReplaceServiceCode(string noteIds, SearchNote searchNote, long loggedInUserID);

        #endregion
    }
}
