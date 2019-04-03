using System;

namespace AstmaAPI.Models.API.Request
{
    public class GetValuesByDateRequest : BaseRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
