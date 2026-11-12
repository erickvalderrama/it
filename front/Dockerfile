# syntax=docker/dockerfile:1
FROM ghcr.io/cirruslabs/flutter:3.41.2 AS build
WORKDIR /app

COPY pubspec.yaml pubspec.lock ./
RUN flutter pub get

COPY . .
ARG API_BASE_URL
RUN flutter build web --release --dart-define=API_BASE_URL=${API_BASE_URL}

FROM nginx:1.29-alpine AS runtime
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/build/web /usr/share/nginx/html
EXPOSE 80
