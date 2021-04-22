using ClassCreaterClient.Moldes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassCreaterClient.Models
{
    public class Skill
    {
        public int ID { get; set; }
        public string SkillName { get; set; }
        public string SkillDescription { get; set; }
        public int ClassID { get; set; }
        public Class Class { get; set; }
        public Byte[] RowVersion { get; set; }
    }
}
