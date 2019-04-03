using AstmaAPI.Models.API.Common;
using AstmaAPI.Models.DBO;
using System.Runtime.Serialization;

namespace AstmaAPI.ViewModels.Response
{
    [DataContract]
    public class AuthResponse
    {
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public User User { get; set; }

        [DataMember]
        public UserPeakFlowmetryBounds PeakFlowmetryBounds { get; set; }
    }
}
