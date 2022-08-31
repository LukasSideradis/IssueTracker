var dataTable;

$(document).ready(function ()
{
    LoadDataTable();
});

function LoadDataTable()
{
    dataTable = $('#tableData').DataTable({
        "createdRow": function (data, row, index) {
            if (data.cells[7].textContent == "Resolved") {
                data.style.backgroundColor = '#bbbbbb';
            }
        },
        "search": {
            "search": GetSearchParams("priority")
        },
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
            "url":"Issue/GetAll"
        },
        "columns": [
            {
                "data": "name",
                "render": function (data, type, row) {
                    return `
                        <a href="Issue/Details/${row.id}/details" class="text">
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

function GetSearchParams(string)
{
    let searchParams = new URLSearchParams(window.location.search);
    if (searchParams.has(string)) {
        return searchParams.get(string);
    }
    else {
        return;
    }
}