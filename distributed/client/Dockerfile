# Stage 1: Build the application
FROM node:20.19.6-alpine AS build

# Set working directory
WORKDIR /app

# Install dependencies for native modules if needed
RUN apk add --no-cache python3 make g++

# Copy package files
COPY package.json package-lock.json* ./

# Install dependencies
RUN npm ci --legacy-peer-deps || npm install --legacy-peer-deps

# Copy the rest of the application files
COPY . .

# Replace .env with the appropriate one according to the environment
COPY .env.docker-build .env

# Build with increased memory limit
ENV NODE_OPTIONS="--max-old-space-size=4096"
RUN npm run build

# Stage 2: Create the final image
FROM node:20.19.6-alpine AS production

# Install serve to serve the static files
RUN npm install -g serve@14.2.4

# Set working directory
WORKDIR /app

# Copy only the built files from the previous stage
COPY --from=build /app/dist ./dist

# Expose the port
EXPOSE 3000

# Command to serve the app
CMD ["serve", "-s", "dist", "-l", "3000"]