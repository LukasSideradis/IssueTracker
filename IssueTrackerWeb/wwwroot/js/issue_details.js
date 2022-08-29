$(document).ready(function ()
{
    let elem = document.querySelectorAll(".modify-readonly");
    for (var i = 0; i < elem.length; i++) {
        elem[i].setAttribute("readonly", '');
    }
});

// set the slider label and slider to the correct position based on priority value
$(document).ready(function ()
{
    $('#priority_slider_label').text($('#priority').val());

    switch ($('#priority').val()) {
        case "Low":
            $('#priority_slider').val("1");
            break;
        case "Standard":
            $('#priority_slider').val("2");
            break;
        case "Important":
            $('#priority_slider').val("3");
            break;
        case "Critical":
            $('#priority_slider').val("4");
            break;
        default:
            break;
    }
});

// set the slider label to the correct position based on slider value
$('#priority_slider').change(function ()
{
    switch ($('#priority_slider').val()) {
        case "1":
            $('#priority_slider_label').text("Low");
            break;
        case "2":
            $('#priority_slider_label').text("Standard");
            break;
        case "3":
            $('#priority_slider_label').text("Important");
            break;
        case "4":
            $('#priority_slider_label').text("Critical");
            break;
        default:
            break;
    }
});

function Delete(url) {
    Swal.fire({
        title: 'Do you want to delete this issue?',
        text: "This action can't be reversed!",
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#87ceeb',
        confirmButtonText: 'Delete'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        window.location = "/Guest/Issue";
                    }
                    else {
                        Swal.fire({
                            text: data.message
                        })
                    }
                }
            })
        }
    })
};