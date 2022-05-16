using GUI.Areas.Reception.Models;
using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Views;
using M3Y.Entities;
using M3Y.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Repository.Service.Concrete
{
    public class ReceptionService : IReceptionService
    {
        #region Vaiables
        readonly IAdmissionsRepository AdmissionsRepository;
        readonly ICompanyRepository CompanyRepository;
        readonly IPatientRepository PatientRepository;
        readonly IFollowUpRepository FollowUpRepository;
        readonly IConsentsRepository ConsentsRepository;
        #endregion

        #region Constructors

        public ReceptionService(IAdmissionsRepository admissionsRepository,
            ICompanyRepository companyRepository,
            IFollowUpRepository followUpRepository,
            IPatientRepository patientRepository,
            IConsentsRepository consentsRepository)
        {
            AdmissionsRepository = admissionsRepository;
            CompanyRepository = companyRepository;
            PatientRepository = patientRepository;
            FollowUpRepository = followUpRepository;
            ConsentsRepository = consentsRepository;
        }

        #endregion

        #region GetMethods

        public async Task<Dictionary<string, Patient>> SearchForPatient(string patient)
        {
            var s1 = await PatientRepository.Reception_Select_Patient_by_Name(patient);
            if (s1.Count() > 0)
            {
                return s1;
            }
            var s2 = await PatientRepository.Reception_Select_Patient_by_Phone(patient);
            if (s2.Count() > 0)
            {
                return s2;
            }
            var s3 = await PatientRepository.Reception_Select_Patient_by_SSN(patient);
            if (s3 != null)
            {
                return new Dictionary<string, Patient>() { { s3.SSN, s3 } }; ;
            }
            return null;
        }

        public async Task<Dictionary<string, CreatePatientRelative>>
            SelectPatientCompany(string patientID)
        {
            var Result = await CompanyRepository.Reception_Select_Company_by_PatientID(patientID);
            return await ConverttoPatientRelativeAsync(Result);
        }

        public async Task<CreatePatientRelative>
            SelectPatientRelativebyID(string patientID, string personID)
        {
            var Result = await CompanyRepository.
                Reception_Select_Company_by_PatientID(patientID);
            var Person = Result.Values.
                Where(x => x.RelativeSSN == personID).FirstOrDefault();
            return ToPatientRelative(Person);
        }

        public async Task<Dictionary<string, PatientsFollowUps>>
            SelectFollowUpbyPatient(string Search)
        {
            var Result = await FollowUpRepository.
                Reception_Select_Patient_by_FllowUpdate();
            var Person = Result.Values.
                Where(x => x.Name.ToLower().Contains(Search.ToLower())).
                ToDictionary(x => x.SSN, x => x);
            return Person;
        }

        public async Task<Dictionary<long, PatientRegestiration>> SelectRegistratedPatient()
        {
            var Result = await AdmissionsRepository.Reception_Select_Registered();
            Dictionary<long, PatientRegestiration> Patients = new Dictionary<long, PatientRegestiration>();
            foreach (var item in Result)
            {
                var Patient = await PatientRepository.Reception_Select_Patient_by_SSN(item.Value.PatientsSSN);
                Patients.Add(item.Key, new PatientRegestiration
                {
                    Patient = Patient,
                    PatientClinic = item.Value
                });
            }
            return Patients;
        }

        public async Task<PatientSearch> DoctorSearchPatientRecent(string ClinicTitle)
        {
            var Patients = await SelectRegistratedPatient();
            Patients = Patients.Where(x => x.Value.PatientClinic.Clinic == ClinicTitle
                 && x.Value.PatientClinic.Isworking == false).ToDictionary(x => x.Key, x => x.Value);
            return new PatientSearch
            {
                PatientRegestiration = Patients,
                PagingInfo = new PagingInfo
                {
                    TotalItems = Patients.Count
                }
            };
        }

        public async Task<Patient> GetPatientAsync(long Admission)
        {
            var admission = await AdmissionsRepository.
                Doctor_Select_Admission_by_ID(Admission);
            return await PatientRepository.
                Reception_Select_Patient_by_SSN(admission.PatientsSSN);
        }
        #endregion

        #region OperationMethods
        public async Task<StoredResult> AddCompanyToPatient(
         PatientRelative PatientCompany,
         Person Company)
        {
            var Result = await CompanyRepository.Reception_Add_Company(Company);
            if (Result.Success)
            {
                PatientCompany.RelativeSSN = Company.SSN;
                return await CompanyRepository.
                    Reception_Add_PatientCompany(PatientCompany);
            }
            else if (Result.ErrorCode == "4")
            {
                PatientCompany.RelativeSSN = Company.SSN;
                return await CompanyRepository.
                    Reception_Add_PatientCompany(PatientCompany);
            }
            return Result;
        }

        public async Task<StoredResult> RegisteratePatient(
            Patient Patient, Admission PatientClinic)
        {
            var Result = await PatientRepository.
                Reception_Add_Patient(Patient);
            if (Result.Success)
            {
                PatientClinic.VisitDate = DateTime.Now;
                PatientClinic.PatientsSSN = Patient.SSN;
                Result = await AdmissionsRepository.
                    Reception_Add_Admission(PatientClinic);
                return Result;
            }
            else if (Result.ErrorCode == "2")
            {
                PatientClinic.VisitDate = DateTime.Now;
                PatientClinic.PatientsSSN = Patient.SSN;
                Result = await AdmissionsRepository.
                    Reception_Add_Admission(PatientClinic);
                return Result;
            }
            return Result;
        }

        public async Task<StoredResult> EditRegisteratePatient(
            Patient Patient, Admission PatientClinic)
        {
            var Result = await PatientRepository.
                Reception_Edit_Patient(Patient);
            if (Result.Success)
            {
                PatientClinic.VisitDate = DateTime.Now;
                PatientClinic.PatientsSSN = Patient.SSN;
                Result = await AdmissionsRepository.
                    Reception_Add_Admission(PatientClinic);
                return Result;
            }
            return Result;
        }

        public async Task<StoredResult> AddConsent(Consent Consent)
        {
            Consent.ConID = await Generics.GetIDAsyc();
            return await ConsentsRepository.Reception_Add_Consent(Consent);
        }

        public async Task<StoredResult> AddConsenttoPatient(PatientConsent Consent)
        {
            var File = await Generics.GetFileAsync(Consent.Image);
            Consent.ImageData = File.Data;
            Consent.ImageExtention = File.Extention;
            Consent.PCID = await Generics.GetIDAsyc();
            return await ConsentsRepository.Reception_Add_PatientConsent(Consent);
        }

        public async Task<StoredResult> EditConsenttoPatient(PatientConsent Consent)
        {
            if (Consent.Image != null)
            {
                var File = await Generics.GetFileAsync(Consent.Image);
                Consent.ImageData = File.Data;
                Consent.ImageExtention = File.Extention;
            }
            else
            {
                var person = await ConsentsRepository.
                    Reception_Select_Consents_by_ID(Consent.PCID);
                Consent.ImageData = person.ImageData;
                Consent.ImageExtention = person.ImageExtention;
            }
            return await ConsentsRepository.Reception_Edit_PatientConsent(Consent);
        }
        #endregion

        #region PrivateMethods
        Dictionary<string, CreatePatientRelative>
            ConverttoPatientRelative(Dictionary<string, PatientsRelatives> Result)
        {
            Dictionary<string, CreatePatientRelative> Companies = new Dictionary<string, CreatePatientRelative>();
            foreach (var item in Result)
            {
                Companies.Add(item.Key, ToPatientRelative(item.Value));
            }
            return Companies;
        }
        CreatePatientRelative ToPatientRelative(PatientsRelatives obj) =>
            new CreatePatientRelative
            {
                Relative = new Person
                {
                    Address = obj.Address,
                    Name = obj.Name,
                    Phone = obj.Phone,
                    Sex = obj.Sex,
                    SSN = obj.RelativeSSN
                },
                PatientRelative = new PatientRelative
                {
                    AdmissionID = obj.AdmissionID,
                    PatientSSN = obj.PatientsSSN,
                    Relation = obj.Relation,
                    RelativeSSN = obj.RelativeSSN
                }
            };
        async Task<Dictionary<string, CreatePatientRelative>>
            ConverttoPatientRelativeAsync(Dictionary<string, PatientsRelatives> Result)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return ConverttoPatientRelative(Result);
            });
            await task;
            return task.Result;
        }
        #endregion

    }
}
