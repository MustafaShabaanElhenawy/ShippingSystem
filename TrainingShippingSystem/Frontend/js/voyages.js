const apiBaseUrl = "https://localhost:44320/api/Voyages";


$(document).ready(function () {

    loadVoyages();

    $("#saveVoyageBtn").click(saveVoyage);

    $("#clearVoyageBtn").click(clearForm);

    $("#searchInput").on("keyup", searchVoyages);

});



function loadVoyages() {

    $.ajax({

        url: apiBaseUrl,

        type: "GET",

        success: function (voyages) {

            displayVoyages(voyages);

        },

        error: function () {

            showMessage(
                "Something went wrong.",
                "error"
            );

        }

    });

}



function displayVoyages(voyages) {

    let rows = "";


    if (voyages.length === 0) {

        rows =
            `<tr>
                <td colspan="6" class="text-center">
                    No records found.
                </td>
            </tr>`;

    }


    $.each(voyages, function (index, voyage) {

        rows += `

            <tr>

                <td>${voyage.ID}</td>

                <td>${voyage.VoyageNumber}</td>

                <td>${voyage.VesselName}</td>

                <td>${formatDate(voyage.ETA)}</td>

                <td>${formatDate(voyage.ETD)}</td>

                <td>

                    <button
                        class="btn btn-warning btn-sm"
                        onclick="editVoyage(${voyage.ID})">

                        Edit

                    </button>

                    <button
                        class="btn btn-danger btn-sm"
                        onclick="deleteVoyage(${voyage.ID})">

                        Delete

                    </button>

                </td>

            </tr>

        `;

    });


    $("#voyagesTableBody").html(rows);

}



function saveVoyage() {

    const id =
        $("#voyageId").val();


    const voyage = {

        VoyageNumber:
            $("#voyageNumber").val().trim(),

        VesselName:
            $("#vesselName").val().trim(),

        ETA:
            $("#eta").val(),

        ETD:
            $("#etd").val()

    };


    if (!voyage.VoyageNumber) {

        showMessage(
            "Voyage Number is required.",
            "error"
        );

        return;

    }


    if (!voyage.VesselName) {

        showMessage(
            "Vessel Name is required.",
            "error"
        );

        return;

    }


    if (!voyage.ETA) {

        showMessage(
            "ETA is required.",
            "error"
        );

        return;

    }


    if (!voyage.ETD) {

        showMessage(
            "ETD is required.",
            "error"
        );

        return;

    }


    if (new Date(voyage.ETD) <
        new Date(voyage.ETA)) {

        showMessage(
            "ETD cannot be earlier than ETA.",
            "error"
        );

        return;

    }


    if (id) {

        updateVoyage(id, voyage);

    }
    else {

        addVoyage(voyage);

    }

}



function addVoyage(voyage) {

    $.ajax({

        url: apiBaseUrl,

        type: "POST",

        contentType: "application/json",

        data: JSON.stringify(voyage),

        success: function () {

            showMessage(
                "Voyage added successfully.",
                "success"
            );

            clearForm();

            loadVoyages();

        },

        error: handleError

    });

}



function updateVoyage(id, voyage) {

    $.ajax({

        url: apiBaseUrl + "/" + id,

        type: "PUT",

        contentType: "application/json",

        data: JSON.stringify(voyage),

        success: function () {

            showMessage(
                "Voyage updated successfully.",
                "success"
            );

            clearForm();

            loadVoyages();

        },

        error: handleError

    });

}



function editVoyage(id) {

    $.ajax({

        url: apiBaseUrl + "/" + id,

        type: "GET",

        success: function (voyage) {

            $("#voyageId").val(voyage.ID);

            $("#voyageNumber")
                .val(voyage.VoyageNumber);

            $("#vesselName")
                .val(voyage.VesselName);

            $("#eta")
                .val(formatDateForInput(voyage.ETA));

            $("#etd")
                .val(formatDateForInput(voyage.ETD));


            $("#saveVoyageBtn")
                .text("Update");

            window.scrollTo({
                top: 0,
                behavior: "smooth"
            });

        },

        error: handleError

    });

}



function deleteVoyage(id) {

    if (!confirm(
        "Are you sure you want to delete this voyage?"
    )) {

        return;

    }


    $.ajax({

        url: apiBaseUrl + "/" + id,

        type: "DELETE",

        success: function () {

            showMessage(
                "Voyage deleted successfully.",
                "success"
            );

            loadVoyages();

        },

        error: handleError

    });

}



function searchVoyages() {

    const search =
        $("#searchInput").val().trim();


    if (!search) {

        loadVoyages();

        return;

    }


    $.ajax({

        url:
            apiBaseUrl +
            "/Search?search=" +
            encodeURIComponent(search),

        type: "GET",

        success: displayVoyages,

        error: handleError

    });

}



function clearForm() {

    $("#voyageId").val("");

    $("#voyageNumber").val("");

    $("#vesselName").val("");

    $("#eta").val("");

    $("#etd").val("");

    $("#saveVoyageBtn")
        .text("Save");

    $("#message").html("");

}



function formatDate(dateString) {

    return new Date(dateString)
        .toLocaleString();

}



function formatDateForInput(dateString) {

    const date =
        new Date(dateString);

    const offset =
        date.getTimezoneOffset();

    const localDate =
        new Date(
            date.getTime() -
            offset * 60000
        );

    return localDate
        .toISOString()
        .slice(0, 16);

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