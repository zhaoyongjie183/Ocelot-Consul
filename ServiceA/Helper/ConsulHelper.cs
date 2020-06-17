using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceA.Helper
{
    public static class ConsulHelper
    {
        // 服务注册

        public static void RegisterConsul(this IApplicationBuilder app, IConfiguration configuration, IHostApplicationLifetime lifetime)
        {
            string serviceName = configuration.GetValue<string>("ServiceName");

            string serviceIP = configuration.GetValue<string>("ServiceIP");

            int prot = configuration.GetValue<int>("ServicePort");

            string consulClientUrl = configuration.GetValue<string>("ConsulAddress");

            string healthCheckRelativeUrl = configuration.GetValue<string>("ServiceHealthCheck");

            string Weight = configuration.GetValue<string>("Weight");

            var consulClient = new ConsulClient(x => {

                x.Address = new Uri($"{consulClientUrl}");

                
            });//请求注册的 Consul 地址

            //健康检查
            var httpCheck = new AgentServiceCheck()

            {
                //删除节点  最低配置60S
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60),//服务启动多久后注册

                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔

                HTTP = $"{healthCheckRelativeUrl}",//健康检查地址

                Timeout = TimeSpan.FromSeconds(5),

               

            };

            // Register service with consul

            var registration = new AgentServiceRegistration()

            {

                Checks = new[] { httpCheck },

                ID = serviceName + "_" + prot,

                Name = serviceName,

                Address = serviceIP,

                Port = prot,

                Tags = new[] { Weight}//配置权重

            };

            consulClient.Agent.ServiceRegister(registration);//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）

            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册

            });

            //return app;

        }
    }
}
