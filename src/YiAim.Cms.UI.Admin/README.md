#### 关于后台对接国际化

接口返回的语言包数据跟i18n提供的数据绑定（修改 `src/lang/index.js`,`src/store/modules/app.js`）

1. i18n.js 修改
2. layout文件夹里面修改 
   components\settings\index.js
   components\Navbar.vue
3. router文件夹里面相关的标题   
4. 其他页面使用到国际化
vue页面使用 `$t()`,js里面使用 `i18n.$t()`
5. 语言切换用的是一个公共组件 src\components\LangSelect\index.vue


