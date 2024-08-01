# MiniLobby

MiniLobby is a .NET Core Web API project designed to manage lobby systems for multiplayer games or collaborative applications. This project is a recreation of the Unity Lobby service, leveraging Entity Framework and SQL Server to handle data persistence and interactions.

## Features

- **Lobby Management**: Create and delete (only host) lobbies.
- **Lobby Data Management**: Update and delete lobby data. (only host)
- **Member Management**: Join or leave a lobby.
- **Member Data Management**: Update and delete member data. (data owner)
- **Data Visibility Options**: Member or lobby data can be marked as Public, Private, or Member, so data is filtered depending on the requester.
- **Host Migration**: If host migration is enabled for a lobby, then on the host leaving, another is chosen at random (if available).
- **Controllers and Repositories**: Clean separation of concerns with controllers managing HTTP requests and repositories handling data operations.

## Technologies Used

- .NET Core Web API
- Entity Framework Core
- SQL Server
- C#

## Getting Started

### Prerequisites

- .NET 8.0 SDK

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/Suleman275/MiniLobby.git
    cd MiniLobby
    ```
2. Configure the database connection string in `appsettings.json`.

3. Apply migrations to set up the database:
    ```sh
    dotnet ef database update
    ```

4. Run the application:
    ```sh
    dotnet watch run
    ```

## Testing

The project includes a Postman test collection and environment to facilitate API testing. You can find these files in the `Postman Tests` folder.

### Running Tests

1. Import the Postman collection and environment:
    - Open Postman.
    - Click on `Import` in the top-left corner.
    - Select the files in the `Postman Tests` folder.

2. Configure the environment:
    - Update the environments according to your needs. (Lobby IDs are generated randomly)

3. Run the requests:
    - Select the imported collection and then a request.
    - Click on `Send` to execute your chosen request.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any changes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
