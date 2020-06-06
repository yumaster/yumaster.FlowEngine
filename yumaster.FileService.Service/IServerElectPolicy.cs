namespace yumaster.FileService.Service
{
    /// <summary>
    /// 服务器选取策略
    /// </summary>
    public interface IServerElectPolicy
    {
        Options.Server ElectServer();
    }
}
