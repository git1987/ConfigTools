using ConfigTools.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools.Binary
{
    /// <summary>
    /// 二进制类型
    /// </summary>
    internal class BinaryType : ObjectType
    {
        string templateFile = "template/Binary";
        public override void BuildCSharpFile(ReadExcelSheet sheet)
        {

        }
    }
}
