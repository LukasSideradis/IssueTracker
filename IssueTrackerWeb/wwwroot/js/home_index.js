$(document).ready(function ()
{
    const scrollElement = document.getElementById('middlePanel');
    scrollElement.style.overflowY = "hidden";
});

var dataTable;

$(document).ready(function ()
{
    LoadDataTableHighestPriority();
    LoadDataTableLatestIssues();
});

function LoadDataTableHighestPriority()
{
    dataTable = $('#tableData_HighestPriority').DataTable({
        "searching": false,
        "info": false,
        "lengthChange": false,
        "dom": "lfrti",
        "pageLength": 5,
        "order": [[2, 'asc']],
        columnDefs: [{
            "orderable": false,
            "targets": '_all'
            }],
        "ajax": {
            "url": "Guest/Issue/GetAll"
        },
        "columns": [
            {
                "data": "name",
                "render": function (data, type, row) {
                    return `
                        <a href="Guest/Issue/Details/${row.id}/details" class="text">
                            ${data}
                        </a>
                           `;
                },
                "width": "25%"
            },
            { "data": "description", "width": "60%" },
            { "data": "priority", "width": "15%" },
        ]
    })
}

function LoadDataTableLatestIssues()
{
    dataTable = $('#tableData_LatestIssues').DataTable({
        "searching": false,
        "info": false,
        "lengthChange": false,
        "dom": "lfrti",
        "pageLength": 5,
        "order": [[2, 'asc']],
        columnDefs: [
            {
                "orderable": false,
                "targets": '_all'
            },
            {
                "render": function (data) {
                    return ToTimeDifference(data);
                },
                "targets": [2]
            }
        ],
        "ajax": {
            "url": "Guest/Issue/GetAll"
        },
        "columns": [
            {
                "data": "name",
                "render": function (data, type, row) {
                    return `
                        <a href="Guest/Issue/Details/${row.id}/details" class="text">
                            ${data}
                        </a>
                           `;
                },
                "width": "25%"
            },
            { "data": "description", "width": "60%" },
            { "data": "createdDate", "width": "15%" },
        ]
    })
}

// Formats the raw date value into a more palatable state
function ToNiceDateFormat(string)
{
    return string.substring(0, 4) + "\-" + string.substring(5, 7) + "\-"
        + string.substring(8, 10) + " " + string.substring(11, 19);
}

function ToTimeDifference(string)
{
    var currentDate = Date.now();
    var createdDate = Date.parse(string);
    var result = Math.floor((currentDate - createdDate) / (24 * 3600 * 1000));

    if (result >= 1 && result < 2) {
        return result + " day ago";
    }
    else {
        return result + " days ago";
    }
}