//== Class definition

var EditNhanVien = function () {
    let kt_edit_form = $("#kt_edit_form");
    let form_buttonSubmit = $("#kt_form_edit_submit");
    let modal_form_submit_text = $("#kt_form_edit_submit_text");
    let editingDataRow = null;
    
    let validator = FormValidation.formValidation(kt_edit_form[0], {
        fields: {
            kt_form_edit_form_ho_nhan_vien: { validators: { notEmpty: { message: "Vui lòng nhập họ nhân viên" } } },
            kt_form_edit_form_ten_nhan_vien: { validators: { notEmpty: { message: "Vui lòng nhập tên nhân viên" } } },
            kt_form_edit_form_ngaysinh: { validators: { notEmpty: { message: "Vui lòng nhập ngày sinh" } } },
            kt_form_edit_form_gioitinh: { validators: { notEmpty: { message: "Vui lòng chọn ngày giới tính" } } },
            kt_form_edit_form_buucucid: { validators: { notEmpty: { message: "Vui lòng chọn bưu cục làm việc" } } }
            
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger(),
            bootstrap: new FormValidation.plugins.Bootstrap5({ rowSelector: ".fv-row", eleInvalidClass: "", eleValidClass: "" })
        },
    });

    let initialComponents = () => {
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
                                        case "IsActive":

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
                            App.sendDataToURL("/NhanVien/Save", data, "POST")
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
                                    (form_buttonSubmit.disabled = !1);
                                    setTimeout(function () { window.location.href = "/NhanVien/Index" }, 1000);
                                }
                            }
                        )
                    )
                        : Swal.fire(App.swalFireErrorDefaultOption());
                    
                    
                });
            }
        });

        // form add
        App.initSelect2Base($('[name="kt_form_edit_form_gioitinh"]'), '/Gender/Filter',);
        $('[name="kt_form_edit_form_gioitinh"]').val(null).trigger("change");

        if (editingData != null) {
            if (editingData.gender != null) {
                $('[name="kt_form_edit_form_gioitinh"]').append("<option value='" + editingData.gender.id + "' selected>" + editingData.gender.name + "</option>").trigger('change');
            }
            if (editingData.buuCuc != null) {
                $('[name="kt_form_edit_form_buucucid"]').append("<option value='" + editingData.buuCuc.id + "' selected>" + editingData.buuCuc.name + "</option>").trigger('change');
            }
        }
        App.initSelect2Base($('[name="kt_form_edit_form_tinhid"]'), '/Tinh/Filter',);

        App.initSelect2Base($('[name="kt_form_edit_form_huyenid"]'), '/Huyen/Filter', {
            query: {
                filterByTinhId: function () {
                    return $('[name="kt_form_edit_form_tinhid"]').val() ? $('[name="kt_form_edit_form_tinhid"]').val() : null;
                }
            }
        });

        App.initSelect2Base($('[name="kt_form_edit_form_khuvucid"]'), '/KhuVuc/Filter', {
            query: {
                filterByHuyenId: function () {
                    return $('[name="kt_form_edit_form_huyenid"]').val() ? $('[name="kt_form_edit_form_huyenid"]').val() : null;
                }
            }
        });

        App.initSelect2Base($('[name="kt_form_edit_form_buucucid"]'), '/BuuCuc/Filter', {
            query: {
                filterByKhuVucId: function () {
                    return $('[name="kt_form_edit_form_khuvucid"]').val() ? $('[name="kt_form_edit_form_khuvucid"]').val() : null;
                }
            }
        });
        flatpickr('.flatpickr', flatpickrDateDefaultOption());
        
    };

    let resetForm = () => {
        kt_edit_form[0].reset();
    }

    

    

    return {
        // public functions
        init: function () {
            initialComponents();
        }
    };
}();