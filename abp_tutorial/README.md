# abp 搭建个人网站教程
项目所用到的版本如下
abp 6.0
.net 6
vue2 

## 1) 初识ABP vNext与项目初建
##### ABP vNext 简介
ABP vNext（以下简称ABP）的前身是asp.net boilerplate，更多信息请看官网介绍。ABP官网：https://www.abp.io/

废话不多说，开始个人网站搭建之旅

> 默认已经有了.net core的开发环境，没有就去下载 https://dotnet.microsoft.com/download

###### 一、创建项目
创建项目有很多种方式：

1. 纯手撸，使用vs手动创建新项目(熟手、巧手特区)

2. 借助abp官网模板直接傻瓜式创建，地址：https://abp.io/get-started
> ![abp官网模板直接傻瓜式创建](../abp_tutorial/images/1.1.png)
3. 第三种，abp cli
> 更多使用方式参考 https://docs.abp.io/zh-Hans/abp/latest/CLI
``` 
dotnet tool install -g Volo.Abp.Cli 
abp new Acme.BookStore
```
为了省事，项目就直接使用2方式创建
>项目类型选择应用程序,UI框架选择->MVC,数据库提供者选择->Entity Framework Core, 数据库选择->MySQL,移动端不需要，小项目也不需要将Web、http API分离，所以也不需要分层

创建项目完成，目录结构如下 (solutionItems是自己创建的文件夹，主要用来管理其他零散的文件如：Dockerfile、gitigore、README.md等)
![项目目录结构](../abp_tutorial/images/1.3.png)
vs2022打开时目录结构
![项目目录结构](../abp_tutorial/images/1.2.png)

###### 二、让项目跑起来

1. 先更改 YiAim.Cms.Web 里面的  appsettings.json 里面的数据库连接串，同时也需要更改  YiAim.Cms.DbMigrator 里面的  appsettings.json 里面的数据库连接串
`"Default": "server=xxx;port=3306;user=xx;password=xxx;database=xx;charset=utf8;SslMode=none;Allow User Variables=True" `
2. 将YiAim.Cms.DbMigrator 设为启动项目，运行该项目进行数据库初始操作（这步很重要）
![数据初始化](../abp_tutorial/images/1.4.png)
完成后，数据库中已经创建了表和初始化了系统自动的一些数据
![数据初始化2](../abp_tutorial/images/1.5.png)
![数据初始化3](../abp_tutorial/images/1.6.png)

3. 然后就可以启动YiAim.Cms.Web项目（要将它设为启动项目），运行界面如下
![web启动](../abp_tutorial/images/1.7.png)
![swagger api](../abp_tutorial/images/1.8.png)

到此abp项目已经能正常运行，本章目标结束。
下章将进行与vue element admin 后台框架的对接，将完成登录、注册等功能。


## 2)对接管理后台的前端vue项目&用户登录

上一章 进行到了让项目跑起来，本章将对接管理后台的前端vue项目&用户登录

项目用到的管理后台UI框架是vue-element-admin vue2版本，下载地址：https://github.com/PanJiaChen/vue-element-admin/tree/i18n

将下载的文件放到，你想放的位置，如
![vue项目结构](../abp_tutorial/images/2.1.png)

###### 将vue项目跑起来
将项目使用vscode 打开
使用命令先下载依赖环境
` npm install `
运行命令
` npm run dev ` 
如果下载依赖不成功，出现 
>npm ERR! fatal: unable to access ‘https://github.com/nhn/raphael.git/’: OpenSSL SSL_read:错误 

由tui-editor 引起的，解决方法：
1. 先删除 package.json 的 tui-editor 配置项

运行 npm i  或者 yarn install 会正常编译。

2. 在手动安装 tui-editor 

 npm install --save tui-editor

运行成功，如图
![vue run success](../abp_tutorial/images/2.2.png)

###### 登录相关
接下来，将完成登录相关的功能。

abp框架提供了用户身份验证（identity server ）的相关模块，我们直接使用就行.
可访问 `xxx/.well-known/openid-configuration` 查看

