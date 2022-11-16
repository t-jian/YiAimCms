import request from '@/utils/abpRequest'
export function getAuthUrl(type) {
    return request({
        url: `/oauth/${type}`,
        method: 'get',
        headers: { 'Content-Type': 'application/json' },
    })
}

export function thirdAuthLogin(type,data){
    return request({
        url: `oauth/${type}/thirdAuthLogin?${data}`,
        method: 'get',
        headers: { 'content-type': 'application/json' },
    })
}