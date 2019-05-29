using Husky.MessageRouting;
using Husky.MessageRouting.Messages;
using Husky.MessageRouting.Messages.DomainObjectProxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineService
{

   public enum ServiceInterfaceType
   {
      Unknown = 0,
      Machine = 1,
      DataCollector = 2
   }
   public enum ServiceConnectionStatus : short
   {
      Disabled = 0,
      Connecting = 1,
      Connected = 2,
      Disconnected = 3
   }

   public enum ServiceDisconnectedStatus : short
   {
      None = 0,
      // Server Exceptions
      ServerNotFound = 1,
      TimeoutException = 2,
      ApplicationFault = 3,
      // Data Collector Configuration Errors
      DataCollectorSerialNumberNotFound = 4,
      NoStationsConfigured = 5
   }

   public interface IServiceInterface
   {
      ServiceInterfaceType InterfaceType { get; }
      IMessageRouterManager MessageRouterManager { get; }
      ManagerProxies ManagerProxies { get; }
      void AsyncSendMessage<T>(T message) where T : MessageBase;

      string LocalTcpIpAddress { get; }

      void ChangeConnectionStatus(ServiceConnectionStatus status, ServiceDisconnectedStatus protocolErrors, ServiceMachineStatus machineConnectionStatus = ServiceMachineStatus.Disconnected);

      void ReConnectRequest();

      int DataBufferingBlockSize { get; }

      bool IntegrateManufacturingStandard { get; }
   }

   public class ServiceInterface : IServiceInterface
   {
      public ServiceConnectionStatus ConnectionStatus { get; private set; }
      private ManagerProxy _DomainManagerProxy;
      public ServiceInterfaceType InterfaceType { get; private set; }

      private readonly ManagerProxies _ManagerProxies;
      public ManagerProxies ManagerProxies { get { return _ManagerProxies; } }

      private TextStatusDomainObjectProxy _LocalTcpIpAddress;
      public string LocalTcpIpAddress
      {
         get { return _LocalTcpIpAddress.Value; }
      }


      private readonly IMessageRouterManager _MessageRouterManager;
      public IMessageRouterManager MessageRouterManager { get { return _MessageRouterManager; } }

      private readonly IMessageRouter _MessageRouter;

      public string ConnectionStatusName { get; private set; }

      public ServiceInterface()
      {
         _MessageRouter = _MessageRouterManager.CreateMessageRouter("ShotscopeNxInterface", true);
      }

      public void ChangeConnectionStatus(ServiceConnectionStatus status, ServiceDisconnectedStatus disconnectedStatus, ServiceMachineStatus machineConnectionStatus = ServiceMachineStatus.Disconnected)
      {

         ChangeConnectionStatus(status, machineConnectionStatus);

         ChangeDisconnectedStatus(disconnectedStatus);

         _DomainManagerProxy.SendAllMessagePackets();
      }



      void IServiceInterface.AsyncSendMessage<T>(T message)
      {
         AsyncSendMessage(message);
      }

      private void ChangeConnectionStatus(ServiceConnectionStatus status, ServiceMachineStatus machineConnectionStatus)
      {
         ConnectionStatus = status;

         _DomainManagerProxy.AddMessage(
            new DeviceVariableUpdateMessage(ConnectionStatusName, ConnectionStatus)
         );

         switch (ConnectionStatus)
         {
            case ServiceConnectionStatus.Connecting:
            case ServiceConnectionStatus.Connected:
               break;
            case ServiceConnectionStatus.Disabled:
               foreach (var machine in _Machines)
               {
                  machine.ChangeMachineStatus(ServiceMachineStatus.Disabled);
               }
               break;
            case ServiceConnectionStatus.Disconnected:
               foreach (var machine in _Machines)
               {
                  machine.ChangeMachineStatus(machineConnectionStatus);
                  machine.IsConnected = false;
                  // TODO: Test / clean-up these falgs
                  machine.IsHuskyMachineServiceConnected = false;
               }

               HeartbeatTimer.Interval = _ConnectionTimerIntervalMilliseconds;
               HeartbeatTimer.Restart();

               break;
         }
      }

      private void ChangeDisconnectedStatus(ServiceDisconnectedStatus disconnectedStatus)
      {
         DisconnectedStatus = disconnectedStatus;
         _DomainManagerProxy.AddMessage(
            new DeviceVariableUpdateMessage(DisconnectedStatusName, DisconnectedStatus)
         );
      }

      private void AsyncSendMessage<T>(T message) where T : MessageBase
      {
         // forward message to message router handler
         var messagePacket = new MessagePacket<T>(_MessageRouter, _MessageRouter);
         messagePacket.AddMessage(message);
         messagePacket.Send();
      }
   }
}
