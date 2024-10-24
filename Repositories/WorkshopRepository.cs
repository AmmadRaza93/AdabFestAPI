﻿using AdabFest_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using WebAPICode.Helpers;

namespace AdabFest_API.Repositories
{
    public class WorkshopRepository
    {
        public static DataTable _dt;
        public static DataSet _ds;

        public WorkshopRepository()
           : base()
        {

            _dt = new DataTable();
            _ds = new DataSet();
        }
        public async Task<RspWorkshop> GetAllWorkshop()
        {
            var RspWorkshop = new RspWorkshop();
            var repo = new List<WorkshopBLL>();
            try
            {
                SqlParameter[] p = new SqlParameter[0];
                
                _dt = await (new DBHelper().GetTableFromSPAsync)("sp_GetAllWorkshop_API", p);

                if (_dt.Rows.Count > 0)
                {
                    repo = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<WorkshopBLL>>().ToList();

                    foreach (var item in repo)
                    {

                        DateTime startDate = item.Date;
                        
                        var a = startDate.ToString("dd-MM-yyyy");
                        
                        item.FinalDate = a ;
                    }


                    foreach (var item in repo)
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
                    RspWorkshop rspWorkshop = new RspWorkshop()
                    {
                        description = "Success.",
                        status = 200,
                        workshop = repo
                    };
                    return rspWorkshop;
                }
                else
                {
                    RspWorkshop rspWorkshop = new RspWorkshop()
                    {
                        description = "Programs not found.",
                        status = 0,
                        workshop = null
                    };
                    return rspWorkshop;
                }
            }
            catch (Exception ex)
            {
                RspWorkshop rspWorkshop = new RspWorkshop()
                {
                    description = "Something went wrong.",
                    status = 0,
                    workshop = null
                };
                return rspWorkshop;
            }
        }
    }
    
    
}
