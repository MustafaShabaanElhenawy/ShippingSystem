// Show / hide password (works on any page)
// $(document).on("click", ".toggle-password", function () {
//     const $input = $($(this).data("target"));
//     const show = $input.attr("type") === "password";

//     $input.attr("type", show ? "text" : "password");
//     $(this).text(show ? "Hide" : "Show");
// });

// (function () {
//     const path = location.pathname.toLowerCase();
//     const isAuthPage =
//         path.endsWith("login.html") || path.endsWith("register.html");
//     const token = sessionStorage.getItem("authToken");

//     Not signed in -> go to the sign in page
//     if (!token && !isAuthPage) {
//         location.replace("login.html");
//         return;
//     }

//     Already signed in -> no need for the sign in / sign up pages
//     if (token && isAuthPage) {
//         location.replace("index.html");
//         return;
//     }

//     Send the token with every jQuery AJAX request
//     $.ajaxSetup({
//         beforeSend: function (xhr) {
//             const t = sessionStorage.getItem("authToken");
//             if (t) {
//                 xhr.setRequestHeader("Authorization", "Bearer " + t);
//             }
//         }
//     });

//     Token expired / app restarted -> back to sign in
//     $(document).ajaxError(function (event, xhr) {
//         if (xhr.status === 401 && !isAuthPage) {
//             sessionStorage.clear();
//             location.replace("login.html");
//         }
//     });

//     Show the username next to the Logout button
//     $(function () {
//         $("#currentUser").text(sessionStorage.getItem("username") || "");
//     });
// })();

// function logout() {
//     $.ajax({
//         url: "https:localhost:44320/api/Auth/Logout",
//         type: "POST",
//         complete: function () {
//             sessionStorage.clear();
//             location.href = "login.html";
//         }
//     });
// }

// Show / hide password (works on any page)
$(document).on("click", ".toggle-password", function () {
    const $input = $($(this).data("target"));
    const show = $input.attr("type") === "password";

    $input.attr("type", show ? "text" : "password");
    $(this).text(show ? "Hide" : "Show");
});

(function () {
    const path = location.pathname.toLowerCase();
    const isAuthPage =
        path.endsWith("login.html") || path.endsWith("register.html");
    const token = sessionStorage.getItem("authToken");
    const isAdmin = sessionStorage.getItem("role") === "Admin";

    // Not signed in -> go to the sign in page
    if (!token && !isAuthPage) {
        location.replace("login.html");
        return;
    }

    // Already signed in -> no need for the sign in / sign up pages
    if (token && isAuthPage) {
        location.replace("index.html");
        return;
    }

    // Audit Logs is for administrators only
    if (path.endsWith("audit-logs.html") && !isAdmin) {
        location.replace("index.html");
        return;
    }

    // Send the token with every jQuery AJAX request
    $.ajaxSetup({
        beforeSend: function (xhr) {
            const t = sessionStorage.getItem("authToken");
            if (t) {
                xhr.setRequestHeader("Authorization", "Bearer " + t);
            }
        }
    });

    // Token expired / app restarted -> back to sign in
    $(document).ajaxError(function (event, xhr) {
        if (xhr.status === 401 && !isAuthPage) {
            sessionStorage.clear();
            location.replace("login.html");
        }
    });

    $(function () {
        // Username next to the Logout button
        $("#currentUser").text(sessionStorage.getItem("username") || "");

        // Hide the Audit Logs link for non-admins (UX only; the API enforces it)
        if (!isAdmin) {
            $('a[href="audit-logs.html"]').closest("li").hide();
        }
    });
})();

function logout() {
    $.ajax({
        url: "https://localhost:44320/api/Auth/Logout",
        type: "POST",
        complete: function () {
            sessionStorage.clear();
            location.href = "login.html";
        }
    });
}


