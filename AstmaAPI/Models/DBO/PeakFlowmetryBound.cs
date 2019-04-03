using AstmaAPI.Models.API.Common;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AstmaAPI.Models.DBO
{
    [DataContract]
    public class PeakFlowmetryBound
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public int Age { get; set; }

        [IgnoreDataMember]
        [Required]
        public int Height { get; set; }

        [IgnoreDataMember]
        [Required]
        public SexEnum Sex { get; set; }

        [DataMember]
        [Required]
        public int Value { get; set; }
    }
}
