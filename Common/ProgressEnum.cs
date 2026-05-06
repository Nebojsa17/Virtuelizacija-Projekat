using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Virtuelizacija_Projekat.Common
{
    [DataContract]
    public enum ProgressEnum
    {
        [EnumMember] IN_PROGRESS,
        [EnumMember] COMPLETED
    }
}
