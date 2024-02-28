using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools
{
    internal abstract class ObjectType
    {
        protected ConfigEnum configEnum = new();
        public abstract void ReadExcels(string folder, bool useLanguage);
    }
}
