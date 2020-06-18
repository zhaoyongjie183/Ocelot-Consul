using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace IdentityServer4_Server
{
    /// <summary>
    /// Copyright (c) 2020 All Rights Reserved.
    /// 描述：identity验证
    /// 创建人： zhaoyongjie
    /// 创建时间：2020/6/17 10:41:25
    /// </summary>
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }

        //定义要保护的资源（webapi）
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        //定义可以访问该API的客户端
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,  //设置模式，客户端模式
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },
                new Client
                 {
                        ClientId = "passwordclient",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },

                        AllowedScopes = { "api1" }
                  }
            };
        }

        /// <summary>
        /// 添加测试用户
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "tony",
                        Password = "123456"
                    },
                    new TestUser
                    {
                        SubjectId = "2",
                        Username = "thor",
                        Password = "456789"
                    }
                };
        }
    }
}
