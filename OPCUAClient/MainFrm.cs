using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Advosol.EasyUA;
using Opc.Ua;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace OPCUAClient
{
   public partial class MainFrm : Form
   {
      private UaClient uaApp;
      private Session session;
      private Subscription subscr = null;
      private HubConnection hubConnection;
      private IHubProxy hubProxy;
      private List<string>readNodesList = new List<string>{ "ns=2;s=LHe.ProcessVariables.Pressure#1",
                                                            "ns=2;s=LHe.ProcessVariables.Pressure#2",
                                                            "ns=2;s=LHe.ProcessVariables.Pressure#3",
                                                            "ns=2;s=LHe.ProcessVariables.Temperature#1",
                                                            "ns=2;s=LHe.ProcessVariables.Temperature#2",
                                                            "ns=2;s=LHe.ProcessVariables.Temperature#3",
                                                            "ns=2;s=Tao.ProcessVariables.Pressure#1",
                                                            "ns=2;s=Tao.ProcessVariables.Pressure#2",
                                                            "ns=2;s=Tao.ProcessVariables.Temperature#1",
                                                            "ns=2;s=Tao.ProcessVariables.Temperature#2"};


      private const string UAServerAddress = "opc.tcp://localhost:62841/Advosol/uaPLUS";
      public MainFrm()
      {
         InitializeComponent();
         //buttons
         CreateSessionBtn.Enabled = false;
         ReadNodeBtn.Enabled = false;
         LoadUaAppConfiguration();
         hubConnection = new HubConnection("http://localhost:8575/");
         hubProxy = hubConnection.CreateHubProxy("OPCUAHub");
         //hubConnection.Start().Wait();
      }

      public delegate bool AsyncDelegate();

      /// <summary>
      /// Create session with opc ua server
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void CreateSessionBtn_Click(object sender, EventArgs e)
      {
         AsyncDelegate andl = new AsyncDelegate(CreateSessionWithUAServer);//声明一个AsyncDelegate类型的对象andl，并让他指向ad对象的TestMethod方法

         IAsyncResult ar = andl.BeginInvoke(null, null);
         if (andl.EndInvoke(ar))
         {
            //MessageBox.Show("Create session successfully.");
            CreateSessionBtn.Enabled = false;
            //AddSubscriptionBtn.Enabled = true;
            if (session != null)
            {
               session.SubscriptionRequestKeepaliveCount = 5;
               session.PublishNotification += new OnNotification(onSessionPublishNotification);

               try
               {
                  //subscription name: s1
                  subscr = session.AddSubscription("s1", 500);
                  subscr.PublishStatusChanged += new EventHandler(onSubscrPublishStatusChanged);
                  //subscr.DataChangeCallback += onSubscrDataChangeNotification;
                  ReadNodeBtn.Enabled = true;
               }
               catch (Exception ex)
               {
                  MessageBox.Show(ex.Message, "Add Subscription failed.");
               }
            }            
         }
      }

      /// <summary>
      /// Read nodes
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void ReadNode_Click(object sender, EventArgs e)
      {
         if (session != null && subscr != null)
         {
            foreach (var nodeId in readNodesList)
            {
               MonitoredItem mi = subscr.AddItem(nodeId);
               mi.Notification += new OnMonitoredItemNotification(miRamp_Notification);  // notification for this MonitoredItem
               //For show
               mi.Tag = lvMonitored.Items.Count;     // line index
               ListViewItem lvi = lvMonitored.Items.Add(mi.StartNodeId.ToString());
               lvi.SubItems.Add("??");
            }
            subscr.ApplyChanges();   // create in the server

         }
      }

      #region Helper Method

      void LoadUaAppConfiguration()
      {
         try
         {
            UaClient.UaAppConfigFileAutoCreate = "OPCUAClient";   // auto create a default UA config file to simplify the configuration handling
            uaApp = new UaClient(this);
            uaApp.LoadConfiguration();    // process the UA configuration

            CreateSessionBtn.Enabled = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message, "Load UA configuration file failed.");
         }
      }

      bool CreateSessionWithUAServer()
      {
         try
         {
            session = uaApp.CreateSession(UAServerAddress, false, "sessionForUAClient");
            session.ReconnectPeriod = 10000;
            session.SessionTimeout = 60000;
            session.StatusChange += new OnStatusChange(onSessionStatusChange);
            //session.NotifyUntrustedCertificate += new OnNotifyUntrustedCertificate(onSessionNotifyUntrustedCertificate);
            session.NamespaceIndexManagement = true;
            session.Connect(null);
            return true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message, "Create session failed.");
            return false;
         }
      }

      void onSessionPublishNotification(Session session, NotificationEventArgs args)
      {
         //MessageBox.Show("Session publish notification");
      }
      void onSubscrPublishStatusChanged(object sender, EventArgs e)
      {
         PublishStatusChangedArgs args = (PublishStatusChangedArgs)e;
         //MessageBox.Show("Status changed: " + args.newState);
      }

      void onSessionStatusChange(Session session, StatusCheckEventArgs e)
      {
         try
         {
            if (e.Status != null)
            {
               string state = e.CurrentState.ToString();
               tbServerState.Text = state;
               //tbNotifications.Text = e.CurrentTime.ToString() + "  " + e.Status.LocalizedText.Text;
            }
            else
            {
               string state = e.CurrentState.ToString();
               tbServerState.Text = state;
            }
         }
         catch (Exception ex)
         {
            //tbNotifications.Text = "Status Change  " + ex.Message;
         }
      }
      #endregion

      private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
      {
            try
            {
               if (session != null)
               {
                  session.Close();    // terminate the open session
                  session.Dispose();
               }
            }
            catch { }
      }

      private void miRamp_Notification(MonitoredItem monitoredItem, IEncodeable notification)
      {
         try
         {
            MonitoredItemNotification dataChange = notification as MonitoredItemNotification;
            if (dataChange != null)
            {
               object val = dataChange.Value.Value;   // the changed value of the subscribed MonitoredItem

               int clh = (int)monitoredItem.Tag;
               ListViewItem lvi = lvMonitored.Items[clh];
               lvi.SubItems[1].Text = val.ToString();
               TransferDataBySignalR(monitoredItem.ResolvedNodeId.ToString(), dataChange.Value.Value.ToString());
            }
            else
            {
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine(monitoredItem.DisplayName + ":  " + ex.Message);
         }
      }

      private void TransferDataBySignalR(string NodeId, string value)
      {
         if (hubConnection.State == ConnectionState.Connected)
         {
            hubProxy.Invoke("SendMessage", NodeId, value);
         }
      }
   }
}
