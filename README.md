# Heartbeat System Assignment

Two-process system where LIDAR perception sends heartbeats to a monitor process.

I have left the compiled files so they can be directly executed. Just in case C++ compilation fails on anyone else's machine.

## Files
- `lidar_perception.c` - LIDAR perception process
- `heartbeat_monitor.c` - Heartbeat monitor process
- `Makefile` - Build configuration

## Build
```bash
make
```

## Run (in 2 separate terminal windows)

**Terminal 1 (Monitor):**
```bash
./heartbeat_monitor
```

**Terminal 2 (LIDAR):**
```bash
./lidar_perception
```

## How it works
- LIDAR sends heartbeats every 1 second to port 8081
- Monitor logs first 5 heartbeats, then every 10th
- Monitor detects timeout if no heartbeat for 5 seconds
- LIDAR has 1% random failure chance per cycle
- When LIDAR fails, monitor catches the timeout and reports error

## Clean up
```bash
make clean
``` 