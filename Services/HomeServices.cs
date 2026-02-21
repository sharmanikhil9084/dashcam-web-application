using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrackanDrive.Dashcam.Interfaces;
using TrackanDrive.Dashcam.Models;
using trackandrive.dashcam.ViewModel;
using TrackanDrive.Dashcam.CommonFunctions;
using TrackanDrive.Web.Interfaces;
using Dapper;

namespace TrackanDrive.Dashcam.Services
{
    [Authorize]
    public class HomeServices : IHome
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDapper _Idapper;

        public HomeServices(
            IHttpContextAccessor httpContextAccessor,
            IDapper idapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _Idapper = idapper;
        }
        public async Task<SendCommandResponse> SendDashcamInstruction(string DeviceImei)
        {
            SendCommandResponse sendCommandResponse = new SendCommandResponse();
            try
            {
                DeviceModel deviceModel = new DeviceModel();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("p_deviceimei", DeviceImei);
                deviceModel = await _Idapper.Get<DeviceModel>("select * from usp_getdevicemodel_web(@p_deviceimei)", dynamicParameters);
               
                string command=string.Empty;
                string proNumber = string.Empty;

				if (deviceModel.model== "JC400")
				{
					command = "RTMP,ON,IN";
					proNumber = "128";
				}
				else if (deviceModel.model == "JC371")
				{
					command = @"{
                            ""dataType"": 0,
                            ""codeStreamType"": 1,
                            ""channel"": ""1"",
                            ""videoIP"": ""172.105.59.200"",
                            ""videoTCPPort"": ""10002"",
                            ""videoUDPPort"": 0
                            }";
                    proNumber = "37121";



					// wakeup command for jc 371//

					var formData_jc = new Dictionary<string, string>
					{
						 { "Imei", DeviceImei },
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
					var result = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", formData_jc);
				}
				else
				{
					command = "RTMP,ON,INOUT";
					proNumber = "128";
				}

				var formData = new Dictionary<string, string>
                {
                         { "Imei", DeviceImei },
                         { "serverFlagId", "0" },
                         { "proNo", proNumber },
                         { "platform", "web" },
                         { "requestId", "6" },
                         { "cmdType", "normallns" },
                         { "token", "123" },
                         { "cmdContent", command },
                         { "offlineFlag", "true"}
                 };
                var response = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", formData);

                if (deviceModel.model == "JC400")
                {
                    command = "RTMP,ON,OUT";
                    formData = new Dictionary<string, string>
                    {
                         { "Imei", DeviceImei },
                         { "serverFlagId", "0" },
                         { "proNo", "128" },
                         { "platform", "web" },
                         { "requestId", "6" },
                         { "cmdType", "normallns" },
                         { "token", "123" },
                         { "cmdContent", command },
                         { "offlineFlag", "true"}
                    };
                    response = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", formData);
                }
				else if (deviceModel.model == "JC371")
				{
					command = @"{
                            ""dataType"": 0,
                            ""codeStreamType"": 1,
                            ""channel"": ""2"",
                            ""videoIP"": ""172.105.59.200"",
                            ""videoTCPPort"": ""10002"",
                            ""videoUDPPort"": 0
                            }";

					formData = new Dictionary<string, string>
					{
						 { "Imei", DeviceImei },
						 { "serverFlagId", "0" },
						 { "proNo", "37121" },
						 { "platform", "web" },
						 { "requestId", "6" },
						 { "cmdType", "normallns" },
						 { "token", "123" },
						 { "cmdContent", command },
						 { "offlineFlag", "true"}
					};
					response = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", formData);



					command = @"{
                            ""dataType"": 0,
                            ""codeStreamType"": 1,
                            ""channel"": ""3"",
                            ""videoIP"": ""172.105.59.200"",
                            ""videoTCPPort"": ""10002"",
                            ""videoUDPPort"": 0
                            }";

					formData = new Dictionary<string, string>
					{
						 { "Imei", DeviceImei },
						 { "serverFlagId", "0" },
						 { "proNo", "37121" },
						 { "platform", "web" },
						 { "requestId", "6" },
						 { "cmdType", "normallns" },
						 { "token", "123" },
						 { "cmdContent", command },
						 { "offlineFlag", "true"}
					};
					response = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", formData);
				}

