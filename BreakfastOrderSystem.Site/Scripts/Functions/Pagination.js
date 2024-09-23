function renderPagination(totalPages, currentPage) {
    const pagination = document.getElementById("pagination");
    pagination.innerHTML = ""; // 清空舊的分頁按鈕

    const prevPage = document.createElement("li");
    prevPage.classList.add("page-item");
    prevPage.innerHTML = `
        <a class="page-link" href="#" aria-label="Previous">
            <span aria-hidden="true">&laquo;</span>
        </a>`;

    prevPage.addEventListener("click", () => {
        if (currentPage > 1) {
            currentPage--;
            window.renderTable(currentPage); // 使用全局的 renderTable
        }
    });
    pagination.appendChild(prevPage);

    for (let i = 1; i <= totalPages; i++) {
        const pageItem = document.createElement("li");
        pageItem.classList.add("page-item");
        if (i === currentPage) {
            pageItem.classList.add("active");
        }
        pageItem.innerHTML = `<a class="page-link" href="#">${i}</a>`;
        pageItem.addEventListener("click", () => {
            currentPage = i;
            window.renderTable(currentPage); // 使用全局的 renderTable
        });
        pagination.appendChild(pageItem);
    }

    const nextPage = document.createElement("li");
    nextPage.classList.add("page-item");
    nextPage.innerHTML = `
        <a class="page-link" href="#" aria-label="Next">
            <span aria-hidden="true">&raquo;</span>
        </a>`;
    nextPage.addEventListener("click", () => {
        if (currentPage < totalPages) {
            currentPage++;
            window.renderTable(currentPage); // 使用全局的 renderTable
        }
    });
    pagination.appendChild(nextPage);
}
