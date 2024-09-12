# Twitch Command Bot

This project is a Twitch bot that listens to chat commands and performs various mouse and keyboard actions on the host machine. It is built using C# and leverages the `TwitchLib.Client` library to interact with Twitch chat, as well as Windows APIs to simulate mouse and keyboard events.

## Features

- Responds to a set of predefined chat commands in a Twitch channel.
- Controls mouse movements and clicks, including left/right clicks and cursor movement.
- Simulates key presses for various keyboard inputs (e.g., `w`, `a`, `s`, `d`, `tab`, `q`, `e`, etc.).
- Timeout feature to limit the frequency of command executions.
- Supports customizable timeout duration and Twitch channel selection via command-line arguments.

## Prerequisites

- **Twitch OAuth Token**: You need to provide a valid Twitch OAuth token in the `Token.oauth` field for the bot to authenticate with Twitch. You can generate one [here](https://twitchapps.com/tmi/).
- **TwitchLib.Client**: Install the `TwitchLib.Client` NuGet package using the following command:
  ```bash
  Install-Package TwitchLib.Client
  ```

## Installation

1. Clone the repository to your local machine:
   ```bash
   git clone <repository-url>
   ```
   
2. Open the project in your preferred C# IDE (e.g., Visual Studio).

3. Install the required dependencies (`TwitchLib.Client`).

4. In the `Token.oauth` field, add your Twitch OAuth token as a string.

5. Build the project.

## Usage

To run the bot, you need to pass optional command-line arguments for custom configurations.

```bash
--timeout <duration>  # Set timeout duration between command executions in milliseconds (default: 10,000 ms).
--channel <channel>   # Specify the Twitch channel the bot should connect to (default: 'panda__ol').
```

### Example

```bash
dotnet run --timeout 15000 --channel my_twitch_channel
```

This example will set the timeout to 15 seconds and connect the bot to `my_twitch_channel`.

## Chat Commands

Here are the chat commands that the bot responds to:

| Command | Description |
|---------|-------------|
| `!w`    | Hold `W` key for 2 seconds (forward in most games). |
| `!a`    | Hold `A` key for 2 seconds (left movement). |
| `!s`    | Hold `S` key for 2 seconds (backward movement). |
| `!d`    | Hold `D` key for 2 seconds (right movement). |
| `!tab`  | Hold `Tab` key for 2 seconds. |
| `!b`    | Press `B` key. |
| `!f`    | Press `F` key. |
| `!q`    | Press `Q` key. |
| `!e`    | Press `E` key. |
| `!lmb`  | Simulate left mouse button click. |
| `!rmb`  | Simulate right mouse button click. |
| `!up`   | Move mouse cursor up. |
| `!down` | Move mouse cursor down. |
| `!left` | Move mouse cursor left. |
| `!right`| Move mouse cursor right. |
| `!spin` | Spin the mouse cursor in a circular motion. |
| `!buy`  | Simulate an in-game buy action by pressing `I` and clicking in specified locations. |
| `!stop` | Stop the bot from executing further commands. |
| `!help` | Display help message with available commands. |

## Notes

- The bot listens for commands in the specified Twitch chat and simulates input on the host machine.
- Ensure that you are running the bot in an environment where simulating keyboard and mouse inputs will not interfere with other activities.
- The `Timer` limits the frequency of command executions based on the configured timeout period.
