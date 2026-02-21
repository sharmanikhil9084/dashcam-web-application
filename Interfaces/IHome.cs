using trackandrive.dashcam.ViewModel;

namespace TrackanDrive.Dashcam.Interfaces
{
    public interface IHome
    {
        Task<SendCommandResponse> SendDashcamInstruction(string DeviceImei);
        Task<SendCommandResponse> SendDashcamStopInstruction(string DeviceImei);
        Task<List<DashcamDevices>> GetDashcamDevicesList(string deviceImei);
        Task<List<DashcamFiles>> GetDashcamFiles(string DeviceImei, string FromDate, string ToDate, string Day);
        Task<string> GetDashcamHistoryHistoryVideo(string deviceImei, string date, string channel);
        //Task PushDashcamFileList(string RawData);

    }
}
