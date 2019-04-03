using System;

namespace AstmaAPI.Models.API.Request
{
    public class AddValueRequest : BaseRequest
    {
        public int Value { get; set; }

        public DateTime Date { get; set; }

        public bool IsMorning { get; set; }
    }
}
