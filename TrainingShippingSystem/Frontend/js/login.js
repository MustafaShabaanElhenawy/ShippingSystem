// const loginUrl = "https://localhost:44320/api/Auth/Login";

// $(document).ready(function () {

//     Coming from the sign up page
//     const params = new URLSearchParams(location.search);

//     if (params.get("registered") === "1") {
//         showAlert("success", "Account created successfully. Please sign in.");
//     }

//     $("#loginForm").on("submit", function (e) {
//         e.preventDefault();
//         doLogin();
//     });

//     $("#username").trigger("focus");
// });

// function showAlert(type, message) {
//     const $alert = $('<div class="alert alert-' + type + '" role="alert"></div>')
//         .text(message);

//     $("#message").empty().append($alert);
// }

// function setLoading(isLoading) {
//     $("#loginBtn").prop("disabled", isLoading);
//     $("#loginSpinner").toggleClass("d-none", !isLoading);
//     $("#loginBtnText").text(isLoading ? "Signing in..." : "Sign In");
// }

// function extractMessage(xhr) {
//     const r = xhr.responseJSON;
//     return r && (r.message || r.Message) ? (r.message || r.Message) : null;
// }

// function doLogin() {

//     const username = $("#username").val().trim();
//     const password = $("#password").val();

//     Frontend validation (the backend validates again)
//     if (!username) {
//         showAlert("danger", "Username is required.");
//         $("#username").trigger("focus");
//         return;
//     }

//     if (!password) {
//         showAlert("danger", "Password is required.");
//         $("#password").trigger("focus");
//         return;
//     }

//     setLoading(true);

//     $.ajax({
//         url: loginUrl,
//         type: "POST",
//         contentType: "application/json",
//         data: JSON.stringify({ Username: username, Password: password }),

//         success: function (res) {
//             sessionStorage.setItem("authToken", res.token);
//             sessionStorage.setItem("username", res.username);
//             location.href = "index.html";
//         },

//         error: function (xhr) {
//             if (xhr.status === 401 || xhr.status === 400) {
//                 showAlert("danger", extractMessage(xhr) || "Invalid data.");
//             }
//             else if (xhr.status === 0) {
//                 showAlert("danger", "Cannot reach the server. Please try again.");
//             }
//             else {
//                 showAlert("danger", "Something went wrong. Please try again.");
//             }

//             setLoading(false);
//         }
//     });
// }


const loginUrl = "https://localhost:44320/api/Auth/Login";

$(document).ready(function () {

    // Coming from the sign up page
    const params = new URLSearchParams(location.search);

    if (params.get("registered") === "1") {
        showAlert("success", "Account created successfully. Please sign in.");
    }

    $("#loginForm").on("submit", function (e) {
        e.preventDefault();
        doLogin();
    });

    $("#username").trigger("focus");
});

function showAlert(type, message) {
    const $alert = $('<div class="alert alert-' + type + '" role="alert"></div>')
        .text(message);

    $("#message").empty().append($alert);
}

function setLoading(isLoading) {
    $("#loginBtn").prop("disabled", isLoading);
    $("#loginSpinner").toggleClass("d-none", !isLoading);
    $("#loginBtnText").text(isLoading ? "Signing in..." : "Sign In");
}

function extractMessage(xhr) {
    const r = xhr.responseJSON;
    return r && (r.message || r.Message) ? (r.message || r.Message) : null;
}

function doLogin() {

    const username = $("#username").val().trim();
    const password = $("#password").val();

    // Frontend validation (the backend validates again)
    if (!username) {
        showAlert("danger", "Username is required.");
        $("#username").trigger("focus");
        return;
    }

    if (!password) {
        showAlert("danger", "Password is required.");
        $("#password").trigger("focus");
        return;
    }

    setLoading(true);

    $.ajax({
        url: loginUrl,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ Username: username, Password: password }),

        success: function (res) {
            sessionStorage.setItem("authToken", res.token);
            sessionStorage.setItem("username", res.username);
            sessionStorage.setItem("role", res.role);
            location.href = "index.html";
        },

        error: function (xhr) {
            if (xhr.status === 401 || xhr.status === 400) {
                showAlert("danger", extractMessage(xhr) || "Invalid data.");
            }
            else if (xhr.status === 0) {
                showAlert("danger", "Cannot reach the server. Please try again.");
            }
            else {
                showAlert("danger", "Something went wrong. Please try again.");
            }

            setLoading(false);
        }
    });
}