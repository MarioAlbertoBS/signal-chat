<script setup lang="ts">
    import { defineProps, ref, watch, nextTick } from 'vue';
    import icon from '../../assets/user.png';
    import { Message } from '../../helpers/Api/MessageRequestHelper';

    interface ChatHistoryProps  {
        chatHistory: Message[];
    };
    
    const props = defineProps<ChatHistoryProps>();

    const chatContainer = ref<HTMLElement | null>(null);

    watch(
        () => props.chatHistory,
        async () => {
            await nextTick();
            if (chatContainer.value) {
                chatContainer.value.scrollTop = chatContainer.value.scrollHeight;
            }
        },
        { deep: true }
    );
</script>

<template>
    <ul ref="chatContainer" role="list" class="divide-y divide-gray-100 h-64 overflow-y-auto">
      <li v-for="message in chatHistory" class="flex justify-between gap-x-6 py-5">
        <div class="flex min-w-0 gap-x-4">
          <img class="h-12 w-12 flex-none rounded-full bg-gray-50" :src="icon" alt="" />
          <div class="min-w-0 flex-auto">
            <p class="text-sm font-semibold leading-6 text-gray-900">{{ message.user }}</p>
            <p class="mt-1 text-sm leading-5 text-gray-800">
                {{ message.message }}
            </p>
          </div>
        </div>
      </li>
    </ul>
</template>