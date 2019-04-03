namespace AstmaAPI.Models.API.Request
{
    public class EditUserRequest : BaseRequest
    {
        public string Name { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }
    }
}
