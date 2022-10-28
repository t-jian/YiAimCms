import Vue from 'vue'

import Cookies from 'js-cookie'

import 'normalize.css/normalize.css' // a modern alternative to CSS resets

import Element from 'element-ui'
import './styles/element-variables.scss'

import '@/styles/index.scss' // global css
import grobalMsgTip from '@/utils/grobalMsgTip'
import App from './App'
import store from './store'
import router from './router'

import i18n from './lang' // internationalization
import './icons' // icon
import './permission' // permission control
import './utils/error-log' // error log

import * as filters from './filters' // global filters

if (process.env.NODE_ENV === 'production') {
  const { mockXHR } = require('../mock')
  mockXHR()
}


Vue.use(Element, {
  size: Cookies.get('size') || 'medium',
  i18n: (key, value) =>{ i18n.t(key, value)}
})
Object.keys(filters).forEach(key => {
  Vue.filter(key, filters[key])
})

Vue.config.productionTip = false
Vue.prototype.$mtip = grobalMsgTip.MessageTip
new Vue({
  el: '#app',
  router,
  store,
  i18n,
  render: h => h(App)
})
