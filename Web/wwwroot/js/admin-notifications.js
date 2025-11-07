// Admin Chat Notification System
(function() {
    'use strict';

    // Only run if user is admin
    if (!document.getElementById('message-counter')) {
        return;
    }

    let unreadCount = 0;
    let connection = null;
    const isOnChatPage = window.location.pathname.toLowerCase().includes('/adminchat');

    // Initialize SignalR connection for notifications
    function initializeNotificationHub() {
        connection = new signalR.HubConnectionBuilder()
            .withUrl("/chatHub")
            .withAutomaticReconnect()
            .build();

        // Listen for new message alerts
        connection.on("NewMessageAlert", () => {
            // Only increment if not on the chat page
            if (!isOnChatPage) {
                unreadCount++;
                updateCounter();
            }
        });

        // Start the connection
        connection.start()
            .then(() => {
                console.log('Notification hub connected');
            })
            .catch(err => {
                console.error('Error connecting notification hub:', err);
            });
    }

    // Update the counter badge
    function updateCounter() {
        const badge = document.getElementById('message-counter');
        
        if (badge) {
            if (unreadCount > 0) {
                badge.textContent = unreadCount > 99 ? '99+' : unreadCount;
                badge.style.display = 'inline-block';
                
                // Add animation effect
                badge.style.animation = 'none';
                setTimeout(() => {
                    badge.style.animation = 'pulse 0.5s ease-in-out';
                }, 10);
            } else {
                badge.style.display = 'none';
            }
        }
    }

    // Reset counter when clicking on chat link
    function resetCounter() {
        unreadCount = 0;
        updateCounter();
        
        // Store the reset in sessionStorage
        sessionStorage.setItem('chatNotificationReset', Date.now().toString());
    }

    // Add click event to chat link
    const chatLink = document.getElementById('admin-chat-link');
    if (chatLink) {
        chatLink.addEventListener('click', resetCounter);
    }

    // If on chat page, reset counter immediately
    if (isOnChatPage) {
        resetCounter();
    }

    // Initialize the notification system
    if (typeof signalR !== 'undefined') {
        initializeNotificationHub();
    } else {
        console.warn('SignalR not loaded, notification system disabled');
    }

    // Add CSS animation for pulse effect
    if (!document.getElementById('notification-pulse-style')) {
        const style = document.createElement('style');
        style.id = 'notification-pulse-style';
        style.textContent = `
            @keyframes pulse {
                0% { transform: scale(1); }
                50% { transform: scale(1.2); }
                100% { transform: scale(1); }
            }
        `;
        document.head.appendChild(style);
    }
})();

