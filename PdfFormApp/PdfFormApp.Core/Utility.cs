using PdfFormApp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFormApp.Core
{
    public class Utility
    {
        private DBHelper _DBHelper;
        private DBHelper DataBaseHelper
        {
            get
            {
                if (_DBHelper == null)
                {
                    _DBHelper = new DBHelper();
                }
                return _DBHelper;
            }
        }
        public string GetConnectionString(int OrgId)
        {
            Parameter[] lstParams = {
                                    new Parameter("@OrgId", OrgId)
            };

            return Convert.ToString(DataBaseHelper.ExecuteScalar("HC_GetOrgConnectionString", lstParams));
        }

        public string GetPdfFormPath(string EBFormID)
        {
            Parameter[] lstParams = {
                                    new Parameter("@EBFormID", EBFormID)
            };

            return Convert.ToString(DataBaseHelper.ExecuteScalar("HC_GetPdfFormPath", lstParams));
        }

        public int EditFormData(FormDataEntity formDataEntity)
        {

            //formData.EBriggsFormID = EBriggsFormID;
            //formData.OriginalEBFormID = OriginalEBFormID;

            //formData.SubSectionID = SubSectionID;
            //formData.FormName = FormName;
            //formData.UpdateFormName = UpdateFormName;

            Parameter[] lstParams = 
            {
                                        new Parameter("@EBFormId", formDataEntity.FormId),
                                        new Parameter("@OrganizationId", formDataEntity.OrganizationId),
                                        new Parameter("@FormData", formDataEntity.FormData),
                                        new Parameter("@FormDataId", formDataEntity.FormUniqueId),
                                        new Parameter("@ReferralId", formDataEntity.ReferralId),
                                        new Parameter("@EmployeeId", formDataEntity.EmployeeId),
                                        new Parameter("@UserId", formDataEntity.UserId),
                                        new Parameter("@EBrigFormId", formDataEntity.EBriggsFormID),


                                        new Parameter("@OriginalEBFormID", formDataEntity.OriginalEBFormID),
                                        new Parameter("@SubSectionID", formDataEntity.SubSectionID),
                                        new Parameter("@FormName", formDataEntity.FormName),
                                        new Parameter("@UpdateFormName", formDataEntity.UpdateFormName),
                                        new Parameter("@EbriggsFormMppingID", formDataEntity.EbriggsFormMppingID)
                
            };

            return Convert.ToInt32(DataBaseHelper.ExecuteScalar("HC_EditPdfFormData", lstParams));
        }

        public ICollection<PDFFieldMapping> GetPdfFieldsData(long id, int typeId, int orgId)
        {
            Parameter[] lstParams = {
                                    new Parameter("@Id",id),
                                     new Parameter("@TypeId", typeId),
                                     new Parameter("@OrganizationID",orgId)
            };
            var dataReader = DataBaseHelper.ExecuteReader("HC_GetPdfFieldsData", lstParams);

            ICollection<PDFFieldMapping> list = BaseEntityController.FillEntities<PDFFieldMapping>(dataReader);
            return list;
        }

        public string GetPdfFormData(string formUniqueId, string orgId)
        {
            Parameter[] lstParams = {
                                    new Parameter("@FormUniqueId", formUniqueId),
                                    new Parameter("@OrgId", orgId)
            };

            return Convert.ToString(DataBaseHelper.ExecuteScalar("HC_GetPdfData", lstParams));
        }

    }
}
