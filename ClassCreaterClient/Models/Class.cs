using ClassCreaterClient.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassCreaterClient.Moldes
{
    public class Class
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TypeID { get; set; }
        public ClassType Type { get; set; }
        public Byte[] RowVersion { get; set; }
    }
}
