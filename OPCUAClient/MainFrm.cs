using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Advosol.EasyUA;
using Opc.Ua;
using Microsoft.AspNet.SignalR.Client;
using LHe.OPCUaClientLib;
using LHe.DomainModel;
using LHe.DomainModelService;

namespace OPCUAClient
{
   public partial class MainFrm : Form
   {
      private OPCUaClient Client;
      private HubConnection hubConnection;
      private IHubProxy hubProxy;
      
      private const string UAServerAddress = "opc.tcp://localhost:62841/Advosol/uaPLUS";
      private DomainModelServices domainModelService;
      public MainFrm()  
      {
         InitializeComponent();
         domainModelService = new DomainModelServices();
         domainModelService.Start();
         domainModelService.HeartBeatChanged += OnHeartBeatChanged;
         Client = new OPCUaClient(UAServerAddress);
         if (Client.IsConnected)
         {
            tbServerState.Text = "Connected.";
         }
         else
         {
            tbServerState.Text = "Disconnected.";
         }
         ConnectHub();
         tbHubState.Text = hubConnection.State.ToString();
         Client.ConnectionManager.UpdateHeartBeatEvent += UpdateUAServerConnectionState;
      }

      public void UpdateUAServerConnectionState(bool isConnected)
      {
         tbServerState.Text = isConnected ? "Connected." : "Disconnected.";
      }

      private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
      {
      }

      //private delegate void LvMonitorUpdate();
      private void TransferDataBySignalR(string NodeId, string value)
      {
         if (hubConnection.State == ConnectionState.Connected)
         {
            hubProxy.Invoke("SendMessage", NodeId, value);
         }
      }

      private void ConnectHub()
      {
         try
         {
            hubConnection = new HubConnection("http://localhost:8575/");
            hubProxy = hubConnection.CreateHubProxy("OPCUAHub");
            hubConnection.Start().Wait();
         }
         catch (Exception ex)
         {
            // The error should have the html returned.
            Console.WriteLine(ex.GetError());
         }
 
      }

      private void OnHeartBeatChanged(object sender, bool isConnected)
      {
         if(tbServerState.InvokeRequired)
         {
            tbServerState.Invoke(new MethodInvoker(delegate { tbServerState.Text = isConnected ? "Connected." : "Disconnected."; }));
         }
      }
   }
}
