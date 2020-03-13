using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Telebot.Intranet
{
    public class LanMonitor : IINetMonitor
    {
        private Process watcher;
        private readonly BackgroundWorker worker;

        public LanMonitor()
        {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += DoWork;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            var si = new ProcessStartInfo(
                utilPath, $"/sxml {scanPath}"
            );

            RaiseNotify("Listening to new devices on network...");

            List<Host> prevScan = new List<Host>();
            List<Host> lastScan = new List<Host>();

            while (!worker.CancellationPending)
            {
                watcher = Process.Start(si);
                watcher.WaitForExit();

                if (worker.CancellationPending)
                    break;

                // we first must ensure we have a prev list to compare new list with
                if (prevScan.Count == 0)
                {
                    prevScan.AddRange(GetHosts());
                }
                else
                {
                    lastScan.AddRange(GetHosts());

                    var connectedHosts = CheckConnected(prevScan, lastScan);
                    //HostsArg disconnectedHosts = CheckDisconnected(prevScan, lastScan);

                    if (connectedHosts.Count > 0)
                        RaiseConnected(connectedHosts);

                    //if (disconnectedHosts.Hosts.Count > 0)
                    //    RaiseDisconnected(disconnectedHosts);

                    prevScan.Clear();
                    prevScan.AddRange(lastScan);

                    lastScan.Clear();
                }


                Thread.Sleep(3000);
            }

            RaiseNotify("Disconnected.");
        }

        private List<Host> CheckConnected(List<Host> prevScan, List<Host> lastScan)
        {
            return lastScan.Except(prevScan).ToList();
        }

        private List<Host> CheckDisconnected(List<Host> prevScan, List<Host> lastScan)
        {
            return prevScan.Except(lastScan).ToList();
        }

        public override void Disconnect()
        {
            if (!IsActive)
            {
                RaiseNotify("Lan monitor is already disconnected.");
                return;
            }

            if (!watcher.HasExited)
                watcher.Kill();

            if (worker.IsBusy)
                worker.CancelAsync();

            IsActive = false;
        }

        public override void Listen()
        {
            if (IsActive)
            {
                RaiseNotify("Lan monitoring is already listening...");
                return;
            }

            if (!worker.IsBusy)
                worker.RunWorkerAsync();

            IsActive = true;
        }
    }
}
