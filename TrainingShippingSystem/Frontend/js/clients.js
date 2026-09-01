const apiBaseUrl = "https://localhost:44320/api/Clients";


$(document).ready(function () {

    loadClients();


    $("#saveClientBtn").click(function () {

        saveClient();

    });


    $("#clearClientBtn").click(function () {

        clearForm();

    });


    $("#searchInput").on("keyup", function () {

        searchClients();

    });

});



function loadClients() {

    $.ajax({

        url: apiBaseUrl,

        type: "GET",

        success: function (clients) {

            displayClients(clients);

        },

        error: function () {

            showMessage(
                "Something went wrong. Please try again.",
                "error"
            );

        }

    });

}



function displayClients(clients) {

    let rows = "";


    if (clients.length === 0) {

        rows =
            `<tr>
                <td colspan="6" class="text-center">
                    No records found.
                </td>
            </tr>`;

    }


    $.each(clients, function (index, client) {

        rows += `

            <tr>

                <td>${client.ID}</td>

                <td>${client.Name}</td>

                <td>${client.Email}</td>

                <td>${client.Phone || ""}</td>

                <td>${client.Address || ""}</td>

                <td>

                    <button
                        class="btn btn-warning btn-sm action-btn"
                        onclick="editClient(${client.ID})">

                        Edit

                    </button>


                    <button
                        class="btn btn-danger btn-sm"
                        onclick="deleteClient(${client.ID})">

                        Delete

                    </button>

                </td>

            </tr>

        `;

    });


    $("#clientsTableBody").html(rows);

}



function saveClient() {

    const id = $("#clientId").val();


    const client = {

        Name: $("#name").val().trim(),

        Email: $("#email").val().trim(),

        Phone: $("#phone").val().trim(),

        Address: $("#address").val().trim()

    };


    if (!client.Name) {

        showMessage("Name is required.", "error");

        return;

    }


    if (!client.Email) {

        showMessage("Email is required.", "error");

        return;

    }


    const emailPattern =
        /^[^\s@]+@[^\s@]+\.[^\s@]+$/;


    if (!emailPattern.test(client.Email)) {

        showMessage("Please enter a valid email.", "error");

        return;

    }


    if (id) {

        updateClient(id, client);

    }
    else {

        addClient(client);

    }

}



function addClient(client) {

    $.ajax({

        url: apiBaseUrl,

        type: "POST",

        contentType: "application/json",

        data: JSON.stringify(client),

        success: function () {

            showMessage(
                "Client added successfully.",
                "success"
            );

            clearForm();

            loadClients();

        },

        error: function (xhr) {

            handleError(xhr);

        }

    });

}



function updateClient(id, client) {

    $.ajax({

        url: apiBaseUrl + "/" + id,

        type: "PUT",

        contentType: "application/json",

        data: JSON.stringify(client),

        success: function () {

            showMessage(
                "Client updated successfully.",
                "success"
            );

            clearForm();

            loadClients();

        },

        error: function (xhr) {

            handleError(xhr);

        }

    });

}



function editClient(id) {

    $.ajax({

        url: apiBaseUrl + "/" + id,

        type: "GET",

        success: function (client) {

            $("#clientId").val(client.ID);

            $("#name").val(client.Name);

            $("#email").val(client.Email);

            $("#phone").val(client.Phone);

            $("#address").val(client.Address);


            $("#saveClientBtn")
                .text("Update");

            window.scrollTo({
                top: 0,
                behavior: "smooth"
            });

        },

        error: function (xhr) {

            handleError(xhr);

        }

    });

}



function deleteClient(id) {

    const confirmed =
        confirm(
            "Are you sure you want to delete this client?"
        );


    if (!confirmed) {

        return;

    }


    $.ajax({

        url: apiBaseUrl + "/" + id,

        type: "DELETE",

        success: function () {

            showMessage(
                "Client deleted successfully.",
                "success"
            );

            loadClients();

        },

        error: function (xhr) {

            handleError(xhr);

        }

    });

}



function searchClients() {

    const search =
        $("#searchInput").val().trim();


    if (!search) {

        loadClients();

        return;

    }


    $.ajax({

        url:
            apiBaseUrl +
            "/Search?search=" +
            encodeURIComponent(search),

        type: "GET",

        success: function (clients) {

            displayClients(clients);

        },

        error: function (xhr) {

            handleError(xhr);

        }

    });

}



function clearForm() {

    $("#clientId").val("");

    $("#name").val("");

    $("#email").val("");

    $("#phone").val("");

    $("#address").val("");

    $("#saveClientBtn")
        .text("Save");


    $("#message").html("");

}



function showMessage(message, type) {

    if (type === "success") {

        $("#message")
            .html(
                `<div class="success-message">
                    ${message}
                </div>`
            );

    }
    else {

        $("#message")
            .html(
                `<div class="error-message">
                    ${message}
                </div>`
            );

    }

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