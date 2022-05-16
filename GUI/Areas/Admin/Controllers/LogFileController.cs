using GUI.Areas.Admin.Models;
using GUI.Models;
using GUI.Models.Identity;
using GUI.Models.Views;
using GUI.Repository;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Manager")]
    public class LogFileController : Controller
    {
        #region Vaiables
        readonly UserManager<User> _userManager;
        readonly ILogFileServices LogFileServices;
        readonly ILogFileRepository LogFileRepository;
        readonly int PageSize = 30;
        Task<User> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        #endregion

        #region Constructors
        public LogFileController(
            ILogFileRepository logFileRepository,
            ILogFileServices logFileServices,
            UserManager<User> userManager)
        {
            _userManager = userManager;
            LogFileServices = logFileServices;
            LogFileRepository = logFileRepository;
        }
        #endregion

        public async Task<IActionResult> Index(string Search, int Page = 1) =>
            View(await SearchLogFile(Search, Page));

        public async Task<IActionResult> Details(string LogFileID, string Search, int Page = 1)
            => View(new SearchLogFile()
            {
                Search = Search,
                DetailsLogFile = await LogFileRepository.
                Select_By_LogFileID(LogFileID),
                Page = Page
            });

        public async Task<JsonResult> SearchResult(string Search) =>
            await SearchJson(Search);

        #region Private Methods
        async Task<SearchLogFile> SearchLogFile(string Search, int Page)
        {
            var UsersLogfiles = (string.IsNullOrEmpty(Search)) ?
                await LogFileRepository.Select_All() :
                await SearchLogFile(Search);
            return new SearchLogFile
            {
                LogFiles = UsersLogfiles.OrderByDescending(x => x.Value.DateTime)
                .Skip((Page - 1) * PageSize).Take(PageSize)
                .ToDictionary(x => x.Key, x => x.Value),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = UsersLogfiles.Count
                },
                Search = Search
            };
        }
        async Task<Dictionary<long, UsersLogFiles>> SearchLogFile(string Search)
        {
            var s1 = await LogFileRepository.Select_By_UserName(Search);
            if (s1.Count() > 0)
            {
                return s1;
            }
            var s2 = await LogFileRepository.Select_By_Specialization(Search);
            if (s2.Count() > 0)
            {
                return s2;
            }
            var s3 = await LogFileRepository.Select_By_Qualification(Search);
            if (s3.Count() > 0)
            {
                return s3;
            }
            var s4 = await LogFileRepository.Select_By_Job(Search);
            if (s4.Count() > 0)
            {
                return s4;
            }
            var s5 = await LogFileRepository.Select_By_Phone(Search);
            if (s5.Count() > 0)
            {
                return s5;
            }
            return new Dictionary<long, UsersLogFiles>();
        }
        async Task<JsonResult> SearchJson(string Search)
        {
            var task = Task.Factory.StartNew<JsonResult>(() =>
            {
                var SearchResult = SearchLogFile(Search);
                JsonResult result = new JsonResult(SearchResult, new Newtonsoft.Json.JsonSerializerSettings { MaxDepth = 6 });
                return result;
            });
            await task;
            return task.Result;
        }
        #endregion

    }
}