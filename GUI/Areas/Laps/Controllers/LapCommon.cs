using GUI.Areas.Laps.Models;
using GUI.Models;
using GUI.Models.Entities;
using GUI.Models.Identity;
using GUI.Repository;
using GUI.Repository.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Areas.Laps.Controllers
{
    [Authorize]
    public class LapCommon : Controller
    {
        #region Private Variables
        readonly protected IInvestigationRepository InvestigationRepository;
        readonly protected IInvestigationServices InvestigationServices;
        readonly protected UserManager<User> _userManager;
        readonly protected ILogFileServices LogFileServices;
        readonly protected IPatientRepository PatientRepository;
        readonly protected int PageSize = 30;
        protected Task<User> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        #endregion

        #region Constructors
        public LapCommon(IInvestigationRepository investigationRepository,
            IInvestigationServices investigationServices,
            ILogFileServices logFileServices,
            IPatientRepository patientRepository,
            UserManager<User> userManager)
        {
            InvestigationRepository = investigationRepository;
            InvestigationServices = investigationServices;
            _userManager = userManager;
            LogFileServices = logFileServices;
            PatientRepository = patientRepository;
        }
        #endregion

        #region PrivateMethods
        protected async Task<JsonResult> SearchJson(string Search, string Sort)
        {
            var SearchResult = await InvestigationRepository.
                Investigation_Select_InvesigationCategory_by_Type(Search);
            var ListResult = SearchResult.Values.
                Where(x => x.Sort == Sort).Select(x => x.Type).Take(6).ToList();
            JsonResult result = new JsonResult(ListResult);
            return result;
        }

        protected async Task<SearchInvestigation> SearchPatient
            (string Search, int Page, string Sort)
        {
            var Investigations = (string.IsNullOrEmpty(Search)) ?
                await InvestigationRepository.
                Investigation_Select_InvesigationCategory_by_Sort(Sort) :
                await InvestigationRepository.
                Investigation_Select_InvesigationCategory_by_Type(Search);

            return new SearchInvestigation
            {
                Investigations = Investigations.Skip((Page - 1) * PageSize).
                Take(PageSize).OrderBy(x => x.Value.Category).ToDictionary(x => x.Key, x => x.Value),
                Search = Search,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = Investigations.Count
                }
            };
        }

        protected async Task<InvestigationViewSearch>
            SearchInvestigation(int Page, string Sort)
        {
            var Investigations = await InvestigationServices.GetInvestigationView(Sort);
            return new InvestigationViewSearch
            {
                InvestigationView = Investigations.Skip((Page - 1) * PageSize).
                Take(PageSize).ToDictionary(x => x.Key, x => x.Value),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = Investigations.Count
                }
            };
        }

        protected async Task<JsonResult> SearchCatJson(string Search, string Sort)
        {
            var SearchResult = await InvestigationRepository.
                Investigation_Select_InvesigationCategory_by_Category(Search);
            var ListResult = SearchResult.Values.
                Where(x => x.Sort == Sort).Select(x => x.Category).Distinct().Take(6).ToList();
            JsonResult result = new JsonResult(ListResult);
            return result;
        }

        protected async Task<PatientInvestigation> ConvertToInvestigation
            (EditPatientInvestigation investigation)
        {
            var user = await CurrentUser;
            return new PatientInvestigation()
            {
                AdmessionID = investigation.AdmessionID,
                Comment = (string.IsNullOrEmpty(investigation.Comment)) ?
                                     "Nothing to comment" : investigation.Comment,
                FiniahDate = DateTime.Now,
                Finish = investigation.Finish,
                Note = investigation.Note,
                PIID = investigation.PIID,
                RequestDate = investigation.RequestDate,
                Result = investigation.Result,
                StaffID = user.Id,
                Type = investigation.Type,
            };
        }

        protected async Task<RayViewSearch> SearchRay(int Page, string Sort)
        {
            var Investigations = await InvestigationServices.GetRayView(Sort);
            return new RayViewSearch
            {
                InvestigationView = Investigations.Skip((Page - 1) * PageSize).
                Take(PageSize).ToDictionary(x => x.Key, x => x.Value),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = Investigations.Count
                }
            };
        }

        #endregion
    }
}
