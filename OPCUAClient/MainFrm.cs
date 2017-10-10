using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Advosol.EasyUA;
using Opc.Ua;
using Microsoft.AspNet.SignalR.Client;
using LHe.OPCUaClientLib;
using LHe.DomainModel;

namespace OPCUAClient
{
   public partial class MainFrm : Form
   {
      private OPCUaClient Client;
      private HubConnection hubConnection;
      private IHubProxy hubProxy;
      
      private const string UAServerAddress = "opc.tcp://localhost:62841/Advosol/uaPLUS";
      public MainFrm()  
      {
         InitializeComponent();
         ReadNodeBtn.Enabled = false;
         Client = new OPCUaClient(UAServerAddress);
         if (Client.IsConnected)
         {
            ReadNodeBtn.Enabled = true;
            tbServerState.Text = "Connected.";
         }
         else
         {
            tbServerState.Text = "Disconnected.";
         }
         ConnectHub();
         tbHubState.Text = hubConnection.State.ToString();
      }

      /// <summary>
      /// Read nodes
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void ReadNode_Click(object sender, EventArgs e)
      {
         foreach (var nodeId in Client.NodeIdsListForProcessVariables)
         {
            MonitoredItem mi = Client.Subscr.AddItem(nodeId);
            ListViewItem lvi = lvMonitored.Items.Add(mi.StartNodeId.ToString());
            lvi.SubItems.Add("??");
         }
         Client.Subscr.ApplyChanges();   // create in the server
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
   }
}
