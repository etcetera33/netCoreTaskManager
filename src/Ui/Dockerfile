#FROM node:11.6.0-alpine AS builder
#COPY . ./app
#WORKDIR /app
#RUN npm i
#RUN $(npm bin)/ng build --prod
#
#FROM nginx:1.15.8-alpine
#COPY --from=builder /app/dist/app/ /usr/share/nginx/html

#FROM nginx:1.13.3-alpine
### Remove default nginx website
#RUN rm -rf /usr/share/nginx/html/*
### From 'builder' stage copy over the artifacts in dist folder to default nginx public folder
#COPY /dist /usr/share/nginx/html
#CMD ["nginx", "-g", "daemon off;"]

# Stage 0, "build-stage", based on Node.js, to build and compile the frontend
FROM tiangolo/node-frontend:10 as build-stage
WORKDIR /app
COPY package*.json /app/
RUN npm install
COPY ./ /app/
ARG configuration=production
RUN npm run build -- --output-path=./dist/out --configuration $configuration
# Stage 1, based on Nginx, to have only the compiled app, ready for production with Nginx
FROM nginx:1.15
COPY --from=build-stage /app/dist/out/ /usr/share/nginx/html
# Copy the default nginx.conf provided by tiangolo/node-frontend
COPY --from=build-stage /nginx.conf /etc/nginx/conf.d/default.conf