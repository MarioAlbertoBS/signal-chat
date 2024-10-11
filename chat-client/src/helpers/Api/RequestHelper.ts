const baseUrl = 'http://localhost:5000/api';

export async function makeRequest(url: string, method: string = 'GET', body: string|null = null, headers = {}) {
    try {
        const response = await fetch(baseUrl.concat(url), {
            method: method,
            headers: {
                'Content-Type': 'application/json',
                ...headers
            },
            body: body
        });

        return response;
    } catch (error) {
        console.error(error);
    }

    return null;
}

async function requestWithBody(url: string, method: string = 'GET', body: string|null = null, headers = {}) {
    try {
        const response = await makeRequest(url, method, body, headers);

        return await response?.json();
    } catch (error) {
        console.error(error);
    }

    return null;
}

export default requestWithBody;