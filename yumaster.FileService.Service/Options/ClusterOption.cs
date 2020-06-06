namespace yumaster.FileService.Service.Options
{
    /// <summary>
    /// 集群选项
    /// </summary>
    public class ClusterOption
    {
        /// <summary>
        /// 本服务器的ID
        /// </summary>
        public int SelfServerId { get; set; }
        public Server[] Servers { get; set; }
    }

    public class Server
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public int Weight { get; set; }
        public bool AllowUpload { get; set; }
    }
}