				SendCommandResponseModel sendCommandResponseModel = JsonConvert.DeserializeObject<SendCommandResponseModel>(response);
                sendCommandResponse.Message = sendCommandResponseModel.data.content;
                if (sendCommandResponse.Message != "OK!")
                    sendCommandResponse.Message = sendCommandResponseModel.data.Msg;
            }
            catch (Exception ex)
            {

                throw;
            }
            return sendCommandResponse;
        }


        public async Task<SendCommandResponse> SendDashcamStopInstruction(string DeviceImei)
        {
            SendCommandResponse sendCommandResponse = new SendCommandResponse();
            try
            {
               
                var formData = new Dictionary<string, string>
                    {
                         { "Imei", DeviceImei },
                         { "serverFlagId", "0" },
                         { "proNo", "128" },
                         { "platform", "web" },
                         { "requestId", "6" },
                         { "cmdType", "normallns" },
                         { "token", "a12341234123" },
                         { "cmdContent", "RTMP,OFF" },
                         { "offlineFlag", "true"}
                 };
                var response = await CommonUtility.PostFormUrlAsync<SendCommandModel>("http://gps.markongps.com:10088/api/device/sendInstruct", formData);
                SendCommandResponseModel sendCommandResponseModel = JsonConvert.DeserializeObject<SendCommandResponseModel>(response);
                sendCommandResponse.Message = sendCommandResponseModel.data.content;
                if (sendCommandResponse.Message != "OK!")
                    sendCommandResponse.Message = sendCommandResponseModel.data.Msg;
            }
            catch (Exception ex)
            {

                throw;
            }
            return sendCommandResponse;
        }
        public async Task<string> GetDashcamHistoryHistoryVideo(string deviceImei, string date, string channel)
        {
            if (!string.IsNullOrEmpty(deviceImei))
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var content = new Dictionary<string, string>
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

                var data = new FormUrlEncodedContent(content);
                HttpClient httpClient = new HttpClient();
                var httpResponseMessage = await httpClient.PostAsync("http://gps.markongps.com:10088/api/device/sendInstruct", data);
                var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
            }
            return "1";

        }


        public async Task<List<DashcamDevices>> GetDashcamDevicesList(string deviceImei)
        {
            var dashcamDevices = new List<DashcamDevices>();
           // int userId = Convert.ToInt32(CommonUtility.DecodeFrom64(_httpContextAccessor.HttpContext.Request.Cookies["dsh"]));
            int userId = 1;

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("p_userid", userId);
            dynamicParameters.Add("p_search", deviceImei);
            dashcamDevices = await _Idapper.GetAll<DashcamDevices>("select * from usp_get_dashcam_devices_web(@p_userid,@p_search)", dynamicParameters);
            return dashcamDevices;
        }

        public async Task<List<DashcamFiles>> GetDashcamFiles(string DeviceImei, string FromDate, string ToDate, string Day)
        {
            var dashcamFiles = new List<DashcamFiles>();
            string[] files=null;
            try
            {
                DateTime startTime;
                DateTime endTime;
                if (Day == "Today")
                {
                    startTime = DateTime.Today; // Today at 00:00:00
                    endTime = DateTime.Today.AddDays(1).AddTicks(-1); // Today at 23:59:59.999
                }
                else if (Day == "Yesterday")
                {
                    startTime = DateTime.Today.AddDays(-1); // Yesterday at 00:00:00
                    endTime = DateTime.Today.AddTicks(-1);  // Yesterday at 23:59:59.999
                }
                else if (Day == "This Week")
                {
                    string[] dates = GetThisWeekDates().Split(",");
                    startTime = DateTime.ParseExact(dates[0], "yyyy-MM-dd", null); // Start of the week
                    endTime = DateTime.ParseExact(dates[1], "yyyy-MM-dd", null).AddDays(1).AddTicks(-1); // End of the week at 23:59:59.999
                }
                else if (Day == "Last Week")
                {
                    string[] dates = GetLastWeekDates().Split(",");
                    startTime = DateTime.ParseExact(dates[0], "yyyy-MM-dd", null); // Start of last week
                    endTime = DateTime.ParseExact(dates[1], "yyyy-MM-dd", null).AddDays(1).AddTicks(-1); // End of last week at 23:59:59.999
                }
                else
                {
                    startTime = DateTime.ParseExact(FromDate, "yyyy-MM-ddTHH:mm", null);
                    endTime = DateTime.ParseExact(ToDate, "yyyy-MM-ddTHH:mm", null);
                }

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("p_deviceimei", DeviceImei);
                dynamicParameters.Add("p_fromtime", startTime);
                dynamicParameters.Add("p_totime", endTime);
                dashcamFiles = await _Idapper.GetAll<DashcamFiles>("select * from usp_getdashcamfiles_web(@p_deviceimei,@p_fromtime,@p_totime)", dynamicParameters);
              
            }
            catch (Exception ex)
            {
                return null;
            }
            return dashcamFiles;
        }

        public string GetThisWeekDates()
        {
            try
            {
                DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                int daysTillCurrentDay = currentDay - DayOfWeek.Sunday;
                string currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay).ToString("yyyy-MM-dd");
                int daysTillLastDay = DayOfWeek.Saturday - currentDay;
                string currentWeekEndDate = DateTime.Now.AddDays(daysTillLastDay).ToString("yyyy-MM-dd");

                return currentWeekStartDate + "," + currentWeekEndDate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public string GetLastWeekDates()
        {
            try
            {
                DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                int daysTillLastWeekStart = currentDay - DayOfWeek.Sunday + 7;
                string lastWeekStartDate = DateTime.Now.AddDays(-daysTillLastWeekStart).ToString("yyyy-MM-dd");
                int daysTillLastWeekEnd = 7 - (DayOfWeek.Saturday - currentDay);
                string lastWeekEndDate = DateTime.Now.AddDays(-daysTillLastWeekEnd).ToString("yyyy-MM-dd");

                return lastWeekStartDate + "," + lastWeekEndDate;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
      
    }

    
}
