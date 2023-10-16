using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class AuditLogDataProvider : BaseDataProvider, IAuditLogDataProvider
    {
        #region Add Agency


        public ServiceResponse AddAuditLog(AuditLogTable model, long loggedInUserID)
        {
            ServiceResponse response = new ServiceResponse();
            if (model != null)
            {
                SaveObject(model, loggedInUserID);
                response.IsSuccess = true;
            }

            return response;
        }

        #endregion

        public ServiceResponse GetTableDisplayValue(AuditDeltaModel model)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                if (model != null)
                {
                    var searchValueData = new SearchValueData { Name = "TableName", Value = model.TableName };
                    searchList.Add(searchValueData);
                    searchValueData = new SearchValueData { Name = "ValueBefore", Value = model.ValueBefore == "(null)" ? "" : model.ValueBefore };
                    searchList.Add(searchValueData);
                    searchValueData = new SearchValueData { Name = "ValueAfter", Value = model.ValueAfter == "(null)" ? "" : model.ValueAfter };
                    searchList.Add(searchValueData);
                }

                AuditDeltaModel data = GetEntity<AuditDeltaModel>(StoredProcedure.GetTableDisplayValue, searchList);
                response.Data = data;
                response.IsSuccess = true;
          

            }
            catch (Exception)
            {
            }
            return response;
        }

    }
}
