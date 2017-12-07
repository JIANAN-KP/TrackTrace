using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace TT_Scan
{
    public class DBOperator
    {
        string myConnectionString = "server = localhost;uid =root;pwd=password;database=pss;port=3306;";
        MySqlConnection conn;
        public DBOperator()
        {
            conn = new MySqlConnection(myConnectionString);
        }

        public void OpenConnection()
        {
            conn.Open();
        }

        public void CloseConnection()
        {
            conn.Close();
        }

        public Dictionary<string, int> InsertUpdateResults(Dictionary<string,string> dict,MatchData data,MatchItem item,ref int iuFlag,ref int duFlag,string type)
        {
            Dictionary<string, int> updateDic = new Dictionary<string, int>();
            foreach(KeyValuePair<string,string> pair in dict)
            {
                #region search if item exist
                /* for the dictionary, source is the key, destination is the value*/
                DateTime dateTime = data.TimeStamp;
                int i = -1;
                using (MySqlConnection mysqlConn = new MySqlConnection(myConnectionString))
                {
                    mysqlConn.Open();
                    MySqlCommand selectCmd = null;
                    string selectSql = "SELECT * FROM pss.match_status WHERE SOURCE_PLATE = '" + pair.Key + "' and TIME_STAMP = '" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    selectCmd = new MySqlCommand(selectSql, mysqlConn);
                    using (MySqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            i = reader.GetInt32(0);                            
                        }
                    }
                    mysqlConn.Close();       
                }
                using (MySqlConnection mysqlConn = new MySqlConnection(myConnectionString))
                {
                    mysqlConn.Open();
                    if (i == -1) //no exist data
                    {
                        MySqlCommand insertCmd = null;
                        string insertSql = "INSERT INTO pss.match_status (SOURCE_PLATE,DESTINATION_PLATE,PROCESS,MATCHING,SCRIPT_COMPLETED,MACHINE,OPERATOR,SCRIPT_USED,PLATES_NUM,VOLUME,ROUNDS_NUM," +
                            "MIX_ROUNDS_NUM,RETURN_TIPS,PLATE_REPLICATE_NUM,SAMPLE_NUM,TOTAL_VOLUME,DILUTION_FACTOR,TIP_WETTING,SETS_NUM,CONTROL_ADDITION,VOLUME_OF_CONTROLS,PLEX1_SET1,PLEX1_SET2,TIME_STAMP) VALUES('" +
                            pair.Key + "','" + pair.Value + "','" + data.ProcessName + "','" +
                            item.itemResult + "','" + data.CompleteStatus + "','" + data.MachineUsed + "','" + data.OperatorName + "','" + data.ScriptUsed + "','" + data.PlatesNum +
                            "','" + data.Volume + "','" + data.RoundsNum + "','" + data.MixRoundsNum + "','" + data.ReturnTips + "','" + data.PlateReplicateNum + "','" + data.SampleNum + "','" + data.TotalVolume
                            + "','" + data.DilutionFactor + "','" + data.TipWetting + "','" + data.SetsNum + "','" + data.ControlAddition + "','" +
                            data.VolumeOfControls + "','" + data.Plex1_Set1 + "','" + data.Plex1_Set2 + "','" + data.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss") + "');";
                        insertCmd = new MySqlCommand(insertSql, mysqlConn);
                        insertCmd.ExecuteNonQuery();
                        string expCode = GetExpCode(pair.Key);
                        if(type.Equals("Ext"))
                        {
                            iuFlag += 1;
                            if(updateDic.ContainsKey(expCode))
                            {
                                updateDic[expCode]++;
                            }else
                            {
                                updateDic.Add(expCode, 1);
                            }
                        }else
                        {
                            duFlag += 1;
                            if (updateDic.ContainsKey(expCode))
                            {
                                updateDic[expCode]++;
                            }
                            else
                            {
                                updateDic.Add(expCode, 1);
                            }
                        } 
                    }
                    else  //have exist data update it, in case some data is new 
                    {
                        MySqlCommand updateCmd = null;
                        string updateSql = "UPDATE pss.match_status set SOURCE_PLATE = '" + pair.Key + "',DESTINATION_PLATE='" + pair.Value
                            + "',PROCESS='" + data.ProcessName + "',MATCHING='" + item.itemResult + "',SCRIPT_COMPLETED='" + data.CompleteStatus + "',MACHINE='" + data.MachineUsed
                            + "',OPERATOR='" + data.OperatorName + "',SCRIPT_USED='"+data.ScriptUsed+ "',PLATES_NUM='"+data.PlatesNum + "',VOLUME='" + data.Volume + "',ROUNDS_NUM='" + data.RoundsNum + "',MIX_ROUNDS_NUM='" + data.MixRoundsNum
                            + "',RETURN_TIPS='" + data.ReturnTips + "',PLATE_REPLICATE_NUM='"+data.PlateReplicateNum + "',SAMPLE_NUM='" + data.SampleNum + "',TOTAL_VOLUME='" + data.TotalVolume + "',DILUTION_FACTOR='" + data.DilutionFactor
                            + "',TIP_WETTING='" + data.TipWetting + "',SETS_NUM='" + data.SetsNum + "',CONTROL_ADDITION='" + data.ControlAddition + "',VOLUME_OF_CONTROLS='" +
                            data.VolumeOfControls + "',PLEX1_SET1='"+data.Plex1_Set1+ "',PLEX1_SET2='"+data.Plex1_Set2 + "' WHERE ID=" + i+";";
                        updateCmd = new MySqlCommand(updateSql, mysqlConn);
                        updateCmd.ExecuteNonQuery();                        
                    }
                    mysqlConn.Close();
                }         
                #endregion      
            }
            return updateDic;
        }

        public void InsertUpdateSummaryInfo(MatchItem item,Dictionary<string,int> updateDic,string updateTitle,bool extFlag)
        {
            bool exist = false;
            foreach(KeyValuePair<string,int> pair in updateDic)
            {
                using (MySqlConnection mysqlConn = new MySqlConnection(myConnectionString))
                {
                    mysqlConn.Open();
                    MySqlCommand selectCmd = null;
                    string selectSql = "SELECT * FROM pss.match_summary where EXPCODE = '" + pair.Key +"';";
                    selectCmd = new MySqlCommand(selectSql, mysqlConn);
                    using (MySqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read().Equals(true))
                        {
                            exist = true;
                        }
                    }
                    mysqlConn.Close();
                }
                if (exist == true && extFlag == true)
                {
                    using (MySqlConnection mysqlConn = new MySqlConnection(myConnectionString))
                    {
                        mysqlConn.Open();
                        MySqlCommand updateCmd = null;
                        string updateSql = "UPDATE pss.match_summary set EXTRACTION_BLOCKS = EXTRACTION_BLOCKS +" + pair.Value + " , " + updateTitle + "= " + updateTitle + "+" + pair.Value.ToString() + " where EXPCODE ='" + pair.Key + "';";
                        updateCmd = new MySqlCommand(updateSql, mysqlConn);
                        updateCmd.ExecuteNonQuery();
                        mysqlConn.Close();
                    }
                }
                else if (exist == true && extFlag == false)
                {
                    using (MySqlConnection mysqlConn = new MySqlConnection(myConnectionString))
                    {
                        mysqlConn.Open();
                        MySqlCommand updateCmd = null;
                        string updateSql = "UPDATE pss.match_summary set " + updateTitle + " = " + updateTitle + "+" + pair.Value.ToString() + " where EXPCODE ='" + pair.Key + "';";
                        updateCmd = new MySqlCommand(updateSql, mysqlConn);
                        updateCmd.ExecuteNonQuery();
                        mysqlConn.Close();
                    }
                }
                else if (exist == false && extFlag == true)
                {
                    using (MySqlConnection mysqlConn = new MySqlConnection(myConnectionString))
                    {
                        mysqlConn.Open();
                        MySqlCommand insertCmd = null;
                        string insertSql = "INSERT INTO pss.match_summary (EXPCODE,EXTRACTION_BLOCKS," + updateTitle + ") VALUES ('" + pair.Key + "'," + pair.Value.ToString() + "," + pair.Value.ToString() + ");";
                        insertCmd = new MySqlCommand(insertSql, mysqlConn);
                        insertCmd.ExecuteNonQuery();
                        mysqlConn.Close();
                    }
                }
                else if (exist == false && extFlag == false)
                {
                    using (MySqlConnection mysqlConn = new MySqlConnection(myConnectionString))
                    {
                        mysqlConn.Open();
                        MySqlCommand insertCmd = null;
                        string insertSql = "INSERT INTO pss.match_summary (EXPCODE," + updateTitle + ") VALUES ('" + pair.Key + "'," + pair.Value.ToString() + ");";
                        insertCmd = new MySqlCommand(insertSql, mysqlConn);
                        insertCmd.ExecuteNonQuery();
                        mysqlConn.Close();
                    }
                }
                CheckAllPass(pair.Key);
            }
        }

        public void CheckAllPass(string expCode)
        {
            bool allPass = true;
            using (MySqlConnection mysqlConn = new MySqlConnection(myConnectionString))
            {
                mysqlConn.Open();
                MySqlCommand selectCmd = null;
                string selectSql = "SELECT EXTRACTION_MATCHING_FAILED,EXTRACTION_SCRIPT_FAILED,DAUGHTER_SCRIPT_FAILED,DAUGHTER_MATCHING_FAILED "
                    + "FROM pss.match_summary WHERE EXPCODE = '" + expCode + "';"; 
                selectCmd = new MySqlCommand(selectSql, mysqlConn);
                using (MySqlDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if((reader.GetInt32(0) != 0)|| (reader.GetInt32(1) != 0)|| (reader.GetInt32(2) != 0)|| (reader.GetInt32(3) != 0))
                        {
                            allPass = false;
                        }
                    }
                }
                mysqlConn.Close();
            }

            if(allPass == false)
            {
                using (MySqlConnection mysqlConn = new MySqlConnection(myConnectionString))
                {
                    mysqlConn.Open();
                    MySqlCommand updateCmd = null;
                    string updateSql = "UPDATE pss.match_summary set ALLPASS = 'No' where EXPCODE = '" + expCode + "';";
                    updateCmd = new MySqlCommand(updateSql, mysqlConn);
                    updateCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
            }
        }

        public string GetExpCode(string str)
        {
            string[] strs = str.Split('-');
            return strs.First();
        }
    }
}
