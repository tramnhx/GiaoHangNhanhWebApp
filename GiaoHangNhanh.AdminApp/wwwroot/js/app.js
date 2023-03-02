var App = function () {
    let run_waitMe = (selector, option) => {
        var optionObj = {
            effect: 'rotation',
            text: 'Đang xử lý...',
            bg: 'rgba(255,255,255,0.7)',
            color: '#3333cc',
            sizeW: '32px',
            sizeH: '32px',
            source: 'img.svg',
            onClose: function () { }
        };

        if (option) {
            for (var key in option) {
                optionObj[key] = option[key];
            }
        }

        $(selector).waitMe(optionObj);
    }

    let stop_waitMe = (selector) => {
        $(selector).waitMe('hide');
    }

    var body = document.getElementsByTagName("BODY")[0];


    let sendDataToURL = (url, data, method, isShowLoading, waitingElementOption) => {
        var defered = $.Deferred();
      
        if (isShowLoading == null || isShowLoading == true) {
            if (!waitingElementOption) {
                waitingElementOption = {
                    selector: 'body'
                };
            }
            run_waitMe(waitingElementOption.selector, waitingElementOption.option);
        }
        
        $.ajax({
            type: method || "POST",
            url: url,
            data: data != null ? JSON.stringify(data) : null,
            dataType: 'JSON',
            contentType: "application/json",
            error: function (res) {
                if (isShowLoading == null  || isShowLoading == true) {
                    stop_waitMe(waitingElementOption.selector);
                }
                console.log(res);
                defered.reject('Không tìm thấy máy chủ.');
            },
            success: function (res) {
                if (isShowLoading == null  || isShowLoading == true) {
                    stop_waitMe(waitingElementOption.selector);
                }

                defered.resolve(res);
            }
        });

        return defered.promise();
    }

    let sendDataFileToURL = (url, data, method, isShowLoading, waitingElementOption) => {
        var defered = $.Deferred();

        if (isShowLoading == null  || isShowLoading == true) {
            if (!waitingElementOption) {
                waitingElementOption = {
                    selector: 'body'
                };
            }
            run_waitMe(waitingElementOption.selector, waitingElementOption.option);
        }

        $.ajax({
            url: url,
            type: method || "POST",
            data: data,
            processData: false,
            contentType: false,
            error: function (xhr, status, p3, p4) {
                if (isShowLoading == null  || isShowLoading == true) {
                    stop_waitMe(waitingElementOption.selector);
                }

                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).Message;
                console.log(err);

                defered.reject('Không tìm thấy máy chủ.');

                return false;
            },
            success: function (res) {
                if (isShowLoading == null  || isShowLoading == true) {
                    stop_waitMe(waitingElementOption.selector);
                }

                defered.resolve(res);
            }
        });

        return defered.promise();
    }

    let isNullOrEmpty = (value) => {
        return typeof value == 'string' && !value.trim() && value == null ||
            typeof value == 'number' && value == null ||
            typeof value == 'object' && value == null ||
            typeof value == 'undefined' || value === null;
    }

    let deleteDataConfirm = (data, url, table, urlTroVe) => {
        var defered = $.Deferred();
        
        Swal.fire({
            title: 'Bạn có chắc chắn muốn xóa không?',
            text: "Bạn không thể khôi phục dữ liêu sau khi xóa!",
            icon: "warning",
            customClass: { cancelButton: "btn btn-light", confirmButton: "btn btn-primary" },
            showCancelButton: true,
            confirmButtonText: 'Đồng ý',
            cancelButtonText: 'Hủy bỏ',
            reverseButtons: true
        }).then(function (result) {
            if (result.value) {
                sendDataToURL(url, data, "POST")
                    .then(function (res) {
                        console.log("res: " + JSON.stringify(res));
                        if (!res.isSuccessed) {
                            Swal.fire({
                                title: 'Thông báo',
                                text: res.message,
                                type: 'warning',
                                confirmButtonText: 'Đóng!',
                                customClass: { confirmButton: "btn btn-primary" }
                            })
                        }
                        else {
                            Swal.fire({
                                title: 'Thông báo',
                                text: 'Xóa thành công.',
                                type: 'success',
                                confirmButtonText: 'Đóng!',
                                customClass: { confirmButton: "btn btn-primary" }
                            })

                            if (table != null) {
                                table.draw();

                            }
                            if (!isNullOrEmpty(urlTroVe)) {
                                window.location.href = urlTroVe;
                            }
                        }

                        defered.resolve(res);
                    });
            }
        });
        return defered.promise();
    };

    let scroll = () => {
        if (window.pageYOffset > 40) {
            if (body.hasAttribute('data-kt-sticky') === false) {
                body.setAttribute('data-kt-sticky', 'on');
            }
        } else {
            if (body.hasAttribute('data-kt-sticky') === true) {
                body.removeAttribute('data-kt-sticky');
            }
        }
    }

    let initevencheckbox = () => {
        if (this.checked) {
            showHideButtonDelete(true);
        } else {
            $('#dtTable tbody input[type="checkbox"]:checked').length > 0 ?
                showHideButtonDelete(true) : showHideButtonDelete(false);
        }
    }

    let showHideButtonDelete = (showDelete) => {
        if (showDelete) {
            $('.showSelected').show();
            $('#selected_count').text($('#dtTable tbody input[type="checkbox"]:checked').length);
            $('.showNotSelected').addClass('hideBtnCreate');
        } else {
            $('.showSelected').hide();
            $('.showNotSelected').removeClass('hideBtnCreate');
            $('#checkbox-select-all').prop('checked', false);
        }
    }

    let initSelect2Base = (el, url, options) => {

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
                                text: (options && options.selectedFields ? item[options.selectedFields[1]] : item.name)
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
    let initSelectCodeBase = (el, url, options) => {

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
                                text: (options && options.selectedFields ? item[options.selectedFields[1]] : item.code)
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

    let readImageURL =(input, element) => {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                element.attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    let dinhDangTien = (n, digits) => {
        if (n) {
            if (parseInt(digits) < 0 || typeof (digits) == 'undefined') {
                digits = 2;
            }

            if (parseFloat(n)) {
                n = parseFloat(parseFloat(n).toFixed(digits));
            }
        }

        let isNavigate = n < 0 ? "-" : "";

        var strTien = Math.abs(n).toString().split('.');
        return isNavigate + parseFloat(strTien[0]).toString().replace(/./g, function (c, i, a) {
            return i > 0 && c !== "." && (a.length - i) % 3 === 0 ? "," + c : c;
        }) + (strTien.length > 1 ? '.' + strTien[1] : '');
    }

    let swalFireSuccessDefaultOption = (message) => {
        return {
            title: message || 'Thông báo',
            text: 'Cập nhật thành công.',
            type: 'success',
            confirmButtonText: 'Đóng!',
            customClass: { confirmButton: "btn btn-primary" },
            icon: 'success',
            timer: 3000
        }
    };

    let swalFireErrorDefaultOption = (message) => {
        return {
            text: message || "Có lỗi xảy ra vui lòng kiểm tra lại." ,
            icon: "error",
            buttonsStyling: !1,
            confirmButtonText: "Đóng !",
            customClass: { confirmButton: "btn btn-primary" }
        }
    };

    return {
        //run_waitMe: run_waitMe,
        //stop_waitMe: stop_waitMe,
        sendDataToURL: sendDataToURL,
        sendDataFileToURL: sendDataFileToURL,
        deleteDataConfirm: deleteDataConfirm,
        scroll: scroll,
        initevencheckbox: initevencheckbox,
        showHideButtonDelete: showHideButtonDelete,
        isNullOrEmpty: isNullOrEmpty,
        initSelect2Base: initSelect2Base,
        initSelectCodeBase: initSelectCodeBase,
        readImageURL: readImageURL,
        dinhDangTien: dinhDangTien,
        swalFireErrorDefaultOption: swalFireErrorDefaultOption,
        swalFireSuccessDefaultOption: swalFireSuccessDefaultOption
    }
}();


window.onscroll = function () { App.scroll() };

$(document).keydown(function (event) {
    if (event.which == 113) { //F2
        if ($('button[name="btnCreate"]').length > 0
        ) {
            $('button[name="btnCreate"]').trigger('click');
        }
        return false;
    }
    else if (event.which == 114) { //F3
        if ($('button[name="btnCreateService"]').length > 0
        ) {
            $('button[name="btnCreateService"]').trigger('click');
        }
        return false;
    }
});