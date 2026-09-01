const billsApiUrl =
    "https://localhost:44320/api/Bills";

const clientsApiUrl =
    "https://localhost:44320/api/Clients";

const voyagesApiUrl =
    "https://localhost:44320/api/Voyages";


let clientsData = [];

let voyagesData = [];


$(document).ready(function () {

    loadClientsDropdown();

    loadVoyagesDropdown();

    loadBills();

    $("#saveBillBtn").click(saveBill);

    $("#clearBillBtn").click(clearForm);

    $("#searchInput").on("keyup", searchBills);

});



function loadClientsDropdown() {

    $.ajax({

        url: clientsApiUrl,

        type: "GET",

        success: function (clients) {

            clientsData = clients;

            let options =
                `<option value="">
                    Select Client
                </option>`;


            $.each(clients, function (index, client) {

                options +=
                    `<option value="${client.ID}">
                        ${client.Name}
                    </option>`;

            });


            $("#clientId").html(options);

        },

        error: handleError

    });

}



function loadVoyagesDropdown() {

    $.ajax({

        url: voyagesApiUrl,

        type: "GET",

        success: function (voyages) {

            voyagesData = voyages;

            let options =
                `<option value="">
                    Select Voyage
                </option>`;


            $.each(voyages, function (index, voyage) {

                options +=
                    `<option value="${voyage.ID}">
                        ${voyage.VoyageNumber}
                    </option>`;

            });


            $("#voyageId").html(options);

        },

        error: handleError

    });

}



function loadBills() {

    $.ajax({

        url: billsApiUrl,

        type: "GET",

        success: displayBills,

        error: handleError

    });

}



function displayBills(bills) {

    let rows = "";


    if (bills.length === 0) {

        rows =
            `<tr>
                <td colspan="7"
                    class="text-center">

                    No records found.

                </td>
            </tr>`;

    }


    $.each(bills, function (index, bill) {

        const client =
            clientsData.find(
                c => c.ID === bill.ClientID
            );

        const voyage =
            voyagesData.find(
                v => v.ID === bill.VoyageID
            );


        rows += `

            <tr>

                <td>${bill.ID}</td>

                <td>${bill.BillNumber}</td>

                <td>
                    ${client ? client.Name : ""}
                </td>

                <td>
                    ${voyage ? voyage.VoyageNumber : ""}
                </td>

                <td>${bill.GrossWeight}</td>

                <td>${bill.NetWeight}</td>

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

    });


    $("#billsTableBody").html(rows);

}



function saveBill() {

    const id =
        $("#billId").val();


    const bill = {

        BillNumber:
            $("#billNumber").val().trim(),

        ClientID:
            Number($("#clientId").val()),

        VoyageID:
            Number($("#voyageId").val()),

        GrossWeight:
            Number($("#grossWeight").val()),

        NetWeight:
            Number($("#netWeight").val())

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
        bill.GrossWeight < 0
    ) {

        showMessage(
            "Valid Gross Weight is required.",
            "error"
        );

        return;

    }


    if (
        $("#netWeight").val() === "" ||
        bill.NetWeight < 0
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

        updateBill(id, bill);

    }
    else {

        addBill(bill);

    }

}



function addBill(bill) {

    $.ajax({

        url: billsApiUrl,

        type: "POST",

        contentType: "application/json",

        data: JSON.stringify(bill),

        success: function () {

            showMessage(
                "Bill added successfully.",
                "success"
            );

            clearForm();

            loadBills();

        },

        error: handleError

    });

}



function updateBill(id, bill) {

    $.ajax({

        url:
            billsApiUrl + "/" + id,

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



function editBill(id) {

    $.ajax({

        url:
            billsApiUrl + "/" + id,

        type: "GET",

        success: function (bill) {

            $("#billId").val(bill.ID);

            $("#billNumber")
                .val(bill.BillNumber);

            $("#clientId")
                .val(bill.ClientID);

            $("#voyageId")
                .val(bill.VoyageID);

            $("#grossWeight")
                .val(bill.GrossWeight);

            $("#netWeight")
                .val(bill.NetWeight);


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



function deleteBill(id) {

    if (!confirm(
        "Are you sure you want to delete this bill?"
    )) {

        return;

    }


    $.ajax({

        url:
            billsApiUrl + "/" + id,

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



function searchBills() {

    const search =
        $("#searchInput").val().trim();


    if (!search) {

        loadBills();

        return;

    }


    $.ajax({

        url:
            billsApiUrl +
            "/Search?search=" +
            encodeURIComponent(search),

        type: "GET",

        success: displayBills,

        error: handleError

    });

}



function clearForm() {

    $("#billId").val("");

    $("#billNumber").val("");

    $("#clientId").val("");

    $("#voyageId").val("");

    $("#grossWeight").val("");

    $("#netWeight").val("");

    $("#saveBillBtn")
        .text("Save");

    $("#message").html("");

}



function showMessage(message, type) {

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



function handleError(xhr) {

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