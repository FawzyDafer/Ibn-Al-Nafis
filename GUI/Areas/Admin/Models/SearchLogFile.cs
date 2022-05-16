using GUI.Models;
using GUI.Models.Views;
using System.Collections.Generic;

namespace GUI.Areas.Admin.Models
{
    public class SearchLogFile
    {
        public PagingInfo PagingInfo { get; set; }
        public Dictionary<long, UsersLogFiles> LogFiles { get; set; }
        public string Search { get; set; }
        public int Page { get; set; }
        public UsersLogFiles DetailsLogFile { get; set; }
    }
}
