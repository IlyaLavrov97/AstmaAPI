using System;

namespace AstmaAPI.ViewModels.Request
{
    public class SignupRequest : BaseRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Sex { get; set; }

        public DateTime BirthDate { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }
    }
}
