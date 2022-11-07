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
    console.log(error, 111)
    return Promise.reject(error)
  }
)

service.interceptors.response.use(
  response => {
    return response.data
  },
  error => {
    const { status, data } = error.response
    if (error.response) {
      // 针对状态码 400接口请求出错的处理
      if (status === 400) {
        Message({
          message: data.error_description || data.error,
          type: 'error',
          duration: 5 * 1000
        })
        return Promise.reject(error)
      }
    }
    if (status === 404) {
      Message({
        message: '接口错误',
        type: 'error',
        duration: 5 * 1000
      })
      return Promise.reject(error)
    }
    if (error.status === 401) {
      MessageBox.confirm(
        '无权限访问',
        '确认注销', {
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
      duration: 3 * 1000
    })
    return Promise.reject(error)
  }
)

export default service
