//== Class definition

var Staff = function () {
    let edit_form = $("#edit_form"),
        edit_form_buttonSubmit = $('#edit_form_submit');

    let validator = FormValidation.formValidation(edit_form[0], {
        fields: {
            edit_form_code: { validators: { notEmpty: { message: "vui lòng nhập mã nhân viên" } } },
            edit_form_firstName: { validators: { notEmpty: { message: "vui lòng nhập tên nhân viên" } } },
            edit_form_phoneNumber: { validators: { notEmpty: { message: "vui lòng nhập số điện thoại" } } }
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger(),
            bootstrap: new FormValidation.plugins.Bootstrap5({ rowSelector: ".fv-row", eleInvalidClass: "", eleValidClass: "" })
        },
    });

    let initialComponents = () => {

        $.each($("a.nav-link"), (i, item) => {

            if ($(item).hasClass("active")) {
                $(item).removeClass("active");
            }
        });

        $("#navProfileDetailEdit").addClass("active");

        edit_form_buttonSubmit.click(function (e) {
            e.preventDefault();

            if (validator) {
                validator.validate().then(function (e) {
                    let formData = new FormData();
                    if (editingData != null) {
                        formData.append("id", (editingData != null ? editingData.id : null));
                    }

                    edit_form.find("select, textarea, input").each((index, el) => {
                        let fieldName = $(el).data("field");

                        if (fieldName) {
                            switch (fieldName) {
                                case "Avatar":
                                    let files = $(el).prop('files');
                                    if (files.length > 0) {
                                        formData.append("Avatar", files[0]);
                                    }

                                case "StrAppUserTypeIds":
                                    formData.append($(el).data("field"), $(el).val());
                                    break;

                                default:
                                    if ($(el).data("field")) {
                                        formData.append($(el).data("field"), $(el).val());
                                    }
                            }
                        }
                    });

                    "Valid" == e ?
                        (
                            edit_form_buttonSubmit.attr("data-kt-indicator", "on"), (edit_form_buttonSubmit.disabled = !0),
                            App.sendDataFileToURL("/Staff/SaveProfileDetail", formData, "POST")
                                .then(function (res) {

                                    edit_form_buttonSubmit.removeAttr("data-kt-indicator");
                                    if (!res.isSuccessed) {
                                        Swal.fire(App.swalFireErrorDefaultOption(res.message))
                                    }
                                    else {
                                        Swal.fire(App.swalFireSuccessDefaultOption());
                                        editingData = {
                                            id: res.resultObj
                                        };
                                        (edit_form_buttonSubmit.disabled = !1);
                                    }
                                }
                                )
                        )
                        : Swal.fire(App.swalFireErrorDefaultOption());
                });
            }
            $('input[name="edit_form_code"]').focus();
        });

        $('#btnDeleteAvatar').click(function (e) {

            if (editingData != null && editingData.id != null) {
                App.sendDataToURL("/Staff/DeleteAvatarByUserId?userId=" + editingData.id, null, "GET")
                    .then(function (res) {
                        if (!res.isSuccessed) {
                            Swal.fire(App.swalFireErrorDefaultOption(res.message));
                        }
                        else {
                            $("#edit_form_previewAvatar").attr("src", res.resultObj);
                        }
                    });
            }
        });

        App.initSelect2Base($('[name="edit_form_appUserTypeIds"]'), '/AppUserType/Filter');

        $('[name="edit_form_appUserTypeIds"]').val(null).trigger("change");

        if (editingData && editingData.appUserTypes && editingData.appUserTypes.length > 0) {
            let htmlOption = "";
            editingData.appUserTypes.forEach((item) => {
                htmlOption += "<option value=" + item.id + " selected>" + item.name + "</option>";
            });

            $('[name="edit_form_appUserTypeIds"]').append(htmlOption).trigger('change');
        }

        flatpickr('.flatpickr', flatpickrDateDefaultOption());
    };

    return {
        // public functions
        init: function () {
            initialComponents();
        }
    };
}();