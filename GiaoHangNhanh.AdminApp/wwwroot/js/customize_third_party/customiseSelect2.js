function initialDropdownlistSelect2Option(url){
    return {
        ajax: {
            type: "POST",
            url: url,
            dataType: 'json',
            contentType: "application/json",
            delay: 250,
            data: function (params) {
                return JSON.stringify({
                    term: params.term,
                    id: id
                });
            },
            processResults: function (res) {
                var data = $.map(res, function (item, i) {
                    return {
                        id: item.Id,
                        text: (item.Ma != null && item.Ma != "" && item.Ma != "null") ? item.Ma + ' - ' + item.Ten : item.Ten
                    }
                });

                return {
                    results: data
                };
            }
        },
        language: {
            noResults: function (term) {
                return "Không tìm thấy.";
            },
            searching: function (term) {
                return "Đang tìm...";
            }
        }
    };
}
