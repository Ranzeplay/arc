FROM alpine:3.21.3 AS dotnet-9-sdk
WORKDIR /build
RUN apk add dotnet9-sdk --no-cache

FROM alpine:3.21.3 AS rust-sdk
WORKDIR /build
RUN apk add rust cargo --no-cache

FROM dotnet-9-sdk AS compiler-build
WORKDIR /build
COPY --link ./compiler/ ./
RUN dotnet build

FROM rust-sdk AS runtime-build
WORKDIR /build
COPY --link ./runtime/ ./
RUN cargo build

FROM rust-sdk AS cli-build
WORKDIR /build
COPY --link ./cli/ ./
RUN cargo build
