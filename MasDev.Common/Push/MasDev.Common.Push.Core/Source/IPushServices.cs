
namespace MasDev.Common.Push
{
    public interface IPushServices
    {
        IGcmService GcmService { get; }
        IApplePushService ApplePushService { get; }
    }
}
