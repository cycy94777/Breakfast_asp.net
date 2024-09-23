function formatDate(dateString) {
    // 檢查是否為 "/Date(...)/" 格式
    if (typeof dateString === 'string' && dateString.startsWith("/Date(") && dateString.endsWith(")/")) {
        // 提取毫秒數
        const milliseconds = parseInt(dateString.substring(6, dateString.length - 2), 10);
        const date = new Date(milliseconds);
        if (isNaN(date.getTime())) {
            return 'Invalid Date';
        }
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}/${month}/${day}`;
    }

    // 如果是其他格式
    const date = new Date(dateString);
    if (isNaN(date.getTime())) {
        return 'Invalid Date';
    }
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}/${month}/${day}`;
}