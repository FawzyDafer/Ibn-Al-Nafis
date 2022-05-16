using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Repository
{
    public interface IDischargeRepository
    {
        #region Get Methods
        Task<Dictionary<long, PatientsDischarges>> History_Select_All_Discharge();
        Task<Dictionary<long, PatientsDischarges>> History_Select_By_Date_Discharge(DateTime dateTime);
        Task<Dictionary<long, PatientsDischarges>> History_Select_By_PatientSSN_Discharge(string PatientSSN);
        Task<Dictionary<long, PatientsDischarges>> History_Select_By_State_Discharge(string State);
        #endregion

        #region Operation Methods
        Task<StoredResult> History_Add_Discharge(Discharge discharge);
        #endregion

    }
}
