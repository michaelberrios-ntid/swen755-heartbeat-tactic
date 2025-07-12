# SWEN 755 - Heartbeat Tactic

This project implements the Heartbeat tactic for detecting failures in critical components like LIDAR, camera, and radar sensors. Each sensor runs in its own process and communicates with a central monitor over TCP.

The monitor sends heartbeat pings to each sensor. If a sensor fails or becomes unresponsive, the monitor tries to recover it. After multiple failures, the sensor enters fallback mode. During fallback, the monitor switches to a backup sensor if available, meanwhile, the primary sensor attempts to recover. 

The system uses real process separation, random failure simulation, and simple socket messaging. Built in C# with .NET console apps.

# Heartbeat Tactic Implementation
- **SensorService**: Simulates sensors (LIDAR, camera, radar) that run in separate processes.
- **Monitor**: Central component that monitors the sensors, sends heartbeat pings, and handles failures.

# Project Structure
```
HeartbeatTactic/
├── SensorService/        # Contains sensor implementations
│   ├── Program.cs        # Main entry point for the sensor service  (cluster of sensors)
│   ├── Sensor.cs         # Base class for sensors
│   ├── LidarSensor.cs    # LIDAR sensor implementation
│   ├── CameraSensor.cs   # Camera sensor implementation
│   └── RadarSensor.cs    # Radar sensor implementation
├── Monitor/              # Contains the monitor implementation
│   ├── Program.cs        # Main entry point for the monitor program 
├── Shared/               # Contains shared code and data structures
│   ├── SensorRegistry.cs # Registry of all sensors
│   └── SensorMessages.cs # Data structure for sensor messages
```

# Requirements
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