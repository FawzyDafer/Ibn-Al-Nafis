using System;

namespace GUI.Models.Entities
{
    public class LogFile
    {
        public string LogFileID { set; get; }
        public string Description { set; get; }
        public DateTime Datetime { get; set; }
        public string UserID { get; set; }
    }
}