1. 可通过`/connect/token`接口获取获取token 
>(严格来说不应该直接访问/connect/token获取token。首先应该从identityserver发现文档/.well-known/openid-configuration中获取配置信息，然后从/.well-known/jwks 获取公钥等信息用于校验token合法性，最后才是获取token),奈何实力有限先将就着用
2. 前端想正常访问接口需要配置跨域
在xxx.Cms.Web项目的appsetting.json 配置前端的域名
![配置跨域](../abp_tutorial/images/2.3.png)
3. xxx.Cms.Web里面在CmsWebModule的ConfigureServices 配置跨域,在OnApplicationInitialization使用 ` app.UseCors(DefaultCorsPolicyName) `
![配置跨域4](../abp_tutorial/images/2.4.png)
代码如下 
``` 
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
![配置跨域5](../abp_tutorial/images/2.5.png)

现在可以愉快的调用接口玩耍了~~~
切换vue项目
1. 先将src/utils/request.js 复制一份重命名为abpRequest.js（这里为了不让整个项目的请求报错，后面将删除request.js文件）,将abpRequest.js的baseURL改成 'https://localhost:44377'

2. 在sr/api 里面新建abp.js,内容如下，主要配置abp基本的请求
```
import request from '@/utils/abpRequest'

export function applicationConfiguration() {
  return request({
    url: '/api/abp/application-configuration',
    method: 'get'
  })
}

```
3. 在src/store/modeules/app.js 里面添加abp应用相关的信息获取,getters.js 里面配置

getters.js 
`getters={abpConfig: state => state.app.abpConfig,}`

app.js
```
import { applicationConfiguration } from '@/api/abp'
state = {abpConfig: null,}
mutations={ SET_ABPCONFIG: (state, abpConfig) => {
    state.abpConfig = abpConfig
  }}
  actions={
      applicationConfiguration({ commit }) {
    return new Promise((resolve, reject) => {
      applicationConfiguration()
        .then(response => {
          commit('SET_ABPCONFIG', response)
          const language = response.localization.currentCulture.cultureName
          const values = response.localization.values
          //setLocale(language, values) //设置语言
          resolve(response)
        })
        .catch(error => {
          reject(error)
        })
    })
  },
  }
```
4. 在permission.js 里面添加对获取请求
```
  //在请求之前获取abp应用配置信息
  let abpConfig = store.getters.abpConfig
  if (!abpConfig) {
    abpConfig = await store.dispatch('app/applicationConfiguration')
  }
```
因为我们已经配置了跨域请求，所有能直接正常访问
![abp+vue 请求](../abp_tutorial/images/2.6.png)
![abp+vue 请求2](../abp_tutorial/images/2.7.png)

5. 接下来继续改造abpRequest.js，因为没有对返回数据进行处理这时候的界面还是提示报错的
```
import axios from 'axios'
import { MessageBox, Message } from 'element-ui'
import store from '@/store'
const service = axios.create({
  baseURL: 'https://localhost:44377', 
  timeout: 5000 
})
service.interceptors.request.use(
  config => {
    config.headers['accept-language'] = store.getters.language
    if (store.getters.token) {
      config.headers['authorization'] = 'Bearer ' + store.getters.token
    }
    return config
  },
  error => {
    console.log(error) 
    return Promise.reject(error)
  }
)

service.interceptors.response.use(
 
  response => {
    return response.data
  },
  error => {
    if (error.status === 401) {
      MessageBox.confirm(
        '无权限访问',
        '确认注销',
        {
          confirmButtonText: '重新登录',
          cancelButtonText: '取消',
          type: 'warning'
        }
      ).then(() => {
        store.dispatch('user/resetToken').then(() => {
          location.reload()
        })
      })
    }
    let message = ''
    if (error.response && error.response.data && error.response.data.error) {
      message = error.response.data.error.message
    } else {
      message = error.message
    }

    Message({
      message: message,
      type: 'error',
      duration: 5 * 1000
    })
    return Promise.reject(error)
  }
)

export default service

```
5. 解决了基础请求配置的问题，接下来正式进入登录相关
在src/api 建立indentity文件夹(主要用来存放用户身份相关的请求)，新建user.js,里面的接口都是对应swagger里面
```
import request from '@/utils/abpRequest'
import { transformAbpListQuery } from '@/utils/abp'
import qs from 'querystring'

export function login(data) {
  return request({
    url: '/connect/token',
    method: 'post',
    headers: { 'content-type': 'application/x-www-form-urlencoded' },
    data: qs.stringify(data)
  })
}

export function getInfo() {
  return request({
    url: '/api/account/my-profile',
    method: 'get'
  })
}

export function logout() {
  return request({
    url: '/api/account/logout',
    method: 'get'
  })
}


