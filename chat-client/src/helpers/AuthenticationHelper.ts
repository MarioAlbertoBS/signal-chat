import useSessionStore from "../stores/useSessionStore";
import { makeRequest } from "./Api/RequestHelper";
import { loginRequest, registerRequest } from "./Api/AuthenticationRequestHelper";

export async function loginUser(userName: string, password: string): Promise<boolean> {
    const userData = await loginRequest(userName, password);

    if (userData === null) {
        return false;
    }

    const sessionStore = useSessionStore();
    sessionStore.login(userData);

    return true;
}

export async function registerUser(userName: string, password: string): Promise<boolean> {
    const isRegistered = await registerRequest(userName, password);

    if (!isRegistered) {
        return false;
    }

    // Login user after register it
    return await loginUser(userName, password);
}

export function logoutUser() {
    const sessionStore = useSessionStore();
    
    sessionStore.logout();
}

export async function validateToken() {
    const sessionStore = useSessionStore();
    if (!sessionStore) {
        console.error("No Session detected");
        return false;
    }

    const token = sessionStore.$state.user?.token;
    const response = await makeRequest(`/validate-token`, 'GET', null, {
        'Authorization': `Bearer ${token}`
    });

    if (response?.status !== 200) {
        sessionStore.logout();
        return false;
    }

    return true;
}