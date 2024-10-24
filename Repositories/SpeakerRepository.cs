using AdabFest_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using WebAPICode.Helpers;

namespace AdabFest_API.Repositories
{
    public class SpeakerRepository
    {
        public static DataTable _dt;
        public static DataSet _ds;

        public SpeakerRepository()
           : base()
        {

            _dt = new DataTable();
            _ds = new DataSet();
        }
        public async Task<RspSpeaker> GetAllSpeakers()
        {
            var RspSpeaker = new RspSpeaker();
            var lstSpeakers = new List<SpeakerBLL>();
            var lstEvent = new List<EventBLL>();
            try
            {
                var ds = await GetSpeakers();
                var _dsSpeaker = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<SpeakerBLL>>();
                var _dsEvent = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<EventBLL>>();

                foreach (var i in _dsSpeaker)
                {
                    lstEvent = new List<EventBLL>();
                    foreach (var j in _dsEvent.Where(k => k.SpeakerID == i.SpeakerID))
                    {
                        lstEvent.Add(new EventBLL
                        {
                            EventID = j.EventID,
                            StatusID = j.StatusID,
                            Name = j.Name,
                            Type = j.Type,
                            Description = j.Description,
                            FromDate = Convert.ToDateTime(j.FromDate).ToString("dd-MM-yyyy"),
                            Image = "http://adabfest-001-site2.gtempurl.com/" + j.Image,
                            ToDate = Convert.ToDateTime(j.ToDate).ToString("dd-MM-yyyy"),
                            EventDate = Convert.ToDateTime(j.EventDate).ToString("dd-MM-yyyy"),
                            EventTime = j.EventTime,
                            EventCity = j.EventCity,
                            LocationLink = j.LocationLink,
                            Location = j.Location,
                            EventEndTime = j.EventEndTime,
                            PhoneNo = j.PhoneNo,
                            Email = j.Email,
                            RemainingTicket = j.RemainingTicket,
                            Facebook = j.Facebook,
                            Instagram = j.Instagram,
                            IsFeatured = j.IsFeatured,
                            Createdon = j.Createdon,
                            Updatedon = j.Updatedon,
                            EventLink = j.EventLink 
                            
                        });
                    }

                    lstSpeakers.Add(new SpeakerBLL
                    {
                        SpeakerID = i.SpeakerID,
                        Name = i.Name,
                        Designation = i.Designation,
                        Company = i.Company,
                        About = i.About,
                        Image = "http://adabfest-001-site2.gtempurl.com/" + i.Image,
                        Createdon = i.Createdon,
                        Events = lstEvent
                    });
                }

                RspSpeaker.speaker = lstSpeakers;
                RspSpeaker.status = 1;
                RspSpeaker.description = "Success";

                return RspSpeaker;
            }
            catch (Exception ex)
            {
                RspSpeaker rspSpeaker = new RspSpeaker()
                {
                    description = "Something went wrong.",
                    status = 0,
                    speaker = null
                };
                return rspSpeaker;
            }
        }
        public async Task<DataSet> GetSpeakers()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] p = new SqlParameter[0];

                ds = await (new DBHelper().GetDatasetFromSPAsync)("sp_GetAllSpeaker_API", p);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    
    
}
