using AdabFest_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using WebAPICode.Helpers;

namespace AdabFest_API.Repositories
{
    public class SettingRepository
    {
        public static DataTable _dt;
        public static DataSet _ds;

        public SettingRepository()
           : base()
        {

            _dt = new DataTable();
            _ds = new DataSet();
        }
        public async Task<RspSetting> GetAllSettings()
        {
            var RspSetting = new RspSetting();
            var repo = new SettingBLL();
            List<FaqBLL> lstFaq = new List<FaqBLL>();
            List<PopupBLL> lstPopup = new List<PopupBLL>();
            ChairBLL chair = new ChairBLL();
            ConferenceChairBLL chairConf = new ConferenceChairBLL();
            try
            {
                SqlParameter[] p = new SqlParameter[0];

                _ds = await (new DBHelper().GetDatasetFromSPAsync)("sp_GetAllSettings_admin", p);

                if (_ds != null)
                {
                    if (_ds.Tables.Count > 0)
                    {
                        if (_ds.Tables[0] != null)
                        {
                            repo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[0])).ToObject<List<SettingBLL>>().FirstOrDefault();

                            if (repo.SplashScreen != null && repo.SplashScreen != "")
                            {
                                repo.SplashScreen = "http://adabfest-001-site2.gtempurl.com/" + repo.SplashScreen;
                            }
                            else
                            {
                                repo.SplashScreen = "";

                            }
                            if (repo.PdfUrl != null && repo.PdfUrl != "")
                            {
                                string stringToReplace = "/ClientApp/dist/";
                                string modifiedPath = repo.PdfUrl.Replace(stringToReplace, "");

                                repo.PdfUrl = "http://adabfest-001-site2.gtempurl.com/" + modifiedPath;
                            }
                            else
                            {
                                repo.PdfUrl = "";

                            }


                        }
                        if (_ds.Tables[1] != null)
                        {
                            lstFaq = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[1])).ToObject<List<FaqBLL>>().ToList();
                        }
                        if (_ds.Tables[2] != null)
                        {
                            chair = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[2])).ToObject<List<ChairBLL>>().FirstOrDefault();
                            if (chair.ImgChair != null && chair.ImgChair != "")
                            {
                                chair.ImgChair = "http://adabfest-001-site2.gtempurl.com/" + chair.ImgChair;
                            }
                            else
                            {
                                chair.ImgChair = "";
                            }
                        }
                        if (_ds.Tables[3] != null)
                        {
                            chairConf = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[3])).ToObject<List<ConferenceChairBLL>>().FirstOrDefault();
                            if (chairConf.ImgConChair != null && chairConf.ImgConChair != "")
                            {
                                chairConf.ImgConChair = "http://adabfest-001-site2.gtempurl.com/" + chairConf.ImgConChair;
                            }
                            else
                            {
                                chairConf.ImgConChair = "";
                            }
                        }
                        if (_ds.Tables[4] != null)
                        {
                            lstPopup = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[4])).ToObject<List<PopupBLL>>().ToList();

                            foreach (var item in lstPopup)
                            {
                                if (item.Image != null && item.Image != "")
                                {
                                    item.Image = "http://adabfest-001-site2.gtempurl.com/" + item.Image;
                                }
                                else
                                {
                                    item.Image = "";
                                }
                            }


                        }
                        repo.faqs = lstFaq;
                        repo.popup = lstPopup;
                        repo.Chair = chair;
                        repo.ChairConference = chairConf;
                    }


                    RspSetting rspSetting = new RspSetting()
                    {
                        description = "Success.",
                        status = 200,
                        setting = repo
                    };
                    return rspSetting;
                }
                else
                {
                    RspSetting rspSetting = new RspSetting()
                    {
                        description = "Setting not found.",
                        status = 0,
                        setting = null
                    };
                    return rspSetting;
                }
            }
            catch (Exception ex)
            {
                RspSetting rspSetting = new RspSetting()
                {
                    description = "Something went wrong.",
                    status = 0,
                    setting = null
                };
                return rspSetting;
            }
        }
    }


}
