# Coin Gambling Problem Plugin for SCP: Secret Laboratory

This repository contains a plugin for SCP: Secret Laboratory designed to simulate a coin gambling system where players can receive random items based on a coin flip. This README provides instructions for setting up and installing the plugin on your SCP:SL server.

## Overview

The `CoinFlipLogger` plugin logs when a player flips a coin and occasionally rewards them with an item from a predefined list based on a 25% chance. The plugin uses a linear congruential generator (LCG) for pseudo-random number generation.
>[!WARNING]
>Will not work with EXILED

## Features

- **Coin Flip Logging:** Logs every coin flip event.
- **Item Reward System:** 25% chance to reward a player with a random item from a predefined list on each coin flip.
- **Non-SCP Items:** Includes various in-game items, excluding SCPs.

## Installation

Follow these steps to install the plugin on your SCP:SL server:

### 1. Download the Plugin

1. **Download the release**
      [Releases](https://github.com/Josephfallen/Coin-Gambling-Problem/releases/tag/Beta)

### 2. Install the Plugin
   
  1.  Locate the CoinFlipLogger.dll file in the output directory (e.g., bin/Release).

  2.  Copy the CoinFlipLogger.dll file to your SCP server’s plugins directory.


### 3. Restart the Server

 
 
   1. Restart your SCP server to load the new plugin. Ensure that the server starts without any errors related to plugin loading.
       Once the server is up, check the server logs to ensure that the CoinFlipLogger plugin has been successfully loaded. Look for an entry like:


    [Info] CoinFlipLogger plugin has been enabled.


### Usage

  Coin Flip Event: The plugin listens for coin flip events. Players’ coin flips will be logged, and there’s a 25% chance they will receive a random item from the list.
  Logging: The plugin logs both the coin flip event and any item rewards given to the players.

### Troubleshooting

   Plugin Not Loading:
       * Ensure the plugin DLL is correctly placed in the plugins directory.
       * Verify that your SCP server is running the correct version compatible with the plugin.

  Item Rewards Not Appearing:
       * Check the plugin logs for errors or configuration issues.

### Contributing

Feel free to fork the repository and make improvements. Submit pull requests to contribute changes or fixes.
License

This project is licensed under the MIT License. See the LICENSE file for more details.
### Contact

For any issues or further assistance, please contact the plugin author or check the repository issues page.
