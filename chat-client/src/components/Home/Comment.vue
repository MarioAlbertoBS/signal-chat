<script setup lang="ts">
    import { ref } from 'vue';
    import { sendMessageRequest } from '../../helpers/Api/MessageRequestHelper';
    const message = ref('');

    async function sendMessage(_event: Event) {
        if(!await sendMessageRequest(message.value)) {
            alert("Error sending the message.");
            return;
        }

        message.value = '';
    }
</script>

<template>
    <form v-on:submit.prevent="sendMessage">
        <div class="col-span-full">
            <div class="mt-2">
                <textarea id="message" v-model="message" rows="3" class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6" placeholder="Say something..."/>
            </div>
        </div>

        <div class="mt-6 flex items-center justify-end gap-x-6">
            <button type="submit" class="rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600">Send</button>
        </div>
    </form>
</template>