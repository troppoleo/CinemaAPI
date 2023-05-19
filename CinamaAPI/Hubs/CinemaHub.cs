using CinemaBL.Enums;
using CinemaBL.Extension;
using CinemaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static CinemaDTO.UserModelDTO;

namespace CinemaAPI.Hubs
{
    [Authorize]
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



        //[Authorize(Roles = "EMPLOYEE", Policy = "OWN_SALA")]  // questo viene fatto a priori
        public override async Task<Task> OnConnectedAsync()
        {
            UserModelDTO um = new UserModelDTO();
            var cu = Context.User;

            var rule = cu.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.ToEnum<UserModelType>();
            switch (rule)
            {
                case UserModelType.ADMIN:
                    //string IdAdmin = cu.Claims.FirstOrDefault(c => c.Type == nameof(um.Id)).Value;
                    await Groups.AddToGroupAsync(Context.ConnectionId, UserModelType.ADMIN.ToString());
                    _nf.SendMessageToAdmin();
                    break;

                case UserModelType.EMPLOYEE:
                    um.JobQualification = cu.Claims.FirstOrDefault(c => c.Type == nameof(um.JobQualification))!.Value;
                    if (um.JobQualification != null)
                    {
                        var job = um.JobQualification.ToEnum<JobEmployeeQualificationEnum>();
                        string IdUser = cu.Claims.FirstOrDefault(c => c.Type == nameof(um.Id)).Value;
                        // creo il gruppo semplicemente con l'Id del employee
                        await Groups.AddToGroupAsync(Context.ConnectionId, IdUser);

                        switch (job)
                        {
                            case JobEmployeeQualificationEnum.GET_TICKET:
                                _nf.SendMessageToGET_TICKET(int.Parse(IdUser));
                                break;

                            case JobEmployeeQualificationEnum.OWN_SALA:                                
                                //Clients.Groups(IdUser).SendAsync("ReceiveSeatRoom", "Connected!!");
                                _nf.SendMessageToOwn_SALA(int.Parse(IdUser));
                                break;
                        }
                    }
                    break;

            }
            

            return base.OnConnectedAsync();
        }
    }
}
