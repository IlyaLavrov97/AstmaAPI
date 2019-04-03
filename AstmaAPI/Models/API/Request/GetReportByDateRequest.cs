using System;

namespace AstmaAPI.Models.API.Request
{
    public class GetReportByDateRequest : BaseRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
