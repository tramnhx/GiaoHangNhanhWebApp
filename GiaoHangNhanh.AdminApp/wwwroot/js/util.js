
var Util = function () {
    let showUnsignedString = (input) => {
        var signedChars = "àảãáạăằẳẵắặâầẩẫấậđèẻẽéẹêềểễếệìỉĩíịòỏõóọôồổỗốộơờởỡớợùủũúụưừửữứựỳỷỹýỵÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬĐÈẺẼÉẸÊỀỂỄẾỆÌỈĨÍỊÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢÙỦŨÚỤƯỪỬỮỨỰỲỶỸÝỴ";
        var unsignedChars = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
        var pattern = new RegExp("[" + signedChars + "]", "g");
        var output = input.replace(pattern, function (m, key, value) {
            return unsignedChars.charAt(signedChars.indexOf(m));
        });
        return output;
    }

    // Đọc số sang chữ
    let mangso = ['không', 'một', 'hai', 'ba', 'bốn', 'năm', 'sáu', 'bảy', 'tám', 'chín'];
    let dochangchuc = (so, daydu) => {
        var chuoi = "";
        chuc = Math.floor(so / 10);
        donvi = so % 10;
        if (chuc > 1) {
            chuoi = " " + mangso[chuc] + " mươi";
            if (donvi == 1) {
                chuoi += " mốt";
            }
        }
        else if (chuc == 1) {
            chuoi = " mười";

            if (donvi == 1) {
                chuoi += " một";
            }
        }
        else if (daydu && donvi > 0) {
            chuoi = " lẻ";
        }

        if (donvi == 5 && chuc > 1) {
            chuoi += " lăm";
        }
        else if (donvi > 1 || (donvi == 1 && chuc == 0)) {
            chuoi += " " + mangso[donvi];
        }

        return chuoi;
    }
    let docblock = (so, daydu) => {
        var chuoi = "";
        tram = Math.floor(so / 100);
        so = so % 100;
        if (daydu || tram > 0) {
            chuoi = " " + mangso[tram] + " trăm";
            chuoi += dochangchuc(so, true);
        }
        else {
            chuoi = dochangchuc(so, false);
        }

        return chuoi;
    }
    let dochangtrieu = (so, daydu) => {
        var chuoi = "";
        trieu = Math.floor(so / 1000000); so = so % 1000000;
        if (trieu > 0) {
            chuoi = docblock(trieu, daydu) + " triệu";
            daydu = true;
        }

        nghin = Math.floor(so / 1000);
        so = so % 1000;

        if (nghin > 0) {
            chuoi += docblock(nghin, daydu) + " nghìn"; daydu = true;
        }

        if (so > 0) {
            chuoi += docblock(so, daydu);
        }

        return chuoi;
    }

    let docTienBangChu = (tien) => {
        if (tien == 0)
            return mangso[0];

        var chuoi = "",
            hauto = "";

        do {
            ty = tien % 1000000000;
            tien = Math.floor(tien / 1000000000);

            if (tien > 0) {
                chuoi = dochangtrieu(ty, true) + hauto + chuoi;
            }
            else {
                chuoi = dochangtrieu(ty, false) + hauto + chuoi;
            }

            hauto = " tỷ";
        } while (tien > 0);

        return chuoi;
    }

    let ChuSo = new Array(" không ", " một ", " hai ", " ba ", " bốn ", " năm ", " sáu ", " bảy ", " tám ", " chín ");
    let Tien = new Array("", " ngàn", " triệu", " tỷ", " ngàn tỷ", " triệu tỷ");

    let DocTienBangChu = (SoTien) => {
        var lan = 0;
        var i = 0;
        var so = 0;
        var KetQua = "";
        var tmp = "";
        var ViTri = new Array();
        if (SoTien < 0) return "Số tiền âm !";
        if (SoTien == 0) return "Không đồng !";
        if (SoTien > 0) {
            so = SoTien;
        }
        else {
            so = -SoTien;
        }
        if (SoTien > 8999999999999999) {
            //SoTien = 0;
            return "Số quá lớn!";
        }
        ViTri[5] = Math.floor(so / 1000000000000000);
        if (isNaN(ViTri[5]))
            ViTri[5] = "0";
        so = so - parseFloat(ViTri[5].toString()) * 1000000000000000;
        ViTri[4] = Math.floor(so / 1000000000000);
        if (isNaN(ViTri[4]))
            ViTri[4] = "0";
        so = so - parseFloat(ViTri[4].toString()) * 1000000000000;
        ViTri[3] = Math.floor(so / 1000000000);
        if (isNaN(ViTri[3]))
            ViTri[3] = "0";
        so = so - parseFloat(ViTri[3].toString()) * 1000000000;
        ViTri[2] = parseInt(so / 1000000);
        if (isNaN(ViTri[2]))
            ViTri[2] = "0";
        ViTri[1] = parseInt((so % 1000000) / 1000);
        if (isNaN(ViTri[1]))
            ViTri[1] = "0";
        ViTri[0] = parseInt(so % 1000);
        if (isNaN(ViTri[0]))
            ViTri[0] = "0";
        if (ViTri[5] > 0) {
            lan = 5;
        }
        else if (ViTri[4] > 0) {
            lan = 4;
        }
        else if (ViTri[3] > 0) {
            lan = 3;
        }
        else if (ViTri[2] > 0) {
            lan = 2;
        }
        else if (ViTri[1] > 0) {
            lan = 1;
        }
        else {
            lan = 0;
        }
        for (i = lan; i >= 0; i--) {
            tmp = DocSo3ChuSo(ViTri[i]);
            KetQua += tmp;
            if (ViTri[i] > 0) KetQua += Tien[i];
            if ((i > 0) && (tmp.length > 0)) KetQua += ',';//&& (!string.IsNullOrEmpty(tmp))
        }
        if (KetQua.substring(KetQua.length - 1) == ',') {
            KetQua = KetQua.substring(0, KetQua.length - 1);
        }
        KetQua = KetQua.substring(1, 2).toUpperCase() + KetQua.substring(2);
        return KetQua + " đồng.";//.substring(0, 1);//.toUpperCase();// + KetQua.substring(1);
    }

    let DocSo3ChuSo = (baso) => {
        var tram;
        var chuc;
        var donvi;
        var KetQua = "";
        tram = parseInt(baso / 100);
        chuc = parseInt((baso % 100) / 10);
        donvi = baso % 10;
        if (tram == 0 && chuc == 0 && donvi == 0) return "";
        if (tram != 0) {
            KetQua += ChuSo[tram] + " trăm ";
            if ((chuc == 0) && (donvi != 0)) KetQua += " linh ";
        }
        if ((chuc != 0) && (chuc != 1)) {
            KetQua += ChuSo[chuc] + " mươi";
            if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh ";
        }
        if (chuc == 1) KetQua += " mười ";
        switch (donvi) {
            case 1:
                if ((chuc != 0) && (chuc != 1)) {
                    KetQua += " mốt ";
                }
                else {
                    KetQua += ChuSo[donvi];
                }
                break;
            case 5:
                if (chuc == 0) {
                    KetQua += ChuSo[donvi];
                }
                else {
                    KetQua += " lăm ";
                }
                break;
            default:
                if (donvi != 0) {
                    KetQua += ChuSo[donvi];
                }
                break;
        }
        return KetQua;
    }

    let formatVND = (n, currency) => {
        return currency + " " + n.toFixed(0).replace(/./g, function (c, i, a) {
            return i > 0 && c !== "," && (a.length - i) % 3 === 0 ? "." + c : c;
        });
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

    let isNullOrEmpty = (value) => {
        return typeof value == 'string' && !value.trim() || typeof value == 'undefined' || value === null;
    }

    let isNullOrEmptySelect2 = (value) => {
        return typeof value == 'string' && !value.trim() || typeof value == 'undefined' || value === null || value == 'null' || value == "-1";
    }

    return {
        showUnsignedString: showUnsignedString,
        dochangchuc: dochangchuc,
        docblock: docblock,
        dochangtrieu: dochangtrieu,
        docTienBangChu: docTienBangChu,
        DocTienBangChu: DocTienBangChu,
        DocSo3ChuSo: DocSo3ChuSo,
        formatVND: formatVND,
        dinhDangTien: dinhDangTien,
        isNullOrEmpty: isNullOrEmpty,
        isNullOrEmptySelect2: isNullOrEmptySelect2
    }
}();

