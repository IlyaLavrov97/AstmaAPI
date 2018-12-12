namespace AstmaAPI.ViewModels.Request
{
    public class EditValueRequest : BaseRequest
    {
        public int ID { get; set; }

        public int Value { get; set; }
    }
}
