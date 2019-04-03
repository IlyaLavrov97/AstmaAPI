using AstmaAPI.Models.DBO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AstmaAPI.Models
{
    [DataContract]
    public class ChartValue
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        [Required]
        public int Value { get; set; }

        [DataMember]
        [Required]
        public DateTime Date { get; set; }

        [DataMember]
        [Required]
        public bool IsMorning { get; set; }

        [IgnoreDataMember]
        [Required]
        public User User { get; set; }

        [IgnoreDataMember]
        public int UserId { get; set; }
    }
}
