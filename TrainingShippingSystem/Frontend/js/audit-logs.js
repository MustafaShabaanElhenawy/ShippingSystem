$(document).ready(function () {

    const API_BASE_URL = "https://localhost:44320/api/AuditLogs";

    let currentPage = 1;
    let currentPageSize = 10;
    let totalPages = 0;

    const detailsModal = new bootstrap.Modal(
        document.getElementById("detailsModal")
    );

    // Escape HTML to safely display API values.
    function escapeHtml(value) {
        if (value === null || value === undefined || value === "") {
            return "-";
        }

        return $("<div>").text(String(value)).html();
    }

    // Convert UTC timestamps to Cairo local time.
    function formatDate(value) {
        if (!value) {
            return "-";
        }

        let dateValue = value;

        // API timestamps without timezone information are treated as UTC.
        if (
            typeof value === "string" &&
            !/(Z|[+-]\d{2}:\d{2})$/i.test(value)
        ) {
            dateValue = value + "Z";
        }

        const date = new Date(dateValue);

        if (isNaN(date.getTime())) {
            return escapeHtml(value);
        }

        return date.toLocaleString("en-GB", {
            timeZone: "Africa/Cairo",
            year: "numeric",
            month: "2-digit",
            day: "2-digit",
            hour: "2-digit",
            minute: "2-digit",
            second: "2-digit",
            hour12: true
        });
    }

    function getActionClass(action) {
        switch (String(action || "").toUpperCase()) {
            case "CREATE":
                return "action-create";

            case "UPDATE":
                return "action-update";

            case "DELETE":
                return "action-delete";

            case "LOGIN":
                return "action-login";

            default:
                return "";
        }
    }

    function showError(message) {
        $("#errorMessage")
            .removeClass("d-none")
            .text(message);
    }

    function hideError() {
        $("#errorMessage")
            .addClass("d-none")
            .text("");
    }

    // Read an error message from an API response (400 / 403 / ...).
    function getErrorMessage(xhr, fallback) {

        const response = xhr.responseJSON;

        if (response && (response.Message || response.message)) {
            return response.Message || response.message;
        }

        return fallback;
    }

    // Supports either a direct array or a named array in the response.
    function extractArray(response, possibleNames) {

        if (Array.isArray(response)) {
            return response;
        }

        if (response && typeof response === "object") {

            for (let i = 0; i < possibleNames.length; i++) {

                const property = possibleNames[i];

                if (Array.isArray(response[property])) {
                    return response[property];
                }
            }
        }

        return [];
    }

    // Load users filter.
    function loadUsers() {

        $.ajax({
            url: API_BASE_URL + "/Users",
            type: "GET",

            success: function (response) {

                const users = extractArray(response, [
                    "Users",
                    "Records",
                    "Data"
                ]);

                $("#userFilter").html(
                    '<option value="">All Users</option>'
                );

                users.forEach(function (user) {

                    const userName =
                        typeof user === "string"
                            ? user
                            : user.UserName || user.Name || user.userName;

                    if (userName) {
                        $("#userFilter").append(
                            $("<option>").val(userName).text(userName)
                        );
                    }
                });
            },

            error: function () {
                console.warn("Could not load audit users.");
            }
        });
    }

    // Load modules filter.
    function loadModules() {

        $.ajax({
            url: API_BASE_URL + "/Modules",
            type: "GET",

            success: function (response) {

                const modules = extractArray(response, [
                    "Modules",
                    "Records",
                    "Data"
                ]);

                $("#moduleFilter").html(
                    '<option value="">All Modules</option>'
                );

                modules.forEach(function (module) {

                    const moduleName =
                        typeof module === "string"
                            ? module
                            : module.ModuleName || module.Name || module.moduleName;

                    if (moduleName) {
                        $("#moduleFilter").append(
                            $("<option>").val(moduleName).text(moduleName)
                        );
                    }
                });
            },

            error: function () {
                console.warn("Could not load audit modules.");
            }
        });
    }

    // Read filters and build API query.
    function getFilters() {

        return {
            Search: $("#searchInput").val().trim(),
            UserName: $("#userFilter").val(),
            ModuleName: $("#moduleFilter").val(),
            ActionType: $("#actionFilter").val(),
            FromDate: $("#fromDate").val(),
            ToDate: $("#toDate").val(),
            PageNumber: currentPage,
            PageSize: currentPageSize
        };
    }

    // Load audit logs.
    function loadAuditLogs() {

        hideError();

        $("#loadingMessage").removeClass("d-none");
        $("#auditLogsTableBody").empty();
        $("#emptyMessage").addClass("d-none");

        $("#resultsInfo").text("Loading records...");

        currentPageSize = parseInt($("#pageSize").val(), 10) || 10;

        $.ajax({

            url: API_BASE_URL,
            type: "GET",
            data: getFilters(),
            dataType: "json",

            success: function (response) {

                $("#loadingMessage").addClass("d-none");

                const records = response.Records || [];

                const totalRecords = response.TotalRecords || 0;

                currentPage = response.PageNumber || currentPage;

                totalPages = response.TotalPages || 0;

                $("#totalActivities").text(totalRecords);
                $("#currentPage").text(currentPage);
                $("#totalPages").text(totalPages);

                $("#resultsInfo").text(
                    totalRecords + " record(s) found"
                );

                if (records.length === 0) {

                    $("#emptyMessage").removeClass("d-none");

                    $("#paginationInfo").text("");
                    renderPagination();

                    return;
                }

                renderAuditLogs(records);

                renderPagination();

                const firstRecord =
                    ((currentPage - 1) * currentPageSize) + 1;

                const lastRecord = Math.min(
                    currentPage * currentPageSize,
                    totalRecords
                );

                $("#paginationInfo").text(
                    "Showing " + firstRecord + " - " +
                    lastRecord + " of " + totalRecords + " records"
                );
            },

            error: function (xhr) {

                $("#loadingMessage").addClass("d-none");

                showError(
                    getErrorMessage(xhr, "Failed to load audit logs.")
                );

                $("#resultsInfo").text("Unable to load records.");

                $("#totalActivities").text("0");
                $("#currentPage").text("1");
                $("#totalPages").text("0");

                $("#pagination").empty();
                $("#paginationInfo").text("");
            }
        });
    }

    // Render table rows.
    function renderAuditLogs(records) {

        const tbody = $("#auditLogsTableBody");

        tbody.empty();

        records.forEach(function (log) {

            const action = log.ActionType || "";

            const actionClass = getActionClass(action);

            const row = `
                <tr>
                    <td>${escapeHtml(log.AuditLogID)}</td>

                    <td>${formatDate(log.CreatedAt)}</td>

                    <td>${escapeHtml(log.UserName)}</td>

                    <td>${escapeHtml(log.ModuleName)}</td>

                    <td>
                        <span class="audit-action ${actionClass}">
                            ${escapeHtml(action)}
                        </span>
                    </td>

                    <td>${escapeHtml(log.RecordID)}</td>

                    <td class="description-cell">
                        ${escapeHtml(log.Description)}
                    </td>

                    <td>
                        <button class="btn btn-sm btn-outline-primary details-btn"
                                data-id="${escapeHtml(log.AuditLogID)}">
                            View Details
                        </button>
                    </td>
                </tr>
            `;

            tbody.append(row);
        });
    }

    // Pagination controls.
    function renderPagination() {

        const pagination = $("#pagination");

        pagination.empty();

        if (totalPages <= 1) {
            return;
        }

        const previousDisabled =
            currentPage <= 1 ? "disabled" : "";

        pagination.append(`
            <li class="page-item ${previousDisabled}">
                <button class="page-link" data-page="${currentPage - 1}">
                    Previous
                </button>
            </li>
        `);

        let startPage = Math.max(1, currentPage - 2);

        let endPage = Math.min(totalPages, currentPage + 2);

        for (let page = startPage; page <= endPage; page++) {

            const active = page === currentPage ? "active" : "";

            pagination.append(`
                <li class="page-item ${active}">
                    <button class="page-link" data-page="${page}">
                        ${page}
                    </button>
                </li>
            `);
        }

        const nextDisabled =
            currentPage >= totalPages ? "disabled" : "";

        pagination.append(`
            <li class="page-item ${nextDisabled}">
                <button class="page-link" data-page="${currentPage + 1}">
                    Next
                </button>
            </li>
        `);
    }

    // Render the details table according to the action type.
    //   UPDATE -> Field / Old Value / New Value
    //   CREATE -> Field / Value (the values of the new record)
    //   DELETE -> Field / Value (the values of the deleted record)
    //   LOGIN  -> no field details
    function renderDetails(action, details) {

        const head = $("#detailsTableHead");
        const body = $("#detailsTableBody");

        body.empty();

        $("#noDetailsMessage").addClass("d-none");

        let title = "Changed Fields";
        let emptyText = "No field changes were recorded for this activity.";

        if (action === "CREATE") {

            title = "Created Values";
            emptyText = "No values were recorded for this activity.";

            head.html(
                "<tr><th>Field Name</th><th>Value</th></tr>"
            );

            details.forEach(function (detail) {
                body.append(`
                    <tr>
                        <td>${escapeHtml(detail.FieldName)}</td>
                        <td>${escapeHtml(detail.NewValue)}</td>
                    </tr>
                `);
            });
        }
        else if (action === "DELETE") {

            title = "Deleted Values";
            emptyText = "No values were recorded for this activity.";

            head.html(
                "<tr><th>Field Name</th><th>Value</th></tr>"
            );

            details.forEach(function (detail) {
                body.append(`
                    <tr>
                        <td>${escapeHtml(detail.FieldName)}</td>
                        <td>${escapeHtml(detail.OldValue)}</td>
                    </tr>
                `);
            });
        }
        else {

            if (action === "LOGIN") {
                title = "Details";
                emptyText = "This activity has no field details.";
            }

            head.html(
                "<tr><th>Field Name</th><th>Old Value</th><th>New Value</th></tr>"
            );

            details.forEach(function (detail) {
                body.append(`
                    <tr>
                        <td>${escapeHtml(detail.FieldName)}</td>
                        <td>${escapeHtml(detail.OldValue)}</td>
                        <td>${escapeHtml(detail.NewValue)}</td>
                    </tr>
                `);
            });
        }

        $("#detailsTitle").text(title);

        if (details.length === 0) {

            $("#detailsTableWrapper").addClass("d-none");

            $("#noDetailsMessage")
                .text(emptyText)
                .removeClass("d-none");
        }
        else {
            $("#detailsTableWrapper").removeClass("d-none");
        }
    }

    // Load one audit log and its details.
    function loadAuditDetails(logID) {

        $("#detailsLoading").removeClass("d-none");
        $("#detailsError").addClass("d-none");
        $("#detailsContent").addClass("d-none");

        $("#detailsTableBody").empty();
        $("#noDetailsMessage").addClass("d-none");

        detailsModal.show();

        $.ajax({

            url: API_BASE_URL + "/" + encodeURIComponent(logID),
            type: "GET",
            dataType: "json",

            success: function (response) {

                $("#detailsLoading").addClass("d-none");

                const log = response.AuditLog;
                const details = response.Details || [];

                if (!log) {
                    $("#detailsError")
                        .removeClass("d-none")
                        .text("Audit log information was not found.");

                    return;
                }

                $("#detailLogId").text(log.AuditLogID);
                $("#detailUser").text(log.UserName || "-");
                $("#detailModule").text(log.ModuleName || "-");
                $("#detailAction").text(log.ActionType || "-");
                $("#detailDescription").text(log.Description || "-");

                renderDetails(
                    String(log.ActionType || "").toUpperCase(),
                    details
                );

                $("#detailsContent").removeClass("d-none");
            },

            error: function (xhr) {

                $("#detailsLoading").addClass("d-none");

                $("#detailsError")
                    .removeClass("d-none")
                    .text(
                        getErrorMessage(
                            xhr,
                            "Failed to load audit log details."
                        )
                    );
            }
        });
    }

    // Apply filters.
    $("#filterForm").on("submit", function (event) {

        event.preventDefault();

        const fromDate = $("#fromDate").val();
        const toDate = $("#toDate").val();

        if (fromDate && toDate && fromDate > toDate) {

            showError("From Date cannot be later than To Date.");

            return;
        }

        currentPage = 1;

        loadAuditLogs();
    });

    // Reset filters.
    $("#resetBtn").on("click", function () {

        $("#filterForm")[0].reset();

        currentPage = 1;

        hideError();

        loadAuditLogs();
    });

    // Change page.
    $("#pagination").on("click", ".page-link", function () {

        const selectedPage = parseInt(
            $(this).attr("data-page"),
            10
        );

        if (
            isNaN(selectedPage) ||
            selectedPage < 1 ||
            selectedPage > totalPages ||
            selectedPage === currentPage
        ) {
            return;
        }

        currentPage = selectedPage;

        loadAuditLogs();
    });

    // Change page size.
    $("#pageSize").on("change", function () {

        currentPage = 1;

        loadAuditLogs();
    });

    // Refresh logs.
    $("#refreshBtn").on("click", function () {

        loadAuditLogs();
    });

    // View details.
    $("#auditLogsTableBody").on(
        "click",
        ".details-btn",
        function () {

            const logID = $(this).attr("data-id");

            loadAuditDetails(logID);
        }
    );

    // Initial loading.
    loadUsers();
    loadModules();
    loadAuditLogs();

});