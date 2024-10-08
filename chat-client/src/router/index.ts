import { createRouter, createWebHistory } from 'vue-router';
import useSessionStore from '../stores/useSessionStore';

const routes = [
    {
        path: '/',
        name: 'Home',
        component: () => import('../views/Home.vue')
    },
    {
        path: '/login',
        name: 'Login',
        component: () => import('../views/Login.vue')
    },
    {
        path: '/register',
        name: 'Register',
        component: () => import('../views/Register.vue')
    },

    // 404 default routes
    {
        path: '/:pathMatch(.*)*',
        name: 'NotFound',
        component: () => import('../views/NotFound.vue')
    }
];

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes
});

router.beforeEach((to, _from, next) => {
    const sessionStore = useSessionStore();

    // If access to home and the user is not authenticated, redirect to Login
    if (to.name === 'Home' && !sessionStore.isAuthenticated) {
        next({name: 'Login'});
    } else if ((to.name === 'Login' || to.name === 'Register') && sessionStore.isAuthenticated) {
        next({name: 'Home'});
    }
    else {
        next();
    }
});

export default router;