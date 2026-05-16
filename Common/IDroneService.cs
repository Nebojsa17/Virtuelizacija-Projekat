using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IDroneService
    {
        [OperationContract]
        ConfirmationEnum StartSession(MetaHeader meta);

        [OperationContract]
        [FaultContract(typeof(SampleError))]
        ProgressEnum PushSample(Sample sample);

        [OperationContract]
        ConfirmationEnum EndSession();
    }
}
