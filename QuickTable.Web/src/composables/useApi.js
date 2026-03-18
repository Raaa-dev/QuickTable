const BASE = 'https://quicktable-production.up.railway.app/api/v1'

export async function fetchAll(endpoint) {
  const res = await fetch(`${BASE}${endpoint}?pageSize=300`)
  const json = await res.json()
  return json.data || []
}

export async function createRecord(endpoint, payload) {
  const res = await fetch(`${BASE}${endpoint}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  })
  if (!res.ok) throw new Error('Server error ' + res.status)
  return res.json()
}

export async function updateRecord(endpoint, id, payload) {
  const res = await fetch(`${BASE}${endpoint}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  })
  if (!res.ok) throw new Error('Server error ' + res.status)
  return res.json()
}

export async function deleteRecord(endpoint, id) {
  const res = await fetch(`${BASE}${endpoint}/${id}`, { method: 'DELETE' })
  if (!res.ok) throw new Error('Server error ' + res.status)
}
