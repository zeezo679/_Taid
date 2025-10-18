﻿// Enhanced site-wide JavaScript functionality - Dark Theme with Page Transitions

document.addEventListener('DOMContentLoaded', function() {
    // Force dark theme application
    applyDarkTheme();
    
    // Initialize all enhanced features
    initializeTooltips();
    initializeAnimations();
    initializeInteractiveElements();
    initializeFormEnhancements();
    initializePageTransitions();

    // Modal robustness: ensure modals are appended to body and positioned correctly
    initializeModalFixes();
});

// Page Transition System
function initializePageTransitions() {
    const loader = document.getElementById('pageLoader');
    const navLinks = document.querySelectorAll('.smooth-nav');
    
    // Handle navigation clicks
    navLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            // Skip if it's logout or external link
            if (this.dataset.page === 'logout' || 
                this.getAttribute('href')?.startsWith('http') || 
                this.getAttribute('href')?.startsWith('#')) {
                return;
            }

            // Show loader
            showPageLoader();
            
            // Add slight delay for visual effect
            setTimeout(() => {
                // Navigation will proceed normally
            }, 150);
        });
    });

    // Handle page load completion
    window.addEventListener('load', () => {
        hidePageLoader();
        animatePageContent();
    });

    // Handle browser navigation
    window.addEventListener('beforeunload', () => {
        showPageLoader();
    });
}

function showPageLoader() {
    const loader = document.getElementById('pageLoader');
    if (loader) {
        loader.classList.add('active');
    }
}

function hidePageLoader() {
    const loader = document.getElementById('pageLoader');
    if (loader) {
        setTimeout(() => {
            loader.classList.remove('active');
        }, 300);
    }
}

function animatePageContent() {
    // Animate main content
    const main = document.querySelector('main[role="main"]');
    if (main) {
        main.style.opacity = '0';
        main.style.transform = 'translateY(20px)';
        
        setTimeout(() => {
            main.style.transition = 'all 0.6s cubic-bezier(0.25, 0.46, 0.45, 0.94)';
            main.style.opacity = '1';
            main.style.transform = 'translateY(0)';
        }, 100);
    }

    // Stagger animate cards - ONLY ONCE
    const cards = document.querySelectorAll('.card:not(.animated)');
    cards.forEach((card, index) => {
        card.classList.add('animated'); // Prevent re-animation
        card.style.opacity = '0';
        card.style.transform = 'translateY(30px) scale(0.95)';
        
        setTimeout(() => {
            card.style.transition = 'all 0.5s cubic-bezier(0.34, 1.56, 0.64, 1)';
            card.style.opacity = '1';
            card.style.transform = 'translateY(0) scale(1)';
        }, 100 + (index * 100));
    });

    // Set active navigation state
    setActiveNavigation();
}

function setActiveNavigation() {
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.smooth-nav');
    
    navLinks.forEach(link => {
        link.classList.remove('active');
        
        const href = link.getAttribute('href');
        if (href) {
            // Check if current path matches the link
            if ((currentPath === '/' && href.includes('/Home')) ||
                (currentPath.includes('/Course') && href.includes('/Course')) ||
                (currentPath.includes('/Instructor') && href.includes('/Instructor')) ||
                (currentPath.includes('/Account') && href.includes('/Account')) ||
                (currentPath.includes('/Privacy') && href.includes('/Privacy'))) {
                link.classList.add('active');
            }
        }
    });
}

// Apply dark theme ONCE and track if it's already applied
let darkThemeApplied = false;

