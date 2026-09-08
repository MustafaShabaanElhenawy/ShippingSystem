$(document).ready(function () {
    loadDashboardStatistics();
});


$(document).ajaxStart(function () {

    $("#loadingIndicator").css("display", "flex");

});


$(document).ajaxStop(function () {

    $("#loadingIndicator").hide();

});

function loadDashboardStatistics() {

    $.ajax({
        url: "https://localhost:44320/api/Dashboard",
        type: "GET",

        success: function (result) {

            $("#totalClients").text(result.TotalClients);
            $("#totalVoyages").text(result.TotalVoyages);
            $("#totalBills").text(result.TotalBills);
            $("#totalContainers").text(result.TotalContainers);
        },

        error: function () {

            alert("Something went wrong. Please try again.");
        }
    });
}