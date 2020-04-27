using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebClient.Models;
using Consul;
using ConsulApi;
using Microsoft.VisualBasic;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            {
                ConsulClient consulClient = new ConsulClient(x=> {
                    x.Address = new Uri("http://localhost:8500");
                });

                var result=consulClient.Agent.Services().Result.Response.Values.ToArray();

                AgentService agentService = null;

                //{
                //    //平均分配 random 随机种子要配置，不然同一时刻生成的随机数是一样的
                //    agentService = result[new Random(DateTime.Now.Millisecond + iSend++).Next(0, result.Length)];
                //}

                //思想就是根据tag标签的数值，往集合里面插入数据，标签值约大。肯定插入的数据就更多，在random的时候随机到的几率就越大，这就是所谓的权重越大请求越多
                List<AgentService> agentServices = new List<AgentService>();

                {
                    //权重
                    foreach (var item in result)
                    {
                        int count = int.Parse(item.Tags?[0].ToString());

                        for (int i = 0; i < count; i++)
                        {
                            agentServices.Add(item);
                        }
                    }

                }

                Console.WriteLine(new Random(DateTime.Now.Millisecond + iSend++).Next(0, agentServices.Count));

                agentService = agentServices[new Random(DateTime.Now.Millisecond + iSend++).Next(0, agentServices.Count)];

                var url = $"http://{agentService.Address}:{agentService.Port}/WeatherForecast";

                Console.WriteLine(url);

                var str = InvokApi(url);

                 var model=Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(str);

                base.ViewBag.UserModel = model;
            }

            return View();
        }

        private int iSend = 1;

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string InvokApi(string url)
        {
            HttpClient httpClient = new HttpClient();

            var responseMessage=httpClient.GetAsync(url).GetAwaiter().GetResult();

            return responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}
