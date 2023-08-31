# Sending SMS Microservice

This microservice application is designed to listen to a message queue, send the received data to an API, and, based on the API's response, attempt to make another API call if necessary. In cases where multiple attempts fail, a dead letter message queue is utilized. Additionally, the application publishes notifications to an event bus to provide relevant updates.

## Problem Statement

Using a third-party API for sending SMS messages can be risky due to the high request volume and potential errors. To handle this, we need a setup that manages SMS requests separately, deals with errors, logs them for tracking, and incorporates a retry strategy when needed.

## Solution Approach

The application consists of the following components:
- `Message Queue`: 
    - A message queue system that clients can subscribe to as publishers to publish SMS commands.
    - The main service subscribes to the message queue as a consumer and listens to the queue for new SMS commands.
- `Main service`: A microservice, subscribers to the message queue as consumers and listens to the queue for new sms-sommand.
- `3rd party API`: An external Api to send short messages to users.
- `Dead letter Queue`: Second message queue to republish the failed messages into it for next try.
- `Dead letter service`: A microservice, subscribers to the dead letter queue as consumers and listens to the queue for new failed messages.
- `Event Publisher`: A service to publish SmsSent notifications into the global event bus.
- `Event Queue`: A queue in a global bus that is notified by the event publisher.
- `Logger`: A logger system to log all events and actions in the system in some levels as information, errors, and critical errors for maintenance purposes.
- `Notifier`: An prompt system to notify the admins and support users by email or some other ways about the critical exceptions in the system like when message queues are not reachable.

## Assumptions and Tradeoffs

- The application uses an in-memory message queue implementation for simplicity. In a real-world scenario, you would replace it with a real message queue system like RabbitMQ or Kafka.
- Error handling and edge cases are not fully addressed in this demonstration to keep the focus on the core concept.
- The application does not include comprehensive logging or extensive error handling due to its simple nature.

## Build and Run Instructions

1. Clone this repository to your local machine.
2. Open the solution file (`.sln`) in your preferred IDE (e.g., Visual Studio or Visual Studio Code).
3. Build the solution to ensure all dependencies are resolved.
4. Run the `Program` class to see a demonstration of sending and receiving messages using the in-memory message queue.

## Running Tests

Tests for this application are not included in this demonstration, but you can easily add unit tests using a testing framework like MSTest, NUnit, or xUnit.

## What's Next

Given more time, the following improvements could be made:
- Implementing real message queue providers like RabbitMQ or Kafka for better scalability and real-world usage.
- Adding comprehensive unit tests to ensure the correctness of the code.
- Enhancing error handling and addressing edge cases to improve the robustness of the application.
- Incorporating logging and monitoring to gain insights into the application's behavior.

## Contact

If you have any questions or suggestions, feel free to contact [your name] at [your email address].


