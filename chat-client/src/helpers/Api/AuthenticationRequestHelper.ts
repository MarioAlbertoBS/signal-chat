import { UserData } from "../../stores/useSessionStore";
import request from './RequestHelper';

export async function loginRequest(username: string, password: string): Promise<UserData | null> {
    const responseBody = await request('/login', 'POST', JSON.stringify({
        userName: username,
        password: password
    }));

    if (responseBody === null || !responseBody.token) {
        return null;
    }

    return responseBody;
}

export async function registerRequest(username: string, password: string): Promise<boolean> {
    const responseBody = await request('/register', 'POST', JSON.stringify({
        userName: username,
        password: password
    }));

    if (responseBody === null || !responseBody.id) {
        return false;
    }

    return true;
}