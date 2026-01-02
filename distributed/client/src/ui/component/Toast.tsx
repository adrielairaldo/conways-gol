import React, { useEffect } from 'react';

interface ToastProps {
    message: string;
    onClose: () => void;
}

/**
 * Toast notification component for displaying error messages.
 * 
 * This component shows a styled error message at the top of the screen
 * that automatically dismisses after 5 seconds. Users can also manually
 * dismiss it by clicking the close button.
 * 
 * The toast uses React's cyan theme colors for consistency with the app design.
 */
export const Toast: React.FC<ToastProps> = ({ message, onClose }) => {
    /**
     * Auto-dismiss the toast after 5 seconds
     */
    useEffect(() => {
        const timer = setTimeout(() => {
            onClose();
        }, 5000);

        return () => clearTimeout(timer);
    }, [onClose]);

    return (
        <div className="fixed top-4 left-1/2 transform -translate-x-1/2 z-50 animate-[slideDown_0.3s_ease-out]">
            <div className="bg-red-500 text-white px-6 py-4 rounded-lg shadow-2xl flex items-center gap-4 min-w-[300px] max-w-[500px]">
                {/* Error Icon */}
                <div className="flex-shrink-0">
                    <svg className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                    </svg>
                </div>

                {/* Message */}
                <div className="flex-1">
                    <p className="font-semibold text-sm">{message}</p>
                </div>

                {/* Close Button */}
                <button
                    onClick={onClose}
                    className="flex-shrink-0 hover:bg-red-600 rounded p-1 transition-colors"
                    aria-label="Close notification"
                >
                    <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                    </svg>
                </button>
            </div>
        </div>
    );
};