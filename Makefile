CC = gcc
CFLAGS = -Wall -std=c99

all: lidar_perception heartbeat_monitor

lidar_perception: lidar_perception.c
	$(CC) $(CFLAGS) -o lidar_perception lidar_perception.c

heartbeat_monitor: heartbeat_monitor.c
	$(CC) $(CFLAGS) -o heartbeat_monitor heartbeat_monitor.c

clean:
	rm -f lidar_perception heartbeat_monitor

.PHONY: all clean 