﻿using AdabFest_API.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using WebAPICode.Helpers;
using static QRCoder.PayloadGenerator;
using static QRCoder.PayloadGenerator.SwissQrCode;
using ZXing;

namespace AdabFest_API.Repositories
{
    public class LoginRepository
    {
        public static DataTable _dt;
        public static DataSet _ds;
        
        public LoginRepository()
           : base()
        {
            
            _dt = new DataTable();
            _ds = new DataSet();
        }
        public async Task<int> GetCustomerInfo(string UserName, string ContactNo)
        {
            int result = 0;
             
            try
            {
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@UserName", UserName);
                p[1] = new SqlParameter("@ContactNo", ContactNo);
                 
                result = Convert.ToInt32(new DBHelper().GetTableFromSP("sp_Insert_Admin", p).Rows[0]["UserID"]);

               
            }
            catch (Exception ex)
            {               
                return 0;
            }
            return result;
        }

        public async Task<RspLoginAdmin> LoginWithPasscode(string passcode)
        {
            var RspLoginAdmin = new RspLoginAdmin();
            var repo = new LoginBLL();
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@passcode", passcode);

                _dt = await (new DBHelper().GetTableFromSPAsync)("sp_AuthenticateWithPasscode_admin", p);

                if (_dt.Rows.Count > 0)
                {
                    repo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<LoginBLL>>().FirstOrDefault();

                    RspLoginAdmin rspLoginAdmin = new RspLoginAdmin()
                    {
                        description = "Login Success.",
                        status = 200,
                        loginAdmin = repo
                    };
                    return rspLoginAdmin;
                }
                else
                {
                    RspLoginAdmin rspLoginAdmin = new RspLoginAdmin()
                    {
                        description = "Incorrect Passcode.",
                        status = 0,
                        loginAdmin = null
                    };
                    return rspLoginAdmin;
                }
            }
            catch (Exception ex)
            {
                RspLoginAdmin rspLoginAdmin = new RspLoginAdmin()
                {
                    description = "Incorrect Passcode.",
                    status = 0,
                    loginAdmin = null
                };
                return rspLoginAdmin;
            }
        }

        //public async Task<RspLogin> loginCustomerSM(string username, string password, string type, string fullname)
        //{
        //    var rspLogin = new RspLogin();
        //    var repo = new UserBLL();
        //    try
        //    {
        //        if (type == "sm")
        //        {
        //            SqlParameter[] pp = new SqlParameter[1];
        //            pp[0] = new SqlParameter("@Email", username);
        //            _dt = (new DBHelper().GetTableFromSP)("sp_CheckCustomerByEmail_API", pp);

        //            if (_dt.Rows.Count == 0)
        //            {
        //                SqlParameter[] p1 = new SqlParameter[5];
        //                p1[0] = new SqlParameter("@UserName", username);
        //                p1[1] = new SqlParameter("@Email", username);
        //                p1[2] = new SqlParameter("@Password", "social");
        //                p1[3] = new SqlParameter("@StatusID", 1);
        //                p1[4] = new SqlParameter("@CreatedDate", DateTime.UtcNow.AddMinutes(300));

        //                _dt = await (new DBHelper().GetTableFromSPAsync)("sp_CustomerSignupSM_API", p1);

        //                repo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<UserBLL>>().FirstOrDefault();

        //                rspLogin = new RspLogin()
        //                {
        //                    description = "Login Success.",
        //                    status = 200,
        //                    login = repo
        //                };
        //                return rspLogin;
        //            }
        //            else
        //            {
                        
        //                int userid = repo.UserID = Convert.ToInt32(_dt.Rows[0][0]);
        //                rspLogin = new RspLogin()
        //                {
        //                    description = "Login Success.",
        //                    status =200,
        //                    login = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<UserBLL>>().FirstOrDefault(),
                            
        //                };
        //                return rspLogin;
        //            }
        //        }
        //        return rspLogin;
        //    }

