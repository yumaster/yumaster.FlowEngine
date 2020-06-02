using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace yumaster.FlowEngine.SSO
{
    public class LoginResult : Response<string>
    {
        public string ReturnUrl;
        public string Token;
    }
}
