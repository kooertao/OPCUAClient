using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Advosol.EasyUA;
using Opc.Ua;
using LHe.DomainModel;

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
      private PersistenceManager _PersistenceManager = new PersistenceManager();
      private UAServerConnectionManager _ConnectionManager = new UAServerConnectionManager();
      public UAServerConnectionManager ConnectionManager {
         get { return _ConnectionManager; }
      }


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
            //machine： LHe,Tao
            ReferenceDescriptionCollection refs = _Browser.Browse((NodeId)ServerRoot, NodeClass.Object | NodeClass.Variable | NodeClass.Method);

            foreach (ReferenceDescription rd in refs)
            {
               var aa = ((NodeId)rd.NodeId).ToString();
               if (((NodeId)rd.NodeId).ToString().Contains("Heartbeat"))
               {
                  MonitoredItem miForHeartBeat = Subscr.AddItem((NodeId)rd.NodeId);
                  miForHeartBeat.Notification += new OnMonitoredItemNotification(miHeartBeat_Notification);
                  continue;
               }
              
               //LHe:MachineState,ProcessVariables and cycleInterrupt
               ReferenceDescriptionCollection subRefs = _Browser.Browse((NodeId)rd.NodeId, NodeClass.Object | NodeClass.Variable | NodeClass.Method);
               foreach (ReferenceDescription rdd in subRefs)
               {
                  MonitoredItem mi = Subscr.AddItem((NodeId)rdd.NodeId);
                  mi.Notification += new OnMonitoredItemNotification(miRamp_Notification);
                  string[] paths = rdd.NodeId.ToString().Substring(7, rdd.NodeId.ToString().Length - 7).Split('.');
                  if (paths.Length == 2 && paths[1] == "ProcessVariables")
                  {
                     ReferenceDescriptionCollection ProcessVariableRefs = _Browser.Browse((NodeId)rdd.NodeId, NodeClass.Object | NodeClass.Variable | NodeClass.Method);
                     foreach (var rddd in ProcessVariableRefs)
                     {
                        MonitoredItem mii = Subscr.AddItem((NodeId)rddd.NodeId);
                        mii.Notification += new OnMonitoredItemNotification(miRamp_Notification);  // notification for this MonitoredItem
                     }
                  }
               }
            }
            Subscr.ApplyChanges();
         }
      }

      private void miHeartBeat_Notification(MonitoredItem monitoredItem, IEncodeable notification)
      {
         try
         {
            MonitoredItemNotification dataChange = notification as MonitoredItemNotification;
            if (dataChange != null)
            {
               var beatCount = int.Parse(dataChange.Value.ToString());
               _ConnectionManager.UpdateHeartBeat(beatCount);
               //if ((StatusCode.IsGood(status) && ConnectionManager.ConnectState == UAServerConnectionState.Disconnected)||
               //   (!StatusCode.IsGood(status) && ConnectionManager.ConnectState == UAServerConnectionState.Connected ))
               //{
               //   _ConnectionManager.UpdateHeartBeat(StatusCode.IsGood(status));
               //}
               
            }
         }
         catch (Exception ex)
         {

         }
      }

      private void miRamp_Notification(MonitoredItem monitoredItem, IEncodeable notification)
      {
         try
         {
            MonitoredItemNotification dataChange = notification as MonitoredItemNotification;
            if (dataChange != null)
            {
               object value = dataChange.Value.Value;   // the changed value of the subscribed MonitoredItem
               var nodeId = monitoredItem.ResolvedNodeId;
               //Persist data
               string[] paths = nodeId.ToString().Substring(7, nodeId.ToString().Length - 7).Split('.');
               if (paths.Length == 2) // machine status
               {
                  if (paths[1] == "MachineState")
                  {
                     _PersistenceManager.SaveMachineState(paths[0], (string)value, DateTime.Now);
                  }
                  else if (paths[1] == "CycleCounter")
                  {
                     //_PersistenceModel.SaveMachineCycleCounter(paths[0], (long)value, timestamp);
                  }
                  else if (paths[1] == "CycleInterruption")
                  {
                     _PersistenceManager.SaveMachineCycleInterruption(paths[0], (string)value, DateTime.Now);
                  }
                  else
                  {
                     _Log.WarnFormat("unexpected path[1] length:{0}", nodeId);
                  }
               }
               else if (paths.Length == 3) // process variable values
               {
                  _PersistenceManager.SaveMachineProcessVariable(paths[0], paths[2], (float)value, DateTime.Now);
               }
               else if (paths.Length == 4) // units
               {
                  //_PersistenceModel.SaveMachineProcessVariable(paths[0], paths[2], (float)value, timestamp);
               }
               else
               {
                  _Log.WarnFormat("unexpected path length:{0}", nodeId);
               }
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine(monitoredItem.DisplayName + ":  " + ex.Message);
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
