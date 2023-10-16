using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    interface IMedicationDataProvider
    {
        #region Medication
        ServiceResponse AddMedication(Zarephath.Core.Models.ViewModel.MedicationModel model);
        ServiceResponse UpdateMedication(MedicationModel model);
        ServiceResponse ExistingMedication(string genericName);
        
        #endregion

        #region MedicationStrenghtUnit

        #endregion
        ServiceResponse DeleteMedication(string _Id);
        ServiceResponse DeleteClientMedicationByClientId(string _clientId);
        ServiceResponse DeleteClientMedicationByClientIdAndMedicationId(string _clientId, string _medicationId);
        ServiceResponse GetAllClientMedicationByClientId(string _clientId);
        ServiceResponse GetAllMedication();

        #region MedicationStrenghtUnit

        #endregion
    }
}
