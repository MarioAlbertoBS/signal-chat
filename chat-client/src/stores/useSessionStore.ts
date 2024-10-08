import { defineStore } from 'pinia';

const useSessionStore = defineStore('session', {
    state: () => { 
        let isAuthenticated: boolean = false;
        let userData: UserData | null = null;

        const storedData = localStorage.getItem("userData");
        if(storedData) {
            userData = JSON.parse(storedData);
            isAuthenticated = true;
        }

        return {
            isAuthenticated: isAuthenticated,
            user: userData
        };
    },
    actions: {
        login(userData: UserData | null) {
            this.isAuthenticated = true,
            this.user = userData
        },
        logout() {
            this.isAuthenticated = false,
            this.user = null
        }
    }
})

export interface UserData {
    id: string,
    userName: string,
    token: string
}

export default useSessionStore;