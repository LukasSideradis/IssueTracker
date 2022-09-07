var dataTable;
var name;

$(document).ready(function () {
    LoadDataTable(name);
 });

function LoadDataTable(name)
{
    dataTable = $('#tableData').DataTable({
        "order": [7, 'asc'],
        columnDefs: [
            {
                "render": function (data) {
                    return ToNiceDateFormat(data);
                },
                targets: [4, 5]
            },
        ],
        "ajax": {
            "url": "../Issue/GetSpecificUserIssues",
            "type": "get",
            "data": {
                userName: name
            }
        },
        "columns": [
            {
                "data": "name",
                "render": function (data, type, row) {
                    return `
                        <a href="../Issue/Details/${row.id}/details" class="text">
                            ${data}
                        </a>
                           `;
                },
                "width": "16%"
            },
            { "data": "description", "width": "20%" },
            { "data": "type", "width": "4%" },
            { "data": "userName", "width": "12%" },
            { "data": "createdDate", "width": "10%" },
            { "data": "lastUpdated", "width": "10%" },
            { "data": "priority", "width": "5%" },
            { "data": "status", "width": "4%" },
        ]
    })
}

// Formats the raw date value into a more palatable state
function ToNiceDateFormat(string)
{
    return string.substring(0, 4) + "\-" + string.substring(5, 7) + "\-"
        + string.substring(8, 10) + " " + string.substring(11, 19);
}