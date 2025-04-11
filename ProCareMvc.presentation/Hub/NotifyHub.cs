using  Microsoft.AspNetCore.SignalR;
using ProCareMvc.business;
namespace ProCareMvc.presentation.Hub
{
    public class NotifyHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotifyHub(IUnitOfWork unitOfWork)
        {
             _unitOfWork = unitOfWork;
        }
        public override async Task<Task> OnConnectedAsync()
        {
             var  result = _unitOfWork.Order.GetAll().Where(x => x.PatientId == Guid.Parse("99999999-9999-9999-9999-999999999991")).ToList();
             await Clients.All.SendAsync("ReceiveNotify", result);
             return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
