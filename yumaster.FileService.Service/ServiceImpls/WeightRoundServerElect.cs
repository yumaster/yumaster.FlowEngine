using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yumaster.FileService.Service.Options;

namespace yumaster.FileService.Service.ServiceImpls
{
    /// <summary>
    /// 加权轮询选择策略
    /// </summary>
    public class WeightRoundServerElect : IServerElectPolicy
    {
        private readonly IOptionsMonitor<ClusterOption> _option;
        private Dictionary<int, Service.Options.Server> _servers;
        private int[] _serverIds;
        private int _serverIdIndex = 0;

        public WeightRoundServerElect(IOptionsMonitor<ClusterOption> option)
        {
            _option = option;
            InitRoundList();
        }

        public Service.Options.Server ElectServer()
        {
            lock (this)
            {
                if (_serverIdIndex >= _serverIds.Length)
                    _serverIdIndex = 0;

                var serverId = _serverIds[_serverIdIndex];
                return _servers[serverId];
            }
        }

        private void InitRoundList()
        {
            var serverIdLst = new List<int>();
            foreach (var server in _option.CurrentValue.Servers)
            {
                for (var i = 0; i < server.Weight; ++i)
                    serverIdLst.Add(server.Id);
            }
            _serverIds = serverIdLst.ToArray();
            _servers = _option.CurrentValue.Servers.ToDictionary(p => p.Id);
        }
    }
}
