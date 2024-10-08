<script setup lang="ts">
    import { ref, onMounted } from 'vue';
    import { useRouter } from 'vue-router';
    import { logoutUser } from '../helpers/AuthenticationHelper';
    import { getRoomMessages, Message } from '../helpers/Api/MessageRequestHelper';
    import ChatHistory from '../components/Home/ChatHistory.vue';
    import Header from '../components/Home/Header.vue';
    import Comment from '../components/Home/Comment.vue';
    import * as signalR from '@microsoft/signalr';

    const chatHistory = ref<Message[]>([]);

    const router = useRouter();

    async function logout() {
        logoutUser();

        router.push({name: 'Login'});
    }

    async function loadHistory() {
        const messages = await getRoomMessages("Default");
        chatHistory.value = messages;
    }

    function connectChatHub() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5000/chatHub")
            .build();

        connection.on("ReceiveMessage", function (message: Message) {
            // Reload history
            chatHistory.value.push(message)
        })

        connection.start().then(function() {
            console.log("Connected to ChatHub");

            connection.invoke("JoinRoom", "38fbece8-4f64-4329-95e6-f73672d6fc2c")
                .catch(err => console.error(err));
        }).catch(err => console.error(err.toString()));
    }

    onMounted(() => {
        connectChatHub();
        loadHistory();
    });
</script>

<template>
    <div>
        <Header @logout="logout"></Header>
        <ChatHistory :chatHistory="chatHistory"></ChatHistory>
        <Comment />
    </div>
</template>