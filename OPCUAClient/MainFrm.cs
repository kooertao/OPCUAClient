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

               var subscr = session.AddSubscription("s1", 500);

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
         if (session != null)
         {
            try
            {
               List<NodeId> selNodes = new List<NodeId>();
               selNodes = sbtl.GetSelectedListViewNodes();

               if (selNodes.Count == 0)
                  MessageBox.Show("No Item Node Selected.");
               else
               {
                  tbResult.Text = "reading ...";
                  List<ReadValueId> nodesToRead = new List<ReadValueId>();
                  foreach (NodeId selNode in selNodes)
                  {
                     ReadValueId rv = new ReadValueId();
                     rv.NodeId = selNode;
                     rv.AttributeId = Attributes.Value;
                     nodesToRead.Add(rv);
                  }

                  List<DataValue> rslt = session.Read(nodesToRead);

                  tbResult.Text = "";
                  for (int i = 0; i < rslt.Count; ++i)
                  {
                     DataValue r = rslt[i];
                     {
                        string val = "null";
                        if (r.Value != null)
                           val = r.Value.ToString();
                        tbResult.Text += val + "   " + r.StatusCode.ToString() + Environment.NewLine;
                     }
                  }
               }
            }
            catch (Exception ex)
            {
               tbResult.Text = "";
               MessageBox.Show(ex.Message, "Read Value failed.");
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
