using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.IO;

namespace MFbSharp
{
    class MFb
    {
        public void FbtoCSV(string fdb,string folder)
        {
            List<string> Tablas = new List<string>();

            FbConnection fb = new FbConnection("User ID=sysdba;Password=masterkey;" +
                           "Database=localhost:" + fdb + ";" +
                           "DataSource=localhost;Charset=NONE;");

            FbCommand fbCommand = new FbCommand("SELECT DISTINCT RDB$RELATION_NAME AS Tablas FROM RDB$RELATION_FIELDS WHERE RDB$SYSTEM_FLAG = 0;", fb);
            fb.Open();
            FbDataReader fbDataReader = fbCommand.ExecuteReader();

            while (fbDataReader.Read())
            {
                Tablas.Add(fbDataReader["Tablas"].ToString());
            }
            fb.Close();
            foreach (string t in Tablas)
            {
                try
                {
                    fbCommand = new FbCommand("SELECT * FROM " + t, fb);
                    FbDataAdapter fbDataAdapter = new FbDataAdapter(fbCommand);
                    DataTable data = new DataTable();
                    fbDataAdapter.Fill(data);

                    ToCSV(data, folder + "\\" + t.Trim() + ".csv");
                    Console.WriteLine("Respaldo de {0} Exportando",t);

                }
                catch (OutOfMemoryException Ex)
                {
                    Console.WriteLine(Ex.Message);
                }
                catch (SqlException Ex)
                {
                    Console.WriteLine(Ex.Message);
                }

            }
        }
        private void ToCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
    }
}
