using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure
{
    public interface IVisitTaskDataProvider
    {
        ServiceResponse AddVisitTask(long visitQuestionId);
        ServiceResponse GetVisitTaskCategory(string VisitTaskType);
        ServiceResponse GetVisitTaskSubCategory(long visitTaskCategoryID);
        ServiceResponse GetModelCategoryList(SearchCategory model);
        ServiceResponse SaveCategory(Category Category);
        ServiceResponse SaveSubCategory(Category Category);
        ServiceResponse AddVisitTask(AddVisitTaskModel addVisitQuestionModel, long loggedInUserId);

        ServiceResponse SetVisitTaskListPage();
        ServiceResponse GetVisitTaskList(SearchVisitTaskListPage searchVisitQuestionListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteVisitTask(SearchVisitTaskListPage searchVisitQuestionListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);

        List<ServiceCodes> GetServiceCodeList(string searchText, int pageSize);
        ServiceResponse GetCareTypeListFromVisitType(SearchModelForCareTypeList searchModelForCareTypeList);
        ServiceResponse BulkUpdateVisitTaskDetail(string BulkType, string VisitTaskIDList, string Catrgory, long loggedInUserId);
        ServiceResponse SaveCloneTask(SearchVisitTaskListPage searchVisitQuestionListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);

        #region Form Mapping with Task
        ServiceResponse GetOrganizationFormList();
        ServiceResponse MapSelectedForms(MapFormModel mapFormModel, long loggedInUserId);
        ServiceResponse GetTaskFormList(long VisitTaskID);
        ServiceResponse OnFormChecked(TaskFormMappingModel model, long loggedInUserId);
        ServiceResponse VisitTaskFormEditCompliance(TaskFormMappingModel model, long loggedInUserId);
        ServiceResponse DeleteMappedForm(long TaskFormMappingID);
        #endregion
    }
}
