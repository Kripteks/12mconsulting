using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace _12MApp.Classes
{
    [DisplayName("STI")]
    public class Sti
    {
		public int ID { get; set; }
		public short IslemTur { get; set; }
		public string EvrakNo { get; set; }
		public int Tarih { get; set; }
		public string MalKodu { get; set; }
		public decimal Miktar { get; set; }
		public decimal Fiyat { get; set; }
		public decimal Tutar { get; set; }
		public string Birim { get; set; }
	}
}