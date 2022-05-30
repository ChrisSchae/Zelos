FROM postgres:14-alpine as db
WORKDIR /app
COPY ./scripts/prefill.sql ./scripts/init.sql
COPY ./scripts/init.sh /docker-entrypoint-initdb.d