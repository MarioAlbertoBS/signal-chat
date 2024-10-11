import { UserData } from "../../stores/useSessionStore";
import requestWithBody from './RequestHelper';

export async function loginRequest(username: string, password: string): Promise<UserData | null> {
    const responseBody = await requestWithBody('/login', 'POST', JSON.stringify({
        userName: username,
        password: password
    }));

    if (responseBody === null || !responseBody.token) {
        return null;
    }

    return responseBody;
}

export async function registerRequest(username: string, password: string): Promise<boolean> {
    const responseBody = await requestWithBody('/register', 'POST', JSON.stringify({
        userName: username,
        password: password
    }));

    if (responseBody === null || !responseBody.id) {
        return false;
    }

    return true;
}