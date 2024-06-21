# MicroServiceMessaging

This project demonstrates Event Driven Microservice Messaging. The architecture involves two key events: `ArticleCreatedEvent` and `ArticleViewedEvent`.

## Overview

### Events
- **ArticleCreatedEvent**: Triggered when an article is created.
- **ArticleViewedEvent**: Triggered when an article is viewed.

### Services
1. **CreateArticle Service**: 
   - Functionality: Allows users to create new articles.
   - Event: Publishes `ArticleCreatedEvent` upon article creation.
   
2. **Newsletter API**:
   - Functionality: Retrieves article information.
   - Event: Publishes `ArticleViewedEvent` when an article is accessed.

### Consumers
1. **ArticleCreatedConsumer**:
   - Consumes `ArticleCreatedEvent`.
   - Action: Writes article creation data to the database.

2. **ArticleViewedConsumer**:
   - Consumes `ArticleViewedEvent`.
   - Action: Logs article view information to the database.

## How It Works
1. **Article Creation**:
   - User creates an article via the `CreateArticle` service.
   - This triggers `ArticleCreatedEvent`.
   - The `ArticleCreatedConsumer` processes this event and stores the data in the database.

2. **Article Viewing**:
   - User views an article via the `Newsletter API`.
   - This triggers `ArticleViewedEvent`.
   - The `ArticleViewedConsumer` processes this event and logs the view in the database.

## Technologies Used
- .NET for backend development.
- Event-driven architecture for scalable and decoupled service communication.
- Message brokers (e.g., RabbitMQ, Kafka) for event handling.

## Getting Started
1. Clone the repository:
   ```bash
   git clone https://github.com/alikrc/MicroServiceMessaging.git
   ```
2. Set up the necessary environment variables and configuration files.
3. Build and run the services using your preferred method (e.g., Docker, .NET CLI).

## Future Enhancements

- Implement additional events for more granular tracking.
- Add more consumers for advanced processing and analytics.
- Integrate with a front-end application for a complete user experience.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.
License

This project is licensed under the MIT License. See the LICENSE file for details.

![image](https://github.com/alikrc/MicroServiceMessaging/assets/2185905/188ab2a5-a8da-43a9-ad66-1de40b128d4b)

![image](https://github.com/alikrc/MicroServiceMessaging/assets/2185905/a23a45c3-8103-472d-ab8d-971d795bca8c)

![image](https://github.com/alikrc/MicroServiceMessaging/assets/2185905/a1da5347-703c-45fe-9377-851a36fdd07e)

![image](https://github.com/alikrc/MicroServiceMessaging/assets/2185905/8fad2552-29cc-475a-95de-8a16ef9b2446)
