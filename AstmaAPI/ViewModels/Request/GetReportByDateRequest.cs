using System;

namespace AstmaAPI.ViewModels.Request
{
    public class GetReportByDateRequest : BaseRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
