using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsulApi
{
    public class ServiceDiscoveryOptions
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Consul配置
        /// </summary>
        public ConsulOptions Consul { get; set; }
    }
}
