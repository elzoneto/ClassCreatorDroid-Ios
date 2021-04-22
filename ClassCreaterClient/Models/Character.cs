using System;
using System.Collections.Generic;
using System.Text;

namespace ClassCreaterClient.Moldes
{
    public class Character
    {
        public int ID { get; set; }
        public string CharacterName { get; set; }
        public string CharacterDescription { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        public int ClassID { get; set; }
        public Class Class { get; set; }
        public Byte[] RowVersion { get; set; }
    }
}
