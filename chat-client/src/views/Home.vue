<script setup lang="ts">
    import { useRouter } from 'vue-router';
    import { logoutUser } from '../helpers/AuthenticationHelper';
    import ChatHistory from '../components/Home/ChatHistory.vue';
    import Header from '../components/Home/Header.vue';
    import Comment from '../components/Home/Comment.vue';

    const router = useRouter();

    async function logout() {
        logoutUser();

        router.push({name: 'Login'});
    }
</script>

<template>
    <div>
        <Header @logout="logout"></Header>
        <ChatHistory></ChatHistory>
        <Comment />
    </div>
</template>