function initializejQueryFunctions(memberId) {
    // 全局变量声明
    var rankList = [];
    var array = [];
    var filteredArray = []; // 保存查询或日期过滤后的结果
    var array_length = 0;
    var table_size = 5; // 每页显示的订单数量
    var start_index = 1;
    var end_index = 0;
    var current_index = 1;
    var max_index = 0;
    var visiblePageCount = 5;
    var isFiltered = false;
    /*var memberId = this.user.id;*/

    // 加载数据并初始化
    fetch(`/api/orders/all?memberId=${memberId}`)  // 修改为fetch请求API
        .then(response => response.json())
        .then(data => {
            rankList = data;
            console.log('Fetched data:', rankList);  // 再次打印 rankList
            array = rankList;
            // 排序 rankList，讓日期最新的排在最前面
            rankList.sort((a, b) => new Date(b.orderTime) - new Date(a.orderTime));

            displayIndexButtons();
        })
        .catch(error => {
            console.error('Error fetching data:', error);
        });

    // 查询功能 - 文本查询
    $(document).on("click", "#text_search_btn", function () {
        current_index = 1;
        start_index = 1;
        filterRankList(); // 点击查询时根据输入内容筛选
        displayIndexButtons();
    });

    function filterRankList() {
        var tab_filter_text = $("#tab_filter_text").val().trim();
        if (tab_filter_text !== "") {
            filteredArray = rankList.filter(function (object) {
                var statusText = getStatusText(object.orderStatus);

                return (
                    object.orderID.toString().includes(tab_filter_text) ||
                    object.orderTime.includes(tab_filter_text) ||
                    object.pickupTime.includes(tab_filter_text) ||
                    object.totalAmount.toString().includes(tab_filter_text) ||
                    statusText.includes(tab_filter_text)
                );
            });
            array = filteredArray; // 将筛选后的数据赋给 array 用于展示
            isFiltered = true; // 设置为筛选模式
        } else {
            array = rankList; // 如果查询为空，展示全部数据
            isFiltered = false; // 重置为非筛选模式
        }
        displayIndexButtons(); // 更新表格数据与分页
    }

    // 初始化日期选择器
    $("#datepicker").datepicker({
        dateFormat: "yy-mm-dd", // 日期格式设定
    });

    // 查询功能 - 日期查询
    $(document).on("click", "#date_search_btn", function () {
        var selectedDate = $("#datepicker").val();
        if (selectedDate) {
            current_index = 1;
            filterByDate(selectedDate); // 根据选择的日期进行筛选
        } else {
            alert("請選擇日期");
        }
    });

    // 日期过滤函数
    function filterByDate(dateText) {
        filteredArray = rankList.filter(function (order) {
            var orderDate = order.orderTime.split("T")[0]; // 提取订单日期部分
            //console.log(orderDate);
            return orderDate === dateText;
        });

        array = filteredArray; // 将筛选后的结果赋给 array
        current_index = 1; // 重置为第一页
        isFiltered = true; // 设置为筛选模式
        displayIndexButtons(); // 调用现有的分页逻辑显示筛选后的订单
    }

    // 分页相关函数
    function preLoadCaculations() {
        array_length = array.length;
        max_index = Math.ceil(array_length / table_size); // 计算最大页数
    }

    function displayIndexButtons() {
        preLoadCaculations();
        $(".index_buttons").empty(); // 清空分页按钮

        // 添加上一页按钮
        var prevDisabled = current_index === 1 ? "disabled" : "";
        $(".index_buttons").append('<button id="prev-btn" ' + prevDisabled + "><</button>");

        // 页码的开始和结束
        // 确保 current_index 超过5时，仍能显示前面的页码范围
        var startPage = Math.max(1, current_index - Math.floor(visiblePageCount / 2));
        var endPage = Math.min(startPage + visiblePageCount - 1, max_index);

        // 调整 startPage 和 endPage，确保它们在合法范围内
        if (endPage - startPage + 1 < visiblePageCount) {
            startPage = Math.max(1, endPage - visiblePageCount + 1);
        }

        // 创建分页按钮
        for (var i = startPage; i <= endPage; i++) {
            var activeClass = i === current_index ? "active" : "";
            $(".index_buttons").append(
                '<button class="' + activeClass + '" data-index="' + i + '">' + i + "</button>"
            );
        }

        // 添加下一页按钮
        var nextDisabled = current_index >= max_index ? "disabled" : "";
        $(".index_buttons").append('<button id="next-btn" ' + nextDisabled + ">></button>");

        // 更新表格
        updateTable(array);
    }

    // 分页按钮点击事件
    $(document).on("click", "#prev-btn", function () {
        if (current_index > 1) {
            current_index--;
            displayIndexButtons(); // 重新计算并显示按钮
        }
    });

    $(document).on("click", "#next-btn", function () {
        if (current_index < max_index) {
            current_index++;
            displayIndexButtons(); // 重新计算并显示按钮
        }
    });

    $(document).on("click", ".index_buttons button[data-index]", function () {
        var index = $(this).data("index");
        current_index = parseInt(index);
        displayIndexButtons(); // 根据点击的页码更新表格和分页按钮
    });

    function highlightIndexButton(totalEntries) {
        start_index = (current_index - 1) * table_size + 1;
        end_index = start_index + table_size - 1;
        if (end_index > totalEntries) {
            end_index = totalEntries;
        }

        $(".footer-left span").text(
            "列出" + start_index + "到" + end_index + "的" + totalEntries + "筆訂單"
        );

        $(".index_buttons button").removeClass("active");
        $(".index_buttons button[data-index='" + current_index + "']").addClass("active");

        $("#prev-btn").prop("disabled", current_index === 1);
        $("#next-btn").prop("disabled", current_index === max_index);
    }

    function updateTable(orders) {
        $(".table table tbody").empty(); // 清空表格内容

        var start = (current_index - 1) * table_size;
        var end = start + table_size;
        if (end > orders.length) {
            end = orders.length;
        }

        for (var i = start; i < end; i++) {
            var order = orders[i];
            console.log(order);
            // 將數字狀態碼轉換為帶有樣式的文本狀態
            var statusText = getStatusText(order["orderStatus"]);

            // 將 orderTime 拆分為日期和時間
            var [orderDatePart, orderTimePart] = order["orderTime"].split("T"); // 拆分 orderTime
            orderTimePart = orderTimePart.split(".")[0];
            var formattedOrderTime = `${orderDatePart}<br>${orderTimePart}`; // 日期和時間換行顯示

            // 將 pickupTime 拆分為日期和時間
            var [pickupDatePart, pickupTimePart] = order["pickupTime"].split("T"); // 拆分 pickupTime
            pickupTimePart = pickupTimePart.split(".")[0];
            var formattedPickupTime = `${pickupDatePart}<br>${pickupTimePart}`; // 日期和時間換行顯示

            var tr = `
        <tr>
            <td>${order["orderID"]}</td>          
            <td>${formattedOrderTime}</td> <!-- 修改此處，將 orderTime 換行呈現 -->        
            <td>${formattedPickupTime}</td> <!-- 修改此處，將 pickupTime 換行呈現 -->
            <td>${order["totalAmount"]}</td>      
            <td>${statusText}</td>
            <td><button class="details-btn" data-order-id="${order["orderID"]}">明細</button></td>
        </tr>`;
            $(".table table tbody").append(tr); // 動態插入行到表格
        }

        highlightIndexButton(orders.length);
    }

    function getStatusText(statusCode) {
        switch (statusCode) {
            case 1:
                return '<span class="status-not-picked">未取餐</span>';
            case 2:
                return '<span class="status-picked">已取餐</span>';
            case 3:
                return '<span class="status-cancelled">已取消</span>';
            case 4:
                return '<span class="status-abandoned">已棄單</span>';
            default:
                return '<span class="status">未知狀態</span>';
        }
    }

    function next() {
        if (current_index < max_index) {
            current_index++;
            updateTable(array);
            highlightIndexButton(array.length);
        }
    }

    function prev() {
        if (current_index > 1) {
            current_index--;
            updateTable(array);
            highlightIndexButton(array.length);
        }
    }

    function indexPagination(index) {
        current_index = parseInt(index);
        updateTable(array);
        highlightIndexButton(array.length);
    }

    // 明细订单明细弹出

    // 动态生成模态框结构
    $("body").append(`
        <div id="orderDetailsModal" class="modal">
            <div class="modal-content">
                <span class="close-btn">&times;</span>
                <div id="orderDetailsContent"></div>
            </div>
        </div>
    `);

    // 点击 "明細" 按钮时触发
    $(document).on("click", ".details-btn", function () {
        var orderId = parseInt($(this).data("order-id"));  // 确保 orderId 为数字类型
        console.log('Clicked Order ID:', orderId);
        showOrderDetails(orderId); // 显示订单详情
    });

    // 关闭模态框
    $(document).on("click", ".close-btn", function () {
        $("#orderDetailsModal").hide();
    });

    // 点击模态框外部区域关闭模态框
    $(window).on("click", function (event) {
        if ($(event.target).is("#orderDetailsModal")) {
            $("#orderDetailsModal").hide();
        }
    });

    // 显示订单详细信息的函数
    function showOrderDetails(orderId) {
        console.log('Order ID passed to function:', orderId);
        var order = rankList.find((order) => parseInt(order.orderID) === orderId);  // 使用 parseInt 確保類型一致

        if (!order) {
            console.error('Order not found with ID:', orderId);
            return;  // 如果找不到訂單，終止函數執行
        }

        var statusText = getStatusText(order.orderStatus);
        var totalAmount = order.totalAmount;
        var pointsUsed = order.pointsUsed || 0;
        var pointsEarned = order.pointsEarned || 0;

        // 計算訂單點餐後的15分鐘期限
        var orderTime = new Date(order.orderTime);
        var cancelTimeLimit = new Date(orderTime.getTime() + 15 * 60 * 1000); // 訂單時間加上15分鐘
        var now = new Date();

        // Debug 输出
        console.log("订单时间:", orderTime);
        console.log("当前时间:", now);
        console.log("允许取消截止时间:", cancelTimeLimit);
        console.log("订单状态:", order.orderStatus);

        // 動態生成訂單詳細信息的 HTML
        var detailsHtml = `
    <h2>訂單明細</h2>
    <p>訂單狀態：${statusText}</p>
    <div class="order-items-container">
    ${order.items.map((item) => {
            var itemOptions = "";
            if (item.description) {
                itemOptions += `<p>${item.description}</p>`;
            }
            if (item.extras) {
                itemOptions += `<p>${item.extras}</p>`;
            }
            var quantity = item.quantity || 1;
            return `
            <div class="order-item">
                <div class="item-image">
                    <img src="${item.image}" alt="${item.name}" />
                </div>
                <div class="item-details">
                    <p class="item-name">${item.name}</p>
                    <p class="item-option">${itemOptions}</p>
                </div>
                <div class="item-quantity">
                    <p>${quantity}</p>
                </div>
                <div class="item-price">
                    <p>NT$${item.price}</p>
                </div>
            </div>
        `;
        }).join("")}
    </div>
    <div class="order-summary">
        <p>點數折抵：<span>${pointsUsed}</span></p>
        <p>總金額：<span>NT$${totalAmount}</span></p>
        <p>獲得點數：<span>${pointsEarned}</span></p>
    </div>
    <button class="cancel-order-btn" id="cancelOrderBtn" data-order-id="${orderId}">取消訂單</button>
  `;

        // 顯示訂單明細
        $("#orderDetailsContent").html(detailsHtml);

        // 打開訂單詳細信息的模態框
        $("#orderDetailsModal").show();

        // 檢查訂單狀態，如果不是 "未取餐" 則禁用取消按鈕
        if (order.orderStatus !== 1) {
            // 改變按鈕狀態：變灰並禁用
            $("#cancelOrderBtn").addClass("disabled").attr("disabled", true).text("無法取消");
        }
    }

    // 动态生成取消订单模态框结构
    $("body").append(`
  <div id="cancelOrderModal" class="modal" style="display:none;">
    <div class="modal-content" style="border:1px solid #ddd; padding:20px; background-color:white; width:320px; height:300px;   margin:auto;">
      <p style="font-weight:bold" class="cancel-text";>取消訂單</p>
      <ol>
        <li>送出訂單後的 <span style="color: red;">15分鐘內</span> 才能取消。</li>
        <li>取消後的訂單將不進行點數的累積以及點數的折抵。</li>
      </ol>
      <button id="cancelClose" style="margin-right:10px;">取消</button>
      <button id="confirmCancel">確認</button>
    </div>
  </div>
`);

    // 当用户点击“明细”按钮时显示订单详细信息
    $(document).on("click", ".cancel-order-btn", function () {
        var orderId = parseInt($(this).data("order-id"));  // 確保 orderId 為數字類型
        console.log("取消訂單按鈕被點擊，訂單ID為:", orderId);

        var order = rankList.find((order) => parseInt(order.orderID) === orderId);

        // 檢查 order 是否存在
        if (!order) {
            alert("找不到訂單，請刷新頁面或稍後再試");
            console.error("找不到訂單:", orderId);
            return; // 停止執行後續邏輯
        }

        console.log("找到的訂單:", order);

        // 檢查訂單狀態，如果不是 "未取餐" 則提示不能取消
        if (order.orderStatus !== 1) {
            $(this).addClass("disabled").attr("disabled", true).text("已取餐");
            console.log("訂單狀態不是未取餐，無法取消:", order.orderStatus);
            return; // 阻止後續取消訂單的邏輯
        }

        $("#cancelOrderModal").show(); // 顯示取消訂單彈出框
        $("#orderDetailsModal").hide();

        //取消button
        $("#cancelClose").off("click").on("click", function () {
            // 隱藏取消訂單的模態框
            $("#cancelOrderModal").hide();
        });


        // 點擊 "確認" 時修改訂單狀態為 "已取消"
        $("#confirmCancel")
            .off("click")
            .on("click", function () {
                // 發送 API 請求，將取消狀態發送給後端
                fetch(`/api/orders/cancel/${orderId}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                })
                    .then(response => {
                        // 檢查是否是 2xx 的成功狀態碼
                        if (!response.ok) {
                            return response.json().then(errorData => {
                                throw new Error(errorData.message || '取消訂單失敗');
                            });
                        }
                        return response.json();
                    })
                    .then(result => {
                        console.log(result);

                        // 如果取消成功，更新前端顯示
                        if (result.message === "訂單已取消，並回收獲得點數和退還使用的點數。") {
                            Swal.fire({
                                icon: 'success',
                                title: '訂單已取消',
                                text: '',
                                customClass: {
                                    confirmButton: 'custom-swal-button'  // 自定義按鈕的 class
                                }
                            });

                            // 更新 UI - 訂單狀態改為「已取消」，保持明細按鈕不變
                            $(`button[data-order-id="${orderId}"]`).closest('tr').find('td:nth-child(5)').text('已取消');

                            // 更新明細按鈕旁邊的狀態為「已取消」
                            $(`button[data-order-id="${orderId}"]`).closest('tr').find('td:nth-child(5)').text('已取消');

                            // 同時更新 rankList 中的訂單狀態，這樣下次訪問同一筆訂單時狀態已經是已取消
                            order.orderStatus = 3;
                            // 如果彈出框已打開，更新彈出框內的訂單狀態
                            $(".order-status-text").text("訂單狀態：已取消");
                        } else {
                            alert("訂單取消失敗，請稍後再試");
                        }
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        alert(`取消訂單失敗: ${error.message}`);
                    });

                // 隱藏取消訂單彈出框
                $("#cancelOrderModal").hide();
                $("#orderDetailsModal").hide();
            });

    });







    // 显示订单详细信息的函数
    function showOrderDetails(orderId) {
        console.log('Order ID passed to function:', orderId);
        var order = rankList.find((order) => parseInt(order.orderID) === orderId);  // 使用 parseInt 確保類型一致

        if (!order) {
            console.error('Order not found with ID:', orderId);
            return;  // 如果找不到訂單，終止函數執行
        }

        var statusText = getStatusText(order.orderStatus);
        var totalAmount = order.totalAmount;
        var pointsUsed = order.pointsUsed || 0;
        var pointsEarned = order.pointsEarned || 0;

        // 計算訂單點餐後的15分鐘期限
        var orderTime = new Date(order.orderTime);
        var cancelTimeLimit = new Date(orderTime.getTime() + 15 * 60 * 1000); // 訂單時間加上15分鐘
        var now = new Date();

        // Debug 输出
        console.log("订单时间:", orderTime);
        console.log("当前时间:", now);
        console.log("允许取消截止时间:", cancelTimeLimit);
        console.log("订单状态:", order.orderStatus);


        // 構造訂單詳細信息的 HTML
        var detailsHtml = `
    <h2>訂單明細</h2>
    <p>訂單狀態：${statusText}</p>
    <div class="order-items-container">
    ${order.items.map((item) => {
            var itemOptions = "";
            if (item.description) {
                itemOptions += `<p>${item.description}</p>`;
            }
            if (item.extras) {
                itemOptions += `<p>${item.extras}</p>`;
            }
            var quantity = item.quantity || 1;
            return `
            <div class="order-item">
                <div class="item-image">
                    <img src="${item.image}" alt="${item.name}" />
                </div>
                <div class="item-details">
                    <p class="item-name">${item.name}</p>
                    <p class="item-option">${itemOptions}</p>
                </div>
                <div class="item-quantity">
                    <p>x${quantity}</p>
                </div>
                <div class="item-price">
                    <p>NT$${item.price}</p>
                </div>
            </div>
        `;
        }).join("")}
    </div>
    <div class="order-summary">
        <p>點數折抵：<span>${pointsUsed}</span></p>
        <p>總金額：<span class="total-amount">NT$${totalAmount}</span></p>
        <p>獲得點數：<span>${pointsEarned}</span></p>
    </div>
  `;

        // 根據訂單狀態決定是否生成取消訂單按鈕
        if (order.orderStatus === 1 && now <= cancelTimeLimit) {  // 訂單狀態為 "未取餐" 且時間在15分鐘內
            detailsHtml += `
        <button class="cancel-order-btn" id="cancelOrderBtn" data-order-id="${orderId}">取消訂單</button>
    `;
        } else if (order.orderStatus === 1 && now > cancelTimeLimit) {  // 訂單超過15分鐘不可取消
            detailsHtml += `
        <button class="cancel-order-btn disabled" disabled>取消訂單</button>
    `;
        } else if (order.orderStatus === 2) {  // 訂單狀態為 "已取餐"
            detailsHtml += `
        <button class="cancel-order-btn disabled" disabled>取消訂單</button>
    `;
        } else if (order.orderStatus === 3) {  // 訂單狀態為 "已取消"
            detailsHtml += `
        <button class="cancel-order-btn disabled" disabled>取消訂單</button>
    `;
        } else if (order.orderStatus === 4) {  // 訂單狀態為 "已棄單"
            detailsHtml += `
        <button class="cancel-order-btn disabled" disabled>取消訂單</button>
    `;
        } else {  // 其他狀態，無法取消訂單
            detailsHtml += `
        <button class="cancel-order-btn disabled" disabled>無法取消</button>
    `;
        }

        // 顯示訂單明細
        $("#orderDetailsContent").html(detailsHtml);
        $("#orderDetailsModal").show();
    }

}
