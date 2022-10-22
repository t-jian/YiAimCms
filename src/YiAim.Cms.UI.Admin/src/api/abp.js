import request from '@/utils/abpRequest'

export function applicationConfiguration() {
  return request({
    url: '/api/abp/application-configuration',
    method: 'get'
  })
}

export function tenantsById(id) {
  return request({
    url: `/api/abp/multi-tenancy/tenants/by-id/${id}`,
    method: 'get'
  })
}

export function tenantsByName(name) {
  return request({
    url: `/api/abp/multi-tenancy/tenants/by-name/${name}`,
    method: 'get'
  })
}
