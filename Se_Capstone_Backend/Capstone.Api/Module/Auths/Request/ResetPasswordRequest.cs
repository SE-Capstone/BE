﻿namespace Capstone.Api.Module.Auths.Request
{
    public class ResetPasswordRequest
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
