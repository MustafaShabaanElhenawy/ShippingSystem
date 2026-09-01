const reportsApiUrl =
    "https://localhost:44320/api/Reports";


$(document).ready(function () {

    loadReports();

});



function loadReports() {

    $.ajax({

        url: reportsApiUrl,

        type: "GET",

        success: function (reports) {

            displayReports(reports);

        },

        error: function () {

            $("#reportsTableBody").html(

                `<tr>

                    <td colspan="9"
                        class="text-center text-danger">

                        Something went wrong.
                        Please try again.

                    </td>

                </tr>`

            );

        }

    });

}



function displayReports(reports) {

    let rows = "";


    if (reports.length === 0) {

        rows =
            `<tr>

                <td colspan="9"
                    class="text-center">

                    No records found.

                </td>

            </tr>`;

    }


    $.each(
        reports,
        function (index, report) {

            rows += `

                <tr>

                    <td>
                        ${report.BillNumber}
                    </td>

                    <td>
                        ${report.ClientName}
                    </td>

                    <td>
                        ${report.VoyageNumber}
                    </td>

                    <td>
                        ${report.VesselName}
                    </td>

                    <td>
                        ${formatDate(report.ETA)}
                    </td>

                    <td>
                        ${formatDate(report.ETD)}
                    </td>

                    <td>
                        ${report.GrossWeight}
                    </td>

                    <td>
                        ${report.NetWeight}
                    </td>

                    <td>
                        ${report.ContainerCount}
                    </td>

                </tr>

            `;

        }
    );


    $("#reportsTableBody")
        .html(rows);

}



function formatDate(dateString) {

    return new Date(dateString)
        .toLocaleString();

}