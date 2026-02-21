
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace trackandrive.dashcam.ViewModel
{
    public class TrackDriveDashcamViewModel
    {
       
    }

    public class Vehicle
    {
        public int Id { get; set; }
        public int DeviceModelId { get; set; }
        public decimal DeviceImei { get; set; }
    }

    public class VehicleModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DashcamDevices
    {
        public string VehicleRegNo { get; set; }
        public string DeviceImei { get; set; }
        public int Channels { get; set; }
        public string Status { get; set; }
        public string LastUpdate { get; set; }
		public string Model { get; set; }
	}
    public class HistoryFiles
    {
        public string filename { get; set; }
    }

    public class EmptyClass
    {
       
    }

    public class SendCommandResponseModel
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("msg")]
        public string msg { get; set; }
        public SendCommandResponseData data { get; set; }
    }
    public class SendCommandResponseData
    {
        [JsonProperty("_code")]
        public int Code { get; set; }
        [JsonProperty("_msg")]
        public string Msg { get; set; }
        [JsonProperty("_content")]
        public string content { get; set; }
    }

    public class SendCommandResponse
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class SendCommandModel
    {
        [Required]
        public string Imei { get; set; }
        [Required]
        public int serverFlagId { get; set; }
        [Required]
        public int proNo { get; set; }
        [Required]
        public string platform { get; set; }
        [Required]
        public string requestId { get; set; }
        [Required]
        public string cmdType { get; set; }
        [Required]
        public string token { get; set; }
        [Required]
        public string cmdContent { get; set; }
        [Required]
        public bool offlineFlag { get; set; }
    }
    //public class HistoryFiles
    //{
    //    public string FileUrl { get; set; }
    //}
    public class DashcamFiles
    {
        public string FileType { get; set; }
        public string FileUrl { get; set; }
        public string Date { get; set; }
        public string AlertName { get; set; }
    }

   

    public class DeviceModel
    {
        public string model { get; set; }
    }

    public class PushingDashcamHistoryDataList
    {
        [JsonProperty("imei")]
        public string Imei { get; set; }

        [JsonProperty("fileNameList")]
        public string fileNameList { get; set; }
    }

    public class Pushing_Dashcam_History_Data
    {
        public int? Id { get; set; }
        public string? Imei { get; set; }
        public string? FileName { get; set; }
        public DateTime? GpsTime { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string Channel { get; set; }
    }
}
