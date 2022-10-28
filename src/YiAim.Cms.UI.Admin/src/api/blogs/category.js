import request from '@/utils/abpRequest'

import { transformAbpListQuery } from '@/utils/abp'

export function getCategory(query) {
  return request({
    url: '/api/app/category',
    method: 'get',
    params: transformAbpListQuery(query)
  })
}
export function getAllCategory(){
    return request({
        url: '/api/app/Category/GetAll',
        method: 'get',
        params:null
      })
}

export function getCategoryById(id) {
  return request({
    url: `/api/app/category/${id}`,
    method: 'get'
  })
}

export function createCategory(data) {
  return request({
    url: '/api/app/Category',
    method: 'post',
    data
  })
}

export function updateCategory(payload) {
  return request({
    url: `/api/app/category/${payload.id}`,
    method: 'put',
    data: payload
  })
}

export function deleteCategory(id) {
  return request({
    url: `/api/app/category/${id}`,
    method: 'delete'
  })
}
