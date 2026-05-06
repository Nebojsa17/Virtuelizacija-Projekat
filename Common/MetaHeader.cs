using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Virtuelizacija_Projekat.Common
{
    [DataContract]
    public class MetaHeader
    {
        [DataMember] public string[] Header { get; set; } // ostavila prazan niz po dogovoru
    }
}
