using LIBRARY;
using System.Data.SqlClient;
using System.Data;
using System;
using Entity;

namespace BL
{
    public class UserRegistrationBL
    {
        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 18/04/2024
        /// Discription: this method is User registration and get data of user                        
        /// </summary>
        /// <param name="LoginAndLogoutEntity"></param>
        /// <returns>This method is return code and message with some user detalis if registration is successfull and also get user details</returns>
        #region UserRegistration
        //this method is User registration and get data of user
        public SerializeResponse<UserRegistrationEntity> UserRegistration(UserRegistrationEntity objEntity)
        {
            InsertLog.WriteErrrorLog("UserRegistrationBL=>UserRegistration=>Started");
            ConvertDataTable bl = new ConvertDataTable();
            SerializeResponse<UserRegistrationEntity> objResponsemessage = new SerializeResponse<UserRegistrationEntity>();

            DataSet ds = new DataSet();
            SqlDataProvider objSDP = new SqlDataProvider();
            string query = "Sp_UserRegistration";
            try
            { 
                string con_Str = Connection.constrEMS;

                SqlParameter prm1 = objSDP.CreateInitializedParameter("@Flag", DbType.String, objEntity.Flag);
                SqlParameter prm2 = objSDP.CreateInitializedParameter("@UserName", DbType.String, objEntity.UserName);
                SqlParameter prm3 = objSDP.CreateInitializedParameter("@UserEmail", DbType.String, objEntity.UserEmail);
                SqlParameter prm4 = objSDP.CreateInitializedParameter("@UserPhone", DbType.String, objEntity.UserPassword);
                SqlParameter prm5 = objSDP.CreateInitializedParameter("@UserPhone", DbType.String, objEntity.UserMobile);

                SqlParameter[] sqlPara = { prm1, prm2, prm3, prm4, prm5 };
                ds = SqlHelper.ExecuteDataset(con_Str, query, sqlPara);

                if (objEntity.Flag == "INSERT" && ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Columns.Count > 2)
                    {
                        objResponsemessage.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                        objResponsemessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                        objResponsemessage.EmailID = Convert.ToString(ds.Tables[0].Rows[0]["Email"]);
                        objResponsemessage.UserID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                    }
                    else
                    {
                        objResponsemessage.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                        objResponsemessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                    }
                }
                else if (objEntity.Flag == "GET" && ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objResponsemessage.ArrayOfResponse = bl.ListConvertDataTable<UserRegistrationEntity>(ds.Tables[0]);
                    objResponsemessage.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                    objResponsemessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                }
            }
            catch (Exception ex)
            {
                objResponsemessage.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("UserRegistrationBL=>Exception" + ex.Message + ex.StackTrace);
            }
            return objResponsemessage;
        } 
        #endregion
    }
}
