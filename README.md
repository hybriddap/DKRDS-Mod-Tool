Diddy Kong Racing DS Mod Tool
=======

An open-source modding tool for **Diddy Kong Racing DS**.

Currently, it supports:
- Viewing basic asset data
- Viewing basic track data
- Viewing track textures

I will implement further functionality throughout development.

## Preparation Instructions
- Have the DKRDS Rom and Tinke Downloaded
- Open the Rom with Tinke
- Locate the assets.bin file:
  ```bash
  root\game\assets.bin
- Extract it for later use with the DKRDS-Mod-Tool

## Build Instructions

This is a **Windows Forms (.NET)** application. To build it:

1. **Clone the repository**:
   ```bash
   https://github.com/hybriddap/DKRDS-Mod-Tool.git
   cd DKRDS-Mod-Tool
2. **Open the solution in Visual Studio**:
- Open DiddyKongModdingView.sln with Visual Studio 2022 or newer.
3. **Ensure the correct SDK is installed**:
- This project targets .NET 8.0.
- Install the .NET 8 SDK if not already installed.
4. **Build the project**:
- Select Debug or Release configuration.
- Press Ctrl+Shift+B to build.
- The executable will be located in:
  ```bash
  bin\Debug\net8.0-windows\

## Credits
Major credits to <a href="https://www.youtube.com/@AtlasOmegaAlpha">Atlas</a> for his extensive research and documentation on this game. This project would not be possible without him.

## License

This project is licensed under the [MIT License](LICENSE.txt) - see the LICENSE file for details.
