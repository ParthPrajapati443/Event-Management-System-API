using Entity;
using LIBRARY;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Security.Policy;

namespace BL
{
    public class ActivityBL
    {
        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 18/04/2024
        /// Discription: this method is add and get activity details from database by admin
        /// </summary>
        /// <param name="ActivityEntity"></param>
        /// <returns>This method is return details of activity based on flag</returns>
        #region Activity
        //this method is add and get activity details in database by admin
        public SerializeResponse<ActivityEntity> Activity(ActivityEntity objEntity)
        {
            InsertLog.WriteErrrorLog("ActivityBL=>Activity=>Started");
            ConvertDataTable bl = new ConvertDataTable();
            SerializeResponse<ActivityEntity> objResponsemessage = new SerializeResponse<ActivityEntity>();

            DataSet ds = new DataSet();
            SqlDataProvider objSDP = new SqlDataProvider();
            string query = "Sp_AddActivity";
            try
            {
                string con_Str = Connection.constrEMS;

                SqlParameter prm1 = objSDP.CreateInitializedParameter("@ActivityID", DbType.String, objEntity.ActivityID);
                SqlParameter prm2 = objSDP.CreateInitializedParameter("@ActivityName", DbType.String, objEntity.ActivityName);
                SqlParameter prm3 = objSDP.CreateInitializedParameter("@ActivityStartDate", DbType.String, objEntity.ActivityStartDate);
                SqlParameter prm4 = objSDP.CreateInitializedParameter("@ActivityEndDate", DbType.String, objEntity.ActivityEndDate);
                SqlParameter prm5 = objSDP.CreateInitializedParameter("@ActivityDiscription", DbType.String, objEntity.ActivityDiscription);
                SqlParameter prm6 = objSDP.CreateInitializedParameter("@ActivityPrice", DbType.String, objEntity.ActivityPrice);
                SqlParameter prm7 = objSDP.CreateInitializedParameter("@EventID", DbType.String, objEntity.EventID);
                SqlParameter prm8 = objSDP.CreateInitializedParameter("@Flag", DbType.String, objEntity.Flag);
                

                SqlParameter[] sqlPara = { prm1, prm2, prm3, prm4, prm5, prm6, prm7, prm8 };
                ds = SqlHelper.ExecuteDataset(con_Str, query, sqlPara);

                //give code and message when activity is inserted or Published
                if ((objEntity.Flag == "Insert" || objEntity.Flag == "Publish") && ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objResponsemessage.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                    objResponsemessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                }
                //This block give the details of non-publish or publish activity of perticular event
                else if ((objEntity.Flag == "ViewActivity" || objEntity.Flag == "ViewWithPublish" ) && ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objResponsemessage.ArrayOfResponse = bl.ListConvertDataTable<ActivityEntity>(ds.Tables[0]);
                }
                //give details of perticular activity for update it
                else if ( objEntity.Flag == "GetActivity" && ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objResponsemessage.ArrayOfResponse = bl.ListConvertDataTable<ActivityEntity>(ds.Tables[0]);
                    objResponsemessage.ArrayOfResponse[0].ActivityStartDate = DateTime.ParseExact(objResponsemessage.ArrayOfResponse[0].ActivityStartDate, "dd-MM-yyyy HH:mm:ss", null).ToString("yyyy-MM-ddTHH:mm");
                    objResponsemessage.ArrayOfResponse[0].ActivityEndDate = DateTime.ParseExact(objResponsemessage.ArrayOfResponse[0].ActivityEndDate, "dd-MM-yyyy HH:mm:ss", null).ToString("yyyy-MM-ddTHH:mm");
                }
                //give code and message when activity is deleted or updated
                else if ((objEntity.Flag == "Delete" || objEntity.Flag == "Update") && ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Columns.Count > 2)
                    {
                        objResponsemessage.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                        objResponsemessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                        objResponsemessage.EventID = Convert.ToInt32(ds.Tables[0].Rows[0]["EventID"]);
                    }
                    else
                    {
                        objResponsemessage.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                        objResponsemessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                    }
                }
            }
            catch (Exception ex)
            {
                objResponsemessage.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("ActivityBL=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return objResponsemessage;
        } 
        #endregion
        
        //public string ConvertToDateTime(string Date , string Format)
        //{
        //    string ODate = string.Empty;
        //    DateTime parsedDate = DateTime.ParseExact(Date, "dd-MM-yyyy HH:mm:ss", null);

        //    if (Format == "yyyy-MM-ddTHH:mm:ss")
        //    {
        //        string isoFormattedDate = parsedDate.ToString("yyyy-MM-ddTHH:mm:ss");
        //    }
        //    else if (Format == "yyyy-MM-ddTHH:mm")
        //    {
        //        string isoFormattedDate = parsedDate.ToString("yyyy-MM-ddTHH:mm");
        //    }
        //    else
        //    {
               
        //    }
        //    return "";
        //}
    }
}
