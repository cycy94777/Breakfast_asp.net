const maxRows = 7; // 每页显示的最大行数
let currentPage = 1; // 当前页面

// 存储原始数据和筛选后的数据
let originalData = data.BlackList;
let filteredData = originalData;


// **在生成 HTML 之后，获取搜索输入框和按钮**
const searchInput = document.getElementById("searchInput");
const searchButton = document.getElementById("searchButton");

// 添加搜索按钮的事件监听器
searchButton.addEventListener("click", () => {
    const searchTerm = searchInput.value.trim().toLowerCase();