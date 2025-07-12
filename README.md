# SWEN 755 - Heartbeat Tactic

This project implements the Heartbeat tactic for detecting failures in critical components like LIDAR, camera, and radar sensors. Each sensor runs in its own process and communicates with a central monitor over TCP.

The monitor sends heartbeat pings to each sensor. If a sensor fails or becomes unresponsive, the monitor tries to recover it. After multiple failures, the sensor enters fallback mode.

The system uses real process separation, random failure simulation, and simple socket messaging to meet the assignment requirements. Built in C# with .NET console apps.

## Requirements
- .NET SDK 6.0 or later - https://dotnet.microsoft.com/en-us/download
- C# development environment
    - VSC with C# Extensions 
    - Or Visual Studio

# Compile and Run
## Start the Sensor Service (Cluster of Sensors)
### Terminal 1
```bash
dotnet run --project SensorService
```

## Start the Monitor
### Terminal 2
```bash
dotnet run --project Monitor
```

## Stop the Monitor
### Terminal 2
```bash
ctrl + C
``` 

## Stop the Sensor Service
### Terminal 1
```bash
ctrl + C
```