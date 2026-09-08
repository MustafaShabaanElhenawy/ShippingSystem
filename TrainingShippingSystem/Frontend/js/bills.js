
const billsApiUrl =
    "https://localhost:44320/api/Bills";
const clientsApiUrl =
    "https://localhost:44320/api/Clients";
const voyagesApiUrl =
    "https://localhost:44320/api/Voyages";
let clientsData = [];
let voyagesData = [];


$(document).ajaxStart(function () {

    $("#loadingIndicator").css("display", "flex");

});


$(document).ajaxStop(function () {

    $("#loadingIndicator").hide();

});


/* =========================================
   FILTER / PAGINATION / SORTING
========================================= */
let currentPage = 1;
const pageSize = 10;
let sortColumn = "ID";
let sortDirection = "DESC";
/* =========================================
   DOCUMENT READY
========================================= */
$(document).ready(function () {
    // Load Bills immediately
    loadBills();
    // Load dropdowns independently
    loadClientsDropdown();
    loadVoyagesDropdown();
    // Form buttons
    $("#saveBillBtn").click(saveBill);
    $("#clearBillBtn").click(clearForm);
    // Search
    $("#searchBillsBtn").click(function () {
        currentPage = 1;
        loadBills();
    });
    // Clear Search
    $("#clearSearchBtn").click(function () {
        clearAdvancedSearch();
    });
    // =========================================
    // SORT BUTTONS
    // =========================================
    $(document).on(
        "click",
        ".sort-btn",
        function () {
            const column =
                $(this).data("column");
            sortBills(column);
        }
    );
});
/* =========================================
   LOAD CLIENTS DROPDOWNS
========================================= */
function loadClientsDropdown() {
    $.ajax({
        url: clientsApiUrl,
        type: "GET",
        success: function (clients) {
            clientsData = clients;
            let formOptions =
                `<option value="">
                    Select Client
                </option>`;
            let searchOptions =
                `<option value="">
                    All Clients
                </option>`;
            $.each(
                clients,
                function (index, client) {
                    formOptions +=
                        `<option value="${client.ID}">
                            ${client.Name}
                        </option>`;
                    searchOptions +=
                        `<option value="${client.ID}">
                            ${client.Name}
                        </option>`;
                }
            );
            $("#clientId").html(
                formOptions
            );
            $("#searchClientId").html(
                searchOptions
            );
        },
        error: handleError
    });
}
/* =========================================
   LOAD VOYAGES DROPDOWNS
========================================= */
function loadVoyagesDropdown() {
    $.ajax({
        url: voyagesApiUrl,
        type: "GET",
        success: function (voyages) {
            voyagesData = voyages;
            let formOptions =
                `<option value="">
                    Select Voyage
                </option>`;
            let searchOptions =
                `<option value="">
                    All Voyages
                </option>`;
            $.each(
                voyages,
                function (index, voyage) {
                    formOptions +=
                        `<option value="${voyage.ID}">
                            ${voyage.VoyageNumber}
                        </option>`;
                    searchOptions +=
                        `<option value="${voyage.ID}">
                            ${voyage.VoyageNumber}
                        </option>`;
                }
            );
            $("#voyageId").html(
                formOptions
            );
            $("#searchVoyageId").html(
                searchOptions
            );
        },
        error: handleError
    });
}
/* =========================================
   LOAD BILLS
   SERVER-SIDE:
   SEARCH
   PAGINATION
   SORTING
========================================= */
function loadBills() {
    const billNumber =
        $("#searchBillNumber")
            .val()
            .trim();
    const clientID =
        $("#searchClientId").val();
    const voyageID =
        $("#searchVoyageId").val();
    const requestData = {
        pageNumber:
            currentPage,
        pageSize:
            pageSize,
        sortBy:
            sortColumn,
        sortDirection:
            sortDirection
    };
    // Bill Number
    if (billNumber) {
        requestData.billNumber =
            billNumber;
    }
    // Client
    if (clientID) {
        requestData.clientID =
            clientID;
    }
    // Voyage
    if (voyageID) {
        requestData.voyageID =
            voyageID;
    }
    console.log(
        "Filter Request:",
        requestData
    );
    $.ajax({
        url:
            billsApiUrl +
            "/Filter",
        type: "GET",
        data: requestData,
        success: function (response) {
            console.log(
                "Filter Response:",
                response
            );
            const bills =
                response.Bills || [];
            const totalRecords =
                response.TotalRecords || 0;
            displayBills(bills);
            renderPagination(
                totalRecords
            );
        },
        error: handleError
    });
}
/* =========================================
   CLEAR ADVANCED SEARCH
========================================= */
function clearAdvancedSearch() {
    $("#searchBillNumber").val("");
    $("#searchClientId").val("");
    $("#searchVoyageId").val("");
    currentPage = 1;
    loadBills();
}
/* =========================================
   DISPLAY BILLS
========================================= */
function displayBills(bills) {
    let rows = "";
    if (
        !bills ||
        bills.length === 0
    ) {
        rows =
            `<tr>
                <td colspan="7"
                    class="text-center">
                    No records found.
                </td>
            </tr>`;
    }
    else {
        $.each(
            bills,
            function (index, bill) {
                const client =
                    clientsData.find(
                        c =>
                            c.ID ===
                            bill.ClientID
                    );
                const voyage =
                    voyagesData.find(
                        v =>
                            v.ID ===
                            bill.VoyageID
                    );
                const clientName =
                    bill.ClientName ||
                    (
                        client
                            ? client.Name
                            : ""
                    );
                const voyageNumber =
                    bill.VoyageNumber ||
                    (
                        voyage
                            ? voyage.VoyageNumber
                            : ""
                    );
                rows += `
                    <tr>
                        <td>
                            ${bill.ID}
                        </td>
                        <td>
                            ${bill.BillNumber}
                        </td>
                        <td>
                            ${clientName}
                        </td>
                        <td>
                            ${voyageNumber}
                        </td>
                        <td>
                            ${bill.GrossWeight}
                        </td>
                        <td>
                            ${bill.NetWeight}
                        </td>
                        <td>
                            <button
                                class="btn btn-warning btn-sm"
                                onclick="editBill(${bill.ID})">
                                Edit
                            </button>
                            <button
                                class="btn btn-danger btn-sm"
                                onclick="deleteBill(${bill.ID})">
                                Delete
                            </button>
                        </td>
                    </tr>
                `;
            }
        );
    }
    $("#billsTableBody").html(
        rows
    );
}
/* =========================================
   PAGINATION
========================================= */
function renderPagination(
    totalRecords
) {
    const totalPages =
        Math.ceil(
            totalRecords / pageSize
        );
    let pagination = "";
    // No pagination needed
    if (totalPages <= 1) {
        $("#pagination").html("");
        return;
    }
    // =========================================
    // PREVIOUS
    // =========================================
    pagination += `
        <li class="page-item
            ${currentPage === 1
            ? "disabled"
            : ""}">
            <button
                class="page-link"
                type="button"
                onclick="changePage(${currentPage - 1})"
                ${currentPage === 1
            ? "disabled"
            : ""}>
                Previous
            </button>
        </li>
    `;
    // =========================================
    // PAGE NUMBERS
    // =========================================
    for (
        let i = 1;
        i <= totalPages;
        i++
    ) {
        pagination += `
            <li class="page-item
                ${currentPage === i
                ? "active"
                : ""}">
                <button
                    class="page-link"
                    type="button"
                    onclick="changePage(${i})">
                    ${i}
                </button>
            </li>
        `;
    }
    // =========================================
    // NEXT
    // =========================================
    pagination += `
        <li class="page-item
            ${currentPage === totalPages
            ? "disabled"
            : ""}">
            <button
                class="page-link"
                type="button"
                onclick="changePage(${currentPage + 1})"
                ${currentPage === totalPages
            ? "disabled"
            : ""}>
                Next
            </button>
        </li>
    `;
    $("#pagination").html(
        pagination
    );
}
/* =========================================
   CHANGE PAGE
========================================= */
function changePage(page) {
    if (page < 1) {
        return;
    }
    currentPage = page;
    loadBills();
}
/* =========================================
   SORTING
   SERVER-SIDE
========================================= */
function sortBills(column) {
    // Same column -> reverse direction
    if (sortColumn === column) {
        if (sortDirection === "ASC") {
            sortDirection = "DESC";
        }
        else {
            sortDirection = "ASC";
        }
    }
    // New column
    else {
        sortColumn = column;
        sortDirection = "ASC";
    }
    // Go to first page
    currentPage = 1;
    // Ask SQL Server to sort
    loadBills();
}
/* =========================================
   SAVE BILL
========================================= */
function saveBill() {
    const id =
        $("#billId").val();
    const bill = {
        BillNumber:
            $("#billNumber")
                .val()
                .trim(),
        ClientID:
            Number(
                $("#clientId").val()
            ),
        VoyageID:
            Number(
                $("#voyageId").val()
            ),
        GrossWeight:
            Number(
                $("#grossWeight").val()
            ),
        NetWeight:
            Number(
                $("#netWeight").val()
            )
    };
    if (!bill.BillNumber) {
        showMessage(
            "Bill Number is required.",
            "error"
        );
        return;
    }
    if (!bill.ClientID) {
        showMessage(
            "Client is required.",
            "error"
        );
        return;
    }
    if (!bill.VoyageID) {
        showMessage(
            "Voyage is required.",
            "error"
        );
        return;
    }
    if (
        $("#grossWeight").val() === "" ||
        bill.GrossWeight <= 0
    ) {
        showMessage(
            "Valid Gross Weight is required.",
            "error"
        );
        return;
    }
    if (
        $("#netWeight").val() === "" ||
        bill.NetWeight <= 0
    ) {
        showMessage(
            "Valid Net Weight is required.",
            "error"
        );
        return;
    }
    if (
        bill.NetWeight >
        bill.GrossWeight
    ) {
        showMessage(
            "Net Weight cannot be greater than Gross Weight.",
            "error"
        );
        return;
    }
    if (id) {
        updateBill(
            id,
            bill
        );
    }
    else {
        addBill(bill);
    }
}
/* =========================================
   ADD BILL
========================================= */
function addBill(bill) {
    $.ajax({
        url: billsApiUrl,
        type: "POST",
        contentType:
            "application/json",
        data:
            JSON.stringify(bill),
        success: function () {
            showMessage(
                "Bill added successfully.",
                "success"
            );
            clearForm();
            currentPage = 1;
            loadBills();
        },
        error: handleError
    });
}
/* =========================================
   UPDATE BILL
========================================= */
function updateBill(
    id,
    bill
) {
    $.ajax({
        url:
            billsApiUrl +
            "/" +
            id,
        type: "PUT",
        contentType:
            "application/json",
        data:
            JSON.stringify(bill),
        success: function () {
            showMessage(
                "Bill updated successfully.",
                "success"
            );
            clearForm();
            loadBills();
        },
        error: handleError
    });
}
/* =========================================
   EDIT BILL
========================================= */
function editBill(id) {
    $.ajax({
        url:
            billsApiUrl +
            "/" +
            id,
        type: "GET",
        success: function (bill) {
            $("#billId").val(
                bill.ID
            );
            $("#billNumber").val(
                bill.BillNumber
            );
            $("#clientId").val(
                bill.ClientID
            );
            $("#voyageId").val(
                bill.VoyageID
            );
            $("#grossWeight").val(
                bill.GrossWeight
            );
            $("#netWeight").val(
                bill.NetWeight
            );
            $("#saveBillBtn")
                .text("Update");
            window.scrollTo({
                top: 0,
                behavior: "smooth"
            });
        },
        error: handleError
    });
}
/* =========================================
   DELETE BILL
========================================= */
function deleteBill(id) {
    if (
        !confirm(
            "Are you sure you want to delete this bill?"
        )
    ) {
        return;
    }
    $.ajax({
        url:
            billsApiUrl +
            "/" +
            id,
        type: "DELETE",
        success: function () {
            showMessage(
                "Bill deleted successfully.",
                "success"
            );
            loadBills();
        },
        error: handleError
    });
}
/* =========================================
   CLEAR FORM
========================================= */
function clearForm() {
    $("#billId").val("");
    $("#billNumber").val("");
    $("#clientId").val("");
    $("#voyageId").val("");
    $("#grossWeight").val("");
    $("#netWeight").val("");
    $("#saveBillBtn")
        .text("Save");
}
/* =========================================
   SHOW MESSAGE
========================================= */
function showMessage(
    message,
    type
) {
    const cssClass =
        type === "success"
            ? "success-message"
            : "error-message";
    $("#message").html(
        `<div class="${cssClass}">
            ${message}
        </div>`
    );
}
/* =========================================
   HANDLE ERROR
========================================= */
function handleError(xhr) {
    console.log(
        "AJAX Error Status:",
        xhr.status
    );
    console.log(
        "AJAX Error Response:",
        xhr.responseText
    );
    if (xhr.status === 400) {
        showMessage(
            xhr.responseText ||
            "Invalid data.",
            "error"
        );
    }
    else if (xhr.status === 404) {
        showMessage(
            "Record not found.",
            "error"
        );
    }
    else {
        showMessage(
            "Something went wrong. Please try again.",
            "error"
        );
    }
}