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
                            App.sendDataToURL("/DongBaoHang/Save", data, "POST")
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
                                    setTimeout(function () { window.location.href = "/DongBaoHang/Index" }, 1000);
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
        App.deleteDataConfirm({ ids: dataRows.map((item) => item.id)}, "/DongBaoHang/DeleteByIds", dtTable, null)
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