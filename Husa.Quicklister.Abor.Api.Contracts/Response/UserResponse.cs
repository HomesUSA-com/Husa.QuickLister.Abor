namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System;

    public class UserResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
