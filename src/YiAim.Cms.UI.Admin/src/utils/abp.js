import store from '@/store'
import router, { resetRouter } from '@/router'

const baseListQuery = {
  page: 1,
  limit: 10,
  sort: undefined,
  filter: undefined
}

export function transformAbpListQuery(query) {
  query.filter = query.filter === '' ? undefined : query.filter

  const abpListQuery = {
    maxResultCount: query.limit,
    skipCount: (query.page - 1) * query.limit,
    sorting:
      query.sort && query.sort.endsWith('ending')
        ? query.sort.replace('ending', '')
        : query.sort,
    ...query
  }

  delete abpListQuery.page
  delete abpListQuery.limit
  delete abpListQuery.sort

  return abpListQuery
}

function shouldFetchAppConfig(providerKey, providerName) {
  const currentUser = store.getters.abpConfig.currentUser

  if (providerName === 'R') { return currentUser.roles.some(role => role === providerKey) }

  if (providerName === 'U') return currentUser.id === providerKey

  return false
}

export function fetchAppConfig(providerKey, providerName) {
  if (shouldFetchAppConfig(providerKey, providerName)) {
    store.dispatch('app/applicationConfiguration').then(abpConfig => {
      resetRouter()
      store.dispatch('user/setRoles', abpConfig.currentUser.roles)
      const grantedPolicies = abpConfig.auth.grantedPolicies
      store
        .dispatch('permission/generateRoutes', grantedPolicies)
        .then(accessRoutes => {
          router.addRoutes(accessRoutes)
        })
    })
  }
}

export function checkPermission(policy) {
  const abpConfig = store.getters.abpConfig
  if (abpConfig.auth.grantedPolicies[policy]) {
    return true
  } else {
    return false
  }
}

export default baseListQuery
