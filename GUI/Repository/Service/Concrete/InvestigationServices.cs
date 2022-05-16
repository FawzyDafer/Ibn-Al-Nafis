using GUI.Areas.Laps.Models;
using GUI.Areas.Medical.Models;
using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Models.Views;
using M3Y.Entities;
using M3Y.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Repository.Service.Concrete
{
    public class InvestigationServices : IInvestigationServices
    {
        #region Vaiables
        readonly IInvestigationRepository InvestigationRepository;
        readonly IPatientRepository PatientRepository;
        readonly IAdmissionsRepository AdmissionsRepository;
        readonly UserManager<User> _userManager;
        #endregion

        #region Constructors
        public InvestigationServices(
            IInvestigationRepository investigationRepository,
            IPatientRepository patientRepository,
            IAdmissionsRepository admissionsRepository,
            UserManager<User> userManager)
        {
            InvestigationRepository = investigationRepository;
            PatientRepository = patientRepository;
            AdmissionsRepository = admissionsRepository;
            _userManager = userManager;
        }
        #endregion

        #region Get Methods
        public async Task<EditInvestigation> GetSelectedInvestigation(string ID)
        {
            var investigation = await InvestigationRepository.
                Investigation_Select_Invesigation_by_TypeID(ID);
            return new EditInvestigation
            {
                Investigation = new Investigation
                {
                    CategoryID = investigation.Category,
                    Type = investigation.Type,
                    TypeID = investigation.TypeID
                }
            };
        }

        public async Task<Dictionary<long, InvestigationView>>
            GetInvestigationView(string Sort)
        {
            Dictionary<long, InvestigationView> Result = new Dictionary<long, InvestigationView>();
            var Investigation = await
                InvestigationRepository.Investigation_Select_Invesigation_by_Sort(Sort);
            var Admissions = Investigation.Values.GroupBy(x => x.AdmessionID);
            long i = 0;
            foreach (var item in Admissions)
            {
                InvestigationView investigation = await SetInvestigationInView(item.FirstOrDefault(), Sort);
                Result.Add(i, investigation);
                i++;
            }
            return Result;
        }

        public async Task<Dictionary<long, RayView>> GetRayView(string Sort)
        {
            Dictionary<long, RayView> Result = new Dictionary<long, RayView>();
            var Investigation = await
                InvestigationRepository.Investigation_Select_Invesigation_by_Sort(Sort);
            var Admissions = Investigation.Values.GroupBy(x => x.AdmessionID);
            long i = 0;
            foreach (var item in Admissions)
            {
                InvestigationView investigation = await SetInvestigationInView(item.FirstOrDefault(), Sort);
                Result.Add(i, ConvertToRayView(investigation));
                i++;
            }
            return Result;
        }

        public async Task<InvestigationView> EditGetInvestigationView(string PIID)
        {
            var Investigation = await InvestigationRepository.
                Investigation_Select_Invesigation_by_PIID(PIID);
            return await SetInvestigationInView(Investigation);
        }

        public async Task<RayView> EditGetRayView(string PIID)
        {
            var Investigation = await InvestigationRepository.
                Investigation_Select_Invesigation_by_PIID(PIID);
            return ConvertToRayView(await SetInvestigationInView(Investigation));
        }

        public async Task<GetPatientInvestigation>
            RetrievePatientData(long Admission, string Sort)
        {
            var admissionData = await
                            AdmissionsRepository.Doctor_Select_Admission_by_ID(Admission);
            var Invesigations = await InvestigationRepository.
                Investigation_Select_Invesigation_by_PatientSSN
                (admissionData.PatientsSSN, Sort);
            Invesigations = Invesigations.Where(x => x.Value.StaffID != null).
                ToDictionary(x => x.Key, x => x.Value);
            foreach (var item in Invesigations)
            {
                item.Value.DoctorName = await GetUserName(item.Value.DoctorID);
            }
            return new GetPatientInvestigation()
            {
                Investigations = Invesigations,
                Admission = Admission
            };
        }

        public async Task<GetPatientInvestigation>
            RetrieveAdmissionData(long Admission, string Sort)
        {
            var Invesigations = await InvestigationRepository.
                Investigation_Select_Invesigation_by_Admission(Sort, Admission);
            Invesigations = Invesigations.Where(x => x.Value.StaffID != null).
                ToDictionary(x => x.Key, x => x.Value);
            foreach (var item in Invesigations)
            {
                item.Value.DoctorName = await GetUserName(item.Value.DoctorID);
            }
            return new GetPatientInvestigation()
            {
                Investigations = Invesigations,
                Admission = Admission
            };
        }

        public async Task<GetPatientInvestigation>
            GetInvestigationData(long Admission, string PIID)
        {
            var Investigation = await InvestigationRepository.
                Investigation_Select_Invesigation_by_PIID(PIID);
            return await SetInvestigationInView(Investigation, Admission, null);
        }

        public async Task<GetPatientInvestigation>
            GetRayData(long Admission, string PIID)
        {
            var Investigation = await InvestigationRepository.
                Investigation_Select_Invesigation_by_PIID(PIID);
            var Files = await InvestigationRepository.
                Investigation_Select_InvesigationFiles_ID_by_PIID(PIID);
            return await SetInvestigationInView(Investigation, Admission, Files);
        }

        public async Task<Patient> GetPatientbyPIID(string PIID)
        {
            var patientinv = await InvestigationRepository.
                Investigation_Select_Invesigation_by_PIID(PIID);
            var admission = await AdmissionsRepository.
                Doctor_Select_Admission_by_ID(patientinv.AdmessionID);
            return await PatientRepository.Reception_Select_Patient_by_SSN(admission.PatientsSSN);
        }

        #endregion

        #region Operation Methods
        public async Task<StoredResult> AddNewLap(Investigation Investigation)
        {
            Investigation.TypeID = await Generics.GetIDAsyc();
            Investigation.Category = await Generics.GetIDAsyc();
            return await InvestigationRepository.
                Investigation_Add_Invesigation(Investigation);
        }

        public async Task<StoredResult> RequestInvestigation(PatientInvestigation PatientInvestigation)
        {
            PatientInvestigation.PIID = await Generics.GetIDAsyc();
            return await InvestigationRepository.
                Doctor_Add_PatientInvestigation(PatientInvestigation);
        }

        public async Task<StoredResult> AddPatientRay(EditPatientRay EditRay)
        {
            var Result = await InvestigationRepository.
                Investigation_Edit_PatientInvestigation
                (ConvertToInvestigation(EditRay));
            if (Result.Success)
            {
                foreach (var item in EditRay.RayImages)
                {
                    var file = await Generics.GetFileAsync(item);
                    Result = await InvestigationRepository.Investigation_Add_InvestigationFile(
                        new InvestigationFile
                        {
                            PID = EditRay.PIID,
                            FileID = await Generics.GetIDAsyc(),
                            FileData = file.Data,
                            FileExtention = file.Extention
                        });
                }
            }
            return Result;
        }
        #endregion

        #region Private Methods
        async Task<InvestigationView> SetInvestigationInView
            (PatientsInvestigations PatientInvestigation, string Sort)
        {
            var PatientsInvestigation = await InvestigationRepository.
                  Investigation_Select_Invesigation_by_Admission
                  (Sort, PatientInvestigation.AdmessionID);
            return new InvestigationView
            {
                Patient = await PatientRepository.
                  Reception_Select_Patient_by_SSN(PatientInvestigation.PatientsSSN),
                Admission = await AdmissionsRepository.
                  Doctor_Select_Admission_by_ID(PatientInvestigation.AdmessionID),
                Doctor = await _userManager.FindByIdAsync(PatientInvestigation.DoctorID),
                PatientsInvestigations = PatientsInvestigation.
                    Where(x => string.IsNullOrEmpty(x.Value.Result) == true).
                    ToDictionary(x => x.Key, x => x.Value)
            };
        }
        async Task<InvestigationView> SetInvestigationInView(PatientInvestigation investigation)
        {
            var admission = await AdmissionsRepository.
                Doctor_Select_Admission_by_ID(investigation.AdmessionID);
            return new InvestigationView
            {
                Admission = admission,
                Patient = await PatientRepository.
                Reception_Select_Patient_by_SSN(admission.PatientsSSN),
                Doctor = await _userManager.FindByIdAsync(investigation.DoctorID),
                PatientInvestigation = await InvestigationRepository.
                Investigation_Select_Invesigation_by_TypeID(investigation.Type),
                EditInvestigation = SetEditInvestigation(investigation)
            };
        }
        async Task<GetPatientInvestigation> SetInvestigationInView
            (PatientInvestigation investigation, long Admission,
            Dictionary<string, InvestigationFile> Files)
        {
            return new GetPatientInvestigation
            {
                PatientInvestigation = await InvestigationRepository.
                Investigation_Select_Invesigation_by_TypeID(investigation.Type),
                Investigation = investigation,
                Admission = Admission,
                Files = Files
            };
        }
        EditPatientInvestigation SetEditInvestigation(PatientInvestigation investigation) =>
            new EditPatientInvestigation()
            {
                AdmessionID = investigation.AdmessionID,
                Comment = investigation.Comment,
                FiniahDate = investigation.FiniahDate,
                Finish = investigation.Finish,
                Note = investigation.Note,
                PIID = investigation.PIID,
                RequestDate = investigation.RequestDate,
                Result = investigation.Result,
                StaffID = investigation.StaffID,
                Type = investigation.Type,
            };
        RayView ConvertToRayView(InvestigationView investigationView) =>
            new RayView
            {
                Admission = investigationView.Admission,
                Doctor = investigationView.Doctor,
                Patient = investigationView.Patient,
                PatientsInvestigations = investigationView.PatientsInvestigations,
                PatientInvestigation = investigationView.PatientInvestigation,
                EditRay = SetEditInvestigation(investigationView.EditInvestigation)
            };
        EditPatientRay SetEditInvestigation(EditPatientInvestigation investigation)
        {
            try
            {
                return new EditPatientRay()
                {
                    AdmessionID = investigation.AdmessionID,
                    Comment = investigation.Comment,
                    FiniahDate = investigation.FiniahDate,
                    Finish = investigation.Finish,
                    Note = investigation.Note,
                    PIID = investigation.PIID,
                    RequestDate = investigation.RequestDate,
                    Result = investigation.Result,
                    StaffID = investigation.StaffID,
                    Type = investigation.Type,
                };
            }
            catch (System.Exception)
            {
                return null;
            }
        }
        PatientInvestigation ConvertToInvestigation(EditPatientRay investigation) =>
             new PatientInvestigation()
             {
                 AdmessionID = investigation.AdmessionID,
                 Result = (string.IsNullOrEmpty(investigation.Result)) ?
                                     "Done" : investigation.Result,
                 FiniahDate = DateTime.Now,
                 Finish = investigation.Finish,
                 Note = investigation.Note,
                 PIID = investigation.PIID,
                 RequestDate = investigation.RequestDate,
                 Comment = investigation.Comment,
                 StaffID = investigation.StaffID,
                 Type = investigation.Type,
             };
        async Task<string> GetUserName(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user.Name;
        }
        #endregion
    }
}
