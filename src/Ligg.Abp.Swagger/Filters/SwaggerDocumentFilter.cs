using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ligg.Abp.Swagger.Filters
{
    //#controller
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var tags = new List<OpenApiTag>
            {
                //com
                 new OpenApiTag {
                    Name = "Auth",
                    Description = "用户登录",
                    ExternalDocs = new OpenApiExternalDocs { Description = "用户登录以" }
                },
                new OpenApiTag {
                    Name = "Usr",
                    Description = "个人设置",
                    ExternalDocs = new OpenApiExternalDocs { Description = "个人设置" }
                },
                new OpenApiTag {
                    Name = "Common",
                    Description = "常用接口",
                    ExternalDocs = new OpenApiExternalDocs { Description = "常用接口" }
                },

                //adm
                 new OpenApiTag {
                    Name = "UsrMtn",
                    Description = "用户维护",
                    ExternalDocs = new OpenApiExternalDocs { Description = "用户维护" }
                },
                 new OpenApiTag {
                    Name = "RolMtn",
                    Description = "角色维护",
                    ExternalDocs = new OpenApiExternalDocs { Description = "角色维护" }
                },

                 //cfg
                  new OpenApiTag {
                    Name = "TrsCfg",
                    Description = "事务配置",
                    ExternalDocs = new OpenApiExternalDocs { Description = "事务配置" }
                },

            };

            #region 实现添加自定义描述时过滤不属于同一个分组的API

            // 当前分组名称
            var groupName = context.ApiDescriptions.FirstOrDefault().GroupName;

            // 当前所有的API对象
            var apis = context.ApiDescriptions.GetType().GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(context.ApiDescriptions) as IEnumerable<ApiDescription>;

            // 不属于当前分组的所有Controller
            // 注意：配置的OpenApiTag，Name名称要与Controller的Name对于才会生效。
            var controllers = apis.Where(x => x.GroupName != groupName).Select(x => ((ControllerActionDescriptor)x.ActionDescriptor).ControllerName).Distinct();

            // 筛选一下tags
            swaggerDoc.Tags = tags.Where(x => !controllers.Contains(x.Name)).OrderBy(x => x.Name).ToList();

            #endregion
        }
    }
}