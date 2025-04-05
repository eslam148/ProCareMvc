using Microsoft.AspNetCore.Mvc;
using ProCareMvc.business;

namespace ProCareMvc.presentation.Controllers
{
    public class LabsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public LabsController(IUnitOfWork unitOfWork) 
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
