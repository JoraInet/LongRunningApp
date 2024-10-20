
FROM node:22.9
WORKDIR /app/LongRunningApp.WebApp/
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build --omit=dev
RUN npm install -g http-server
CMD ["http-server", "dist/long-running-app.web-app/browser/", "-p", "4200"]