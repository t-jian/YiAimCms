import request from '@/utils/abpRequest'

export function upload(data) {
    return request({
      url: '/api/file/upload',
      method: 'post',
      data
    })
  }