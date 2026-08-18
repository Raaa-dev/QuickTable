# Step 1 - Build Stage
FROM node:20-alpine AS build
WORKDIR /app

# Cache package installation
COPY package*.json ./
RUN npm ci || npm install

# Copy source code and build
COPY . .
ARG VITE_API_BASE_URL=""
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL
RUN npm run build

# Step 2 - Production Nginx Stage
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Clean default nginx files
RUN rm -rf ./*

# Copy built assets
COPY --from=build /app/dist .

# Copy custom Nginx configuration (reverse proxy + router fallback)
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]