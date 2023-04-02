var DuyetChuyenHoan = function () {
    let dtTable = null;
    let modal = $("#kt_modal_edit"),
        modal_form = $("#kt_modal_edit_form"),
        modal_form_buttonSubmit = $('#kt_modal_edit_submit'),
        modal_header_text = $("#kt_modal_edit_header_text"),
        modal_form_submit_text = $("#kt_modal_edit_submit_text");
    filter_form_buttonSubmit = $("#kt_filter_form_button_submit");


    let editingDataRow = null;

    let validator = FormValidation.formValidation(modal_form[0], {
        fields: {
            kt_modal_edit_form_vandon_id: { validators: { notEmpty: { message: "vui lòng nhập mã vận đơn" } } },
            kt_modal_edit_form_nguyennhan: { validators: { notEmpty: { message: "vui lòng nhập nguyên nhân chuyển hoàn" } } },

        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger(),
            bootstrap: new FormValidation.plugins.Bootstrap5({ rowSelector: ".fv-row", eleInvalidClass: "", eleValidClass: "" })
        },
    });

    let initialComponents = () => {

        $('#kt_filter_form_button_submit').click(function (e) {
            e.preventDefault();
            dtTable.draw();
        });

        

        $('#dtTableSearch').on('keyup', function (e) {
            e.preventDefault();
            if (e.keyCode == 13) {
                dtTable.draw();
            }
        });

        $('[name="btnDuyet"]').click(function (e) {
            e.preventDefault();
            let arr = [];

            $('#dtTable tbody input[type="checkbox"]:checked').each(function () {
                let selectedDataRow = dtTable.row($(this).parents('tr')).data();
                arr.push({ id: selectedDataRow.id });
                editingDataRow = selectedDataRow;
            });

            if (arr.length == 1) {
                modal_header_text.text("Duyệt chuyển hoàn");
                modal_form_submit_text.text("Cập nhật");
                modal.modal('show');
            }
            else {
                show.modal.message("Chỉ được duyệt 1 đơn");
            }
            
        });

        $('#filterByVanDonId').change(function (e) {
            dtTable.draw();
        });

        $('[name="btnRefreshData"]').click(function (e) {
            e.preventDefault();
            dtTable.draw();
        });

        $('#kt_modal_edit_submit').click(function (e) {
            e.preventDefault();
            console.log(editingDataRow);
            let result = {};
            if (validator) {
                validator.validate().then(function (e) {
                    "Valid" == e ?
                        (
                            modal_form_buttonSubmit.attr("data-kt-indicator", "on"), (modal_form_buttonSubmit.disabled = !0),

                            modal_form.find("select, textarea, input").each((index, el) => {
                                let fieldName = $(el).data("field");

                                if (fieldName) {

                                    switch (fieldName) {

                                        default:
                                            result[$(el).data("field")] = $(el).val();
                                    }
                                }
                            }),
                            data = {
                                Id: (editingDataRow != null ? editingDataRow.id : null),
                                Data: result
                            },
                            App.sendDataToURL("/DuyetChuyenHoan/Save", data, "POST")
                                .then(function (res) {
                                    modal_form_buttonSubmit.removeAttr("data-kt-indicator");
                                    if (!res.isSuccessed) {
                                        Swal.fire(App.swalFireErrorDefaultOption(res.message))
                                    }
                                    else {
                                        Swal.fire(App.swalFireSuccessDefaultOption())
                                        editingDataRow = null;
                                        dtTable.draw();
                                        resetForm();
                                        (modal_form_buttonSubmit.disabled = !1);
                                    }
                                }
                                )
                        )
                        : Swal.fire(App.swalFireErrorDefaultOption());
                });
            }
            $('input[name="kt_modal_edit_form_code"]').focus();
        });

        //$('#btnDuyet').click(function (e) {
        //    e.preventDefault();
        //    let arr = [];

        //    $('#dtTable tbody input[type="checkbox"]:checked').each(function () {
        //        let selectedDataRow = dtTable.row($(this).parents('tr')).data();
        //        arr.push({ id: selectedDataRow.id });
        //    });

        //    if (arr.length > 0) {
        //        modal.modal("show");
        //    }
        //    if (arr.length >= 2) {
        //        show.message("Chỉ được chọn 1 dòng");
        //    }
        //});

        // filter
        App.initSelectCodeBase($('#filterByVanDonId'), '/VanDon/Filter');
        App.initSelectCodeBase($('#kt_modal_edit_form_buucucduyethoan_id'), '/BuuCuc/Filter');
    };




    let initialDatatable = function () {
        var datatableOption = initialDatatableOption();
        datatableOption.ajax.url = "/DuyetChuyenHoan/DataTableGetList";
        datatableOption.ajax.data = {
            textSearch: function () {
                return $('#dtTableSearch').val();
            },
            filterByVanDonId: function () {
                return $('#filterByVanDonId').val();
            },
        },
            datatableOption.order = [[1, "desc"]];
        datatableOption.columnDefs = [
            {
                "targets": [1],
                className: 'dt-body-center',
                "visible": false
            },
            {
                "targets": [0, 4],
                className: 'dt-body-center',
                "orderable": false
            }
        ];

        datatableOption.initComplete = function () {
            var datatablesColumnsApi = this.api().columns();

            new Promise(function (resolve, reject) {
                datatablesColumnsApi.every(function (index) {
                    var column = this;

                    if (index == 0) {
                        $(column.header()).html('<div class="form-check form-check-sm form-check-custom form-check-solid">\
                            <input id="checkbox-select-all" class="form-check-input" type ="checkbox"  value ="" >\
                                    </div>');
                    }
                });

                resolve();
            }).then(function (res) {
                dtTable.columns.adjust();
            });
        };
        datatableOption.columns = [
            {
                "autoWidth": true, "render": function (data, type, full, meta) {
                    let html = '<div class="form-check form-check-sm form-check-custom form-check-solid">\
                            <input class="form-check-input" onchange="App.initevencheckbox()" type ="checkbox" >\
                                    </div>';
                    return html;
                }
            },
            { "data": "id", "name": "id", "autoWidth": true, "title": "Id" },

            { "data": "maVanDon", "name": "maVanDon", "autoWidth": true, "title": "Mã vận đơn" },

            { "data": "buuCucGuiHang", "name": "buuCucGuiHang", "autoWidth": true, "title": "Bưu cục gửi hàng"},

            { "data": "phanLoaiChuyenPhat", "name": "phanLoaiChuyenPhat", "autoWidth": true, "title": "Phân loại chuyển phát" },

            { "data": "tenKhachHang", "name": "tenKhachHang", "autoWidth": true, "title": "Tên khách hàng" },

            { "data": "rutVeDichDen", "name": "rutVeDichDen", "autoWidth": true, "title": "Rút về đích đến" },

            { "data": "nguyenNhanChuyenHoan", "name": "nguyenNhanChuyenHoan", "autoWidth": true, "title": "Nguyên nhân chuyển hoàn" },

            { "data": "ngayDuyetChuyenHoan", "name": "ngayDuyetChuyenHoan", "autoWidth": true, "title": "Ngày duyệt" },

            { "data": "buuCucDuyetChuyenHoan", "name": "buuCucDuyetChuyenHoan", "autoWidth": true, "title": "Bưu cục duyệt" },

            { "data": "isDaDuyet", "name": "isDaDuyet", "autoWidth": true, "title": "Duyệt chuyển hoàn" },

        ]
        dtTable = $('#dtTable').DataTable(datatableOption);

    };


    return {
        // public functions
        init: function () {
            initialDatatable();
            initialComponents();
        }
    };
}();