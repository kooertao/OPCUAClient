using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHe.DomainModel;
using System.Timers;

namespace LHe.DomainModelService
{
   public class DomainModelServices
   {
      private UAServerConnectionManager _UAServerConnectionManager;

      private static TimeSpan _HeartBeatCheckTimerInterval = new TimeSpan(0, 0, 1);
      internal Timer HeartBeatCheckTimer { get; private set; }
      private int HeartBeatCheckCount = 0;
      private int HeartBeatCheckFailedTimes = 0;
      public event EventHandler<bool> HeartBeatChanged;

      public DomainModelServices()
      {
         
      }

      public void Start()
      {
         _UAServerConnectionManager = new UAServerConnectionManager();
         HeartBeatCheckCount = _UAServerConnectionManager.BeatCount;
         //Init timer
         HeartBeatCheckTimer = new Timer();
         HeartBeatCheckTimer.Interval = _HeartBeatCheckTimerInterval.TotalMilliseconds;
         HeartBeatCheckTimer.Elapsed += OnHeartBeatCheckTimer_Elapsed;
         HeartBeatCheckTimer.Start();
      }

      private void OnHeartBeatCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
      {
         if (_UAServerConnectionManager.BeatCount == HeartBeatCheckCount)
         {
            HeartBeatCheckFailedTimes++;
         }
         if (HeartBeatCheckFailedTimes >= 3)
         {
            //failed
            if (HeartBeatChanged != null)
            {
               HeartBeatChanged(this, false);
            }
         }
         else
         {
            if (HeartBeatChanged != null)
            {
               HeartBeatChanged(this, true);
            }
         }
      }
   }
}
