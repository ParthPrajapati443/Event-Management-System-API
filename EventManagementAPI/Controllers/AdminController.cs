using BL;
using Entity;
using LIBRARY;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EventManagementAPI.Controllers
{
    public class AdminController : ApiController
    {
        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 18/04/2024
        /// Discription: POST API for user or admin login
        /// </summary>
        /// <param name="loginEntity"></param>
        /// <returns>This api is return code and message </returns>
        #region POST API for Add Event 
        //POST API for add event in database for admin
        [Route("api/Admin/AddEvent")]
        [HttpPost]
        public HttpResponseMessage AddEvent(AddEventEntity addEventEntity)
        {
            EventBL addEvent = new EventBL();
            SerializeResponse<AddEventEntity> response = new SerializeResponse<AddEventEntity>();
            try
            {
                //get 200 if event successfull
                response = addEvent.AddEvent(addEventEntity);
            }
            catch (Exception ex)
            {
                response.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("AdminController=>AddEvent=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        #endregion

        #region POST API for Get Event
        //POST API for get event details from database for admin and user
        [Route("api/Admin/GetEvent")]
        [HttpPost]
        public HttpResponseMessage GetEvent(GetEventEntity getEventEntity)
        {
            EventBL getEvent = new EventBL();
            SerializeResponse<AddEventEntity> response = new SerializeResponse<AddEventEntity>();
            try
            {
                //get 200 if event successfull
                response = getEvent.GetEvent(getEventEntity);
            }
            catch (Exception ex)
            {
                response.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("AdminController=>GetEvent=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        #endregion

        #region POST API for Update Event 
        //POST API for update event in database for admin
        [Route("api/Admin/UpdateEvent")]
        [HttpPost]
        public HttpResponseMessage UpdateEvent(AddEventEntity addEventEntity)
        {
            EventBL updateEvent = new EventBL();
            SerializeResponse<AddEventEntity> response = new SerializeResponse<AddEventEntity>();
            try
            {
                //get 200 if event successfull
                response = updateEvent.UpdateEvent(addEventEntity);
            }
            catch (Exception ex)
            {
                response.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("AdminController=>UpdateEvent=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        #endregion

        #region POST API for Delete Event 
        //POST API for publish and unpublish event and it's activity
        [Route("api/Admin/DeleteEvent")]
        [HttpPost]
        public HttpResponseMessage DeleteEvent(ActivityEntity objEntity)
        {
            EventBL deleteEvent = new EventBL();
            SerializeResponse<ActivityEntity> response = new SerializeResponse<ActivityEntity>();
            try
            {
                response = deleteEvent.DeleteEvent(objEntity);
            }
            catch (Exception ex)
            {
                response.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("AdminController=>DeleteEvent=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        #endregion

        #region POST API for Add and Get Activity 
        //POST API for add activity details in database for admin and get activity details for user and admin
        [Route("api/Admin/Activity")]
        [HttpPost]
        public HttpResponseMessage Activity(ActivityEntity ActivityEntity)
        {
            ActivityBL activity = new ActivityBL();
            SerializeResponse<ActivityEntity> response = new SerializeResponse<ActivityEntity>();
            try
            {
                //get 200 if activity successfull
                response = activity.Activity(ActivityEntity);
            }
            catch (Exception ex)
            {
                response.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("AdminController=>Activity=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        #endregion

        #region POST API for Publish or Unpublish Event 
        //POST API for publish and unpublish event and it's activity
        [Route("api/Admin/PublishUnpublishEvent")]
        [HttpPost]
        public HttpResponseMessage PublishUnpublishEvent(ActivityEntity objEntity)
        {
            EventBL publishUnpublishEvent = new EventBL();
            SerializeResponse<ActivityEntity> response = new SerializeResponse<ActivityEntity>();
            try
            {
                response = publishUnpublishEvent.PublishUnpublishEvent(objEntity);
            }
            catch (Exception ex)
            {
                response.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("AdminController=>PublishUnpublishEvent=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        #endregion
    }
}
