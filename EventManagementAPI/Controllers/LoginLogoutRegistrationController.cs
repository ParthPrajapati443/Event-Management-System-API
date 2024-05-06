using BL;
using Entity;
using LIBRARY;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EventManagementAPI.Controllers
{
    
    public class LoginLogoutRegistrationController : ApiController
    {
        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 18/04/2024
        /// Discription: POST API for user or admin login
        /// </summary>
        /// <param name="LoginAndLogoutEntity"></param>
        /// <returns>This api is return code and message with some user detalis if login is successfull </returns>
        #region POST API for login
        //POST API for user or admin login
        [Route("api/Registration/Login")]
        [HttpPost]
        public HttpResponseMessage Login(LoginAndLogoutEntity loginEntity)
        {
            LoginAndLogoutBL login = new LoginAndLogoutBL();
            ResponseMessage response = new ResponseMessage();
            try
            {
                //get 200 if login successfull
                response = login.Login(loginEntity);
            }
            catch (Exception ex)
            {
                response.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("LoginLogoutRegistrationController=>Login=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        #endregion

        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 18/04/2024
        /// Discription: POST API for user or admin login
        /// </summary>
        /// <param name="loginEntity"></param>
        /// <returns>This api  is return code and message with some user detalis if registration is successfull and also get user details </returns>
        #region POST API for User registration
        //POST API for User registration(SignUp) and get data of user
        [Route("api/Registration/SignUp")]
        [HttpPost]
        public HttpResponseMessage SignUp(UserRegistrationEntity userEntity)
        {

            UserRegistrationBL userRegistration = new UserRegistrationBL();
            SerializeResponse<UserRegistrationEntity> response = new SerializeResponse<UserRegistrationEntity>(); ;
            try
            {
                //Here validation for User Phone number.
                if (ModelState.IsValid)
                {
                    response = userRegistration.UserRegistration(userEntity);
                }
                else
                {
                    string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    response.Message = messages;
                }
            }
            catch (Exception ex)
            {
                response.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("LoginLogoutRegistrationController=>SignUp=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        #endregion

        /// <summary>
        /// Author :Parth Prajapti
        /// Date: 18/04/2024
        /// Discription: POST API for user or admin login
        /// </summary>
        /// <param name="loginEntity"></param>
        /// <returns>This api is return code and message </returns>
        #region POST API for logout
        //POST API for logout user and admin
        [Route("api/Registration/Logout")]
        [HttpPost]
        public HttpResponseMessage Logout(LoginAndLogoutEntity loginEntity)
        {
            LoginAndLogoutBL logout = new LoginAndLogoutBL();
            ResponseMessage response = new ResponseMessage();
            try
            {
                //get 200 as code and message if logout successfull
                response = logout.Logout(loginEntity);
            }
            catch (Exception ex)
            {
                response.Message = "500|Exception Occurred";
                InsertLog.WriteErrrorLog("LoginLogoutRegistrationController=>Logout=>Exception=>" + ex.Message + ex.StackTrace);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        } 
        #endregion
    }
}
