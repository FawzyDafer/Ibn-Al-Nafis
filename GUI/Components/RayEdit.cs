using GUI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Components
{
    public class RayEdit : ViewComponent
    {
        #region Private Variables
        readonly IInvestigationRepository InvestigationRepository;
        #endregion

        public RayEdit(IInvestigationRepository investigationRepository) =>
            InvestigationRepository = investigationRepository;
        [HttpGet]
        public async Task<IViewComponentResult> InvokeAsync(string Category) =>
            View(await InvestigationRepository.
                Investigation_Select_Categories_by_Category(Category));
    }
}
