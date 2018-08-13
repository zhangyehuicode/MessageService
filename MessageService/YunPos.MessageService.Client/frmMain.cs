using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YunPos.MessageService.Client
{
    public partial class frmMain : Form, Proxy.IMessageServiceCallback
    {
        bool _isRunning = false;
        Proxy.MessageServiceClient _client;
        string _clientId = string.Empty;

        public frmMain()
        {
            InitializeComponent();
            Init();
        }

        #region methods
        void Init()
        {
            btnOperate.Click += btnOperate_Click;
            btnSend.Click += btnSend_Click;
        }

        void RefreshStatus()
        {
            if (_isRunning)
            {
                lblStatus.Text = "已连接";
                lblStatus.ForeColor = Color.Green;
                btnOperate.Text = "断开";
            }
            else
            {
                lblStatus.Text = "未连接";
                lblStatus.ForeColor = Color.Black;
                btnOperate.Text = "连接";
            }
        }
        public void OnMessageReceived(Proxy.MessageEntity message)
        {
            txtReceivedMessage.Text += string.Concat(message.Content, "\r\n");
            txtReceivedMessage.ScrollToCaret();
        }

        public string GetData(Proxy.MessageEntity message)
        {
            txtReceivedMessage.Text += string.Concat(message.Content, "\r\n");
            return DateTime.Now.ToString();
        }

        #endregion

        #region event handlers
        private void btnOperate_Click(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                _client.Unsubscribe(_clientId);
                _client.Close();
            }
            else
            {
                if (string.IsNullOrEmpty(txtClientId.Text))
                {
                    MessageBox.Show("请填写ClientId");
                    return;
                }
                _clientId = txtClientId.Text;
                _client = new Proxy.MessageServiceClient(new System.ServiceModel.InstanceContext(this));
                _client.Subscribe(_clientId);
            }
            _isRunning = !_isRunning;
            RefreshStatus();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                MessageBox.Show("请先连接");
                return;
            }
            if (string.IsNullOrEmpty(txtDestClient.Text))
            {
                MessageBox.Show("请填写发送目标");
                return;
            }
            if (string.IsNullOrEmpty(txtMessage.Text))
            {
                MessageBox.Show("请填写消息");
                return;
            }
            var message = new Proxy.MessageEntity
            {
                Receiver = txtDestClient.Text,
                MessageType = 0,
                Content = txtMessage.Text,
            };
            _client.SendMessage(message);
            txtReceivedMessage.Text += string.Format("给 {0} 发送消息 {1},\r\n", txtDestClient.Text, txtMessage.Text);
        }

        #endregion
    }
}
