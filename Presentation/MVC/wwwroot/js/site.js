var updateRow;
function ToastSuccess(message) {
    $.toast({
        heading: 'Success',
        text: message,
        showHideTransition: 'slide',
        icon: 'success',
        position: 'bottom-right'
    });
}

function ToastError(message) {
    $.toast({
        heading: 'Error',
        text: message,
        showHideTransition: 'fade',
        icon: 'error',
        position: 'bottom-right'
    });
}

function SweetSuccess(message = "Saved Successfully!") {
    Swal.fire({
        title: "Success",
        text: message,
        icon: "success"
    });
}

function SweetError(message = "Something went wrong!") {
    Swal.fire({
        icon: "error",
        title: "Oops...",
        text: message
    });
}


$(document).ready(function () {

    var modal = $("#modal");
    var btn = $("body").on("click", ".js-modal", function () {
        var btn = $(this);
        var title = btn.data("title") || "Modal";
        modal.find(".modal-title").text(title);
        var url = btn.data("url");
        if (btn.data("edit") != undefined)
            updateRow = btn.parents("tr");        
        $.get({
            url: url,
            success: function (data) {                
                modal.find(".modal-body").html(data);
                $.validator.unobtrusive.parse(modal);
            },
            error: function (err) {
                modal.find(".modal-body").html("<div class='alert alert-danger'>Error loading content</div>");
            }
        })

        modal.modal("show");
    })
    var deletebtn = $("body").on("click", ".js-delete-modal", function () {
        var deletebtn = $(this);
        var title = deletebtn.data("title") || "Modal";
        modal.find(".modal-title").text(title);
        var url = deletebtn.data("url");
        var row = deletebtn.parents("tr");
        console.log(row);
        bootbox.confirm({
            message: 'Are you sure, You want to delete this item?',
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        url: url,
                        type: "DELETE",
                        success: function () {                            
                            $(row).remove();
                            SweetSuccess("Deleted Successfully Successfully");
                        },
                        error: function (xhr, status, error)  {
                            SweetError();
                        }
                    })
                }
            }
        });        
    })

});

function onModalSuccess(item) {
    
    $("#modal").modal("hide");
    if (updateRow != undefined) {
        $(updateRow).replaceWith(item);
        updateRow = undefined;
    } else {

        $("tbody").append(item);
    }

    //$("html").load("/Categories/Index");
    SweetSuccess();

}