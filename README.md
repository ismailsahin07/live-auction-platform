# Distributed Live Auction Platform

A distributed live auction platform designed for high-throughput data ingestion, reliable transactional processing, and stateful caching. Built on a robust .NET architecture, this project integrates specialized open-source messaging and data persistence tools to handle real-time bidding at scale.

## Architecture Overview

This platform utilizes a dual-messaging approach, separating concerns based on data velocity and transactional requirements:

*   **Apache Kafka** handles the continuous, high-velocity stream of incoming bids where strict ordering and maximum throughput are essential.
*   **RabbitMQ** orchestrates individual, high-value commands (such as payment processing) where complex routing, dead-lettering, and reliable retry mechanisms are critical.

## Microservices Breakdown

*   **Catalog API**: Exposes CRUD operations for auction items. Backed by **MongoDB** (via EF Core) as the primary datastore. Implements **Redis** caching for upcoming and highly viewed items to minimize database load.
*   **Live Bidding Engine**: A high-performance service receiving incoming bids via **SignalR** or **gRPC**. It leverages **Redis distributed locks** to evaluate the current highest bid atomically, preventing race conditions. Approved bids are instantly published to a Kafka topic as an immutable, append-only stream.
*   **Auction Settlement Worker**: A background service that consumes the Kafka bid stream. Upon auction timer expiration, it computes the final winner and publishes an `AuctionCompleted` integration event to a RabbitMQ exchange.
*   **Payment & Notification Services**: Independent microservices subscribed to RabbitMQ queues. RabbitMQ ensures no lost transactions by managing dead-lettering and retry logic during payment gateway failures, before finally updating the order state in MongoDB via EF Core.

## The Data Lifecycle

1. **Bid Submission**: A user submits a bid routed through the API Gateway.
2. **Atomic Evaluation**: The Bidding Engine acquires a Redis lock for the specific auction ID, verifies the bid against the cached maximum, and securely updates the cache.
3. **Event Ingestion**: The raw, validated bid event is pushed to Kafka for decoupled, high-speed ingestion.
4. **Settlement**: When the auction closes, the Settlement Worker reads the final state from the stream and publishes a structured settlement message to RabbitMQ.
5. **Processing & Persistence**: The Payment Service consumes the RabbitMQ message, processes the financial transaction, and permanently updates the final order state in MongoDB using EF Core.

## Tech Stack

*   **Framework**: .NET, Entity Framework (EF) Core
*   **Communication**: gRPC, SignalR, API Gateway
*   **Database**: MongoDB
*   **Caching & Distributed Locks**: Redis
*   **Event Streaming**: Apache Kafka
*   **Message Broker**: RabbitMQ
*   **Containerization**: Docker & Docker Compose

## Local Development

You can orchestrate this entire environment locally using Docker. The provided Docker Compose configuration spins up the .NET APIs alongside the necessary infrastructure containers in a single command.

To start the environment, run:

```bash
docker-compose up -d
```
