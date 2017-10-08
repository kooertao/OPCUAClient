using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using Advosol.EasyUA;
using Opc.Ua;

namespace LHe.OPCUaClientLib
{
   public class OPCUaClient
   {
      private readonly static ILog _Log = LogManager.GetLogger(typeof(OPCUaClient));
      //public string ServerAddress { get; private set; }
      public bool IsConnected { get; private set; }
      public string SessionState { get; private set; }
      public List<string> NodeIdsListForProcessVariables { get; private set; }
      public Subscription Subscr { get; private set; }

      private string _ServerAddress;
      private UaClient _UaApp;
      private Session _Session;
      //private Subscription _Subscr;
      private Browser _Browser;
      
      
      private const string ServerRoot = "ns=2;s=LHE Machines";
      public delegate bool AsyncDelegate();


      public OPCUaClient(string serverAddress)
      {
         _ServerAddress = serverAddress;
         NodeIdsListForProcessVariables = new List<string>();
         LoadUaAppConfiguration();
         CreateSession();
         GetNodeIdsList();
      }

      private void LoadUaAppConfiguration()
      {
         try
         {
            UaClient.UaAppConfigFileAutoCreate = "OPCUAClient";   // auto create a default UA config file to simplify the configuration handling
            _UaApp = new UaClient(this);
            _UaApp.LoadConfiguration();    // process the UA configuration
         }
         catch (Exception ex)
         {
            //MessageBox.Show(ex.Message, "Load UA configuration file failed.");
         }
      }

      private void CreateSession()
      {
         AsyncDelegate andl = new AsyncDelegate(CreateSessionWithUAServerDelegate);//声明一个AsyncDelegate类型的对象andl，并让他指向ad对象的TestMethod方法

         IAsyncResult ar = andl.BeginInvoke(null, null);
         if (andl.EndInvoke(ar))
         {
            //MessageBox.Show("Create session successfully.");
            //AddSubscriptionBtn.Enabled = true;
            IsConnected = true;
            if (_Session != null)
            {
               _Session.SubscriptionRequestKeepaliveCount = 5;
               _Session.PublishNotification += new OnNotification(onSessionPublishNotification);

               try
               {
                  //subscription name: s1
                  Subscr = _Session.AddSubscription("s1", 500);
                  Subscr.PublishStatusChanged += new EventHandler(onSubscrPublishStatusChanged);
                  //subscr.DataChangeCallback += onSubscrDataChangeNotification;
               }
               catch (Exception ex)
               {
                  //MessageBox.Show(ex.Message, "Add Subscription failed.");
               }
            }
         }
         else
         {
            IsConnected = false;
         }
      }

      private bool CreateSessionWithUAServerDelegate()
      {
         try
         {
            _Session = _UaApp.CreateSession(_ServerAddress, false, "sessionForUAClient");
            _Session.ReconnectPeriod = 10000;
            _Session.SessionTimeout = 60000;
            _Session.StatusChange += new OnStatusChange(onSessionStatusChange);
            //session.NotifyUntrustedCertificate += new OnNotifyUntrustedCertificate(onSessionNotifyUntrustedCertificate);
            _Session.NamespaceIndexManagement = true;
            _Session.Connect(null);
            return true;
         }
         catch (Exception ex)
         {
            //MessageBox.Show(ex.Message, "Create session failed.");
            return false;
         }
      }

      private void GetNodeIdsList() 
      {
         if (_Session != null && Subscr != null)
         {
            if (_Browser == null)
            {
               _Browser = _Session.CreateBrowser();
            }
            ReferenceDescriptionCollection refs = _Browser.Browse((NodeId)ServerRoot, NodeClass.Object | NodeClass.Variable | NodeClass.Method);
            foreach (ReferenceDescription rd in refs)
            {
               //lbNodeObjects.Items.Add(rd);
               ReferenceDescriptionCollection subRefs = _Browser.Browse((NodeId)rd.NodeId, NodeClass.Object | NodeClass.Variable | NodeClass.Method);
               foreach (ReferenceDescription rdd in subRefs)
               {
                  string[] paths = rdd.NodeId.ToString().Substring(7, rdd.NodeId.ToString().Length - 7).Split('.');
                  if (paths.Length == 2) // machine status
                  {
                     if (paths[1] == "MachineState")
                     {
                        //_PersistenceModel.SaveMachineState(paths[0], (string)value, timestamp);
                     }
                     else if (paths[1] == "CycleCounter")
                     {
                        //_PersistenceModel.SaveMachineCycleCounter(paths[0], (long)value, timestamp);
                     }
                     else if (paths[1] == "CycleInterruption")
                     {
                        //_PersistenceModel.SaveMachineCycleInterruption(paths[0], (string)value, timestamp);
                     }
                     else if (paths[1] == "ProcessVariables")
                     {
                        ReferenceDescriptionCollection ProcessVariableRefs = _Browser.Browse((NodeId)rdd.NodeId, NodeClass.Object | NodeClass.Variable | NodeClass.Method);
                        foreach (ReferenceDescription rddd in ProcessVariableRefs)
                        {
                           NodeIdsListForProcessVariables.Add(rddd.NodeId.ToString());
                        }
                     }
                     else
                     {
                        _Log.WarnFormat("unexpected path[1] length:{0}", rdd.NodeId);
                     }
                  }
               }
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
         //MessageBox.Show("Status changed: " + args.newState);
      }
      void onSessionStatusChange(Session session, StatusCheckEventArgs e)
      {
         try
         {
            if (e.Status != null)
            {
               string state = e.CurrentState.ToString();
               SessionState = state;
            }
            else
            {
               string state = e.CurrentState.ToString();
               SessionState = state;
            }
         }
         catch (Exception ex)
         {
            //tbNotifications.Text = "Status Change  " + ex.Message;
         }
      }
   }
}
