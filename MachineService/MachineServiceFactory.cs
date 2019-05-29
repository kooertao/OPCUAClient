using Husky.Services.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MachineService
{
   public interface IMachineServiceFactory : IServiceFactory<IHuskyMachineServiceAsync>
   {
      string ServerTcpIpAddress { get; set; }
   }

   public class MachineServiceFactory : ServiceFactory<IHuskyMachineServiceAsync>, IMachineServiceFactory
   {
      private const string ShotscopeNxServerConfigurationName_NetTcpBinding = "NetTcpBinding_IHuskyMachineService";
      private const string ShotscopeNxServerEndpointAddress_NetTcpBinding = @"net.tcp://{0}:8735/Design_Time_Addresses/Husky.Services.DomainModelServicesHost/HuskyMachineService";
      private const string ShotscopeNxServerConfigurationName_HttpBinding = "HttpBinding_IHuskyMachineService";
      private const string ShotscopeNxServerEndpointAddress_HttpBinding = @"http://{0}/Design_Time_Addresses/Husky.Services.DomainModelServicesHost/HuskyMachineService";

      private string _ShotscopeNxServerConfigurationName;
      private string _ShotscopeNxServerEndpointAddress;

      private string _ServerTcpIpAddress = string.Empty;
      public string ServerTcpIpAddress
      {
         get { return _ServerTcpIpAddress; }
         set { _ServerTcpIpAddress = value; }
      }

      private EndpointAddress EndpointAddress
      {
         get { return new EndpointAddress(string.Format(_ShotscopeNxServerEndpointAddress, ServerTcpIpAddress)); }
      }

      public MachineServiceFactory()
         : base()
      {
         //var appConfigSettings = ShotscopeNxInterfaceIoC.Get<IShotscopeNxInterfaceAppConfigSettings>();
         //_ShotscopeNxServerConfigurationName = appConfigSettings.ShotscopeNxInterface_HuskyMachineServiceClientConfigurationName;
         _ShotscopeNxServerConfigurationName = ShotscopeNxServerConfigurationName_NetTcpBinding;

         // TODO: Generalize this by looking at the configuration ansd choose the address format accordingly
         //       (... or just put the address format string directly in App.config for most flexibility)

         if (_ShotscopeNxServerConfigurationName == ShotscopeNxServerConfigurationName_NetTcpBinding)
         {
            _ShotscopeNxServerEndpointAddress = ShotscopeNxServerEndpointAddress_NetTcpBinding;
         }
         else if (_ShotscopeNxServerConfigurationName == ShotscopeNxServerConfigurationName_HttpBinding)
         {
            _ShotscopeNxServerEndpointAddress = ShotscopeNxServerEndpointAddress_HttpBinding;
         }
         else
         {
            throw new ArgumentException("Unrecognized setting ShotscopeNxInterface_HuskyMachineServiceClientConfigurationName '{0}'.",
               _ShotscopeNxServerConfigurationName);
         }

         _ShotscopeNxServerEndpointAddress = ShotscopeNxServerEndpointAddress_NetTcpBinding;
         this.CreateService = () =>
         {
            try
            {
               var factory = ChannelFactory();
               var machineServiceAsync = factory.CreateChannel();
               return machineServiceAsync;
            }
            catch (UriFormatException)
            {
               return null;
            }
         };
      }

      internal ChannelFactory<IHuskyMachineServiceAsync> ChannelFactory()
      {
         // TODO: Add app.config setting for tcp or http binding
         return new ChannelFactory<IHuskyMachineServiceAsync>(_ShotscopeNxServerConfigurationName, EndpointAddress);
      }
   }
}
