import router from './router'
import store from './store'
import { Message } from 'element-ui'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import getPageTitle from '@/utils/get-page-title'

NProgress.configure({ showSpinner: false }) // NProgress Configuration

const whiteList = ['/login','/auth-redirect'] 

router.beforeEach(async(to, from, next) => {

  NProgress.start()
  document.title = getPageTitle(to.meta.title)
  //在请求之前获取abp应用配置信息
  let abpConfig = store.getters.abpConfig
  if (!abpConfig) {
    abpConfig = await store.dispatch('app/applicationConfiguration')
  }
  if (whiteList.indexOf(to.path) !== -1) {
    //在白名单之中直接放行
     next()
  }else{
    if (abpConfig.currentUser.isAuthenticated) {
      //登录的逻辑处理
      if (to.path === '/login') {
        next({ path: '/' })
        NProgress.done()
      } else {
        if (store.getters.name&&store.getters.token) {
          next()
        } else {
          try {
           await store.dispatch('user/getInfo')
           await store.dispatch('user/setRoles', abpConfig.currentUser.roles)
           const accessRoutes = await store.dispatch('permission/generateRoutes',abpConfig.auth.grantedPolicies)
           router.addRoutes(accessRoutes)
           next()
          } catch (error) {
            await store.dispatch('user/resetToken')
            Message.error(error || 'Has Error')
             next(`/login?redirect=${to.path}`)
          }
          NProgress.done()
        }
      }
    } else {
      //没有登录直接前往登录页面
      next(`/login?redirect=${to.path}`)
    }
  }
})

router.afterEach(() => {
  NProgress.done()
})
