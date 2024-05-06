using System.Data.SqlClient;
using System.Data;
using System;
using LIBRARY;
using Entity;

namespace BL
{
    public class LoginAndLogoutBL
    {
        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 18/04/2024
        /// Discription: this method is used for login of user or admin
        /// </summary>
        /// <param name="LoginAndLogoutEntity"></param>
        /// <returns>This method is return code and message with some user detalis if login is successfull</returns>
        #region Login method for user and admin
        //this method is used for login of user or admin
        public ResponseMessage Login(LoginAndLogoutEntity objEntity)
        {
            InsertLog.WriteErrrorLog("LoginAndLogoutBL=>Login=>Started");
            ConvertDataTable bl = new ConvertDataTable();
            ResponseMessage responseMessage = new ResponseMessage();
            DataSet ds = new DataSet();
            SqlDataProvider objSDP = new SqlDataProvider();
            string query = "Sp_Login";
            try
            {
                string con_Str = Connection.constrEMS;

                SqlParameter prm1 = objSDP.CreateInitializedParameter("@EmailId", DbType.String, objEntity.EmailID);
                SqlParameter prm2 = objSDP.CreateInitializedParameter("@Password", DbType.String, objEntity.Password);
                SqlParameter prm3 = objSDP.CreateInitializedParameter("@Role", DbType.String, objEntity.Role);

                SqlParameter[] sqlPara = { prm1, prm2, prm3 };
                ds = SqlHelper.ExecuteDataset(con_Str, query, sqlPara);
                if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Columns.Count > 2)
                    {
                        responseMessage.code = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                        responseMessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                        responseMessage.Email = Convert.ToString(ds.Tables[0].Rows[0]["Email"]);
                        responseMessage.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                    }
                    else
                    {
                        responseMessage.code = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                        responseMessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("LoginAndLogoutBL=>Login=>Exception" + ex.Message + ex.StackTrace);
            }
            return responseMessage;
        }
        #endregion

        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 18/04/2024
        /// Discription: this method is user and admin logout
        /// </summary>
        /// <param name="LoginAndLogoutEntity"></param>
        /// <returns>This method is return code and message</returns>
        #region Logout method for user and admin
        //this method is user and admin logout
        public ResponseMessage Logout(LoginAndLogoutEntity objEntity)
        {
            InsertLog.WriteErrrorLog("LoginAndLogoutBL=>Logout=>Started=>");
            ConvertDataTable bl = new ConvertDataTable();
            ResponseMessage responseMessage = new ResponseMessage();
            DataSet ds = new DataSet();
            SqlDataProvider objSDP = new SqlDataProvider();
            string query = "Sp_Logout";
            try
            {
                string con_Str = Connection.constrEMS;

                SqlParameter prm1 = objSDP.CreateInitializedParameter("@Email", DbType.String, objEntity.EmailID);

                SqlParameter[] sqlPara = { prm1 };
                ds = SqlHelper.ExecuteDataset(con_Str, query, sqlPara);
                if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    responseMessage.code = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                    responseMessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                }
            }
            catch (Exception ex)
            {
                responseMessage.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("LoginAndLogoutBL=>Logout=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return responseMessage;
        } 
        #endregion
    }
}
