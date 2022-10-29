import request from '@/utils/abpRequest'

import { transformAbpListQuery } from '@/utils/abp'

export function getPage(query) {
  return request({
    url: '/api/app/blog',
    method: 'get',
    params: transformAbpListQuery(query)
  })
}

export function getBlogById(id) {
  return request({
    url: `/api/app/blog/${id}`,
    method: 'get'
  })
}

export function createBlog(data) {
  return request({
    url: '/api/app/blog',
    method: 'post',
    data
  })
}

export function updateBlog(payload) {
  return request({
    url: `/api/app/blog/${payload.id}`,
    method: 'put',
    data: payload
  })
}

export function deleteBlog(id) {
  return request({
    url: `/api/app/blog/${id}`,
    method: 'delete'
  })
}
export function updateTaxis(id, cid) {
  let data = { NewsId: id, ColumnId: cid }
  return request({
    url: '/api/app/blog/updateTaxis',
    method: 'post',
    data
  })
}
