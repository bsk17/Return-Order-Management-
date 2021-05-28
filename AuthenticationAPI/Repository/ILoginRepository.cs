using AuthenticationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationAPI.Repository
{
    public interface ILoginRepository
    {
        UserResponse Login(UserRequest userRequest);
    }
}
