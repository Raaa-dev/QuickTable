const BASE = 'https://quicktable-production.up.railway.app/api/v1'

const request = async (endpoint, options = {}) => {
  const res = await fetch(`${BASE}${endpoint}`, {
    credentials: 'include',
    headers: { 'Content-Type': 'application/json' },
    ...options,
    body: options.body ? JSON.stringify(options.body) : undefined,
  })
  if (!res.ok) throw new Error('Server error ' + res.status)
  return res.status === 204 ? null : res.json()
}

export const fetchAll     = async (endpoint)              => { const json = await request(`${endpoint}?pageSize=300`); return json?.data || [] }
export const createRecord = (endpoint, payload)           => request(endpoint, { method: 'POST', body: payload })
export const updateRecord = (endpoint, id, payload)       => request(`${endpoint}/${id}`, { method: 'PUT', body: payload })
export const deleteRecord = (endpoint, id)                => request(`${endpoint}/${id}`, { method: 'DELETE' })