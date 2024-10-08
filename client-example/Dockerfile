# Step 1: Build the application
# Use an official Node.js image as the build environment
FROM node:18 AS build-stage

# Set the working directory inside the container
WORKDIR /app

# Copy the package.json and package-lock.json (if available)
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the app for production
RUN npm run build

# Step 2: Serve the application
# Use an official Nginx image as the serving environment
FROM nginx:stable-alpine AS production-stage

# Copy the built application from the previous step
COPY --from=build-stage /app/dist /usr/share/nginx/html

# Copy custom Nginx configuration file if you have one
COPY nginx.conf /etc/nginx/nginx.conf

# Expose port 80 for the application
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