        //    catch (Exception ex)
        //    {
        //        rspLogin = new RspLogin()
        //        {
        //            description = "Incorrect Email or Password.",
        //            status = 0,
        //            login = null
        //        };
        //        return rspLogin;
        //    }
        //}
        public async Task<int> AttendeeRegister(AttendeeRegsiterBLL obj)
        {
            int result = 0;
            try
            {
                SqlParameter[] p1 = new SqlParameter[1];
                p1[0] = new SqlParameter("@Email", obj.Email);
                _dt = await (new DBHelper().GetTableFromSPAsync)("sp_CheckUser_API", p1);
                if (_dt.Rows.Count == 0)
                {
                    SqlParameter[] p = new SqlParameter[6];
                    p[0] = new SqlParameter("@Email", obj.Email);
                    p[1] = new SqlParameter("@UserName", obj.FullName);
                    p[2] = new SqlParameter("@Phone", obj.PhoneNo);
                    p[3] = new SqlParameter("@StatusID", 101);
                    p[4] = new SqlParameter("@CreatedDate", DateTime.UtcNow.AddMinutes(300));
                    p[5] = new SqlParameter("@UpdatedBy", 1);
                    result = (new DBHelper().ExecuteNonQueryReturn)("sp_RegisterAttendees_API", p);
                    //result = Convert.ToInt32(new DBHelper().GetTableFromSP("sp_RegisterAttendees_API", p).Rows[0]["UserID"]);
                }
                else
                {
                    return 0;
                }
                 
            }
            catch (Exception ex)
            {
                return 0;
            }
            return result;
        }
        public async Task<int> CustomerSignup(UserBLL user)
        {
            int result = 0;
            try
            {
                SqlParameter[] p1 = new SqlParameter[1];
                p1[0] = new SqlParameter("@Email", user.Email);
                _dt = await (new DBHelper().GetTableFromSPAsync)("sp_CheckUser_API", p1);
                if (_dt.Rows.Count == 0)
                {
                    SqlParameter[] p = new SqlParameter[9];
                    p[0] = new SqlParameter("@UserName", user.FullName);
                    p[1] = new SqlParameter("@Email", user.Email);
                    p[2] = new SqlParameter("@Image", user.Image);
                    p[3] = new SqlParameter("@Address", user.Address);
                    p[4] = new SqlParameter("@ContactNo", user.PhoneNo);
                    p[5] = new SqlParameter("@Password", user.Password);
                    p[6] = new SqlParameter("@StatusID", 1);
                    p[7] = new SqlParameter("@CreatedDate", DateTime.UtcNow.AddMinutes(300));
                    p[8] = new SqlParameter("@UpdatedBy", 1);


                    result = Convert.ToInt32(new DBHelper().GetTableFromSP("sp_CustomerSignup_API", p).Rows[0]["UserID"]);
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
            return result;
        }
        public async Task<int> CustomerEdit(UserBLL user)
        {
            int result = 0;
            try
            {
                SqlParameter[] p = new SqlParameter[5];
                p[0] = new SqlParameter("@FullName", user.FullName);                
                p[1] = new SqlParameter("@ContactNo", user.PhoneNo);
               
                p[2] = new SqlParameter("@Updatedon", DateTime.UtcNow.AddMinutes(300));
                
                p[3] = new SqlParameter("@UserID", user.AttendeesID);
                p[4] = new SqlParameter("@Password", user.Password);


                //result = Convert.ToInt32(new DBHelper().GetTableFromSP("sp_CustomerEdit_API", p).Rows[0]["UserID"]);
                result = await (new DBHelper().ExecuteNonQueryReturnAsync)("sp_CustomerEdit_API", p);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return result;
        }
        public async Task<int> InsertCustomerToken(PushTokenBLL token)
        {
            int result = 0;
            try
            {
                var userID = Convert.ToInt32(token.UserID);
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@UserID", userID);
                _dt = (new DBHelper().GetTableFromSP)("sp_CheckTokenByUserID_API", p);
                if (_dt.Rows.Count != 0)
                {
                    return 0;
                }
                else
                {
                    SqlParameter[] p1 = new SqlParameter[4];
                    p1[0] = new SqlParameter("@Token", token.Token);
                    p1[1] = new SqlParameter("@UserID", token.UserID);
                    p1[2] = new SqlParameter("@StatusID", 1);
                    p1[3] = new SqlParameter("@Device", token.Device);

                    //result = await (new DBHelper().ExecuteNonQueryReturnAsync)("sp_CustomerSignup_API", p);
                    result = Convert.ToInt32(new DBHelper().GetTableFromSP("sp_InsertToken_API", p1).Rows[0]["TokenID"]);
                    //result = await int.Parse(new DBHelper().GetTableFromSPAsync("sp_CustomerSignup_API", p).Rows[0]["ID"].ToString());
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
            return result;
        }

        public RspCustomerLogin GetUserbyID(int UserID)
        {
            var user = new UserBLL();
            var rsp = new RspCustomerLogin();
            DataSet _ds = new DataSet();
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@UserID", UserID);
                _ds = (new DBHelper().GetDatasetFromSP)("sp_GetUserByID_API", p);

                if (_ds != null)
                {
                    if (_ds.Tables.Count > 0)
                    {
                        if (_ds.Tables[0].Rows.Count > 0)
                        {
                            user = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[0])).ToObject<List<UserBLL>>().FirstOrDefault();
                        }
                        else user = null;

                    }
                }
                if (user == null)
                {
                    rsp.user = null;
                    rsp.Status = "0";
                    rsp.Description = "Incorrect email or password";
                }
                else
                {

                    rsp.user = user;
                    rsp.Status = "200";
                    rsp.Description = "Success";

                }
            }
            catch (Exception ex)
            {
                rsp.Status = "0";
                rsp.Description = "Failed to load data.";
            }
            return rsp;
        }
        public RspQR GetDataQR(int UserID)
        {
            var user = new QRBLL();
            var rsp = new RspQR();
            DataSet _ds = new DataSet();
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@UserID", UserID);
                _ds = (new DBHelper().GetDatasetFromSP)("sp_QRDataByUserID_API", p);

                if (_ds != null)
                {
                    if (_ds.Tables.Count > 0)
                    {
                        if (_ds.Tables[0].Rows.Count > 0)
                        {
                            user = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[0])).ToObject<List<QRBLL>>().FirstOrDefault();

                            DateTime startDate = user.FromDate;
                            DateTime endDate = user.ToDate;
                            var a = startDate.ToString("MMM-dd-yyyy");
                            var b = endDate.ToString("MMM-dd-yyyy");
                            user.FinalDate = a + " " + "-" + " " + b;
                        }
                        else user = null;

                    }
                }
                if (user == null)
                {
                    rsp.qr = null;
                    rsp.Status = "0";
                    rsp.Description = "User Not Found.";
                }
                else
                {

                    rsp.qr = user;
                    rsp.Status = "200";
                    rsp.Description = "Success";

                }
            }
            catch (Exception ex)
            {
                rsp.Status = "0";
                rsp.Description = "Failed to load data.";
            }
            return rsp;
        }
        public RspForgetPwd ForgetPassword(string email)
        {
            var bll = new UserBLL();
            var rsp = new RspForgetPwd();
            try
            {
                var data = GetUserFromEmail(email);
                if (data.AttendeesID != 0)
                {
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    int passwordLength = 8;

                    Random random = new Random();

                    // Generate the first part of the password without the digit
                    string password = new string(Enumerable.Repeat(chars, passwordLength - 1)
                                                          .Select(s => s[random.Next(s.Length)]).ToArray());

                    // Insert a random digit at a random position in the password
                    int positionToInsertDigit = random.Next(password.Length);
                    char digit = chars.Substring(52, 10)[random.Next(10)]; // Selecting from the digit part of 'chars'
                    password = password.Insert(positionToInsertDigit, digit.ToString());

                    int result = 0;
                    try
                    {
                        SqlParameter[] p = new SqlParameter[3];
                        p[0] = new SqlParameter("@Password", password);
                        p[1] = new SqlParameter("@Email", data.Email);
                        p[2] = new SqlParameter("@Updatedon", DateTime.UtcNow.AddMinutes(300));

                        result = (new DBHelper().ExecuteNonQueryReturn)("sp_EditPassword_API", p);
                        
                        rsp.Status = "1";
                        rsp.Email = email;
                        rsp.Password = password;
                    }
                    catch (Exception ex)
                    {
                        return rsp;
                    }
                }
                else
                {
                    rsp.Status = "0";
                    rsp.Email = "";
                    rsp.Password = "";
                }
                return rsp;
            }
            catch (Exception ex)
            {
                rsp.Status = "0";
                rsp.Password = "Not Found";
                return rsp;
            }
        }
        public UserBLL GetUserFromEmail(string email)
        {
            try
            {
                var lst = new UserBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@Email", email);
                _dt = (new DBHelper().GetTableFromSP)("sp_GetUserByEmail_API", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<UserBLL>>().FirstOrDefault();
                    }
                }

                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }

       
    }


}
