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
      private UAServerConnectionState _ConnectState;

      private int _BeatCount = int.MaxValue;
      public int BeatCount
      {
         get { return _BeatCount; }
         private set { _BeatCount = value; }
      }
      public UAServerConnectionState ConnectState
      {
         get { return _ConnectState; }
      }

      public delegate void UpdateHeartBeatHandler(bool isConnected);   //声明委托
      public event UpdateHeartBeatHandler UpdateHeartBeatEvent;

      public void UpdateHeartBeat(int beatCount)
      {
         if (beatCount != _BeatCount && UpdateHeartBeatEvent != null)
         {
            UpdateHeartBeatEvent(true);
            _BeatCount = beatCount;
         }
         else
         {
            UpdateHeartBeatEvent(false);
         } 

         //_ConnectState = isConnected ? UAServerConnectionState.Connected : UAServerConnectionState.Disconnected;
         //if (UpdateHeartBeatEvent != null)
         //{
         //   UpdateHeartBeatEvent(isConnected);
         //}
      }
   }
}
