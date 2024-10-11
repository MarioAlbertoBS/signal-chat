import { defineStore } from 'pinia';
import * as crypto from 'crypto-ts';

const secretKey = 'secret';

const useSessionStore = defineStore('session', {
    state: () => { 
        let isAuthenticated: boolean = false;
        let userData: UserData | null = null;

        const storedData = localStorage.getItem("userData");
        if(storedData) {
            const decryptedData = crypto.AES.decrypt(storedData, secretKey);
            userData = JSON.parse(decryptedData.toString(crypto.enc.Utf8));
            isAuthenticated = true;
        }

        return {
            isAuthenticated: isAuthenticated,
            user: userData
        };
    },
    actions: {
        login(userData: UserData | null) {
            this.isAuthenticated = true;
            this.user = userData;

            const cypherText = crypto.AES.encrypt(JSON.stringify(userData), secretKey);

            localStorage.setItem("userData", cypherText.toString());
        },
        logout() {
            this.isAuthenticated = false;
            this.user = null;

            localStorage.removeItem("userData");
        }
    }
})

export interface UserData {
    id: string,
    userName: string,
    token: string
}

export default useSessionStore;