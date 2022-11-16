import request from '@/utils/abpRequest'
import { transformAbpListQuery } from '@/utils/abp'
import qs from 'querystring'

export function login(data) {
  return request({
    url: '/ym/connect/token',
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

export function getUserById(id) {
  return request({
    url: `/api/identity/users/${id}`,
    method: 'get'
  })
}

export function createUser(payload) {
  return request({
    url: '/api/identity/users',
    method: 'post',
    data: payload
  })
}

export function updateUser(payload) {
  return request({
    url: `/api/identity/users/${payload.id}`,
    method: 'put',
    data: payload
  })
}

export function deleteUser(id) {
  return request({
    url: `/api/identity/users/${id}`,
    method: 'delete'
  })
}

export function getRolesByUserId(id) {
  return request({
    url: `/api/identity/users/${id}/roles`,
    method: 'get'
  })
}

export function getAssignableRoles() {
  return request({
    url: '/api/identity/users/assignable-roles',
    method: 'get'
  })
}