function applyDarkTheme() {
    // Only apply if not already applied
    if (darkThemeApplied) return;
    
    // Set CSS custom properties
    const root = document.documentElement;
    root.style.setProperty('--bs-body-bg', '#1a1a1a');
    root.style.setProperty('--bs-body-color', '#ffffff');
    root.style.setProperty('--bs-emphasis-color', '#ffffff');
    root.style.setProperty('--bs-secondary-bg', '#2d2d2d');
    root.style.setProperty('--bs-tertiary-bg', '#404040');
    
    // Force body styling
    document.body.style.backgroundColor = '#1a1a1a';
    document.body.style.color = '#ffffff';
    
    // Apply dark theme to all cards ONCE
    document.querySelectorAll('.card:not(.dark-themed)').forEach(card => {
        card.classList.add('dark-themed'); // Mark as already themed
        card.style.backgroundColor = '#252525';
        card.style.borderColor = '#404040';
        card.style.color = '#ffffff';
    });
    
    // Apply dark theme to form controls ONCE
    document.querySelectorAll('.form-control:not(.dark-themed), .form-select:not(.dark-themed)').forEach(control => {
        control.classList.add('dark-themed'); // Mark as already themed
        control.style.backgroundColor = '#1e1e1e';
        control.style.borderColor = '#404040';
        control.style.color = '#ffffff';
    });
    
    // Apply dark theme to navbar
    const navbar = document.querySelector('.navbar');
    if (navbar && !navbar.classList.contains('dark-themed')) {
        navbar.classList.add('dark-themed');
        navbar.style.backgroundColor = '#252525';
        navbar.style.borderBottomColor = '#404040';
    }
    
    // Apply dark theme to footer
    const footer = document.querySelector('footer');
    if (footer && !footer.classList.contains('dark-themed')) {
        footer.classList.add('dark-themed');
        footer.style.backgroundColor = '#252525';
        footer.style.borderTopColor = '#404040';
        footer.style.color = '#b3b3b3';
    }
    
    darkThemeApplied = true;
}

// Tooltip functionality
function initializeTooltips() {
    // Initialize Bootstrap tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Animation observers
function initializeAnimations() {
    // Intersection Observer for scroll animations
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };
    
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting && !entry.target.classList.contains('observed')) {
                entry.target.classList.add('animate-fade-in', 'observed');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);
    
    // Observe elements that should animate on scroll
    document.querySelectorAll('.table, .alert').forEach(el => {
        observer.observe(el);
    });
}

// Interactive elements - PREVENT RE-ATTACHMENT
function initializeInteractiveElements() {
    // Enhanced button interactions - only for new buttons
    document.querySelectorAll('.btn-primary:not(.interactive), .btn-secondary:not(.interactive), .btn-outline-primary:not(.interactive)').forEach(btn => {
        btn.classList.add('interactive'); // Mark as already enhanced
        
        btn.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-2px) scale(1.02)';
            this.style.transition = 'transform 0.3s cubic-bezier(0.34, 1.56, 0.64, 1)';
        });
        
        btn.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
        });
        
        btn.addEventListener('mousedown', function() {
            this.style.transform = 'translateY(0) scale(0.98)';
        });
        
        btn.addEventListener('mouseup', function() {
            this.style.transform = 'translateY(-2px) scale(1.02)';
        });
    });

    // REMOVED: Card hover effects - no longer applying any hover effects to cards
    // Enhanced card hover effects - DISABLED
    document.querySelectorAll('.card:not(.hover-enhanced)').forEach(card => {
        card.classList.add('hover-enhanced'); // Mark as processed but don't add hover effects
        
        // No hover effects applied to cards anymore
    });
}

