Table = $('.tableUpdate').DataTable({
    "destroy": true,
    "ordering": true,
    //"sDom":"flrtip"
    //"sDom": 'r<"contactsTable"t><p>',
    "oLanguage": {
        "oPaginate": {
            "sNext": '<i class="fa fa-chevron-right" ></i>',
            "sPrevious": '<i class="fa fa-chevron-left" ></i>'
        }
    }
});