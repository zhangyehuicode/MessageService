using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YunPos.MessageService.Host
{
    public class TextBoxWriter : System.IO.TextWriter
    {
        TextBox _textBox;
        delegate void VoidAction();

        public TextBoxWriter(TextBox txtBox)
        {
            _textBox = txtBox;
        }

        public override void Write(string value)
        {
            VoidAction action = delegate
            {
                _textBox.AppendText(value);
            };
            _textBox.BeginInvoke(action);
        }

        public override void WriteLine(string value)
        {
            VoidAction action = delegate
            {
                _textBox.AppendText(value);
            };
            _textBox.BeginInvoke(action);
        }

        public override System.Text.Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    } 
}
