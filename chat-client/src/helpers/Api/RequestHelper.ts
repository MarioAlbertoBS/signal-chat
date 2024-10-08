const baseUrl = 'http://localhost:5000/api';

async function request(url: string, method: string = 'GET', body: string = '', headers = {}) {
    try {
        const response = await fetch(baseUrl.concat(url), {
            method: method,
            headers: {
                'Content-Type': 'application/json',
                ...headers
            },
            body: body
        });

        return await response.json();
    } catch (error) {
        console.error(error);
    }

    return null;
}

export default request;