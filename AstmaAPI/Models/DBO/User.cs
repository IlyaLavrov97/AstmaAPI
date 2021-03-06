﻿using AstmaAPI.Models.API.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AstmaAPI.Models.DBO
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [IgnoreDataMember]
        [Required]
        [StringLength(256)]
        public string Login { get; set; }

        [IgnoreDataMember]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataMember]
        [Required]
        [StringLength(256)]
        public string Surname { get; set; }

        [DataMember]
        [Required]
        public SexEnum Sex { get; set; }

        [DataMember]
        [Required]
        public DateTime BirthDate { get; set; }

        [DataMember]
        public int Height { get; set; }

        [DataMember]
        public int Weight { get; set; }

        [IgnoreDataMember]
        public UserToken UserToken { get; set; }

        [IgnoreDataMember]
        public ICollection<ChartValue> Values { get; set; }
    }
}
