#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <signal.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <string.h>
#include <time.h>
#include <sys/select.h>

int running = 1;
int listen_socket;
time_t last_heartbeat;
int heartbeat_count = 0;
const int TIMEOUT_SECONDS = 5;

void signal_handler(int signum) {
    printf("[MONITOR] Shutting down...\n");
    running = 0;
    close(listen_socket);
    exit(signum);
}

void setup_socket() {
    listen_socket = socket(AF_INET, SOCK_DGRAM, 0);
    if (listen_socket < 0) {
        printf("[MONITOR] Failed to create socket\n");
        exit(1);
    }
    
    struct sockaddr_in server_addr;
    memset(&server_addr, 0, sizeof(server_addr));
    server_addr.sin_family = AF_INET;
    server_addr.sin_addr.s_addr = INADDR_ANY;
    server_addr.sin_port = htons(8081);
    
    if (bind(listen_socket, (struct sockaddr*)&server_addr, sizeof(server_addr)) < 0) {
        printf("[MONITOR] Failed to bind socket\n");
        exit(1);
    }
    
    printf("[MONITOR] Listening on port 8081\n");
}

void check_for_heartbeat() {
    char buffer[256];
    struct sockaddr_in client_addr;
    socklen_t client_len = sizeof(client_addr);
    
    fd_set readfds;
    FD_ZERO(&readfds);
    FD_SET(listen_socket, &readfds);
    
    struct timeval timeout;
    timeout.tv_sec = 1;
    timeout.tv_usec = 0;
    
    int result = select(listen_socket + 1, &readfds, NULL, NULL, &timeout);
    
    if (result > 0 && FD_ISSET(listen_socket, &readfds)) {
        int bytes_received = recvfrom(listen_socket, buffer, sizeof(buffer) - 1, 0,
                                     (struct sockaddr*)&client_addr, &client_len);
        
        if (bytes_received > 0) {
            buffer[bytes_received] = '\0';
            
            if (strstr(buffer, "HEARTBEAT:lidar") != NULL) {
                last_heartbeat = time(NULL);
                heartbeat_count++;
                
                // Log first 5 heartbeats, then every 10th
                if (heartbeat_count <= 5 || heartbeat_count % 10 == 0) {
                    printf("[MONITOR] Heartbeat #%d received from LIDAR\n", heartbeat_count);
                }
            }
        }
    }
}

void check_timeout() {
    time_t current_time = time(NULL);
    
    if (current_time - last_heartbeat > TIMEOUT_SECONDS) {
        printf("[MONITOR] ERROR: LIDAR process timeout detected!\n");
        printf("[MONITOR] No heartbeat received for %ld seconds\n", current_time - last_heartbeat);
        printf("[MONITOR] LIDAR process may have crashed or failed\n");
        
        // Reset timer to avoid spam
        last_heartbeat = current_time;
    }
}

int main() {
    signal(SIGINT, signal_handler);
    signal(SIGTERM, signal_handler);
    
    printf("=== HEARTBEAT MONITOR PROCESS ===\n");
    printf("[MONITOR] Starting heartbeat monitor\n");
    
    setup_socket();
    last_heartbeat = time(NULL);
    
    while (running) {
        check_for_heartbeat();
        check_timeout();
    }
    
    close(listen_socket);
    return 0;
} 