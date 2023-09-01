# Sending SMS Microservice

This microservice application is designed to listen to a message queue, send the received data to an API, and, based on the API's response, attempt to make another API call if necessary. In cases where multiple attempts fail, a dead letter message queue is utilized. Additionally, the application publishes notifications to an event bus to provide relevant updates.

## Problem Statement

Using a third-party API for sending SMS messages can be risky due to the high request volume and potential errors. To handle this, we need a setup that manages SMS requests separately, deals with errors, logs them for tracking, and incorporates a retry strategy when needed.

## Solution Approach

The application consists of the following components:
- **Message Queue**: 
    - A message queue system that clients can subscribe to as publishers to publish SMS commands.
    - The main service subscribes to the message queue as a consumer and listens to the queue for new SMS commands.
- **Main service**:
    - The Main Service functions as a microservice that subscribes to the Message Queue as consumers. It actively listens to the queue for incoming SMS commands.
- **3rd-party API**: 
    - An external API that is used to send SMS messages to users.
- **Dead letter Queue**: 
    - A second message queue that is used to republish failed messages for a retry attempt.
    - The dead letter service subscribes to the dead letter queue as a consumer and listens to the queue for new failed messages.
- **Dead letter service**:
    - The Dead Letter Service operates as a microservice that subscribes to the Dead Letter Queue as consumers. It monitors the queue for new failed messages, facilitating necessary actions.
- **Event Publisher**: 
    - A service that publishes SMSSent notifications to the global event bus.
- **Event Queue**:
    - A queue in the global event bus that is notified by the event publisher.
- **Logger**:
    - A logger system that logs all events and actions in the system at different levels, such as information, errors, and critical errors. This is for maintenance purposes.
- **Notifier**:
    - A system that notifies admins and support users of critical exceptions in the system, such as when message queues are not reachable. This can be done via email or other means.

## Architecture

In this section, we will discuss the architectural principles and patterns used in the development of the SMS Microservice, as outlined in the exercise.

![](/Architecture.png)

### Microservice Architecture

The SMS Microservice is designed following the Microservice Architecture pattern. This means that the application is structured as a set of loosely coupled and independently deployable services. In this case, the microservice is responsible for sending SMS messages to customers and operates independently within the larger M-KOPA ecosystem.

### Clean Architecture

The microservice adheres to Clean Architecture principles, emphasizing the separation of concerns and maintaining a clean and modular codebase. Clean Architecture divides the application into layers, including the core business logic, interfaces, and external dependencies. This separation allows for easier testing, maintainability, and adaptability.

#### Key Components of Clean Architecture:

- **Entities:** Represent core business objects, such as SMS messages.
- **Use Cases:** Contain application-specific business rules, including the logic for degueue Message Queue.
- **Interfaces (e.g., Controllers, Gateways):** Define input and output ports for communication with external systems specially for DI and Unit Testing.
- **Frameworks and Drivers:** Handle external concerns, such as interacting with the message queue and the 3rd-party SMS service.

### Dependency Injection

Dependency Injection (DI) is employed in the microservice to promote modularity and testability. With DI, components receive their dependencies from the outside, rather than creating them internally. This facilitates flexibility and allows different implementations of components to be injected, enabling future developers to choose and configure concrete implementations.

### Repository Pattern

The Repository Pattern is used to abstract data access and storage operations related to SMS messages. While the exercise doesn't provide explicit details about database interactions, the pattern would be essential for handling data access operations. The Repository Pattern provides a consistent and clean API for interacting with data sources, supporting the storage and retrieval of SMS-related information.

## Assumptions and Tradeoffs

- The application uses an in-memory message queue implementation for simplicity. A sample by RabbitMQ is already implemented but is disabled in `program.cs`, to enable it uncomment the RabbitGateways region inside block in `Program.cs` and comment InMemoryGateways region inside block.
    ```cs
    #region RabbitGateways
    // comment this block to disable RabbitMQ solution
    #endregion
    .
    .
    .
    #region InMemoryGateways
    // comment this block to disable InMemory solution
    #endregion
    ```
- Error handling and edge cases are not fully addressed in this demonstration to keep the focus on the core concept.
- The application does not include comprehensive logging or extensive error handling due to its simple nature.

## Build and Run Instructions

1. Extract the `SMSMicroservice.rar`.
2. Open the solution file (`.sln`) in your preferred IDE (e.g., Visual Studio or Visual Studio Code).
3. Build the solution to ensure all dependencies are resolved.
4. Open SMSApi in your command prompt and execute the `dotnet 
run` command to run a sample of the external SMS API separately.
    ```bash
    dotnet run
    ```
5. Run the `SMSMicroService` project to see a Swagger screen.
6. Execute `api/v1/queue/send` API to send some sample messages to the Message Queue (the number of messages can be configured in `appsettings.json`).
    ```json
      "Dummy": {
            "Count": "100"
        }
    ```

6.Observe a demonstration of sending messages using the in-memory message queue in both console log, **SMSAPi** & **SMSMicroService**.

## Running Tests

Tests for this application are included in two sections, which are written using xUnit.
1. Unit Tests:
    - The unit tests were implemented for RabbitMQ to encounter real challenges instead of InMemory Queue, *so running RabbitMQ service is needed*.
    - This section covers most of the units.
    - To prevent concurrency execution in the same queue, many of them are divided into different classes and have different queue names.    
    - Due to the complexity and time-consuming nature of real message queues, such as counting the messages in the queue, an In-Memory database have been created to store both failed and successful messages for testing purposes.
2. Integrated Tests:
    - Health checker API to determine if the service is running.
    - Happy path integrated testing.

## What's Next

Given more time, the following improvements could be made:
- Implementing other real message queue providers like AzureBus or Kafka for better scalability and real-world usage.
- Adding comprehensive unit tests to ensure the correctness of the code. For the time being, unit tests are only written for RabbitMQ.
- Enhancing error handling and addressing edge cases to improve the robustness of the application.
- Incorporating logging and monitoring to gain insights into the application's behavior.

## Contact

If you have any questions or suggestions, feel free to contact [Hamid Jolany] at [hjolany@gmail.com](http://hjolany.com).
  