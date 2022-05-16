using GUI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Components
{
    public class CategoryEdit : ViewComponent
    {
        #region Private Variables
        readonly IInvestigationRepository InvestigationRepository;
        #endregion

        public CategoryEdit(IInvestigationRepository investigationRepository) =>
            InvestigationRepository = investigationRepository;

        [HttpGet]
        public async Task<IViewComponentResult> InvokeAsync(string Category) =>
            View(await InvestigationRepository.
                Investigation_Select_Categories_by_Category(Category));
    }
}
