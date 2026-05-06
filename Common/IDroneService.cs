using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtuelizacija_Projekat.Common
{
    [ServiceContract]
    public interface IDroneService
    {
        [OperationContract]
        ConfirmationEnum StartSession(MetaHeader meta);

        [OperationContract]
        // fali [FaultContract(typeof(XXXXXXXXXX))] ali sam greske ostavila za kasnije kada budemo radili implementaciju
        ProgressEnum PushSample(Sample sample);

        [OperationContract]
        ConfirmationEnum EndSession();
    }
}
