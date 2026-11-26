const registerUrl = "https://localhost:44320/api/Auth/Register";
const usernamePattern = /^[A-Za-z0-9_.]{3,30}$/;

$(document).ready(function () {

    $("#registerForm").on("submit", function (e) {
        e.preventDefault();
        doRegister();
    });

    $("#password").on("input", updateStrength);

    $("#registerForm input").on("input", function () {
        clearFieldError($(this));
    });

    $("#username").trigger("focus");
});

function showAlert(type, message) {
    const $alert = $('<div class="alert alert-' + type + '" role="alert"></div>')
        .text(message);

    $("#message").empty().append($alert);
}

function setFieldError(selector, message) {
    $(selector).addClass("is-invalid");
    $(selector + "Error").text(message);
}

function clearFieldError($input) {
    $input.removeClass("is-invalid");
    $("#" + $input.attr("id") + "Error").text("");
}

function setLoading(isLoading) {
    $("#registerBtn").prop("disabled", isLoading);
    $("#registerSpinner").toggleClass("d-none", !isLoading);
    $("#registerBtnText").text(isLoading ? "Creating account..." : "Create Account");
}

function extractMessage(xhr) {
    const r = xhr.responseJSON;
    return r && (r.message || r.Message) ? (r.message || r.Message) : null;
}

// Frontend validation (the backend validates again)
function validateForm(data) {

    let ok = true;

    $("#registerForm input").each(function () {
        clearFieldError($(this));
    });

    if (!data.Username) {
        setFieldError("#username", "Username is required.");
        ok = false;
    }
    else if (!usernamePattern.test(data.Username)) {
        setFieldError(
            "#username",
            "3-30 characters: letters, numbers, dot or underscore."
        );
        ok = false;
    }

    if (!data.Password) {
        setFieldError("#password", "Password is required.");
        ok = false;
    }
    else if (data.Password.length < 8) {
        setFieldError("#password", "Password must be at least 8 characters.");
        ok = false;
    }
    else if (!/[A-Za-z]/.test(data.Password) || !/\d/.test(data.Password)) {
        setFieldError(
            "#password",
            "Password must contain at least one letter and one number."
        );
        ok = false;
    }

    if (!data.ConfirmPassword) {
        setFieldError("#confirmPassword", "Please confirm your password.");
        ok = false;
    }
    else if (data.Password !== data.ConfirmPassword) {
        setFieldError("#confirmPassword", "Passwords do not match.");
        ok = false;
    }

    return ok;
}

function updateStrength() {

    const pwd = $("#password").val();

    const levels = [
        { w: 0, cls: "", text: "" },
        { w: 25, cls: "bg-danger", text: "Weak" },
        { w: 40, cls: "bg-danger", text: "Weak" },
        { w: 60, cls: "bg-warning", text: "Fair" },
        { w: 80, cls: "bg-info", text: "Good" },
        { w: 100, cls: "bg-success", text: "Strong" }
    ];

    let score = 0;

    if (pwd.length >= 8) score++;
    if (pwd.length >= 12) score++;
    if (/[a-z]/.test(pwd) && /[A-Z]/.test(pwd)) score++;
    if (/\d/.test(pwd)) score++;
    if (/[^A-Za-z0-9]/.test(pwd)) score++;

    const level = levels[pwd ? Math.max(score, 1) : 0];

    $("#strengthBar")
        .removeClass("bg-danger bg-warning bg-info bg-success")
        .addClass(level.cls)
        .css("width", level.w + "%");

    $("#strengthText").text(level.text);
}

function doRegister() {

    const data = {
        Username: $("#username").val().trim(),
        Password: $("#password").val(),
        ConfirmPassword: $("#confirmPassword").val()
    };

    $("#message").empty();

    if (!validateForm(data)) {
        return;
    }

    setLoading(true);

    $.ajax({
        url: registerUrl,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),

        success: function () {
            // Sign up done -> now sign in
            location.href = "login.html?registered=1";
        },

        error: function (xhr) {
            const message = extractMessage(xhr);

            if (xhr.status === 409) {
                setFieldError("#username", message || "Username already exists.");
            }
            else if (xhr.status === 400) {
                showAlert("danger", message || "Invalid data.");
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