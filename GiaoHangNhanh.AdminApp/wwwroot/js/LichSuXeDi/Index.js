//== Class definition

var LichSuXeDi = function () {
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
            kt_modal_edit_form_buuCucId: { validators: { notEmpty: { message: "vui lòng chọn trạm tiếp" } } },
            kt_modal_edit_form_vanDonId: { validators: { notEmpty: { message: "vui lòng nhập mã vận đơn" } } },
            kt_modal_edit_form_sealXe: { validators: { notEmpty: { message: "vui lòng nhập mã seal xe" } } },
            kt_modal_edit_form_sealBao: { validators: { notEmpty: { message: "vui lòng nhập mã seal bao" } } },
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

        $('[name="btnCreate"]').click(function (e) {
            e.preventDefault();

            editingDataRow = null;
            modal_header_text.text("Thêm bưu cục");
            modal_form_submit_text.text("Thêm");
            resetForm();
            modal.modal("show");
        });

        $('[name="btnRefreshData"]').click(function (e) {
            e.preventDefault();
            dtTable.draw();
        });

        $('#kt_modal_edit_submit').click(function (e) {
            e.preventDefault();
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
                            App.sendDataToURL("/LichSuXeDi/Save", data, "POST")
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

        $('#btnDelete').click(function (e) {
            e.preventDefault();
            let arr = [];

            $('#dtTable tbody input[type="checkbox"]:checked').each(function () {
                let selectedDataRow = dtTable.row($(this).parents('tr')).data();
                arr.push({ id: selectedDataRow.id });
            });

            if (arr.length > 0) {
                deleteDataRows(arr);
            }
        });

        // modal add
        App.initSelect2Base($('[name="kt_modal_edit_form_buuCucId"]'), '/BuuCuc/Filter');
        App.initSelectCodeBase($('[name="kt_modal_edit_form_vanDonId"]'), '/VanDon/Filter');

    };

    let initialDatatable = function () {
        var datatableOption = initialDatatableOption();
        datatableOption.ajax.url = "/LichSuXeDi/DataTableGetList";
        datatableOption.ajax.data = {
            textSearch: function () {
                return $('#dtTableSearch').val();
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

                $('#checkbox-select-all').on('change', function (e) {
                    e.preventDefault();

                    $('#dtTable tbody input[type="checkbox"]').prop('checked', this.checked);

                    if (this.checked) {
                        App.showHideButtonDelete(true);
                    } else {
                        App.showHideButtonDelete(false);
                    }
                });

                resolve();
            }).then(function (res) {
                dtTable.columns.adjust();
            });
        };
        datatableOption.columns = [
            {
                "autoWidth": true, "title": "", "render": function (data, type, full, meta) {
                    let html = '<div class="form-check form-check-sm form-check-custom form-check-solid">\
                            <input class="form-check-input" onchange="App.initevencheckbox()" type ="checkbox" value ="" >\
                                    </div>';
                    return html;
                }
            },
            { "data": "id", "name": "id", "autoWidth": true, "title": "Id" },
            {
                "data": "vanDon", "name": "vanDon", "autoWidth": true, "title": "Mã vận đơn", "render": function (data, type, full, meta) {
                    return '<span class="text-muted">' + data.code + '</span>';
                }
            },
            {
                "data": "buuCuc", "name": "buuCuc", "autoWidth": true, "title": "Trạm Tiếp", "render": function (data, type, full, meta) {
                    return '<span class="text-muted">' + data.name + '</span>';
                }
            },
            { "data": "sealXe", "name": "sealXe", "autoWidth": true, "title": "Seal xe" },
            { "data": "sealBao", "name": "sealBao", "autoWidth": true, "title": "Seal bao" },
            { "data": "createdDate", "name": "createdDate", "autoWidth": true, "title": "Ngày thao tác" },
            {
                width: "120px", "title": "Hành động", "render": function (data, type, full, meta) {
                    let html = '<div class="d-flex justify-content-center flex-shrink-0">';
                    if (user.roles.isAllowEdit == true) {
                        html += '<a href="#" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1 edit">\
					                <span class="svg-icon svg-icon-3">\
						                <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">\
							                <path d="M12.2674799,18.2323597 L12.0084872,5.45852451 C12.0004303,5.06114792 12.1504154,4.6768183 12.4255037,4.38993949 L15.0030167,1.70195304 L17.5910752,4.40093695 C17.8599071,4.6812911 18.0095067,5.05499603 18.0083938,5.44341307 L17.9718262,18.2062508 C17.9694575,19.0329966 17.2985816,19.701953 16.4718324,19.701953 L13.7671717,19.701953 C12.9505952,19.701953 12.2840328,19.0487684 12.2674799,18.2323597 Z" fill="#000000" fill-rule="nonzero" transform="translate(14.701953, 10.701953) rotate(-135.000000) translate(-14.701953, -10.701953)"></path>\
							                <path d="M12.9,2 C13.4522847,2 13.9,2.44771525 13.9,3 C13.9,3.55228475 13.4522847,4 12.9,4 L6,4 C4.8954305,4 4,4.8954305 4,6 L4,18 C4,19.1045695 4.8954305,20 6,20 L18,20 C19.1045695,20 20,19.1045695 20,18 L20,13 C20,12.4477153 20.4477153,12 21,12 C21.5522847,12 22,12.4477153 22,13 L22,18 C22,20.209139 20.209139,22 18,22 L6,22 C3.790861,22 2,20.209139 2,18 L2,6 C2,3.790861 3.790861,2 6,2 L12.9,2 Z" fill="#000000" fill-rule="nonzero" opacity="0.3"></path>\
						                </svg>\
					                </span>\
				                </a>';
                    }
                    if (user.roles.isAllowDelete == true) {
                        html += '<a href="#" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm delete">\
					                <span class="svg-icon svg-icon-3">\
						                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">\
							                <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">\
								                <rect x="0" y="0" width="24" height="24"></rect>\
								                <path d="M6,8 L6,20.5 C6,21.3284271 6.67157288,22 7.5,22 L16.5,22 C17.3284271,22 18,21.3284271 18,20.5 L18,8 L6,8 Z" fill="#000000" fill-rule="nonzero"></path>\
								                <path d="M14,4.5 L14,4 C14,3.44771525 13.5522847,3 13,3 L11,3 C10.4477153,3 10,3.44771525 10,4 L10,4.5 L5.5,4.5 C5.22385763,4.5 5,4.72385763 5,5 L5,5.5 C5,5.77614237 5.22385763,6 5.5,6 L18.5,6 C18.7761424,6 19,5.77614237 19,5.5 L19,5 C19,4.72385763 18.7761424,4.5 18.5,4.5 L14,4.5 Z" fill="#000000" opacity="0.3"></path>\
							                </g>\
						                </svg>\
					                </span>\
				                </a>';
                    }
                    html += '</div>';

                    return html;

                }
            },
        ]
        dtTable = $('#dtTable').DataTable(datatableOption);

        $('#dtTable tbody').on('click', 'a.edit', function (e) {
            e.preventDefault();
            let selectedDataRow = dtTable.row($(this).parents('tr')).data();
            editingDataRow = selectedDataRow;
            $('[name="kt_modal_edit_form_vanDonId"]').append("<option value=" + selectedDataRow.buuCuc.id + " selected>" + selectedDataRow.buuCuc.code + "</option>").trigger('change');
            $('[name="kt_modal_edit_form_buuCucId"]').append("<option value=" + selectedDataRow.buuCuc.id + " selected>" + selectedDataRow.buuCuc.name + "</option>").trigger('change');
            $('input[data-field="SealXe"]').val(selectedDataRow.sealXe);
            $('input[data-field="SealBao"]').val(selectedDataRow.sealBao);
            $('input[data-field="CreatedDate"]').val(selectedDataRow.createdDate);

            modal_header_text.text("Cập nhật lịch sủ xe đi");
            modal_form_submit_text.text("Cập nhật");
            setTimeout(function () { $('input[name="kt_modal_edit_form_code"]').focus() }, 500);
            modal.modal('show');

        });

        $('#dtTable tbody').on('click', 'a.delete', function (e) {
            e.preventDefault();
            let selectedDataRow = dtTable.row($(this).parents('tr')).data();
            if (selectedDataRow) {
                deleteDataRows([selectedDataRow]);
            }
        });
    };

    let resetForm = () => {
        modal_form[0].reset();
        $('[name="kt_modal_edit_form_buuCucId"]').val(null).trigger('change');
        $('[name="kt_modal_edit_form_vanDonId"]').val(null).trigger('change');
        setTimeout(function () { $('input[name="kt_modal_edit_form_code"]').focus() }, 500);
    }

    function deleteDataRows(dataRows) {
        App.deleteDataConfirm({ ids: dataRows.map((item) => item.id) }, "/LichSuXeDi/DeleteByIds", dtTable, null)
            .then(function () {
                dtTable.draw();
                App.showHideButtonDelete(false);
            });
    }

    return {
        // public functions
        init: function () {
            initialDatatable();
            initialComponents();
        }
    };
}();