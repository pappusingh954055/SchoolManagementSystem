using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.DTOs
{
    public class LogOutRequest
    {
        public Guid UserId { get; set; }

        public string RefrershToken { get; set; } = null;
    }
}
