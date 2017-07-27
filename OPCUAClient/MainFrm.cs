using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advosol.EasyUA;
using Opc.Ua;

namespace OPCUAClient
{
   public partial class MainFrm : Form
   {
      private UaClient uaApp;
      private Session session;
      private ShowBrowseTreeList sbtl = null;
      private Subscription subscr = null;

      private const string UAServerAddress = "opc.tcp://localhost:62841/Advosol/uaPLUS";//Test
      public MainFrm()
      {
         InitializeComponent();
         try
         {
            UaClient.UaAppConfigFileAutoCreate = "OPCUAClient";   // auto create a default UA config file to simplify the configuration handling
            uaApp = new UaClient(this);
            uaApp.LoadConfiguration();    // process the UA configuration
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message, "Load UA configuration file failed.");
         }
      }

      private void ReadNodeBtn_Click(object sender, EventArgs e)
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
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message, "Add Subscription failed.");
            }
         }
      }

      void onSessionPublishNotification(Session session, NotificationEventArgs args)
      {
         //MessageBox.Show("Session publish notification");
      }
      void onSubscrPublishStatusChanged(object sender, EventArgs e)
      {
         PublishStatusChangedArgs args = (PublishStatusChangedArgs)e;
         MessageBox.Show("Status changed: " + args.newState);
      }

      private void CreateSessionBtn_Click(object sender, EventArgs e)
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

            //btnCreateSession.Enabled = false;
            //btnTerminateSession.Enabled = true;
            //btnAddSubscripton.Enabled = true;
            //btnDeleteSubscription.Enabled = true;
            //gbSession.Enabled = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message, "Create session failed.");
         }
      }

      void onSessionStatusChange(Session session, StatusCheckEventArgs e)
      {
         try
         {
            if (e.Status != null)
            {
               string state = e.CurrentState.ToString();
               //tbServerState.Text = state;
               //tbNotifications.Text = e.CurrentTime.ToString() + "  " + e.Status.LocalizedText.Text;
            }
            else
            {
               string state = e.CurrentState.ToString();
              // tbServerState.Text = state;
            }
         }
         catch (Exception ex)
         {
            //tbNotifications.Text = "Status Change  " + ex.Message;
         }
      }

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
                     //mi.Notification += new OnMonitoredItemNotification(mi_Notification);

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
