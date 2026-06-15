using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Server
{
    public class DroneEventPublisher
    {
        public delegate void DroneLogEventHandler(object sender, DroneLogEventArgs e);
        public delegate void DroneLogBaseEventHandler(object sender, EventArgs e);

        public event DroneLogBaseEventHandler OnTransferStarted;
        public event DroneLogEventHandler OnSampleReceived;
        public event DroneLogBaseEventHandler OnTransferCompleted;
        public event DroneLogEventHandler OnWarningRaised;
        public event DroneLogEventHandler OnAccelerationSpike;
        public event DroneLogEventHandler OnOutOfBandWarning;
        public event DroneLogEventHandler OnWindSpike;

        public void StartTransfer()
        {
            if(OnTransferStarted != null) OnTransferStarted.Invoke(this, EventArgs.Empty);
        }
        public void EndTransfer()
        {
            if(OnTransferCompleted != null) OnTransferCompleted.Invoke(this, EventArgs.Empty);
        }
        public void Recieved(int r, int m) 
        {
            if(OnSampleReceived != null) OnSampleReceived.Invoke(this, new DroneLogEventArgs(max: m,recieved: r));
        }
        public void Warning(string warning)
        {
            if(OnWarningRaised != null) OnWarningRaised.Invoke(this, new DroneLogEventArgs(warning));
        }
        public void AccelerationSpike(Direction type)
        {
            if (OnAccelerationSpike != null) OnAccelerationSpike.Invoke(this, new DroneLogEventArgs(direction: type));
        }
        public void OutOfBandWarning(Direction type)
        {
            if (OnOutOfBandWarning != null) OnOutOfBandWarning.Invoke(this, new DroneLogEventArgs(direction: type));
        }
        public void WindSpike(Direction type)
        {
            if (OnWindSpike != null) OnWindSpike.Invoke(this, new DroneLogEventArgs(direction: type));
        }
    }
}
