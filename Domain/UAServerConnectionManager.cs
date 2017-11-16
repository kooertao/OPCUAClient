using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHe.DomainModel
{
   public enum UAServerConnectionState : short
   {
      Connected,
      Disconnected
   }
   public class UAServerConnectionManager
   {
      public static int BeatCount = 0;

      //public delegate void UpdateHeartBeatHandler(bool isConnected);   //声明委托
      //public event UpdateHeartBeatHandler UpdateHeartBeatEvent;

      public void UpdateHeartBeat(int beatCount)
      {
         BeatCount = beatCount;
      }
   }
}
