import useSessionStore from "../stores/useSessionStore";
import { loginRequest, registerRequest } from "./Api/AuthenticationRequestHelper";

export async function loginUser(userName: string, password: string): Promise<boolean> {
    const userData = await loginRequest(userName, password);

    if (userData === null) {
        return false;
    }

    const sessionStore = useSessionStore();
    sessionStore.login(userData);

    // Save session data in local storage
    localStorage.setItem("userData", JSON.stringify(userData));

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
    
    localStorage.removeItem("userData");
    sessionStore.logout();
}