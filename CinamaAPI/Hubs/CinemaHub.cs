using CinemaBL.Enums;
using CinemaBL.Extension;
using CinemaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CinemaAPI.Hubs
{
    public class CinemaHub : Hub
    {
        private readonly INotify _nf;

        public CinemaHub(INotify nf)
        {
            _nf = nf;
        }
        //public void SendSeatRoom(string user, string message)
        //{
        //    Clients.All.SendAsync("ReceiveSeatRoom", user, message);
        //}


        //public override Task OnConnectedAsync()
        //{
        //    return base.OnConnectedAsync();
        //}

        [Authorize(Roles = "EMPLOYEE", Policy = "OWN_SALA")]
        public override async Task<Task> OnConnectedAsync()
        {
            UserModelDTO um = new UserModelDTO();
            var cu = Context.User;
            
            um.JobQualification = cu.Claims.FirstOrDefault(c => c.Type == nameof(um.JobQualification)).Value;

            if (um.JobQualification.ToEnum<JobEmployeeQualificationEnum>() == JobEmployeeQualificationEnum.OWN_SALA)
            {
                string IdUser = cu.Claims.FirstOrDefault(c => c.Type == nameof(um.Id)).Value;
                // creo il gruppo semplicemente con l'Id del employee
                await Groups.AddToGroupAsync(Context.ConnectionId, IdUser);
                //Clients.Groups(IdUser).SendAsync("ReceiveSeatRoom", "Connected!!");
                _nf.SendMessageToOwn_SALA(int.Parse(IdUser));
            }

            return base.OnConnectedAsync();
        }
    }
}
