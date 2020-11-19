using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Publisher
{
    public class EnumContainer
    {
        public enum EnumStatus
        {
            /// <summary>
            /// 未激活
            /// </summary>
            Invalid = 1,
            /// <summary>
            /// 成功的
            /// </summary>
            Successed = 2,
            /// <summary>
            /// 失败的
            /// </summary>
            Failed = 4
        }
    }
}
