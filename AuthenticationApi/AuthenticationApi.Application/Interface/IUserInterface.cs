using AuthenticationApi.Application.DTOs;
using E_CommerceSharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Interface
{
    public interface IUserInterface
    {
        Task<Response> Register(AppUserDto appUserDto);

        Task<Response> Login(LoginDto loginDto);

        Task<GetUserDto> GetUser(int userId);
    }
}
