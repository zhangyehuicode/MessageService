using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YunPos.MessageService.Host
{
    public partial class frmMain : Form
    {
        //ServiceHost _WcfHost;
        bool _isRunning = false;
        System.Threading.Timer _ServiceTimer;
        CoreService _coreService;

        public frmMain()
        {
            InitializeComponent();
            Init();
        }

        #region methods
        void Init()
        {
            _ServiceTimer = new System.Threading.Timer(RefreshServiceStatus, null, Timeout.Infinite, Timeout.Infinite);
            btnOperate.Click += btnOperate_Click;
            btnSendMessage.Click += btnSendMessage_Click;
            btnClient.Click += btnClient_Click;
            btnSyncData.Click += btnSyncData_Click;
            btnExchange.Click += btnExchange_Click;
            Console.SetOut(new TextBoxWriter(txtLog));
        }

        void RefreshStatus()
        {
            if (_isRunning)
            {
                lblStatus.Text = "已启动";
                lblStatus.ForeColor = Color.Green;
                btnOperate.Text = "停止";
            }
            else
            {
                lblStatus.Text = "未启动";
                lblStatus.ForeColor = Color.Black;
                btnOperate.Text = "启动";
            }
        }

        void RefreshServiceStatus(object state)
        {
            var subscriberList = MessageServiceManager.GetSubscriberList();
            Invoke(new Action(() =>
            {
                lstClientList.Items.Clear();
                lstClientList.Items.AddRange(subscriberList.ToArray());
            }));
            _ServiceTimer.Change(1000 * 3, Timeout.Infinite);
        }

        #endregion

        #region event handlers

        //PushTask _pushTask;
        private void btnOperate_Click(object sender, EventArgs e)
        {
            //if (_isRunning)
            //{
            //    if (_pushTask != null)
            //        _pushTask.Stop();
            //}
            //else
            //{
            //    _pushTask = new PushTask();
            //    _pushTask.Start(null);
            //    _ServiceTimer.Change(1000 * 3, Timeout.Infinite);
            //}
            if (_isRunning)
            {
                if (_coreService != null)
                    _coreService.Stop();
            }
            else
            {
                _coreService = new CoreService();
                _coreService.Start();
                _ServiceTimer.Change(1000 * 3, Timeout.Infinite);
            }
            _isRunning = !_isRunning;
            RefreshStatus();
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            var selectedItem = lstClientList.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("请选择需要客户端");
                return;
            }
            if (string.IsNullOrEmpty(txtMessage.Text))
            {
                MessageBox.Show("请输入消息内容");
                return;
            }
            var message = new MessageEntity
            {
                Receiver = selectedItem.ToString(),
                MessageType = MessageTypes.Default,
                Content = txtMessage.Text,
            };
            MessageServiceManager.PushMessage(message);
        }

        void btnClient_Click(object sender, EventArgs e)
        {
            new frmClient().Show();
        }

        void btnSyncData_Click(object sender, EventArgs e)
        {
            //new frmSyncData().Show();
        }

        //ExchangeTask _exchangeTask;
        void btnExchange_Click(object sender, EventArgs e)
        {
            //if (_exchangeTask == null)
            //{
            //    _exchangeTask = new ExchangeTask();
            //    _exchangeTask.Start(null);
            //}
            //else
            //{
            //    _exchangeTask.Stop();
            //    _exchangeTask = null;
            //}
        }


        #endregion
    }
}
