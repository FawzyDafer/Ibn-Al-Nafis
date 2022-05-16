namespace GUI.Models.Entities
{
    public class InvestigationFile
    {
        public string FileID { get; set; }
        public byte[] FileData { get; set; }
        public string FileExtention { get; set; }
        public string PID { get; set; }
    }
}
