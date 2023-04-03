//== Class definition

var EditBaoHang = function () {
    let dtTable = null;
    let kt_edit_form = $("#kt_edit_form");
    let form_buttonSubmit = $("#kt_form_edit_submit");
    let modal_form_submit_text = $("#kt_form_edit_submit_text");
    let editingDataRow = null;
    
    let validator = FormValidation.formValidation(kt_edit_form[0], {
        fields: {
            edit_form_code: { validators: { notEmpty: { message: "Vui lòng nhập mã seal bao" } } },
           
            
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger(),
            bootstrap: new FormValidation.plugins.Bootstrap5({ rowSelector: ".fv-row", eleInvalidClass: "", eleValidClass: "" })
        },
    });

    let initialComponents = () => {
        $('#dtTableSearch').on('keyup', function (e) {
            e.preventDefault();
            if (e.keyCode == 13) {
                dtTable.draw();
            }
        });
        $('[name="btnRefreshData"]').click(function (e) {
            e.preventDefault();
            dtTable.draw();
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
        form_buttonSubmit.click(function (e) {
            e.preventDefault();
            let result = {};
            if (validator) {
                validator.validate().then(function (e) {
                    "Valid" == e ?
                        (
                            form_buttonSubmit.attr("data-kt-indicator", "on"), (form_buttonSubmit.disabled = !0),
                            kt_edit_form.find("select, textarea, input").each((index, el) => {
                                let fieldName = $(el).data("field");
                                if (fieldName) {

                                    switch (fieldName) {
                                        case "IsDongBao":

                                            result[$(el).data("field")] = $(el).is(":checked");

                                            break;
                                        default:
                                            result[$(el).data("field")] = $(el).val();
                                    }
                                }
                            }),
                            data = {
                                Id: (editingData != null ? editingData.id : null),
                                Data: result
                            },
                            App.sendDataToURL("/ThaoBaoHang/Save", data, "POST")
                            .then(function (res)
                            {
                                form_buttonSubmit.removeAttr("data-kt-indicator");
                                if (!res.isSuccessed) {
                                    Swal.fire(App.swalFireErrorDefaultOption(res.message))
                                }
                                else {
                                    Swal.fire(App.swalFireSuccessDefaultOption())
                                    editingDataRow = null;
                                    resetForm();
                                    dtTable.draw();
                                    (form_buttonSubmit.disabled = !1);
                                }
                            }
                        )
                    )
                        : Swal.fire(App.swalFireErrorDefaultOption());
                    
                    
                });
            }
            dtTable.draw();

        });

        // form add
        App.initSelectCodeBase($('[name="edit_form_donHangIds"]'), '/VanDon/Filter', { selectedFields: ["id", "code"] });
        flatpickr('.flatpickr', flatpickrDateDefaultOption());
        
        $('[name="edit_form_donHangIds"]').val(null).trigger("change");
        if (editingData && editingData.vanDons && editingData.vanDons.length > 0) {
            let htmlOption = "";
            editingData.vanDons.forEach((item) => {
                htmlOption += "<option value=" + item.id + " selected>" + item.code + "</option>";
            });

            $('[name="edit_form_donHangIds"]').append(htmlOption).trigger('change');
        }
        
    };
    let initialDatatable = function () {
        var datatableOption = initialDatatableOption();
        datatableOption.ajax.url = "/DongBaoHang/DataTableVanDonInBaoGetList";
        datatableOption.ajax.data = {
            textSearch: function () {
                return $('#dtTableSearch').val();
            },
            filterByMaSealBao: function () {
                return $('#filterByMaSealBao').val();
            },
        };
        datatableOption.order = [[1, "desc"]];
        datatableOption.columnDefs = [
            {
                "targets": [1],
                className: 'dt-body-center',
                "visible": false
            },
            {
                "targets": [0, 3],
                className: 'dt-body-center',
                "orderable": true
            }
        ];

        datatableOption.initComplete = function () {
            var datatablesColumnsApi = this.api().columns();

            new Promise(function (resolve, reject) {
                datatablesColumnsApi.every(function (index) {
                    var column = this;

                    if (index == 0) {
                        $(column.header()).html('<div class="form-check form-check-sm form-check-custom form-check-solid">\
                            <input id="checkbox-select-all"  class="form-check-input" type ="checkbox"  value ="" >\
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
                },
            },
            {
                "data": "appUser", "name": "appUser", "autoWidth": true, "title": "Nhân viên thao tác", "render": function (data, type, full, meta) {
                    return '<span class="text-muted">' + data.fullName + '</span>';
                },
            },
            { "data": "isTrongBao", "name": "isTrongBao", "autoWidth": true, "title": "Trong bao" },
            {
                width: "120px", "title": "Hành động", "render": function (data, type, full, meta) {
                    let html = '<div class="d-flex justify-content-center flex-shrink-0">';
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

        
        $('#dtTable tbody').on('click', 'a.delete', function (e) {
            e.preventDefault();
            let selectedDataRow = dtTable.row($(this).parents('tr')).data();
            if (selectedDataRow) {
                deleteDataRows([selectedDataRow]);
            }
        });
    };
    function deleteDataRows(dataRows) {
        App.LayDonHang({ ids: dataRows.map((item) => item.id), id: editingData.id }, "/ThaoBaoHang/LayHangRaBao", dtTable, null)
            .then(function () {
                dtTable.draw();
                App.showHideButtonDelete(false);
            });
    }
    let resetForm = () => {
        kt_edit_form[0].reset();
    }

    

    

    return {
        // public functions
        init: function () {
            initialComponents();
            initialDatatable();
        }
    };
}();