export function getUsers(query) {
  return request({
    url: '/api/identity/users',
    method: 'get',
    params: transformAbpListQuery(query)
  })
}

```
在src/store/modules里的user.js 修改获取token以及获取用户信息等操作

<font color="red">获取token的接口参数要特别注意一下</font>，本人被耗在这里几个小时 ~~ ~

clientSetting里面的参数要从YiAim.Cms.Dbmigrator里面的 appsertting.json里面找
![clientSetting](../abp_tutorial/images/2.9.png)
初始化在YiAim.Cms.Domain，具体请看源代码
![clientSetting2](../abp_tutorial/images/2.1.1.png)
默认初始可以授权的应用有4个
![授权的应用有4个](../abp_tutorial/images/2.1.2png.png)
```
login({ commit }, userInfo) {
    const { username, password } = userInfo
    return new Promise((resolve, reject) => {
      const clientSetting = {
        grant_type: "password",
        scope: "Cms",
        username: username.trim(),
        password: password,
        client_id: "Cms_App",
        client_secret: ""
      }
      clientSetting.password="1q2w3E*"
      login(clientSetting).then(response => {
        const token=response.access_token
        commit("SET_TOKEN", token);
        setToken(token).then(()=>{
          resolve()
        })
        
      }).catch(error => {
        reject(error)
      })
    })
```
至此输入账号密码<font color="red">默认账号admin/1q2w3E*</font>  token已能正常获取到！

接下来是获取用户相关的信息，使用接口api/account/my-profile,能正常获取用户信息后发现vue项目并不能跳转

需要我们去去改造一下permission.js里面的内容同时也需要修改一下src/uitls/auth.js

auth.js
```
import Cookies from 'js-cookie'
import store from '@/store'
const TokenKey = 'Admin-Token'

export function getToken() {
  return Cookies.get(TokenKey)
}

export async function setToken(token) {
  const result = Cookies.set(TokenKey, token)
  await store.dispatch('app/applicationConfiguration')
  return result
}

export async function removeToken() {
  const result = Cookies.remove(TokenKey)
  await store.dispatch('app/applicationConfiguration')
  return result
}

```

![2.1.3](../abp_tutorial/images/2.1.3.png)


先在permission.js简单处理一下，让我们可以看到效果

> vue-element-admin的菜单权限是使用用户角色来控制的。通过/api/abp/application-configuration接口的auth.grantedPolicies字段，与对应的菜单路由绑定，就可以实现权限控制了。

permission.js
```
router.beforeEach(async(to, from, next) => {
  NProgress.start()
  document.title = getPageTitle(to.meta.title)
  //在请求之前获取abp应用配置信息
  let abpConfig = store.getters.abpConfig
  if (!abpConfig) {
    abpConfig = await store.dispatch('app/applicationConfiguration')
  }
  if (abpConfig.currentUser.isAuthenticated) {
    if (to.path === '/login') {
      next({ path: '/' })
      NProgress.done() 
    } else {
        //主要是这里
      const hasRoles =abpConfig.currentUser.roles||['admin']
      if (hasRoles) {
        next()
      } else {
        try {
          // get user info
          // note: roles must be a object array! such as: ['admin'] or ,['developer','editor']
          const { roles } = await store.dispatch('user/getInfo')

          // generate accessible routes map based on roles
          const accessRoutes = await store.dispatch('permission/generateRoutes', roles)

          // dynamically add accessible routes
          router.addRoutes(accessRoutes)

          // hack method to ensure that addRoutes is complete
          // set the replace: true, so the navigation will not leave a history record
          next({ ...to, replace: true })
        } catch (error) {
          // remove token and go to login page to re-login
          await store.dispatch('user/resetToken')
          Message.error(error || 'Has Error')
          next(`/login?redirect=${to.path}`)
          NProgress.done()
        }
      }
    }
  } else {
    /* has no token*/

    if (whiteList.indexOf(to.path) !== -1) {
      // in the free login whitelist, go directly
      next()
    } else {
      // other pages that do not have permission to access are redirected to the login page.
      next(`/login?redirect=${to.path}`)
      NProgress.done()
    }
  }
})

```
到此，已经能正常进入首页
![2.1.4](../abp_tutorial/images/2.1.4.png)

最后，本章到此已经结束，不然篇幅就太长了。本章我们完成了与vue-element-admin的接口对接，完成了登录、应用程序基本初始化功能。
下章将要完成的是 菜单权限，注销及其用户头像等信息的补全。






























