using GUI.Areas.Reception.Models;
using GUI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Components
{
    public class PatientsData : ViewComponent
    {
        #region Private Variables
        readonly IPatientRepository PatientRepository;
        readonly IAdmissionsRepository AdmissionsRepository;
        #endregion

        public PatientsData(IPatientRepository patientRepository,
            IAdmissionsRepository admissionsRepository)
        {
            PatientRepository = patientRepository;
            AdmissionsRepository = admissionsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(long AdmissionID)
        {
            var admission = await AdmissionsRepository.
                Doctor_Select_Admission_by_ID(AdmissionID);
            return View(new PatientRegestiration
            {
                PatientClinic = admission,
                Patient = await PatientRepository.
                    Reception_Select_Patient_by_SSN(admission.PatientsSSN)
            });
        }

        #region Private Methods
        #endregion
    }
}
