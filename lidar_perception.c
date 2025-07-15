#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <signal.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <string.h>
#include <time.h>

int running = 1;
int heartbeat_socket;

void signal_handler(int signum) {
    printf("[LIDAR] Shutting down...\n");
    running = 0;
    close(heartbeat_socket);
    exit(signum);
}

void setup_socket() {
    heartbeat_socket = socket(AF_INET, SOCK_DGRAM, 0);
    if (heartbeat_socket < 0) {
        printf("[LIDAR] Failed to create socket\n");
        exit(1);
    }
}

void send_heartbeat() {
    struct sockaddr_in monitor_addr;
    memset(&monitor_addr, 0, sizeof(monitor_addr));
    monitor_addr.sin_family = AF_INET;
    monitor_addr.sin_port = htons(8081);
    monitor_addr.sin_addr.s_addr = inet_addr("127.0.0.1");
    
    char msg[64];
    snprintf(msg, sizeof(msg), "HEARTBEAT:lidar:%ld", time(NULL));
    
    sendto(heartbeat_socket, msg, strlen(msg), 0,
           (struct sockaddr*)&monitor_addr, sizeof(monitor_addr));
    printf("[LIDAR] Heartbeat sent\n");
}

int main() {
    signal(SIGINT, signal_handler);
    signal(SIGTERM, signal_handler);
    
    printf("=== LIDAR PERCEPTION PROCESS ===\n");
    printf("[LIDAR] Starting LIDAR perception module\n");
    
    setup_socket();
    
    int cycle_count = 0;
    srand(time(NULL));
    
    while (running) {
        // Check for random failure (1% chance)
        if (rand() % 100 == 0) {
            printf("[LIDAR] ERROR: LIDAR sensor failure!\n");
            printf("[LIDAR] FATAL ERROR: LIDAR sensor malfunction\n");
            close(heartbeat_socket);
            return 1;
        }
        
        // Process LIDAR data
        usleep(100000); // 100ms
        
        // Send heartbeat every 10 cycles
        if (cycle_count % 10 == 0) {
            send_heartbeat();
        }
        
        cycle_count++;
    }
    
    close(heartbeat_socket);
    return 0;
} 