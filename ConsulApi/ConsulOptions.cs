using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsulApi
{
    public class ConsulOptions
    {
        /// <summary>
        /// Consul服务器的地址
        /// </summary>
        public string HttpEndPoint { get; set; }

        /// <summary>
        /// 数据中心名称
        /// </summary>
        public string DataCenter { get; set; }

        /// <summary>
        /// Dns服务器端点
        /// </summary>
        public DnsEndPoint DnsEndPoint { get; set; }
    }
}
