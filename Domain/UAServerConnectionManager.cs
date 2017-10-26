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
      public UAServerConnectionState ConnectState
      {
         get { return _ConnectState; }
      }

      public event EventHandler OnUpdateHeartBeat;

      public void UpdateHeartBeat(bool isConnected)
      {
         _ConnectState = isConnected ? UAServerConnectionState.Connected : UAServerConnectionState.Disconnected;
      }
   }
}
