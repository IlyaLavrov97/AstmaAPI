using System;

namespace AstmaAPI.ViewModels.Request
{
    public class AddValueRequest : BaseRequest
    {
        public int Value { get; set; }

        public DateTime Date { get; set; }

        public bool IsMorning { get; set; }
    }
}
