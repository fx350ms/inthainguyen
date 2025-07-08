// Function to format a number as currency
function formatCurrency(number, locale = 'vi-VN', currency = 'VND') {
    return new Intl.NumberFormat(locale, {
        style: 'currency',
        currency: currency,
    }).format(number);
}

function formatThousand(number, locale = 'vi-VN', currency = 'VND') {
    return new Intl.NumberFormat(locale, {
        currency: currency,
    }).format(number);
}
// Function to format a date to dd/MM/yyyy
Date.prototype.toDDMMYYYY = function () {
    const day = String(this.getDate()).padStart(2, '0');
    const month = String(this.getMonth() + 1).padStart(2, '0');
    const year = this.getFullYear();
    return `${day}/${month}/${year}`;
};

// Function to format a date to dd/MM/yyyy HH:mm
Date.prototype.toDDMMYYYYHHmm = function () {
    const day = String(this.getDate()).padStart(2, '0');
    const month = String(this.getMonth() + 1).padStart(2, '0');
    const year = this.getFullYear();
    const hours = String(this.getHours()).padStart(2, '0');
    const minutes = String(this.getMinutes()).padStart(2, '0');
    return `${day}/${month}/${year} ${hours}:${minutes}`;
};

// Example usage:
// console.log(new Date().toDDMMYYYY()); // "07/01/2025"
// console.log(new Date().toDDMMYYYYHHmm()); // "07/01/2025 14:30"
function formatDateToDDMMYYYY(date) {
    if (!(date instanceof Date)) {
        date = new Date(date);
    }
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}

function formatDateToDDMMYYYYHHmm(date) {
    if (!(date instanceof Date)) {
        date = new Date(date);
    }
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${day}/${month}/${year} ${hours}:${minutes}`;
}

function PlaySound(type, callback) {
    let audio;
    let timeout;
    if (type === 'success') {
        audio = new Audio('/sounds/success.mp3');
    } else if (type === 'warning') {
        audio = new Audio('/sounds/alert.mp3');
    }

    if (audio) {
        audio.play();
        timeout = setTimeout(() => {
            if (callback) callback('timeout');
        }, 3000); // Timeout sau 3 giây phòng trường hợp lỗi

        audio.onended = () => {
            clearTimeout(timeout);
            if (callback) callback('completed');
        };
    } else {
        if (callback) callback('no_audio');
    }
}

// Function to create a delay with a callback
function delay(milliseconds, callback) {
    setTimeout(() => {
        if (callback) callback();
    }, milliseconds);
}

// Example usage:
// delay(2000, () => {
//     console.log('Executed after 2 seconds');
// });
//window.location.href = '/Orders';
// Example usage:
// console.log(formatCurrency(123456789)); // "123.456.789 đ"
// console.log(formatDateToDDMMYYYY(new Date())); // "07/01/2025"
