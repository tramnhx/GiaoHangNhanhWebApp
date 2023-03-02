//== Class definition

var EditVanDon = function () {
    let kt_edit_form = $("#kt_edit_form");
    let form_buttonSubmit = $("#kt_form_edit_submit");

    let editingDataRow = null;

    let validator = FormValidation.formValidation(kt_edit_form[0], {
        fields: {
            kt_modal_edit_form_buucucid: { validators: { notEmpty: { message: "vui lòng chọn bưu cục" } } },
            kt_form_edit_form_dichvuid: { validators: { notEmpty: { message: "vui lòng chọn dịch vụ" } } },
            kt_form_edit_form_phuongthucthanhtoanid: { validators: { notEmpty: { message: "vui lòng chọn phương thức thanh toán" } } },
            kt_form_edit_form_ten_nguoi_nhan: { validators: { notEmpty: { message: "vui lòng điền tên người gửi" } } },
            kt_form_edit_form_dien_thoai_nguoi_nhan: { validators: { notEmpty: { message: "vui lòng điền số điện thoại người nhận" } } },
            kt_form_edit_form_dia_chi_nguoi_nhan: { validators: { notEmpty: { message: "vui lòng điền địa chỉ người nhận" } } },
            kt_form_edit_form_ngayguihang: { notEmpty: { message: "vui lòng chọn ngày gửi hàng" } }
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger(),
            bootstrap: new FormValidation.plugins.Bootstrap5({ rowSelector: ".fv-row", eleInvalidClass: "", eleValidClass: "" })
        },
    });

    let initialComponents = () => {

        $('[name="kt_form_edit_form_khuvucid"]').change(function () {
            loadBuuCucByKhuVuc();
        });

        $('#kt_form_edit_submit').click(function (e) {
            e.preventDefault();
            let result = {};
            if (validator) {
                validator.validate().then(function (e) {
                    let formData = new FormData();
                    if (editingData != null) {
                        formData.append("id", (editingData != null ? editingData.id : null));
                    }
                    "Valid" == e ?
                        (
                            form_buttonSubmit.attr("data-kt-indicator", "on"), (form_buttonSubmit.disabled = !0),
                            kt_edit_form.find("select, textarea, input").each((index, el) => {
                                let fieldName = $(el).data("field");

                                if (fieldName) {

                                    switch (fieldName) {

                                        default:
                                            result[$(el).data("field")] = $(el).val();
                                    }
                                }
                            }),
                            data = {
                                Id: (editingData != null ? editingData.id : null),
                                Data: result
                            },
                            App.sendDataToURL("/VanDon/Save", data, "POST")
                                .then(function (res) {
                                    form_buttonSubmit.removeAttr("data-kt-indicator");
                                    if (!res.isSuccessed) {
                                        Swal.fire(App.swalFireErrorDefaultOption(res.message))
                                    }
                                    else {
                                        Swal.fire(App.swalFireSuccessDefaultOption())
                                        editingDataRow = null;
                                        resetForm();
                                        (form_buttonSubmit.disabled = !1);
                                    }
                                }
                                )
                        )
                        : Swal.fire(App.swalFireErrorDefaultOption());
                });
            }
        });

        // form add
        App.initSelect2Base($('[name="kt_form_edit_form_tinhid"]'), '/Tinh/Filter',);

        App.initSelect2Base($('[name="kt_form_edit_form_huyenid"]'), '/Huyen/Filter', {
            query: {
                filterByTinhId: function () {
                    return $('[name="kt_form_edit_form_tinhid"]').val() ? $('[name="kt_form_edit_form_tinhid"]').val() : '';
                }
            }
        });

        App.initSelect2Base($('[name="kt_form_edit_form_khuvucid"]'), '/KhuVuc/Filter', {
            query: {
                filterByHuyenId: function () {
                    return $('[name="kt_form_edit_form_huyenid"]').val() ? $('[name="kt_form_edit_form_huyenid"]').val() : '';
                }
            }
        });

        App.initSelect2Base($('[name="kt_form_edit_form_buucucid"]'), '/BuuCuc/Filter', {
            query: {
                filterByKhuVucId: function () {
                    return $('[name="kt_form_edit_form_khuvucid"]').val() ? $('[name="kt_form_edit_form_khuvucid"]').val() : '';
                }
            }
        });

        App.initSelect2Base($('[name="kt_form_edit_form_dichvuid"]'), '/DichVu/Filter');
        App.initSelect2Base($('[name="kt_form_edit_form_phuongthucthanhtoanid"]'), '/PhuongThucThanhToan/Filter');
        App.initSelect2Base($('[name="kt_form_edit_form_congtyguihangid"]'), '/CongTyGuiHang/Filter');
        userSelect2Custom($('[name="kt_form_edit_form_userid"]'), '/Staff/Filter');


        if (editingData != null) {
            if (editingData.buuCuc != null) {
                $('[name="kt_form_edit_form_buucucid"]').append("<option value='" + editingData.buuCuc.id + "' selected>" + editingData.buuCuc.name + "</option>").trigger('change');
            }
            if (editingData.dichVu != null) {
                $('[name="kt_form_edit_form_dichvuid"]').append("<option value='" + editingData.dichVu.id + "' selected>" + editingData.dichVu.name + "</option>").trigger('change');
            }
            if (editingData.phuongThucThanhToan != null) {
                $('[name="kt_form_edit_form_phuongthucthanhtoanid"]').append("<option value='" + editingData.phuongThucThanhToan.id + "' selected>" + editingData.phuongThucThanhToan.name + "</option>").trigger('change');
            }
            if (editingData.congTyGuiHang != null) {
                $('[name="kt_form_edit_form_congtyguihangid"]').append("<option value='" + editingData.congTyGuiHang.id + "' selected>" + editingData.congTyGuiHang.name + "</option>").trigger('change');
            }
            if (editingData.user != null) {
                $('[name="kt_form_edit_form_userid"]').append("<option value='" + editingData.user.id + "' selected>" + editingData.user.fullName + "</option>").trigger('change');
            }
        }

        flatpickr('.flatpickr', flatpickrDateDefaultOption());

    };

    let resetForm = () => {
        kt_edit_form[0].reset();
    }

    function loadBuuCucByKhuVuc() {
        $('[name="kt_form_edit_form_buucucid"]').empty();
        if ($('[name="kt_form_edit_form_khuvucid"]').val() != null) {
            var data = {
                khuVucId: $('[name="kt_form_edit_form_khuvucid"]').val()
            };
            $.ajax({
                url: '/BuuCuc/GetByKhuVucId',
                data: data,
                error: function (textStatus, error) {
                    return false;
                },
                success: function (res) {
                    var html = '<option value="' + res.resultObj.id + '">' + res.resultObj.name + '</option>';
                    $('[name="kt_form_edit_form_buucucid"]').append(html);
                    $('[name="kt_form_edit_form_buucucid"]').trigger('change');
                }
            });
        }
    }

    let userSelect2Custom = (el, url, options) => {

        el.select2(
            {
                ajax: {
                    url: url,
                    data: function (params) {
                        var query = {
                            textSearch: params.term
                        };

                        if (options && options.query) {
                            for (var key in options.query) {
                                query[key] = options.query[key];
                            }
                        }

                        return query;
                    },
                    processResults: function (res) {

                        var data = $.map(res.items, function (item, i) {

                            return {
                                id: (options && options.selectedFields ? item[options.selectedFields[0]] : item.id),
                                text: (options && options.selectedFields ? item[options.selectedFields[1]] : item.userName)
                            }
                        });
                        if (options && options.append0 != null && options.append0 == true) {
                            data.unshift({
                                id: 0,
                                text: "Không chọn"
                            })
                        }

                        return {
                            results: data
                        };
                    }
                },
                allowClear: true,
            }).trigger('change');
    }

    return {
        // public functions
        init: function () {
            initialComponents();
        }
    };
}();