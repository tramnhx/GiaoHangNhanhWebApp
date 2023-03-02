using System;

namespace GiaoHangNhanh.Utilities.Exceptions
{
    public class GiaoHangNhanhException : Exception
    {
        public GiaoHangNhanhException()
        {

        }
        public GiaoHangNhanhException(string message) : base(message)
        {

        }
        public GiaoHangNhanhException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
