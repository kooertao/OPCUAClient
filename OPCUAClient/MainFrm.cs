using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Advosol.EasyUA;
using Opc.Ua;
using Microsoft.AspNet.SignalR.Client; //PM> Install-package Microsoft.AspNet.SignalR.Client

namespace OPCUAClient
{
   public partial class MainFrm : Form
   {
      private UaClient uaApp;
      private Session session;
      private ShowBrowseTreeList sbtl = null;
      private Subscription subscr = null;

      private const string UAServerAddress = "opc.tcp://localhost:62841/Advosol/uaPLUS";
      public MainFrm()
      {
         InitializeComponent();
         //buttons
         CreateSessionBtn.Enabled = false;
         AddSubscriptionBtn.Enabled = false;
         ReadNodeBtn.Enabled = false;
         BrowseItemsBtn.Enabled = false;
         LoadUaAppConfiguration();
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
            MessageBox.Show("Create session successfully.");
            CreateSessionBtn.Enabled = false;
            AddSubscriptionBtn.Enabled = true;
         }
      }

      /// <summary>
      /// add subscription of opc ua server
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void AddSubscriptionBtn_Click(object sender, EventArgs e)
      {
         if (session != null)
         {
            session.SubscriptionRequestKeepaliveCount = 5;
            session.PublishNotification += new OnNotification(onSessionPublishNotification);
            try
            {

               subscr = session.AddSubscription("s1", 500);
               subscr.PublishStatusChanged += new EventHandler(onSubscrPublishStatusChanged);
               //subscr.DataChangeCallback += onSubscrDataChangeNotification;

               AddSubscriptionBtn.Enabled = false;
               ReadNodeBtn.Enabled = true;
               BrowseItemsBtn.Enabled = true;
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message, "Add Subscription failed.");
            }
         }
      }

      /// <summary>
      /// Browse items
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void BrowseItemsBtn_Click(object sender, EventArgs e)
      {
         if (session != null)
         {
            try
            {
               if (sbtl != null)    // previous treeList instance
               {
                  sbtl.Dispose();
                  sbtl = null;
               }

               lvItems.Items.Clear();
               tvItems.Nodes.Clear();
               sbtl = new ShowBrowseTreeList(session, tvItems, lvItems);
               if (sbtl != null)
               {
                  sbtl.Show("UaServer");
               }

            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message, "Browse failed.");
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
            List<NodeId> nodes = new List<NodeId>();
            nodes = sbtl.GetSelectedListViewNodes();

            if (nodes.Count == 0)
               MessageBox.Show("no Node Selected.");
            else
            {
               try
               {
                  foreach (NodeId nid in nodes)
                  {
                     MonitoredItem mi = subscr.AddItem(nid);
                     mi.Tag = lvMonitored.Items.Count;     // line index
                     mi.Notification += new OnMonitoredItemNotification(mi_Notification);

                     ListViewItem lvi = lvMonitored.Items.Add(mi.StartNodeId.ToString());
                     lvi.SubItems.Add("??");
                  }

                  subscr.ApplyChanges();   // create in the server
               }
               catch (Exception ex)
               {
                  MessageBox.Show(ex.Message, "Add Items failed.");
               }
            }
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


      void mi_Notification(MonitoredItem monitoredItem, IEncodeable notification)
      {
         try
         {
            MonitoredItemNotification dataChange = notification as MonitoredItemNotification;
            if (dataChange != null)
            {
               //Transform data by signalR(For realTimeView)
               var processVariable = new ProcessVariableUIInfo
               {
                  VariableName = monitoredItem.StartNodeId.Identifier.ToString(),
                  VariableValue = Convert.ToDouble(dataChange.Value.Value)
               };
               TransferDataBySignalR(dataChange.Value.Value.ToString());

               //Save to DB(Web access) TODO!!!!
               string val = dataChange.Value.Value.ToString();
               int clh = (int)monitoredItem.Tag;
               ListViewItem lvi = lvMonitored.Items[clh];
               lvi.SubItems[1].Text = val;
            }
            else
            {
            }
         }
         catch (Exception ex)
         {
            //tbPublishNotification.Text = monitoredItem.DisplayName + ":  " + ex.Message;
         }
      }

      void TransferDataBySignalR(string value)
      {
         var hubConnection = new HubConnection("http://localhost:10282/");
         IHubProxy hubProxy = hubConnection.CreateHubProxy("OPCUAHub");
         hubConnection.Start().Wait();

         if (hubConnection.State == ConnectionState.Connected)
         {
            hubProxy.Invoke("SendMessage", value);
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
   }
}
