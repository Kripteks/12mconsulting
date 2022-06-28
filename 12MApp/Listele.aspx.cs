using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _12MApp
{
    public partial class Listele : System.Web.UI.Page
    {
        List<Classes.Stk> _stkList = null;

        private MssqlEntityFramework GetEntity() => new MssqlEntityFramework("192.168.1.20,1433", "12M", "sa", "1453");
        protected void Page_Load(object sender, EventArgs e)
        {   
            using (MssqlEntityFramework sqlHelper = GetEntity())
            {
                // Örnek Kullanım - 1
                _stkList = sqlHelper.GetList<Classes.Stk>(sqlHelper.GetCommand(sqlHelper.ToSelectString<Classes.Stk>()));

                /* Örnek Kullanım - 2
                 _stkList = sqlHelper.GetList<Classes.Stk>(sqlHelper.GetCommand("SELECT * FROM STK")); */
            }
        }


        protected void btnAra_Click(object sender, EventArgs e)
        {
            using (MssqlEntityFramework sqlHelper = GetEntity())
            {
                if (dateIlkTarih.Text.Trim().Length == 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "İlk tarih alanı boş geçilemez." + "');", true);
                    return;
                }

                if (dateSonTarih.Text.Trim().Length == 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Son tarih alanı boş geçilemez." + "');", true);
                    return;
                }

                if (txtMalzemeKodu.Text.Trim().Length == 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Malzeme Kodu/Adı alanı boş geçilemez." + "');", true);
                    return;
                }

                string malzemeKodu = txtMalzemeKodu.Text;
                int ilkTarih = ConversionEntity.Parse<int>(ConversionEntity.Parse<DateTime>(dateIlkTarih.Text).ToOADate());
                int sonTarih = ConversionEntity.Parse<int>(ConversionEntity.Parse<DateTime>(dateSonTarih.Text).ToOADate());

                var malzemeFirst = _stkList.Where(p => p.MalKodu.Contains(malzemeKodu) || p.MalAdi.Contains(malzemeKodu)).FirstOrDefault();

                if (malzemeFirst != null)
                {
                    DataTable dataReport = sqlHelper.GetDataTable(sqlHelper.GetCommand("exec Rapor @MalzemeKod, @IlkTarih, @SonTarih",
                        new System.Data.SqlClient.SqlParameter() { ParameterName = "@MalzemeKod", SqlDbType = SqlDbType.NVarChar, Value = malzemeKodu },
                        new System.Data.SqlClient.SqlParameter() { ParameterName = "@IlkTarih", SqlDbType = SqlDbType.Int, Value = ilkTarih },
                        new System.Data.SqlClient.SqlParameter() { ParameterName = "@SonTarih", SqlDbType = SqlDbType.Int, Value = sonTarih }));
                    dataReport.Columns.Add("StokMiktar", typeof(double));

                    var stokMiktar = 0;
                    for (int index = 0; index < dataReport.Rows.Count; index++)
                    {
                        DataRow item = dataReport.Rows[index];

                        if (item != null)
                        {
                            if (item["IslemTur"].ToString() == "Giriş")
                            {
                                stokMiktar += ConversionEntity.Parse<int>(item["GirisMiktar"]);
                            }
                            else
                            {
                                stokMiktar -= ConversionEntity.Parse<int>(item["CikisMiktar"]);
                            }

                            dataReport.Rows[index]["StokMiktar"] = stokMiktar;
                        }
                        stokMiktar = 0;
                    }


                    GridRapor.DataSource = dataReport;
                    GridRapor.DataBind();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Malzeme bulunamadı." + "');", true);
                }
            }
        }
    }
}