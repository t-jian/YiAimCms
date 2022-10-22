
# 配置跨越
```
//1、定义跨越名称
private const string DefaultCorsPolicyName = "Default";
//2、在 ConfigureServices 使用
ConfigureCors(context, configuration);
//3、在OnApplicationInitialization 里面 app.UseRouting()之后 app.UseAuthentication()之前配置
app.UseCors(DefaultCorsPolicyName);

private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddPolicy(DefaultCorsPolicyName, builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }
```
# ABP的授权模块
> 可以参考下IdentityServer4官网。运行ABP模板项目，看一下IdentityServer4发现文档，uri是：/.well-known/openid-configuration

# 关于程序数据初始化
在应用abpvnext框架的单体程序时，需要初始化整个框架的admin管理员信息及客户端认证等相关信息，如果没有相关客户端认证信息，程序在访问：
接口地址：https://localhost:XXXX/connect/token时就会报：
报error: "invalid_client"
>  二，解决获取Token时报"invalid_client"

获取Token:
1、接口地址：https://localhost:44349/connect/token
2、接口需要以：application/x-www-form-urlencoded 的方式调用；
3、接口参数：

- 默认管理员账号 默认用户名是admin，默认密码是1q2w3E* 

> 关于使用 使用接口/connect/token 登录成功并返回