using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace _12MApp.Classes
{
    [DisplayName("STK")]
    public class Stk
    {
        public int ID { get; set; }
        public string MalKodu { get; set; }
        public string MalAdi { get; set; }
    }
}