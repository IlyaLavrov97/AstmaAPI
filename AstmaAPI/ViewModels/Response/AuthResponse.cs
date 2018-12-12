using System.Runtime.Serialization;

namespace AstmaAPI.ViewModels.Response
{
    [DataContract]
    public class AuthResponse
    {
        [DataMember]
        public string Token { get; set; }
    }
}
