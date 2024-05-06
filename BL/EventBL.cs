using LIBRARY;
using System.Data.SqlClient;
using System.Data;
using System;
using Entity;
using System.Security.Policy;
using System.IO;

namespace BL
{
    public class EventBL
    {
        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 18/04/2024
        /// Discription: this method is add event details in database by admin
        /// </summary>
        /// <param name="AddEventEntity"></param>
        /// <returns>This method is return code and message about event</returns>
        #region Add Event
        //this method is add event details in database by admin
        public SerializeResponse<AddEventEntity> AddEvent(AddEventEntity objEntity)
        {
            InsertLog.WriteErrrorLog("EventBL=>AddEvent=>Started=>");
            ConvertDataTable bl = new ConvertDataTable();
            SerializeResponse<AddEventEntity> objResponsemessage = new SerializeResponse<AddEventEntity>();

            DataSet ds = new DataSet();
            SqlDataProvider objSDP = new SqlDataProvider();
            string query = "Sp_AddEvent";

            try
            {
                //convert Base64 string into image 
                byte[] imageBytes = Convert.FromBase64String(objEntity.EventImage);
                //create folder to store images in main folder of project
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Images");
                string imgpath = AppDomain.CurrentDomain.BaseDirectory + "Images\\" + DateTime.Now.ToString("HH:mm:ss").Replace(":", "-") + ".png"; //+ objEntity.ImageType;
                //save image in created folder
                File.WriteAllBytes(imgpath, imageBytes);
                //Make Connection With Database
                string con_Str = Connection.constrEMS;

                SqlParameter prm1 = objSDP.CreateInitializedParameter("@EventName", DbType.String, objEntity.EventName);
                SqlParameter prm2 = objSDP.CreateInitializedParameter("@EventStartDate", DbType.String, objEntity.EventStartDate);
                SqlParameter prm3 = objSDP.CreateInitializedParameter("@EventEndDate", DbType.String, objEntity.EventEndDate);
                SqlParameter prm4 = objSDP.CreateInitializedParameter("@EventDiscription", DbType.String, objEntity.EventDiscription);
                SqlParameter prm5 = objSDP.CreateInitializedParameter("@EventAddress", DbType.String, objEntity.EventAddress);
                SqlParameter prm6 = objSDP.CreateInitializedParameter("@EventCreatedBy", DbType.Int32, objEntity.EventCreatedBy);
                SqlParameter prm7 = objSDP.CreateInitializedParameter("@EventCreatedBy", DbType.String, imgpath);
                SqlParameter prm8 = objSDP.CreateInitializedParameter("@Flag", DbType.String, objEntity.Flag);

                SqlParameter[] sqlPara = { prm1, prm2, prm3, prm4, prm5, prm6, prm7, prm8 };
                //Executing SP and get respose 
                ds = SqlHelper.ExecuteDataset(con_Str, query, sqlPara);
                //This block get code and message when event is inserted
                if (objEntity.Flag == "Insert" && ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objResponsemessage.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                    objResponsemessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                }
            }
            catch (Exception ex)
            {
                objResponsemessage.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("EventBL=>AddEvent=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return objResponsemessage;
        }
        #endregion

        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 18/04/2024
        /// Discription: this method is get event details from database by admin
        /// </summary>
        /// <param name="GetEventEntity"></param>
        /// <returns>This method is return details of event based on flag</returns>
        #region Get Event
        //this method is get event details from database by admin
        public SerializeResponse<AddEventEntity> GetEvent(GetEventEntity objEntity)
        {
            InsertLog.WriteErrrorLog("EventBL=>GetEvent=>Started=>");
            ConvertDataTable bl = new ConvertDataTable();
            SerializeResponse<AddEventEntity> objResponsemessage = new SerializeResponse<AddEventEntity>();

            DataSet ds = new DataSet();
            SqlDataProvider objSDP = new SqlDataProvider();
            string query = "Sp_GetEvent";

            try
            { 
                string Con_str = Connection.constrEMS;
                SqlParameter prm1 = objSDP.CreateInitializedParameter("@EventID", DbType.Int32, objEntity.EventID);
                SqlParameter prm2 = objSDP.CreateInitializedParameter("@EventCreatedBy", DbType.Int32, objEntity.EventCreatedBy);
                SqlParameter prm3 = objSDP.CreateInitializedParameter("@Flag", DbType.String, objEntity.Flag);

                SqlParameter[] Sqlpara = { prm1, prm2, prm3 };
                ds = SqlHelper.ExecuteDataset(Con_str, query, Sqlpara);

                //give details of unpublish event for update
                if (objEntity.Flag == "GetEvent" && ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objResponsemessage.ArrayOfResponse = bl.ListConvertDataTable<AddEventEntity>(ds.Tables[0]);
                    objResponsemessage.ArrayOfResponse[0].EventStartDate = DateTime.ParseExact(objResponsemessage.ArrayOfResponse[0].EventStartDate, "dd-MM-yyyy HH:mm:ss", null).ToString("yyyy-MM-ddTHH:mm");
                    objResponsemessage.ArrayOfResponse[0].EventEndDate = DateTime.ParseExact(objResponsemessage.ArrayOfResponse[0].EventEndDate, "dd-MM-yyyy HH:mm:ss", null).ToString("yyyy-MM-ddTHH:mm");
                    foreach (var item in objResponsemessage.ArrayOfResponse)
                    {
                        //item.EventImage = item.EventImage.ToString().Split('.')[1];
                        byte[] imageArray = System.IO.File.ReadAllBytes(item.EventImage);
                        string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                        item.EventImage = base64ImageRepresentation;
                    }
                }
                //give details of unpublish and publish event for both user and admin
                else if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objResponsemessage.ArrayOfResponse = bl.ListConvertDataTable<AddEventEntity>(ds.Tables[0]);
                    foreach (var item in objResponsemessage.ArrayOfResponse)
                    {
                        //item.EventImage = item.EventImage.ToString().Split('.')[1];
                        byte[] imageArray = System.IO.File.ReadAllBytes(item.EventImage);
                        string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                        item.EventImage = base64ImageRepresentation;
                    }
                }
               
            }
            catch (Exception ex)
            {
                objResponsemessage.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("EventBL=>GetEvent=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return objResponsemessage;
        }
        #endregion

        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 24/04/2024
        /// Discription: this method is publish or unpublish event's and it's activity by updating value of EventIsActive and IsActive
        /// </summary>
        /// <param name="ActivityEntity"></param>
        /// <returns>This method is return code and message when event and activity is published or unpublished</returns>
        #region Publish and UnpublishEvent
        //this method is publish and unpublish event
        public SerializeResponse<ActivityEntity> PublishUnpublishEvent(ActivityEntity objEntity)
        {
            InsertLog.WriteErrrorLog("EventBL=>PublishUnpublishEvent=>Started");
            ConvertDataTable bl = new ConvertDataTable();
            SerializeResponse<ActivityEntity> objResponsemessage = new SerializeResponse<ActivityEntity>();

            DataSet ds = new DataSet();
            SqlDataProvider objSDP = new SqlDataProvider();
            string query = "Sp_PublishUnpublishEvent";
            try
            {
                string con_Str = Connection.constrEMS;

                SqlParameter prm1 = objSDP.CreateInitializedParameter("@EventCreatedBy", DbType.String, objEntity.EventID);
                SqlParameter prm2 = objSDP.CreateInitializedParameter("@Flag", DbType.String, objEntity.Flag);

                SqlParameter[] sqlPara = { prm1, prm2 };
                ds = SqlHelper.ExecuteDataset(con_Str, query, sqlPara);

                //give code and message when event is published or unpublished
                if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objResponsemessage.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                    objResponsemessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                }
                
            }
            catch (Exception ex)
            {
                objResponsemessage.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("EventBL=>PublishUnpublishEvent=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return objResponsemessage;
        }
        #endregion

        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 25/04/2024
        /// Discription: this method is update event details in database
        /// </summary>
        /// <param name="AddEventEntity"></param>
        /// <returns>This method is return code and message when event is updated </returns>
        #region Update Event
        //this method is update event details in database by admin
        public SerializeResponse<AddEventEntity> UpdateEvent(AddEventEntity objEntity)
        {
            InsertLog.WriteErrrorLog("EventBL=>UpdateEvent=>Started=>");
            ConvertDataTable bl = new ConvertDataTable();
            SerializeResponse<AddEventEntity> objResponsemessage = new SerializeResponse<AddEventEntity>();

            DataSet ds = new DataSet();
            SqlDataProvider objSDP = new SqlDataProvider();
            string query = "Sp_UpdateEvent";
            byte[] imageBytes;
            string imgpath = "";

            try
            {
                if (objEntity.EventImage != null) 
                {
                    //convert Base64 string into image 
                    imageBytes = Convert.FromBase64String(objEntity.EventImage);
                    //create folder to store images in main folder of project
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Images");
                    imgpath = AppDomain.CurrentDomain.BaseDirectory + "Images\\" + DateTime.Now.ToString("HH:mm:ss").Replace(":", "-") + ".png"; 
                    //save image in created folder
                    File.WriteAllBytes(imgpath, imageBytes);
                }
                
                //Make Connection With Database
                string con_Str = Connection.constrEMS;

                SqlParameter prm1 = objSDP.CreateInitializedParameter("@EventID", DbType.Int32, objEntity.EventID);
                SqlParameter prm2 = objSDP.CreateInitializedParameter("@EventName", DbType.String, objEntity.EventName);
                SqlParameter prm3 = objSDP.CreateInitializedParameter("@EventStartDate", DbType.String, objEntity.EventStartDate);
                SqlParameter prm4 = objSDP.CreateInitializedParameter("@EventEndDate", DbType.String, objEntity.EventEndDate);
                SqlParameter prm5 = objSDP.CreateInitializedParameter("@EventDiscription", DbType.String, objEntity.EventDiscription);
                SqlParameter prm6 = objSDP.CreateInitializedParameter("@EventAddress", DbType.String, objEntity.EventAddress);
                SqlParameter prm7 = objSDP.CreateInitializedParameter("@EventImage", DbType.String, imgpath);
                SqlParameter prm8 = objSDP.CreateInitializedParameter("@Flag", DbType.String, objEntity.Flag);
            
                SqlParameter[] sqlPara = { prm1, prm2, prm3, prm4, prm5, prm6, prm7, prm8 };
                //Executing SP and get respose 
                ds = SqlHelper.ExecuteDataset(con_Str, query, sqlPara);
                //This block get code and message when event is inserted
                if (objEntity.Flag == "Update" && ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objResponsemessage.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                    objResponsemessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                }
            }
            catch (Exception ex)
            {
                objResponsemessage.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("EventBL=>UpdateEvent=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return objResponsemessage;
        }
        #endregion

        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 24/04/2024
        /// Discription: this method is delete event's and it's activity by updating value of EventIsDelete and ActivityIsDelete
        /// </summary>
        /// <param name="ActivityEntity"></param>
        /// <returns>This method is return code and message when event and activity is deleted</returns>
        #region Delete Event
        //this method is delete event and it's activity
        public SerializeResponse<ActivityEntity> DeleteEvent(ActivityEntity objEntity)
        {
            InsertLog.WriteErrrorLog("EventBL=>DeleteEvent=>Started");
            ConvertDataTable bl = new ConvertDataTable();
            SerializeResponse<ActivityEntity> objResponsemessage = new SerializeResponse<ActivityEntity>();

            DataSet ds = new DataSet();
            SqlDataProvider objSDP = new SqlDataProvider();
            string query = "Sp_DeleteEvent";
            try
            {
                string con_Str = Connection.constrEMS;

                SqlParameter prm1 = objSDP.CreateInitializedParameter("@EventID", DbType.String, objEntity.EventID);
                SqlParameter prm2 = objSDP.CreateInitializedParameter("@Flag", DbType.String, objEntity.Flag);

                SqlParameter[] sqlPara = { prm1, prm2 };
                ds = SqlHelper.ExecuteDataset(con_Str, query, sqlPara);

                //give code and message when event and is it's activity deleted
                if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objResponsemessage.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["Code"]);
                    objResponsemessage.Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]);
                }

            }
            catch (Exception ex)
            {
                objResponsemessage.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("EventBL=>DeleteEvent=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return objResponsemessage;
        }
        #endregion
    }
}
