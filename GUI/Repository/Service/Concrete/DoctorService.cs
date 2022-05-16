using GUI.Areas.Medical.Models;
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
    public class DoctorService : IDoctorService
    {
        #region Vaiables
        readonly IClinicRepository ClinicRepository;
        readonly ICompanyRepository CompanyRepository;
        readonly IAdmissionsRepository AdmissionsRepository;
        readonly IHistoryRepository HistoryRepository;
        readonly IDischargeRepository DischargeRepository;
        readonly IExaminationRepository ExaminationRepository;
        readonly IInvestigationRepository InvestigationRepository;
        readonly IFollowUpRepository FollowUpRepository;
        readonly IPatientRepository PatientRepository;
        readonly IMedicineRepository MedicineRepository;
        readonly IPainAssissmentRepository PainAssissmentRepository;
        #endregion

        #region Constructors
        public DoctorService(IClinicRepository clinicRepository,
            ICompanyRepository companyRepository,
            IFollowUpRepository followUpRepository,
            IHistoryRepository historyRepository,
            IInvestigationRepository investigationRepository,
            IExaminationRepository examinationRepository,
            IDischargeRepository dischargeRepository,
            IPatientRepository patientRepository,
            IMedicineRepository medicineRepository,
            IPainAssissmentRepository painAssissmentRepository,
            IAdmissionsRepository admissionsRepository)
        {
            ClinicRepository = clinicRepository;
            CompanyRepository = companyRepository;
            FollowUpRepository = followUpRepository;
            HistoryRepository = historyRepository;
            PatientRepository = patientRepository;
            InvestigationRepository = investigationRepository;
            DischargeRepository = dischargeRepository;
            ExaminationRepository = examinationRepository;
            MedicineRepository = medicineRepository;
            AdmissionsRepository = admissionsRepository;
            PainAssissmentRepository = painAssissmentRepository;
        }
        #endregion

        #region Get Methods
        public async Task<ClincWorkingDays> GetClincWorkingDays(string ClinicTitle)
        {
            var ClinicsDay = await ClinicRepository.
                Admin_Select_ClinicDays_by_Clinic(ClinicTitle);
            ClincWorkingDays clincWorkingDays = new ClincWorkingDays()
            { Clinic = ClinicTitle };
            foreach (var item in ClinicsDay)
            {
                if (item.DayID == 1)
                {
                    clincWorkingDays.Sunday = item.DayID;
                }
                if (item.DayID == 2)
                {
                    clincWorkingDays.Monday = item.DayID;
                }
                if (item.DayID == 3)
                {
                    clincWorkingDays.Tuesday = item.DayID;
                }
                if (item.DayID == 4)
                {
                    clincWorkingDays.Wednesday = item.DayID;
                }
                if (item.DayID == 5)
                {
                    clincWorkingDays.Thursday = item.DayID;
                }
                if (item.DayID == 6)
                {
                    clincWorkingDays.Friday = item.DayID;
                }
                if (item.DayID == 7)
                {
                    clincWorkingDays.Saturday = item.DayID;
                }
            }

            int cons = -1;

            if (clincWorkingDays.Sunday != 1)
            {
                clincWorkingDays.Sunday = cons;
            }
            if (clincWorkingDays.Monday != 2)
            {
                clincWorkingDays.Monday = cons;
            }
            if (clincWorkingDays.Tuesday != 3)
            {
                clincWorkingDays.Tuesday = cons;
            }
            if (clincWorkingDays.Wednesday != 4)
            {
                clincWorkingDays.Wednesday = cons;
            }
            if (clincWorkingDays.Thursday != 5)
            {
                clincWorkingDays.Thursday = cons;
            }
            if (clincWorkingDays.Friday != 6)
            {
                clincWorkingDays.Friday = cons;
            }
            if (clincWorkingDays.Saturday != 7)
            {
                clincWorkingDays.Saturday = cons;
            }

            return clincWorkingDays;
        }

        public async Task<AdmissionHistory> GetAdmissionHistory(long admission)
        {
            try
            {
            var History = await HistoryRepository.
                History_Select_By_AddmissionID_PatientsHistory(admission);
            var Medicines = (await MedicineRepository.
                History_Select_By_AddmissionID_Medicines(admission)).Values.ToList();
            if (Medicines.Count == 0)
            {
                Medicines.Add(new PatientsMedicines());
            }
            var pain = await PainAssissmentRepository.
                History_Select_Pain_By_AdmissionID(admission);
            var reason = History.Values.Where(x => x.Description == "Reason for admission and presenting comblain.").LastOrDefault();
            var Hist = History.Values.Where(x => x.Description == "History of each presenting complain.").LastOrDefault();
            return new AdmissionHistory
            {
                Medicine = Medicines,
                PainAssesment = Getpain(pain),
                Admission = admission,
                DoctorID = Hist.DoctorID,
                History = reason.Problem,
                Reason = reason.Problem,
                SystemEnquiry = GetEnquiry(History),
                Nutritional = GetNutritional(History),
                GynecologicalHistory = GetGynecological(History),
                PsychoSocial = GetPsychoSocial(History),
                PastHistory = GetPastHistory(History),
                SurgialHistory = GetSurgialHistory(History),
                Allergies = GetAllergies(History),
                FamilyHistory = GetFamilyHistory(History)
            };
            }
            catch (Exception)
            {
                return new AdmissionHistory() { Admission = admission};
            }
        }

        public async Task<Digonose> GetDiagnose(long admission, string Clinic)
        {
            var Examination = await ExaminationRepository.
                Examination_Select_By_AddmissionID_PatientsExamination(admission);
            var Diagnose = Examination.Values.LastOrDefault();
            if (Diagnose is null)
            {
                return new Digonose
                {
                    Admission = admission,
                    Clinic = Clinic
                };
            }
            return new Digonose
            {
                Admission = Diagnose.AdmissionID,
                Clinic = Diagnose.Clinic,
                DoctorID = Diagnose.DoctorID,
                Examination = Diagnose.Problem,
                ID = Diagnose.ExaminationID
            };
        }

        public async Task<FllowUp> GetFllowUp(long admission)
        {
            var fllowup = await FollowUpRepository.Doctor_Select_FoolowUp_by_Admission(admission);
            var fllowupdata = fllowup.Values.FirstOrDefault();
            if (fllowupdata is null)
            {
                return new FllowUp
                {
                    AdmisssionID = admission
                };
            }
            else
            {
                return new FllowUp
                {
                    AdmisssionID = admission,
                    FllowUpID = fllowupdata.FlowUpID,
                    FllowupReason = fllowupdata.FllowupReason,
                    FollowUp = fllowupdata.FollowUp,
                    FollowUpBegin = fllowupdata.FollowUpBegin,
                    FollowUpEnd = fllowupdata.FollowUpEnd,
                    DoctorID = fllowupdata.DoctorID
                };
            }
        }

        public async Task<bool> ValidateHistory(long Admission)
        {
            var History = await HistoryRepository.
                History_Select_By_AddmissionID_PatientsHistory(Admission);
            if (History.Count == 0)
                return true;
            else
                return false;
        }

        public async Task<bool> ValidateExamination(long Admission)
        {
            var Examination = await ExaminationRepository.
                Examination_Select_By_AddmissionID_PatientsExamination(Admission);
            if (Examination.Count == 0)
                return true;
            else
                return false;
        }

        #endregion

        #region Operation Methods

        public async Task<StoredResult> AddFollowUp(FllowUp FllowUp)
        {
            FllowUp.FollowUpBegin = FllowUp.FollowUp.AddDays(-3);
            FllowUp.FollowUpEnd = FllowUp.FollowUp.AddDays(3);
            if (string.IsNullOrEmpty(FllowUp.FllowUpID))
            {
                FllowUp.FllowUpID = await Generics.GetIDAsyc();
                return await FollowUpRepository.Doctor_Add_NewFollowUp(FllowUp);
            }
            else
            {
                return await FollowUpRepository.Doctor_Edit_NewFollowUp(FllowUp);
            }
        }

        public async Task<StoredResult> SetClincWorkingDays(ClincWorkingDays ClincWorkingDays)
        {
            var Result = await ClinicRepository.
                Admin_Delete_ClinicDays_by_Clinic(ClincWorkingDays.Clinic);
            if (Result.Success)
            {
                List<int> Days = new List<int>()
                {
                    ClincWorkingDays.Saturday,
                    ClincWorkingDays.Sunday,
                    ClincWorkingDays.Monday,
                    ClincWorkingDays.Tuesday,
                    ClincWorkingDays.Wednesday,
                    ClincWorkingDays.Thursday,
                    ClincWorkingDays.Friday
                };
                foreach (var item in Days)
                {
                    if (item > 0)
                    {
                        Result = await ClinicRepository.Admin_Add_ClinicDays(new ClinicsDay
                        {
                            Clinic = ClincWorkingDays.Clinic,
                            DayID = item
                        });
                    }
                }
            }
            return Result;
        }

        public async Task AddHistory(AdmissionHistory admissionHistory)
        {
            await AddPatientHistory(admissionHistory);
            await AddHistories(admissionHistory);
            if (admissionHistory.PainAssesment.HasPain)
            {
                if (string.IsNullOrEmpty(admissionHistory.PainAssesment.PainID))
                {
                    await PainAssissmentRepository.History_Add_Assissment(admissionHistory.PainAssesment);
                }
                else
                {
                    await PainAssissmentRepository.History_Edit_Assissment(admissionHistory.PainAssesment);
                }
            }
            foreach (var item in admissionHistory.Medicine)
            {
                if (!string.IsNullOrEmpty(item.Medicine))
                {
                    await MedicineRepository.History_Add_Medicines(new Medicine
                    {
                        AlternativeID = "-",
                        EffectiveMaterial = "-",
                        MedicineName = item.Medicine,
                        MedicineID = await Generics.GetIDAsyc()
                    });
                    await MedicineRepository.History_Add_PatientsMedicines(new PatientMedicine
                    {
                        AdmissionID = admissionHistory.Admission,
                        Continue = item.Continue,
                        DoctorID = admissionHistory.DoctorID,
                        Dose = item.Dose,
                        Frequency = item.Frequency,
                        ReasonIfKnown = item.ReasonIfKnown,
                        MedicineID = item.Medicine
                    });
                }
            }
        }

        public async Task AddDiagnose(Digonose Digonose)
        {
            await InvestigationRepository.Investigation_Add_Category(new InvestigationCategory
            {
                Category = Digonose.Clinic,
                CategoryID = await Generics.GetIDAsyc(),
                Sort = "Examinations"
            });
            if (string.IsNullOrEmpty(Digonose.ID))
            {
                await ExaminationRepository.Examination_Add_Examination(new Examination
                {
                    Category = Digonose.Clinic,
                    Description = $"Diagnose of {Digonose.Clinic}",
                    ExaminationID = await Generics.GetIDAsyc()
                });
                await ExaminationRepository.Examination_Add_PatientsExamination(new PatientExamination
                {
                    AdmissionID = Digonose.Admission,
                    Problem = Digonose.Examination,
                    DoctorID = Digonose.DoctorID,
                    Note = "-",
                    ExaminationID = $"Diagnose of {Digonose.Clinic}"
                });
            }
            else
            {
                var result = await ExaminationRepository.Examination_Add_Examination(new Examination
                {
                    Category = Digonose.Clinic,
                    Description = $"Diagnose of {Digonose.Clinic}",
                    ExaminationID = await Generics.GetIDAsyc()
                });
                await ExaminationRepository.Examination_Edit_PatientsExamination(new PatientExamination
                {
                    AdmissionID = Digonose.Admission,
                    Problem = Digonose.Examination,
                    DoctorID = Digonose.DoctorID,
                    Note = "-",
                    ExaminationID = (result.Success) ? result.ID : Digonose.ID
                });
            }
        }

        public async Task<StoredResult> Discharge(Discharge discharge)
        {
            discharge.DateTime = DateTime.Now;
            discharge.ISDischarge = true;
            discharge.DischargeSummary =
                (string.IsNullOrEmpty(discharge.DischargeSummary)) ? "-" :
                discharge.DischargeSummary;
            return await DischargeRepository.History_Add_Discharge(discharge);
        }

        #endregion

        #region Private Methods
        async Task AddPatientHistory(AdmissionHistory admissionHistory)
        {
            await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
            {
                AdmissionID = admissionHistory.Admission,
                Problem = admissionHistory.Reason,
                DoctorID = admissionHistory.DoctorID,
                Note = "-",
                HistoryID = "Reason for admission and presenting comblain."
            });
            await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
            {
                AdmissionID = admissionHistory.Admission,
                Problem = admissionHistory.History,
                DoctorID = admissionHistory.DoctorID,
                Note = "-",
                HistoryID = "History of each presenting complain."
            });
            if (!string.IsNullOrEmpty(admissionHistory.SystemEnquiry.HeadandNeck))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.SystemEnquiry.HeadandNeck,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "Head and Neck"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.SystemEnquiry.CNS))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.SystemEnquiry.CNS,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "CNS"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.SystemEnquiry.CVS))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.SystemEnquiry.CVS,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "CVS"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.SystemEnquiry.RespiratorySystem))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.SystemEnquiry.RespiratorySystem,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "Respiratory System"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.SystemEnquiry.GIT))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.SystemEnquiry.GIT,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "GIT"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.SystemEnquiry.Grenitourinary))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.SystemEnquiry.Grenitourinary,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "Grenitourinary"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.SystemEnquiry.Skin))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.SystemEnquiry.Skin,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "Skin"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.SystemEnquiry.Musculoskeletal))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.SystemEnquiry.Musculoskeletal,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "Musculoskeletal"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.Nutritional.Abnormal))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.Nutritional.Abnormal,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "Abnormal"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.Nutritional.Unhealthy))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.Nutritional.Unhealthy,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "Unhealthy"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.Nutritional.Disease))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.Nutritional.Disease,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "Disease"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.GynecologicalHistory.Gynecological))
            {
                if (string.IsNullOrEmpty(admissionHistory.GynecologicalHistory.ID))
                {
                    await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                    {
                        AdmissionID = admissionHistory.Admission,
                        Problem = admissionHistory.GynecologicalHistory.Gynecological,
                        DoctorID = admissionHistory.DoctorID,
                        Note = "-",
                        HistoryID = "Gynecological and obstetrics History"
                    });
                }
                else
                {
                    await HistoryRepository.History_Edit_PatientsHistory(new PatientHistory
                    {
                        AdmissionID = admissionHistory.Admission,
                        Problem = admissionHistory.GynecologicalHistory.Gynecological,
                        DoctorID = admissionHistory.DoctorID,
                        Note = "-",
                        HistoryID = admissionHistory.GynecologicalHistory.ID
                    });

                }
            }
        }

        async Task AddHistories(AdmissionHistory admissionHistory)
        {
            await AddHistory(admissionHistory.PastHistory,
                admissionHistory.Admission, admissionHistory.DoctorID);

            await AddHistory(admissionHistory.SurgialHistory,
                      admissionHistory.Admission, admissionHistory.DoctorID);

            await AddHistory(admissionHistory.Allergies,
                admissionHistory.Admission, admissionHistory.DoctorID);

            await AddHistory(admissionHistory.FamilyHistory,
                admissionHistory.Admission, admissionHistory.DoctorID);

            if (!string.IsNullOrEmpty(admissionHistory.PsychoSocial.Mode))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.PsychoSocial.Mode,
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "Mood"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.PsychoSocial.Smoking.Name))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.PsychoSocial.Smoking.Name,
                    DoctorID = admissionHistory.DoctorID,
                    Note = string.IsNullOrEmpty(admissionHistory.PsychoSocial.Smoking.Detials) ? "-" :
                    admissionHistory.PsychoSocial.Smoking.Detials,
                    HistoryID = "Smoking"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.PsychoSocial.Alkohol.Name))
            {
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.PsychoSocial.Alkohol.Name,
                    DoctorID = admissionHistory.DoctorID,
                    Note = string.IsNullOrEmpty(admissionHistory.PsychoSocial.Alkohol.Detials) ? "-" :
                    admissionHistory.PsychoSocial.Alkohol.Detials,
                    HistoryID = "Alcohol"
                });
            }
            if (!string.IsNullOrEmpty(admissionHistory.PsychoSocial.Level))
            {
                await HistoryRepository.History_Add_History(new History
                {
                    Category = "Psycho Social",
                    Description = "Besides",
                    HistoryID = await Generics.GetIDAsyc()
                });
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.PsychoSocial.Level,
                    DoctorID = admissionHistory.DoctorID,
                    Note = admissionHistory.PsychoSocial.Climp,
                    HistoryID = "Besides"
                });
            }
            if (admissionHistory.PsychoSocial.LiveAlone)
            {
                await HistoryRepository.History_Add_History(new History
                {
                    Category = "Psycho Social",
                    Description = "Live alone",
                    HistoryID = await Generics.GetIDAsyc()
                });
                await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                {
                    AdmissionID = admissionHistory.Admission,
                    Problem = admissionHistory.PsychoSocial.LiveAlone.ToString(),
                    DoctorID = admissionHistory.DoctorID,
                    Note = "-",
                    HistoryID = "Live alone"
                });
            }
        }

        async Task AddHistory(List<PastHistory> PastHistory, long admission, string doctorid)
        {
            foreach (var item in PastHistory)
            {
                if (!string.IsNullOrEmpty(item.Problem))
                {
                    if (string.IsNullOrEmpty(item.ID))
                    {
                        await HistoryRepository.History_Add_History(new History
                        {
                            Category = "Past History",
                            Description = item.Problem,
                            HistoryID = await Generics.GetIDAsyc()
                        });
                        await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                        {
                            AdmissionID = admission,
                            Problem = item.Details,
                            DoctorID = doctorid,
                            Note = "-",
                            HistoryID = item.Problem
                        });
                    }
                    else
                    {
                        var result = await HistoryRepository.History_Add_History(new History
                        {
                            Category = "Past History",
                            Description = item.Problem,
                            HistoryID = await Generics.GetIDAsyc()
                        });
                        await HistoryRepository.History_Edit_PatientsHistory(new PatientHistory
                        {
                            AdmissionID = admission,
                            Problem = item.Details,
                            DoctorID = doctorid,
                            Note = "-",
                            HistoryID = (result.Success) ? result.ID : item.ID
                        });
                    }
                }
            }
        }

        async Task AddHistory(List<PastSurgialHistory> SurgialHistory, long admission, string doctorid)
        {
            foreach (var item in SurgialHistory)
            {
                if (!string.IsNullOrEmpty(item.Procedure))
                {
                    if (string.IsNullOrEmpty(item.ID))
                    {
                        await HistoryRepository.History_Add_History(new History
                        {
                            Category = "Surgial History",
                            Description = item.Procedure,
                            HistoryID = await Generics.GetIDAsyc()
                        });
                        await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                        {
                            AdmissionID = admission,
                            Problem = item.Details,
                            DoctorID = doctorid,
                            Note = "-",
                            HistoryID = item.Procedure
                        });
                    }
                    else
                    {
                        var result = await HistoryRepository.History_Add_History(new History
                        {
                            Category = "Surgial History",
                            Description = item.Procedure,
                            HistoryID = await Generics.GetIDAsyc()
                        });
                        await HistoryRepository.History_Edit_PatientsHistory(new PatientHistory
                        {
                            AdmissionID = admission,
                            Problem = item.Details,
                            DoctorID = doctorid,
                            Note = "-",
                            HistoryID = (result.Success) ? result.ID : item.ID
                        });
                    }
                }
            }
        }

        async Task AddHistory(List<AllergiesandAdverseReactions> Allergies, long admission, string doctorid)
        {
            foreach (var item in Allergies)
            {
                if (!string.IsNullOrEmpty(item.Name))
                {
                    if (string.IsNullOrEmpty(item.ID))
                    {
                        await HistoryRepository.History_Add_History(new History
                        {
                            Category = "Allergies and Adverse Reactions",
                            Description = item.Name,
                            HistoryID = await Generics.GetIDAsyc()
                        });
                        await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                        {
                            AdmissionID = admission,
                            Problem = item.ReactionManifestations,
                            DoctorID = doctorid,
                            Note = "-",
                            HistoryID = item.Name
                        });
                    }
                    else
                    {
                        var result = await HistoryRepository.History_Add_History(new History
                        {
                            Category = "Allergies and Adverse Reactions",
                            Description = item.Name,
                            HistoryID = await Generics.GetIDAsyc()
                        });
                        await HistoryRepository.History_Edit_PatientsHistory(new PatientHistory
                        {
                            AdmissionID = admission,
                            Problem = item.ReactionManifestations,
                            DoctorID = doctorid,
                            Note = "-",
                            HistoryID = (result.Success) ? result.ID : item.ID
                        });
                    }
                }
            }
        }

        async Task AddHistory(FamilyHistory family, long admission, string doctorid)
        {
            var familyhistory = new List<FamilyHistory.Familyhis>()
            {
                family.Stroke,
                family.Cancer,
                family.Diabetes,
                family.Hypertention,
                family.ActiveTuberculosis,
                family.IshemicHeart,
            };
            foreach (var item in familyhistory)
            {
                if (!string.IsNullOrEmpty(item.FamilyMember))
                {
                    if (string.IsNullOrEmpty(item.ID))
                    {
                        await HistoryRepository.History_Add_History(new History
                        {
                            Category = "Family History",
                            Description = item.Disease,
                            HistoryID = await Generics.GetIDAsyc()
                        });
                        await HistoryRepository.History_Add_PatientsHistory(new PatientHistory
                        {
                            AdmissionID = admission,
                            Problem = item.FamilyMember,
                            DoctorID = doctorid,
                            Note = "-",
                            HistoryID = item.Disease
                        });
                    }
                    else
                    {
                        var result = await HistoryRepository.History_Add_History(new History
                        {
                            Category = "Family History",
                            Description = item.Disease,
                            HistoryID = await Generics.GetIDAsyc()
                        });
                        await HistoryRepository.History_Edit_PatientsHistory(new PatientHistory
                        {
                            AdmissionID = admission,
                            Problem = item.FamilyMember,
                            DoctorID = doctorid,
                            Note = "-",
                            HistoryID = (result.Success) ? result.ID : item.ID
                        });
                    }
                }
            }
        }

        PainAssissment Getpain(PatientsPains pain)
        {
            try
            {
                return new PainAssissment
                {
                    AdmissionID = pain.AdmissionID,
                    Aggravates = pain.Aggravates,
                    Character = pain.Character,
                    Duration = pain.Duration,
                    Frequency = pain.Frequency,
                    HasPain = pain.HasPain,
                    Impact = pain.Impact,
                    Location = pain.Location,
                    PainID = pain.PainID,
                    PastPain = pain.PastPain,
                    Rate = pain.Rate,
                    Referral = pain.Referral,
                    Relieves = pain.Relieves,
                    Therapies = pain.Therapies,
                    DoesPatienthaspain = pain.HasPain
                };
            }
            catch (Exception)
            {
                return new PainAssissment();
            }
        }

        SystemEnquiry GetEnquiry(Dictionary<long, PatientsHistories> History)
        {
            var reason = History.Values.Where(x => x.Description == "Reason for admission and presenting comblain.").LastOrDefault();
            var Hist = History.Values.Where(x => x.Description == "History of each presenting complain.").LastOrDefault();
            var Head = History.Values.Where(x => x.Description == "Head and Neck").LastOrDefault();
            var CNS = History.Values.Where(x => x.Description == "CNS").LastOrDefault();
            var CVS = History.Values.Where(x => x.Description == "CVS").LastOrDefault();
            var Respiratory = History.Values.Where(x => x.Description == "Respiratory System").LastOrDefault();
            var GIT = History.Values.Where(x => x.Description == "GIT").LastOrDefault();
            var Grenitourinary = History.Values.Where(x => x.Description == "Grenitourinary").LastOrDefault();
            var Skin = History.Values.Where(x => x.Description == "Skin").LastOrDefault();
            var Musculoskeletal = History.Values.Where(x => x.Description == "Musculoskeletal").LastOrDefault();
            try
            {
                return new SystemEnquiry()
                {
                    HeadandNeck = Head.Problem,
                    CNS = CNS.Problem,
                    CVS = CVS.Problem,
                    GIT = GIT.Problem,
                    Grenitourinary = Grenitourinary.Problem,
                    Musculoskeletal = Musculoskeletal.Problem,
                    RespiratorySystem = Respiratory.Problem,
                    Skin = Skin.Problem,
                    ID = Head.HistoryID,
                };
            }
            catch (Exception)
            {
                return new SystemEnquiry();
            }
        }

        NutritionalAssessment GetNutritional(Dictionary<long, PatientsHistories> History)
        {
            var Abnormal = History.Values.Where(x => x.Description == "Abnormal").LastOrDefault();
            var Unhealthy = History.Values.Where(x => x.Description == "Unhealthy").FirstOrDefault();
            var Disease = History.Values.Where(x => x.Description == "Disease").FirstOrDefault();
            try
            {
                return new NutritionalAssessment
                {
                    Abnormal = Abnormal.Problem,
                    Disease = Disease.Problem,
                    Unhealthy = Unhealthy.Problem,
                    ID = Abnormal.HistoryID
                };
            }
            catch (Exception)
            {
                return new NutritionalAssessment();
            }
        }

        GynecologicalHistory GetGynecological(Dictionary<long, PatientsHistories> History)
        {
            var Gynecological = History.Values.Where(x => x.Description == "Gynecological and obstetrics History").LastOrDefault();
            try
            {
                return new GynecologicalHistory
                {
                    ID = Gynecological.HistoryID,
                    Gynecological = Gynecological.Problem,
                    HasGynecologicalHistory = (string.IsNullOrEmpty(Gynecological.Problem)) ? false : true
                };
            }
            catch (Exception)
            {
                return new GynecologicalHistory();
            }
        }

        PsychoSocial GetPsychoSocial(Dictionary<long, PatientsHistories> History)
        {
            var mode = History.Values.Where(x => x.Description == "Mood").LastOrDefault();
            var Smoking = History.Values.Where(x => x.Description == "Smoking").LastOrDefault();
            var Alcohol = History.Values.Where(x => x.Description == "Alcohol").LastOrDefault();
            var Besides = History.Values.Where(x => x.Description == "Besides").LastOrDefault();
            var Live = History.Values.Where(x => x.Description == "Live alone").LastOrDefault();
            try
            {
                return new PsychoSocial
                {
                    ID = Besides.HistoryID,
                    Alkohol = new SpecialHabits
                    {
                        Name = Alcohol.Problem,
                        Detials = Alcohol.Note,
                        ID = Alcohol.HistoryID
                    },
                    Smoking = new SpecialHabits
                    {
                        Name = Smoking.Problem,
                        Detials = Smoking.Note,
                        ID = Smoking.HistoryID
                    },
                    Mode = mode.Problem,
                    Climp = Besides.Problem,
                    Level = Besides.Note,
                    LiveAlone = (string.IsNullOrEmpty(Live.Problem)) ? false : bool.Parse(Live.Problem)
                };
            }
            catch (Exception)
            {
                return new PsychoSocial();
            }
        }

        List<PastHistory> GetPastHistory(Dictionary<long, PatientsHistories> History)
        {
            var PastHistory = History.Values.Where(x => x.Category == "Past History").ToList();
            List<PastHistory> pHistory = new List<PastHistory>();
            foreach (var item in PastHistory)
            {
                pHistory.Add(new PastHistory
                {
                    ID = item.HistoryID,
                    Details = item.Problem,
                    Problem = item.Description,
                    Checked = (string.IsNullOrEmpty(item.Problem)) ? false : true
                });
            }
            if (pHistory.Count == 0)
            {
                pHistory.Add(new PastHistory());
            }
            return pHistory;
        }

        List<PastSurgialHistory> GetSurgialHistory(Dictionary<long, PatientsHistories> History)
        {
            var SurgialHistory = History.Values.Where(x => x.Category == "Surgial History").ToList();
            List<PastSurgialHistory> pHistory = new List<PastSurgialHistory>();
            foreach (var item in SurgialHistory)
            {
                pHistory.Add(new PastSurgialHistory
                {
                    ID = item.HistoryID,
                    Details = item.Problem,
                    Procedure = item.Description,
                    PastSerguies = (string.IsNullOrEmpty(item.Problem)) ? false : true
                });
            }
            if (pHistory.Count == 0)
            {
                pHistory.Add(new PastSurgialHistory());
            }
            return pHistory;
        }

        List<AllergiesandAdverseReactions> GetAllergies(Dictionary<long, PatientsHistories> History)
        {
            var SurgialHistory = History.Values.Where(x => x.Category == "Allergies and Adverse Reactions").ToList();
            List<AllergiesandAdverseReactions> pHistory = new List<AllergiesandAdverseReactions>();
            foreach (var item in SurgialHistory)
            {
                pHistory.Add(new AllergiesandAdverseReactions
                {
                    ID = item.HistoryID,
                    ReactionManifestations = item.Problem,
                    Name = item.Description,
                    Reaction = (string.IsNullOrEmpty(item.Problem)) ? false : true
                });
            }
            if (pHistory.Count == 0)
            {
                pHistory.Add(new AllergiesandAdverseReactions());
            }
            return pHistory;
        }

        FamilyHistory GetFamilyHistory(Dictionary<long, PatientsHistories> History)
        {
            var SurgialHistory = History.Values.Where(x => x.Category == "Family History").ToList();
            FamilyHistory pHistory = new FamilyHistory();
            foreach (var item in SurgialHistory)
            {
                if (item.Description == "Diabetes")
                {
                    pHistory.HasFamilyHistory = true;
                    pHistory.Diabetes = new FamilyHistory.Familyhis
                    {
                        ID = item.HistoryID,
                        Disease = item.Description,
                        FamilyMember = item.Problem
                    };
                }
                if (item.Description == "ActiveTuberculosis")
                {
                    pHistory.HasFamilyHistory = true;
                    pHistory.ActiveTuberculosis = new FamilyHistory.Familyhis
                    {
                        ID = item.HistoryID,
                        Disease = item.Description,
                        FamilyMember = item.Problem
                    };
                }
                if (item.Description == "Cancer")
                {
                    pHistory.HasFamilyHistory = true;
                    pHistory.Cancer = new FamilyHistory.Familyhis
                    {
                        ID = item.HistoryID,
                        Disease = item.Description,
                        FamilyMember = item.Problem
                    };
                }
                if (item.Description == "Hypertention")
                {
                    pHistory.HasFamilyHistory = true;
                    pHistory.Hypertention = new FamilyHistory.Familyhis
                    {
                        ID = item.HistoryID,
                        Disease = item.Description,
                        FamilyMember = item.Problem
                    };
                }
                if (item.Description == "IshemicHeart")
                {
                    pHistory.HasFamilyHistory = true;
                    pHistory.IshemicHeart = new FamilyHistory.Familyhis
                    {
                        ID = item.HistoryID,
                        Disease = item.Description,
                        FamilyMember = item.Problem
                    };
                }
                if (item.Description == "Stroke")
                {
                    pHistory.HasFamilyHistory = true;
                    pHistory.Stroke = new FamilyHistory.Familyhis
                    {
                        ID = item.HistoryID,
                        Disease = item.Description,
                        FamilyMember = item.Problem
                    };
                }
            }
            return pHistory;
        }

        #endregion
    }
}
