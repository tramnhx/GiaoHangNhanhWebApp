using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BHSNetCoreLib
{
    public class StringUtil
    {
        public string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = param.Substring(0, length);
            //return the result of the operation
            return result;
        }
        public string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            string result = param.Substring(param.Length - length, length);
            //return the result of the operation
            return result;
        }

        public string Mid(string param, int startIndex, int length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable
            string result = param.Substring(startIndex, length);
            //return the result of the operation
            return result;
        }

        public string Mid(string param, int startIndex)
        {
            //start at the specified index and return all characters after it
            //and assign it to a variable
            string result = param.Substring(startIndex);
            //return the result of the operation
            return result;
        }
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static string BoDauVaLayKyTuDau(string s)
        {
            string result = string.Empty;
            string temp = convertToUnSign(s);
            if (temp.IndexOf(' ') > -1)
            {
                string[] temps = temp.Split();
                foreach (string item in temps)
                {
                    result += item[0];
                }
            }
            else
            {
                result = temp;
            }

            return result;
        }
        public static string ChuyenSo(string number)
        {
            return replace_special_word(join_unit(number)).ToUpper().Trim();
        }
        private static string join_unit(string n)
        {
            int sokytu = n.Length;
            int sodonvi = (sokytu % 3 > 0) ? (sokytu / 3 + 1) : (sokytu / 3);
            n = n.PadLeft(sodonvi * 3, '0');
            sokytu = n.Length;
            string chuoi = "";
            int i = 1;
            while (i <= sodonvi)
            {
                if (i == sodonvi) chuoi = join_number((int.Parse(n.Substring(sokytu - (i * 3), 3))).ToString()) + unit(i) + chuoi;
                else chuoi = join_number(n.Substring(sokytu - (i * 3), 3)) + unit(i) + chuoi;
                i += 1;
            }
            return chuoi;
        }

        private static string unit(int n)
        {
            string chuoi = "";
            if (n == 1) chuoi = " đồng ";
            else if (n == 2) chuoi = " nghìn ";
            else if (n == 3) chuoi = " triệu ";
            else if (n == 4) chuoi = " tỷ ";
            else if (n == 5) chuoi = " nghìn tỷ ";
            else if (n == 6) chuoi = " triệu tỷ ";
            else if (n == 7) chuoi = " tỷ tỷ ";
            return chuoi;
        }


        private static string convert_number(string n)
        {
            string chuoi = "";
            if (n == "0") chuoi = "không";
            else if (n == "1") chuoi = "một";
            else if (n == "2") chuoi = "hai";
            else if (n == "3") chuoi = "ba";
            else if (n == "4") chuoi = "bốn";
            else if (n == "5") chuoi = "năm";
            else if (n == "6") chuoi = "sáu";
            else if (n == "7") chuoi = "bảy";
            else if (n == "8") chuoi = "tám";
            else if (n == "9") chuoi = "chín";
            return chuoi;
        }


        private static string join_number(string n)
        {
            string chuoi = "";
            int i = 1, j = n.Length;
            while (i <= j)
            {
                if (i == 1) chuoi = convert_number(n.Substring(j - i, 1)) + chuoi;
                else if (i == 2) chuoi = convert_number(n.Substring(j - i, 1)) + " mươi " + chuoi;
                else if (i == 3) chuoi = convert_number(n.Substring(j - i, 1)) + " trăm " + chuoi;
                i += 1;
            }
            return chuoi;
        }


        private static string replace_special_word(string chuoi)
        {
            chuoi = chuoi.Replace("không mươi không ", "");
            chuoi = chuoi.Replace("không trăm ", "");
            chuoi = chuoi.Replace("không mươi", "lẻ");
            chuoi = chuoi.Replace("i không", "i");
            chuoi = chuoi.Replace("i năm", "i lăm");
            chuoi = chuoi.Replace("một mươi", "mười");
            chuoi = chuoi.Replace("mươi một", "mươi mốt");
            return chuoi;
        }


        public static string FirstCharToUpper(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }

        /// <summary>
        /// Chuyển đổi giá trị value thành dạng Url
        /// trả về giá trị dạng Url
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi</param>
        /// <param name="isTrim">Có tự động cắt khoảng trắng 2 đầu không? True: có; false: không
        /// </param><returns>Giá trị cần chuyển đổi</returns>
        public static string ToUrlFormat(string result)
        {
            result = result.ToString().Trim().ToLower();

            result = ToNoSignFormat(result);
            //Loại bỏ các ký tự đặc biệt
            result = Regex.Replace(result, "[^a-zA-Z0-9 ]+", " ", RegexOptions.Compiled);
            result = result.Replace(" ", "-");
            result = Regex.Replace(result, @"-+", "-");
            return result;
        }


        /// <summary>
        /// Chuyển đổi value thành không dấu
        /// trả về giá trị value không dấu
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi</param>
        /// <param name="isTrim">Có tự động cắt khoảng trắng 2 đầu không? True: có; false: không
        /// </param>
        /// <returns>trả về giá trị value không dấu</returns>
        public static string ToNoSignFormat(string result)
        {
            result = result.Trim();
            //Giúp bỏ dấu tiếng việt
            result = result.Normalize(NormalizationForm.FormD);
            result = Regex.Replace(result, "\\p{IsCombiningDiacriticalMarks}+", String.Empty);
            result = result.Replace('\u0111', 'd').Replace('\u0110', 'D');

            return result;
        }
        public static List<string> TachHoVaTen(string hoVaTen)
        {
            List<string> result = null;
            if (!string.IsNullOrEmpty(hoVaTen))
            {
                result = new List<string>();

                if (hoVaTen.IndexOf(' ') == -1)
                {
                    result.Add(string.Empty);
                    result.Add(hoVaTen);
                }
                else
                {
                    List<string> temp = hoVaTen.Split(' ').ToList();
                    string str = string.Empty;
                    for (int i = 0; i <= temp.Count - 2; i++)
                    {
                        str += temp[i] + " ";
                    }
                    result.Add(str);
                    result.Add(temp[temp.Count - 1]);
                }
            }

            return result;
        }
    }
}
