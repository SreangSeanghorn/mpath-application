using MPath.Application.ResponsesDTOs;
using MPath.Application.Shared.Responses;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Commands
{
    public class UserRegisterCommand : ICommand<UserRegisteredResponse>
    {
        public UserRegisterCommand(string userName, string email, string password)
        {
            this.UserName = userName;
            this.Email = email;
            this.Password = password;

        }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
    }



}