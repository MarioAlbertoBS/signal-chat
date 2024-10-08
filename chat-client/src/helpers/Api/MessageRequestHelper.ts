import request from "./RequestHelper";
import useSessionStore from "../../stores/useSessionStore";

export const defaultRoom: string = '38fbece8-4f64-4329-95e6-f73672d6fc2c';

export async function sendMessageRequest(message: string): Promise<boolean> {
    
    const sessionStore = useSessionStore();
    if (!sessionStore) {
        console.error("No Session detected");
        return false;
    }

    const token = sessionStore.$state.user?.token;
    const messageResponse = await request('/messages', 'POST', JSON.stringify({
        message: message,
        roomId: defaultRoom
    }), {
        'Authorization': `Bearer ${token}`
    });

    if(messageResponse === null) {
        return false;
    }

    return true;
}

export async function getRoomMessages(roomName: string): Promise<Message[]> {
    const sessionStore = useSessionStore();
    if (!sessionStore) {
        console.error("No Session detected");
        return [];
    }

    const token = sessionStore.$state.user?.token;
    const response = await request(`/rooms/${roomName}/messages`, 'GET', null, {
        'Authorization': `Bearer ${token}`
    });

    if (response == null) {
        return [];
    }

    // return response.map((message: Message) => message);
    return response.messages;
}

export interface Message {
    id: number,
    user: string,
    createdAt: string,
    message: string
}