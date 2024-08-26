FROM node:20 AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

EXPOSE 80

# CMD [ "node", "index.js" ]