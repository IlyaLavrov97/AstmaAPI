using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstmaAPI.ViewModels.Request
{
    public class GetValuesByDateRequest : BaseRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
