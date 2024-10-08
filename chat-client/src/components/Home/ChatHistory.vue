<script setup lang="ts">
    import { defineProps } from 'vue';
    import icon from '../../assets/user.png';
    
    defineProps(['history']);

  const history = [
    {
      name: 'Leslie Alexander'
    },
    {
      name: 'Michael Foster'
    },
    {
      name: 'Dries Vincent'
    },
    {
      name: 'Lindsay Walton'
    }
  ]
</script>

<template>
    <ul role="list" class="divide-y divide-gray-100">
      <li v-for="person in history" class="flex justify-between gap-x-6 py-5">
        <div class="flex min-w-0 gap-x-4">
          <img class="h-12 w-12 flex-none rounded-full bg-gray-50" :src="icon" alt="" />
          <div class="min-w-0 flex-auto">
            <p class="text-sm font-semibold leading-6 text-gray-900">{{ person.name }}</p>
            <p class="mt-1 text-sm leading-5 text-gray-800">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
            </p>
          </div>
        </div>
      </li>
    </ul>
</template>