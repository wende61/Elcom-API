using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Elcom.Common.RequestModel.Common
{
    public class Attachement
    {
        public byte[] data { get; set; }
        public DataType dataType { get; set; }
        public string FileName { get; set; }
    }
}