// Form enhancements
function initializeFormEnhancements() {
    // Enhanced form validation feedback
    document.querySelectorAll('form:not(.enhanced)').forEach(form => {
        form.classList.add('enhanced');
        
        // Removed loading animation on form submit
        // Forms will submit normally without loading states
    });

    // Enhanced input focus effects
    document.querySelectorAll('.form-control:not(.focus-enhanced), .form-select:not(.focus-enhanced)').forEach(input => {
        input.classList.add('focus-enhanced');
        
        input.addEventListener('focus', function() {
            this.style.borderColor = '#0ea5e9';
            this.style.boxShadow = '0 0 0 0.2rem rgba(14, 165, 233, 0.25)';
            this.style.transform = 'scale(1.02)';
            this.style.transition = 'all 0.3s cubic-bezier(0.34, 1.56, 0.64, 1)';
            this.parentElement.classList.add('focused');
        });
        
        input.addEventListener('blur', function() {
            this.style.borderColor = '#404040';
            this.style.boxShadow = 'none';
            this.style.transform = 'scale(1)';
            this.parentElement.classList.remove('focused');
        });
    });

    // Real-time validation feedback
    document.querySelectorAll('input[data-val="true"]:not(.validation-enhanced)').forEach(input => {
        input.classList.add('validation-enhanced');
        input.addEventListener('blur', function() {
            validateField(this);
        });
    });
}

// Ensure Bootstrap modals render above transformed ancestors and align clicks
function initializeModalFixes() {
    // Guard if Bootstrap JS isn't loaded
    if (!window.bootstrap) return;

    // Delegate to handle any modal in the app
    document.addEventListener('show.bs.modal', function (event) {
        const modal = event.target;
        // Append to body to escape any transformed/sticky parent contexts
        if (modal && modal.parentElement !== document.body) {
            document.body.appendChild(modal);
        }
        // Add helper class to body for CSS overrides
        document.body.classList.add('modal-open-fix');
    });

    document.addEventListener('hidden.bs.modal', function () {
        // Remove helper class when all modals are closed
        const anyOpen = document.querySelector('.modal.show');
        if (!anyOpen) {
            document.body.classList.remove('modal-open-fix');
        }
    });

    
}

// Field validation helper
function validateField(field) {
    const isValid = field.checkValidity();
    
    if (!isValid) {
        field.classList.add('is-invalid');
        field.classList.remove('is-valid');
        field.style.borderColor = '#ef4444';
        field.style.boxShadow = '0 0 0 0.2rem rgba(239, 68, 68, 0.25)';
        field.style.transform = 'scale(1.01)';
    } else {
        field.classList.add('is-valid');
        field.classList.remove('is-invalid');
        field.style.borderColor = '#10b981';
        field.style.boxShadow = '0 0 0 0.2rem rgba(16, 185, 129, 0.25)';
        field.style.transform = 'scale(1)';
    }
}

// Utility functions
function showAlert(message, type = 'success') {
    const alertHtml = `
        <div class="alert alert-${type} alert-dismissible fade show animate-slide-down" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;
    
    // Insert at top of main content
    const mainContent = document.querySelector('main');
    if (mainContent) {
        mainContent.insertAdjacentHTML('afterbegin', alertHtml);
    }
}

// Smooth scrolling for anchor links
function initializeSmoothScroll() {
    document.querySelectorAll('a[href^="#"]:not(.smooth-scroll-enhanced)').forEach(anchor => {
        anchor.classList.add('smooth-scroll-enhanced');
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
}

// Auto-hide alerts
function initializeAlertAutoHide() {
    setTimeout(function() {
        const alerts = document.querySelectorAll('.alert');
        alerts.forEach(alert => {
            if (bootstrap.Alert) {
                const bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            }
        });
    }, 5000);
}

// Initialize additional features
initializeSmoothScroll();
initializeAlertAutoHide();

// Global utility functions
window.toggleMobileMenu = function() {
    const navbar = document.querySelector('.navbar-collapse');
    if (navbar) {
        const bsCollapse = new bootstrap.Collapse(navbar);
        bsCollapse.toggle();
    }
};

// Export utility functions for external use
window.DemoUtils = {
    showAlert: showAlert,
    validateField: validateField,
    applyDarkTheme: applyDarkTheme,
    showPageLoader: showPageLoader,
    hidePageLoader: hidePageLoader,
    animatePageContent: animatePageContent
};
// REMOVE THE PROBLEMATIC INTERVAL - This was causing the blinking!
// setInterval(optimizedApplyDarkTheme, 2000); // ❌ REMOVED
