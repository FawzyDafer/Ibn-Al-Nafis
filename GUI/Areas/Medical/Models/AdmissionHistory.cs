using GUI.Models.Entities;
using GUI.Models.Views;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GUI.Areas.Medical.Models
{
    public class AdmissionHistory
    {
        [Required(ErrorMessage = "Can not modify empty patient", AllowEmptyStrings = false)]
        public long Admission { get; set; }
        public string DoctorID { get; set; }
        [Display(Name = "Reason for admission and presenting comblain.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please add patient complain")]
        public string Reason { get; set; }
        public PainAssissment PainAssesment { get; set; }
        [Display(Name = "History of each presenting complain")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please add patient past history")]
        public string History { get; set; }
        public SystemEnquiry SystemEnquiry { get; set; }
        public NutritionalAssessment Nutritional { get; set; }
        public List<PatientsMedicines> Medicine { get; set; }
        public List<PastHistory> PastHistory { get; set; }
        public List<PastSurgialHistory> SurgialHistory { get; set; }
        public List<AllergiesandAdverseReactions> Allergies { get; set; }
        public FamilyHistory FamilyHistory { get; set; }
        public GynecologicalHistory GynecologicalHistory { get; set; }
        public PsychoSocial PsychoSocial { get; set; }
    }

    public class SystemEnquiry
    {
        public string ID { get; set; }
        [Display(Name = "System Enquiry")]
        public bool Systemenquiry { get; set; }
        public string HeadandNeck { get; set; }
        public string CNS { get; set; }
        public string CVS { get; set; }
        public string RespiratorySystem { get; set; }
        public string GIT { get; set; }
        public string Grenitourinary { get; set; }
        public string Skin { get; set; }
        public string Musculoskeletal { get; set; }
    }

    public class NutritionalAssessment
    {
        public string ID { get; set; }
        public string Abnormal { get; set; }
        public string Unhealthy { get; set; }
        public string Disease { get; set; }
    }

    public class PastHistory
    {
        public string ID { get; set; }
        public bool Checked { get; set; }
        public string Problem { get; set; }
        public string Details { get; set; }
    }

    public class PastSurgialHistory
    {
        public string ID { get; set; }
        public bool PastSerguies { get; set; }
        public string Procedure { get; set; }
        public string Details { get; set; }
    }

    public class AllergiesandAdverseReactions
    {
        public string ID { get; set; }
        public bool Reaction { get; set; }
        public string Name { get; set; }
        public string ReactionManifestations { get; set; }
    }

    public class FamilyHistory
    {
        public bool HasFamilyHistory { get; set; }
        public Familyhis Diabetes { get; set; }
        public Familyhis Hypertention { get; set; }
        public Familyhis IshemicHeart { get; set; }
        public Familyhis Stroke { get; set; }
        public Familyhis ActiveTuberculosis { get; set; }
        public Familyhis Cancer { get; set; }
        public class Familyhis
        {
            public string ID { get; set; }
            public string FamilyMember { get; set; }
            public string Disease { get; set; }
        }
    }

    public class GynecologicalHistory
    {
        public string ID { get; set; }
        public bool HasGynecologicalHistory { get; set; }
        [Display(Name = "Gynecological and Obsterics History")]
        public string Gynecological { get; set; }
    }

    public class PsychoSocial
    {
        public string ID { get; set; }
        public string Mode { get; set; }
        public SpecialHabits Smoking { get; set; }
        public SpecialHabits Alkohol { get; set; }
        public string Level { get; set; }
        public string Climp { get; set; }
        public bool LiveAlone { get; set; }
    }

    public class SpecialHabits
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Detials { get; set; }
    }

}
