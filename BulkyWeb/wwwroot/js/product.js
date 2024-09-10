﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});



function loadDataTable() {
    dataTable = $('#tblData').DataTable(
        {
            "ajax": {url:'/admin/product/getall'},
            "columns": [
                { data: 'title', "width":"18%" },
                { data: 'isbn', "width": "10%" },
                { data: 'author', "width": "18%" },
                { data: 'price' ,"width": "10%" },
                { data: 'category.name', "width": "18%" },
                {
                    data: 'id',
                    "render": function (data) {
                        return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">
                            <i class="bi bi-pen""></i>Edit
                             </a>
                             <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2">
                            <i class="bi bi-trash2"></i>Delete
                             </a>
                        </div > `
                    },
                    "width": "26%"
                }
            ]
        }
    );
}
function Delete(url) {
    Swal.fire({
        title: "Be Consious! for Deleting",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}