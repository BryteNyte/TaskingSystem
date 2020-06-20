$(document).ready(function () {
    var UserTable = $('#datatable-responsive').DataTable({
        retrieve: true,
        paging: false,
        dom: 'lrtip',
        initComplete: function () {
            this.api().columns([5]).every(function () {
                var column = this;
                console.log(column);
                var select = $("#company");
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>');
                });
            });

            $("#company").material_select();
        }
    });
    $('#UsersearchButton').click(function () {

        var search = [];

        $.each($('#company option:selected'), function () {
            search.push($(this).val());
        });

        search = search.join('|');
        UserTable.column(5).search(search, true, false).draw();
        $active = $("#yesNo option:selected").text();
        UserTable.search($active, true).draw();

    });
});

$(document).ready(function () {
    var UserTable = $('#datatable-responsive ').DataTable();

    $('#UsersearchButton').click(function () {

        UserTable.column(1).search($("#filter_Name").val()).draw();

    });
});

$(document).ready(function () {
    var YesNoTable = $('#datatable-responsive').DataTable();
    $('#YesNoSearchBtn').click(function () {
        YesNoTable.column(1).search($("#filt_ynName").val()).draw();
    });

});
$(document).ready(function () {
    var CmpTable = $('#datatable-responsive ').DataTable();

    $('#CmpsearchButton').click(function () {
        $active = $("#yesNo option:selected").text();
        CmpTable.column(1).search($("#filt_comName").val()).draw();
        CmpTable.search($active, true).draw();
    });
});