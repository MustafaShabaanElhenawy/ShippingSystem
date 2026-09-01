const containersApiUrl =
    "https://localhost:44320/api/Containers";

const billsApiUrl =
    "https://localhost:44320/api/Bills";


let billsData = [];


$(document).ready(function () {

    loadBillsDropdown();

    loadContainers();

    $("#saveContainerBtn")
        .click(saveContainer);

    $("#clearContainerBtn")
        .click(clearForm);

    $("#searchInput")
        .on("keyup", searchContainers);

});



function loadBillsDropdown() {

    $.ajax({

        url: billsApiUrl,

        type: "GET",

        success: function (bills) {

            billsData = bills;

            let options =
                `<option value="">
                    Select Bill
                </option>`;


            $.each(bills, function (index, bill) {

                options +=
                    `<option value="${bill.ID}">
                        ${bill.BillNumber}
                    </option>`;

            });


            $("#billId").html(options);

        },

        error: handleError

    });

}



function loadContainers() {

    $.ajax({

        url: containersApiUrl,

        type: "GET",

        success: displayContainers,

        error: handleError

    });

}



function displayContainers(containers) {

    let rows = "";


    if (containers.length === 0) {

        rows =
            `<tr>
                <td colspan="5"
                    class="text-center">

                    No records found.

                </td>
            </tr>`;

    }


    $.each(
        containers,
        function (index, container) {

            const bill =
                billsData.find(
                    b =>
                        b.ID === container.BillID
                );


            rows += `

                <tr>

                    <td>${container.ID}</td>

                    <td>
                        ${container.ContainerNumber}
                    </td>

                    <td>
                        ${container.ContainerType}
                    </td>

                    <td>
                        ${bill ? bill.BillNumber : ""}
                    </td>

                    <td>

                        <button
                            class="btn btn-warning btn-sm"
                            onclick="editContainer(${container.ID})">

                            Edit

                        </button>


                        <button
                            class="btn btn-danger btn-sm"
                            onclick="deleteContainer(${container.ID})">

                            Delete

                        </button>

                    </td>

                </tr>

            `;

        }
    );


    $("#containersTableBody").html(rows);

}



function saveContainer() {

    const id =
        $("#containerId").val();


    const container = {

        ContainerNumber:
            $("#containerNumber")
                .val()
                .trim(),

        ContainerType:
            $("#containerType")
                .val()
                .trim(),

        BillID:
            Number($("#billId").val())

    };


    if (!container.ContainerNumber) {

        showMessage(
            "Container Number is required.",
            "error"
        );

        return;

    }


    if (!container.ContainerType) {

        showMessage(
            "Container Type is required.",
            "error"
        );

        return;

    }


    if (!container.BillID) {

        showMessage(
            "Bill is required.",
            "error"
        );

        return;

    }


    if (id) {

        updateContainer(
            id,
            container
        );

    }
    else {

        addContainer(container);

    }

}



function addContainer(container) {

    $.ajax({

        url: containersApiUrl,

        type: "POST",

        contentType:
            "application/json",

        data:
            JSON.stringify(container),

        success: function () {

            showMessage(
                "Container added successfully.",
                "success"
            );

            clearForm();

            loadContainers();

        },

        error: handleError

    });

}



function updateContainer(
    id,
    container
) {

    $.ajax({

        url:
            containersApiUrl +
            "/" +
            id,

        type: "PUT",

        contentType:
            "application/json",

        data:
            JSON.stringify(container),

        success: function () {

            showMessage(
                "Container updated successfully.",
                "success"
            );

            clearForm();

            loadContainers();

        },

        error: handleError

    });

}



function editContainer(id) {

    $.ajax({

        url:
            containersApiUrl +
            "/" +
            id,

        type: "GET",

        success: function (container) {

            $("#containerId")
                .val(container.ID);

            $("#containerNumber")
                .val(
                    container.ContainerNumber
                );

            $("#containerType")
                .val(
                    container.ContainerType
                );

            $("#billId")
                .val(container.BillID);


            $("#saveContainerBtn")
                .text("Update");


            window.scrollTo({
                top: 0,
                behavior: "smooth"
            });

        },

        error: handleError

    });

}



function deleteContainer(id) {

    if (!confirm(
        "Are you sure you want to delete this container?"
    )) {

        return;

    }


    $.ajax({

        url:
            containersApiUrl +
            "/" +
            id,

        type: "DELETE",

        success: function () {

            showMessage(
                "Container deleted successfully.",
                "success"
            );

            loadContainers();

        },

        error: handleError

    });

}



function searchContainers() {

    const search =
        $("#searchInput")
            .val()
            .trim();


    if (!search) {

        loadContainers();

        return;

    }


    $.ajax({

        url:
            containersApiUrl +
            "/Search?search=" +
            encodeURIComponent(search),

        type: "GET",

        success: displayContainers,

        error: handleError

    });

}



function clearForm() {

    $("#containerId").val("");

    $("#containerNumber").val("");

    $("#containerType").val("");

    $("#billId").val("");

    $("#saveContainerBtn")
        .text("Save");

    $("#message").html("");

}



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