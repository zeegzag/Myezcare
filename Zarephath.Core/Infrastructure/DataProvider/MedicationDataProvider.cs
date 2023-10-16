using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using ViewModel = Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class MedicationDataProvider : BaseDataProvider, IMedicationDataProvider
    {

        #region Medication

        public ServiceResponse AddMedication(ViewModel.MedicationModel model)
        {
            ServiceResponse response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "MedicationId", Value = Convert.ToString(model.MedicationId)},
                    new SearchValueData {Name = "MedicationName", Value = Convert.ToString(model.MedicationName)},
                    new SearchValueData {Name = "Generic_Name", Value = Convert.ToString(model.Generic_Name)},
                    new SearchValueData {Name = "Brand_Name", Value = Convert.ToString(model.Brand_Name)},
                    new SearchValueData {Name = "Product_Type", Value = Convert.ToString(model.Product_Type)},
                    new SearchValueData {Name = "Route", Value = Convert.ToString(model.Route)},
                    new SearchValueData {Name = "Dosage_Form", Value = Convert.ToString(model.Dosage_Form)}
                };
            Int64 data = Convert.ToInt64(GetScalar(StoredProcedure.SaveMedication, searchlist));
            if (data > 0)
            {
                response.Data = data;
                response.IsSuccess = true;
                //response.Message = Resource.NoteSavedSuccessfully;
            }
            return response;
        }

        public ServiceResponse UpdateMedication(MedicationModel model)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse GetAllMedication()
        {
            throw new NotImplementedException();
        }

        ServiceResponse IMedicationDataProvider.ExistingMedication(string genericName)
        {
            throw new NotImplementedException();
        }

        ServiceResponse IMedicationDataProvider.DeleteMedication(string _Id)
        {
            throw new NotImplementedException();
        }

        ServiceResponse IMedicationDataProvider.DeleteClientMedicationByClientId(string _clientId)
        {
            throw new NotImplementedException();
        }

        ServiceResponse IMedicationDataProvider.DeleteClientMedicationByClientIdAndMedicationId(string _clientId, string _medicationId)
        {
            throw new NotImplementedException();
        }

        ServiceResponse IMedicationDataProvider.GetAllClientMedicationByClientId(string _clientId)
        {
            throw new NotImplementedException();
        }

        ServiceResponse IMedicationDataProvider.GetAllMedication()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
