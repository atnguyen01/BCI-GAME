# The Consultant’s Case of Concentration
This project aims to incorporate BCI via fNIRS into a simple text-based AR game.

It is March 8th, 1930, London. You are a private consultant, working in your office as usual, when you get a phone call from a local detective asking for your help in a case of murder. Your job is to help the detective identify suspicious or note-worthy clues gathered from each of the suspects, all the while making sure your mind doesn’t stray, lest you miss some important details.

## Features
- A short, 15 minute AR game, including physical objects as clues
- Depending on how focused you are, gives different dialogue and player choices
- Detects your focus state via fNIRS headset

NOTE: Currently incomplete, using a random number generator to simulate focus state.

For instructions on how to run the game, either on Unity Desktop or HoloLens, please refer to the MRTK-fNIRS section below.



# MRTK-fNIRS
Derived from: https://github.com/WPIHCILab/Unity-fNIRS/tree/main

## Features
- TCP client that receives processed fNIRS data from Turbo Satori
- Real-time visualization of fNIRS data in Unity or HoloLens using Unity's built-in graphics tools and Mixed Reality Toolkit
- Data logging and export functionality for offline analysis

## Getting Started
To get started with MRTK-fNIRS, you will need:
- Unity 2019.4 or later
- Visual Studio 2022.3.8f1 (untested with other versions)
- [Turbo Satori](https://nirx.net/turbosatori) with valid license
- (For real-time fNIRS processing) A secured network connection between Turbo Satori and compatible platforms (NIRScout and NIRSport)
- Microsoft HoloLens 2

Once you have these components, you can clone this repository and open the Unity project. From there, you can run the project and start receiving fNIRS data from Turbo Satori.

## Before you use
Make sure that the device running Turbo-Satori, MRTK-fNIRS, and the HoloLens are all on the same WiFi network, and that the port number field is the same between Turbo-Satori and MRTK-fNIRS (“55555” by default in MRTK-fNIRS).
Also make sure that your HoloLens is in developer mode, which can be turned on in Settings -> Update & Security -> For Developers.

## How to Use
If you want to test/view MRTK-fNIRS in Unity:
- Import the project into Unity.
- Open the “TSI Network Interface” script and in the Start() method, input the IP address of the device running Turbo-Satori into the ipAddress variable.
- Run Turbo-Satori (either real-time or a simulation), then run this (Unity) app.

If you want to view MRTK-fNIRS on HoloLens, first, follow the instructions above on inputting IP address into Unity. Then, you will need to build it in Unity and deploy it to the HoloLens. First, import the project into Unity and go to File -> Build Settings. Select “Universal Windows Platform” for Platform, and click “Add Open Scenes.” Make sure the settings are as followed:
- (optional) Target Device: HoloLens
- Architecture: ARM64
- Build Type: D3D Project
- Target SDK Version: Latest Installed
- Minimum Platform Version: 10.0.10240.0
- Visual Studio Version: Latest Installed
- Build and Run on: Release

Click “Build” and create or select a folder to hold the build. Once the build is finished, enter the folder and find and open the .sln file with Visual Studio. The settings at the top should be as followed:
- Solution Configuration: Release
- Solution Platforms/Architecture: ARM64
- Run (green triangle): Remote Machine

Next, go to Project -> Properties, then Configuration Properties -> Debugging. Enter the HoloLens IP address, which you can obtain by asking the HoloLens “what is my IP address,” click “OK,” and run by going to Debug -> Start Without Debugging. If your device has not paired with the HoloLens before, Visual Studio will ask for a PIN code, which you can get from the HoloLens by going to Settings -> Update & Security -> For Developers and clicking “Pair.” Enter the PIN into Visual Studio.
Once Visual Studio is done deploying, MRTK-fNIRS should be installed on your HoloLens and automatically open.
