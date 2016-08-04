namespace Iquality.Shared.OutboxMailer.Core.Models
{
    public enum ProcessStatus
    {
        NotProcessed = 0,
        Busy = 1,
        Sent = 2,
        Error = -1
    }
}
