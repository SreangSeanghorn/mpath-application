using MPath.Application.ResponsesDTOs;
using MPath.Application.Shared.Responses;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Commands
{
    public class UserLoginCommand: ICommand<BaseResponse<UserLoginResponseDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserLoginCommand(string email, string password){
            this.Email = email;
            this.Password = password;
        }
        
    }
}