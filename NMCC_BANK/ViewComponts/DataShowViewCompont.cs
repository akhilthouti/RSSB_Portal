using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace INDO_FIN_NET.ViewComponts
{
    public class DataShowViewCompont:ViewComponent
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData1;
        public DataShowViewCompont(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData1 = iNDO_;
        }
        public async Task <IViewComponentResult> InvokeAsync() 
        {
            return View();
        }
    }
}
