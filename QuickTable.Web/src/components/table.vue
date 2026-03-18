<script setup lang="ts">
export interface Column {
  key: string
  label: string
}

const props = defineProps<{
  columns: Column[]
  rows: any[]
}>()

const emit = defineEmits(['edit', 'delete'])
</script>

<template>
  <div class="overflow-x-auto">
    <table class="table">
      
      <!-- HEADER -->
      <thead>
        <tr>
          <th v-for="col in columns" :key="col.key">
            {{ col.label }}
          </th>
          <th>Action</th>
        </tr>
      </thead>

      <!-- BODY -->
      <tbody>
        <tr v-for="row in rows" :key="row.id">
          <td v-for="col in columns" :key="col.key">
            {{ row[col.key] }}
          </td>

          <td class="flex gap-2">
            <button class="btn btn-xs btn-info" @click="emit('edit', row)">
              Edit
            </button>
            <button class="btn btn-xs btn-error" @click="emit('delete', row)">
              Delete
            </button>
          </td>
        </tr>
      </tbody>

    </table>
  </div>
</template>