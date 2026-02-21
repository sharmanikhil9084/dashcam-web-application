using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using trackandrive.dashcam.ViewModel;
using TrackanDrive.Dashcam.CommonFunctions;
using TrackanDrive.Dashcam.Interfaces;
using TrackanDrive.Dashcam.Models;
using TrackanDrive.Web.Interfaces;

namespace trackandrive.dashcam.Controllers
{

    public class HomeController : Controller
    {

        private readonly IHome _Ihome;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly TrackanDriveDbContext _trackgeeDbContext;
        private readonly IDapper _Idapper;
        public HomeController(IHome ihome, TrackanDriveDbContext trackgeeDbContext, IHttpContextAccessor iHttpContextAccessor,IDapper idapper)
        {
            _Ihome = ihome;
            _trackgeeDbContext = trackgeeDbContext;
            _IHttpContextAccessor = iHttpContextAccessor;
            _Idapper = idapper;
        }

        public bool IsValid()
        {
            try
            {
                bool flag = true;
                var httpContext = _IHttpContextAccessor.HttpContext;
                string cookieValue = CommonUtility.DecodeFrom64(httpContext.Request.Cookies["dsh"]);
                if (string.IsNullOrEmpty(cookieValue))
                    flag = false;
                return flag;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        [Route("live-stream")]
        public async Task<IActionResult> LiveStream()
        {

            try
            {

                //if (!IsValid())
                //{
                //    return Redirect("http://pgsql.web.trackanddrive.com/");
                //}
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
            return View();
        }

        [Route("history")]
        public async Task<IActionResult> History()
        {
            try
            {

                //if (!IsValid())
                //{
                //    return Redirect("http://pgsql.web.trackanddrive.com/");
                //}
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
            return View();
        }


        [Route("dashcam-files")]
        public async Task<IActionResult> DashcamFiles()
        {

            try
            {

                //if (!IsValid())
                //{
                //    return Redirect("http://pgsql.web.trackanddrive.com/");
                //}
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
            return View();
        }

        [Route("GetDashcamDeviceList")]
        public async Task<IActionResult> GetDashcamDeviceList(string? deviceImei)
        {
            List<DashcamDevices> dashcamDevices=new List<DashcamDevices>();
            try
            {
                //if (!IsValid())
                //{
                //    return Redirect("http://pgsql.web.trackanddrive.com/");
                //}
                dashcamDevices = await _Ihome.GetDashcamDevicesList(deviceImei);
                return Ok(dashcamDevices);
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
           
        }

      

        [HttpPost("CaptureImage")]
        public async Task<IActionResult> CaptureImage(string deviceImei, bool channel_1, bool channel_2)
        {

            //if (!IsValid())
            //{
            //    return Redirect("http://pgsql.web.trackanddrive.com/");
            //}
            try
            {

                string command = "Picture,";
                if (channel_2 && channel_2)
                    command += "Inout";
                else if (channel_1)
                    command += "in";
                else if (channel_2)
                    command += "out";


                if (!string.IsNullOrEmpty(deviceImei))
                {
                    var content = new Dictionary<string, string>
                    {
                        {"Imei",deviceImei},
                        {"serverFlagId","1"},
                        {"proNo","128"},
                        { "platform","web"},
                        {"requestId","6"},
                        {"cmdType","normallns"},
                        {"token","a12341234123"},
                        {"cmdContent",command},
                    };

                    var result = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", content);
                    SendCommandResponseModel sendCommandResponseModel = new SendCommandResponseModel();
                    sendCommandResponseModel = JsonConvert.DeserializeObject<SendCommandResponseModel>(result);
                    return Ok(sendCommandResponseModel.data.Msg);
                }
                return Ok("");
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
        }

        [HttpPost("CapturedVideo")]
        public async Task<IActionResult> CapturedVideo(string deviceImei, string seconds, string channel)
        {

            //if (!IsValid())
            //{
            //    return Redirect("http://pgsql.web.trackanddrive.com/");
            //}
            try
            {
                string command = string.Empty;
                if (channel == "1")
                {
                    command = "video,in," + seconds + "s";
                }
                else if (channel == "2")
                {
                    command = "video,out," + seconds + "s";
                }




                if (!string.IsNullOrEmpty(deviceImei))
                {
                    var content = new Dictionary<string, string>
                    {
                        {"Imei",deviceImei},
                        {"serverFlagId","1"},
                        {"proNo","128"},
                        { "platform","web"},
                        {"requestId","6"},
                        {"cmdType","normallns"},
                        {"token","a12341234123"},
                        {"cmdContent",command},
                    };

                    var result = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", content);
                    SendCommandResponseModel sendCommandResponseModel = new SendCommandResponseModel();
                    sendCommandResponseModel = JsonConvert.DeserializeObject<SendCommandResponseModel>(result);
                    return Ok(sendCommandResponseModel.data.Msg);

                }
                return Ok("");
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
        }

        [Route("GetDashcamHistoryVideo")]
        public async Task<IActionResult> GetDashcamHistoryVideo(string deviceImei, string date, string channel, string fromTime, string toTime)
        {
            try
            {
                SendCommandResponseModel sendCommandResponseModel = new SendCommandResponseModel();
                if (!string.IsNullOrEmpty(deviceImei))
                {
                    
                    DeviceModel deviceModel = new DeviceModel();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("p_deviceimei", deviceImei);
                    deviceModel = await _Idapper.Get<DeviceModel>("select * from usp_getdevicemodel_web(@p_deviceimei)", dynamicParameters);

                    if (deviceModel.model!=null)
                    {
                        var session = _IHttpContextAccessor.HttpContext.Session;

                        if (deviceModel.model == "JC371")
                        {

                            string fromDateString = date + " " + fromTime;
                            string toDateString = date + " " + toTime;

                            DateTime fromDate = DateTime.ParseExact(fromDateString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                            DateTime toDate = DateTime.ParseExact(toDateString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                            string beginTime = fromDate.ToString("yyMMddHHmmss");
                            string endTime = toDate.ToString("yyMMddHHmmss");

                            string command = @"{
                                  ""channel"": " + Convert.ToInt32(channel) + @",
                                  ""beginTime"": " + beginTime + @",
                                  ""endTime"": " + endTime + @",
                                  ""alarmFlag"": 0,
                                  ""resourceType"": 0,
                                  ""codeType"": 0,
                                  ""storageType"": 0,
                                  ""instructionID"": ""123456789""
                                  }";
                            Console.WriteLine(command);

                            var formData = new Dictionary<string, string>
                             {
                               { "Imei", deviceImei },
                               { "serverFlagId", "0" },
                               { "proNo", "37381" },
                               { "platform", "web" },
                               { "requestId", "6" },
                               { "cmdType", "normallns" },
                               { "token", "123" },
                               { "cmdContent", command },
                               { "offlineFlag", "false"}
                              };
                            var response = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", formData);
                            sendCommandResponseModel = JsonConvert.DeserializeObject<SendCommandResponseModel>(response);
                            if (sendCommandResponseModel.data.Msg == "Device not online") return Ok(sendCommandResponseModel.data.Msg);
                            session.SetString("Model", "JC371");
                            session.SetString("channel", channel);
                            session.SetString("deviceImei", deviceImei);
                        }
                        else
                        {
                            var content = new Dictionary<string, string>
                            {
                                {"Imei",deviceImei},
                                {"cmdContent","FILELIST,http://dashcam.trackandrive.com/api/DashcamHistory/PushDashcamFileList"},
                                {"serverFlagId","0"},
                                {"proNo","128"},
                                {"platform","web"},
                                {"requestId","6"},
                                {"cmdType","normallns"},
                                {"token","a12341234123"},
                                {"offlineFlag","false"},
                            };

                            var result = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", content);
                            sendCommandResponseModel = JsonConvert.DeserializeObject<SendCommandResponseModel>(result);
                            if (sendCommandResponseModel.data.Msg == "Device not online") return Ok(sendCommandResponseModel.data.Msg);
                            content = new Dictionary<string, string>
                            {
                                {"Imei",deviceImei},
                                {"cmdContent","FILELIST"},
                                {"serverFlagId","0"},
                                {"proNo","128"},
                                {"platform","web"},
                                {"requestId","6"},
                                {"cmdType","normallns"},
                                {"token","a12341234123"},
                                {"offlineFlag","false"},
                            };

                            result = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", content);
                            sendCommandResponseModel = JsonConvert.DeserializeObject<SendCommandResponseModel>(result);
                            if (sendCommandResponseModel.data.Msg == "Device not online") return Ok(sendCommandResponseModel.data.Msg);

                            // Set session values
                            session.SetString("deviceImei", deviceImei);
                            session.SetString("date", date);
                            session.SetString("channel", channel);
                            session.SetString("fromTime", fromTime);
                            session.SetString("toTime", toTime);
                            session.SetString("Model", "NON-JC371");
                            
                        }

                    }
                }
                return Ok(sendCommandResponseModel.data.Msg); 
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
        }
        [Route("UploadDashcamHistoryVideos")]
        public async Task<IActionResult> UploadDashcamHistoryVideos()
        {

            try
            {

                //if (!IsValid())
                //{
                //    return Redirect("http://pgsql.web.trackanddrive.com/");
                //}

                var session = _IHttpContextAccessor.HttpContext.Session;

                // Retrieve session values
                string deviceImei = session.GetString("deviceImei");
                string date = Convert.ToDateTime(session.GetString("date")).ToString("dd-MM-yyyy");
                int channel = Convert.ToInt32(session.GetString("channel"));
                string fromTime = session.GetString("fromTime");
                string toTime = session.GetString("toTime");

                if (!string.IsNullOrEmpty(deviceImei) &&
                   !string.IsNullOrEmpty(date) &&
                   !string.IsNullOrEmpty(fromTime) &&
                   !string.IsNullOrEmpty(toTime))
                {

                    DateTime fromDate = DateTime.ParseExact(date + " " + fromTime, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                    DateTime toDate = DateTime.ParseExact(date + " " + toTime, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);

                    var historyFiles = new List<HistoryFiles>();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("p_deviceimei", deviceImei);
                    dynamicParameters.Add("p_fromtime", fromDate);
                    dynamicParameters.Add("p_totime", toDate);
                    dynamicParameters.Add("p_channel", channel);
                    historyFiles = await _Idapper.GetAll<HistoryFiles>("SELECT * FROM usp_GetDashcamHistoryFiles_Web(@p_deviceimei,@p_fromtime,@p_totime,@p_channel)", dynamicParameters);

                    if (historyFiles.Any())
                    {
                        foreach (var item in historyFiles)
                        {
                            var content = new Dictionary<string, string>
                             {
                                { "Imei", deviceImei},
                                { "serverFlagId", "0" },
                                { "proNo", "128" },
                                { "platform", "web" },
                                { "requestId", "6" },
                                { "cmdType", "normallns" },
                                { "token", "a12341234123" },
                                { "cmdContent", "HVIDEO,"+item.filename+","+channel+"" },
                                { "offlineFlag", "true"}
                             };
                            var result = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", content);
                            await Task.Delay(5000);
                        }
                        DeleteDashcamFiles(deviceImei);
                    }
                    else
                    {
                        DeleteDashcamFiles(deviceImei);
                        return Ok("Data not found");
                    }
                   
                }

                return Ok("");
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
        }
        public async void DeleteDashcamFiles(string deviceImei)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_deviceimei", deviceImei);
            await _Idapper.Get<EmptyClass>("select * from usp_DeleteHistoryFiles_Web(@p_deviceimei)", dynamicParameters);
        }

        public async void DeleteJC371HistoryFiles(string deviceImei)
        {

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_deviceimei", deviceImei);
            await _Idapper.Get<EmptyClass>("select * from usp_DeleteHistoryFiles_Web(@p_deviceimei)", dynamicParameters);

        }

        [Route("StreamingDashcamHistory")]
        public async Task<IActionResult> StreamingDashcamHistory()
        {
            //if (!IsValid())
            //{
            //    return Redirect("http://pgsql.web.trackanddrive.com/");
            //}


            try
            {

                var session = _IHttpContextAccessor.HttpContext.Session;

                // Retrieve session values
                string deviceImei = session.GetString("deviceImei");
                string date = Convert.ToDateTime(session.GetString("date")).ToString("dd-MM-yyyy");
                string channel = session.GetString("channel");
                string fromTime = session.GetString("fromTime");
                string toTime = session.GetString("toTime");

                if (!string.IsNullOrEmpty(deviceImei) &&
                   !string.IsNullOrEmpty(date) &&
                   !string.IsNullOrEmpty(channel) &&
                   !string.IsNullOrEmpty(fromTime) &&
                   !string.IsNullOrEmpty(toTime))
                {

                    string fromDateString = date + " " + fromTime;
                    DateTime fromDate = DateTime.ParseExact(fromDateString, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);


                    string toDateString = date + " " + toTime;
                    DateTime toDate = DateTime.ParseExact(toDateString, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);


                    List<HistoryFiles> historyFiles = new List<HistoryFiles>(); 
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("p_deviceimei", deviceImei);
                    dynamicParameters.Add("p_fromtime", fromDate);
                    dynamicParameters.Add("p_totime", toDate);
                    dynamicParameters.Add("p_channel", channel);
                    historyFiles= await _Idapper.GetAll<HistoryFiles>("select * from usp_GetDashcamHistoryFiles_Web(@p_deviceimei,@p_fromtime,@p_totime,@p_channel)", dynamicParameters);


                    if (historyFiles.Any())
                    {
                        string fileList = string.Join(",", historyFiles.Select(a=>a.filename));

                        var content = new Dictionary<string, string>
                        {
                        { "Imei", deviceImei},
                        { "serverFlagId", "0" },
                        { "proNo", "128" },
                        { "platform", "web" },
                        { "requestId", "6" },
                        { "cmdType", "normallns" },
                        { "token", "a12341234123" },
                        { "cmdContent", "REPLAYLIST,"+fileList },
                        { "offlineFlag", "true"}
                         };
                      
                        var result = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", content);

                        DeleteDashcamFiles(deviceImei);

                    }

                }

                return Ok("1");
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
        }

        [Route("SendHistoryCommand")]
        public async Task<IActionResult> SendHistoryCommand(string deviceImei, string fromTime, string toTime, string channel)
        {

            //if (!IsValid())
            //{
            //    return Redirect("http://pgsql.web.trackanddrive.com/");
            //}

            SendCommandResponseModel sendCommandResponseModel = new SendCommandResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(deviceImei))
                {


                    DateTime fromT = DateTime
                       .ParseExact(fromTime, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

                    DateTime toT = DateTime
                     .ParseExact(toTime, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

                    double minutesDiff = (toT - fromT).TotalMinutes;
                    if (minutesDiff > 40)
                    {
                        return Ok("time error");
                    }
                    var wakeupCommand = new Dictionary<string, string>
                    {
                         { "Imei", deviceImei },
                         { "cmdContent", "WAKEUP_QUERY" },
                         { "serverFlagId", "1" },
                         { "proNo", "128" },
                         { "platform", "web" },
                         { "requestId", "6" },
                         { "cmdType", "normallns" },
                         { "token", "123" },
                         { "timeOut", "5" },
                         { "removeLimitFlag", "true"}
                    };
                    var result = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", wakeupCommand);
                    sendCommandResponseModel = JsonConvert.DeserializeObject<SendCommandResponseModel>(result);
                    Console.WriteLine(sendCommandResponseModel.data.Msg);

                    Thread.Sleep(5000);

                    //string fromDateTimeString = date + " " + fromTime;
                    //string toDateTimeString = date + " " + toTime;

                    string fromTimeFormat = DateTime
                        .ParseExact(fromTime, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture)
                        .ToString("yyMMddHHmmss");

                    string toTimeFormat = DateTime
                        .ParseExact(toTime, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture)
                        .ToString("yyMMddHHmmss");


                    string command = @"{
    ""serverAddress"": ""172.105.59.200"",
    ""tcpPort"": 10003,
    ""udpPort"": 0,
    ""channel"": " + channel + @",
    ""resourceType"": 0,
    ""codeType"": 0,
    ""storageType"": 0,
    ""playMethod"": 0,
    ""forwardRewind"": 0,
    ""beginTime"": """ + fromTimeFormat + @""",
    ""endTime"": """ + toTimeFormat + @""",
    ""instructionID"": ""123456789""
}";


                    var content = new Dictionary<string, string>
                    {
                        {"imei",deviceImei},
                        {"cmdContent",command},
                        {"serverFlagId","0"},
                        {"proNo","37377"},
                        {"platform","web"},
                        {"requestId","6"},
                        {"cmdType","normallns"},
                        {"token","a12341234123"},
                    };


                    result = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", content);
                    sendCommandResponseModel = JsonConvert.DeserializeObject<SendCommandResponseModel>(result);
                    Console.WriteLine(sendCommandResponseModel.data.Msg);
                }
                return Ok(sendCommandResponseModel.data.Msg);
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
        }

        [Route("OffDashcamHistory")]
        public async Task<IActionResult> OffDashcamHistory(string deviceImei)
        {

            //if (!IsValid())
            //{
            //    return Redirect("http://pgsql.web.trackanddrive.com/");
            //}
            try
            {
                if (!string.IsNullOrEmpty(deviceImei))
                {

                    var content = new Dictionary<string, string>
                    {
                        {"Imei",deviceImei},
                        {"cmdContent","REPLAYLIST,OFF"},
                        {"serverFlagId","0"},
                        {"proNo","128"},
                        {"platform","web"},
                        {"requestId","6"},
                        {"cmdType","normallns"},
                        {"token","a12341234123"},
                        {"offlineFlag","false"},
                    };


                    var result = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", content);

                }
                return Ok("1");
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
        }


        [Route("GetDashcamFiles")]
        public async Task<IActionResult> GetDashcamFiles(string DeviceImei, string FromDate, string ToDate, string Day)
        {
            try
            {
                //if (!IsValid())
                //{
                //    return Redirect("http://pgsql.web.trackanddrive.com/");
                //}

                var dashcamFiles = await _Ihome.GetDashcamFiles(DeviceImei, FromDate, ToDate, Day);
                return Json(dashcamFiles);
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
        }

        [Route("SendDashcamInstruction")]
        public async Task<IActionResult> SendDashcamInstruction(string DeviceImei)
        {
            try
            {
                //if (!IsValid())
                //{
                //    return Redirect("http://pgsql.web.trackanddrive.com/");
                //}

                SendCommandResponse sendCommandResponse = new SendCommandResponse();
                sendCommandResponse = await _Ihome.SendDashcamInstruction(DeviceImei);
                return Json(sendCommandResponse);
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
        }

        [Route("SendDashcamStopInstruction")]
        public async Task<IActionResult> SendDashcamStopInstruction(string DeviceImei)
        {
            try
            {

                //if (!IsValid())
                //{
                //    return Redirect("http://pgsql.web.trackanddrive.com/");
                //}

                SendCommandResponse sendCommandResponse = new SendCommandResponse();
                sendCommandResponse = await _Ihome.SendDashcamStopInstruction(DeviceImei);
                return Json(sendCommandResponse);
            }
            catch (Exception ex)
            {
                return Redirect("http://pgsql.web.trackanddrive.com/");
            }
        }
       

    }
}
