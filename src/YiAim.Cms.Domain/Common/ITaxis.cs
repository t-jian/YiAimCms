using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAim.Cms
{
    internal interface ITaxis
    {
        /// <summary>
        /// 排序
        /// </summary>
        int Taxis {get;set; }
    }
    public  class TaxisEntity : ITaxis
    {
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Taxis { get; set; } = 0;
    }

}
