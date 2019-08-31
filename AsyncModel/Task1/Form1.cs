using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;

namespace Task1
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Делегат с сигнатурой как у метода Concat
        /// </summary>
        readonly Func<string, string, string,string> mydel = new Func<string, string, string,string>(Concat);
        /// <summary>
        /// Объект синхронизации
        /// </summary>
        SynchronizationContext sync;
        /// <summary>
        /// Метод конкатенации двух строк
        /// </summary>
        /// <param name="a">Строка из TextBox1</param>
        /// <param name="b">Строка из TextBox2</param>
        /// <param name="c">Строка метода вызова</param>
        /// <returns>Конкатенацию двух строк типа String</returns>
        static string Concat(string a, string b,string c)
        {
            MessageBox.Show("Поток выполнения:"+Thread.CurrentThread.ManagedThreadId.ToString()+"\nМетод выполнения: "+c);
            return a + b;
        }
        public void callback(IAsyncResult asyncResult)
        {
            MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
            AsyncResult async = (AsyncResult)asyncResult;
            var delegateAdd = (Func<string, string, string>)async.AsyncDelegate;
            var result = delegateAdd.EndInvoke(asyncResult);
            sync.Post(delegate { textBox3.Text = result.ToString(); }, null);
        }
        

        private void BtnComplete_Click(object sender, EventArgs e)
        {            
            var res = mydel.BeginInvoke(textBox1.Text, textBox2.Text, "IsComplete", null, null);
            while (true)
            {
                if (res.IsCompleted)
                {
                    break;
                }
            }
            textBox3.Text = mydel.EndInvoke(res);
        }

        private void BtnEndInvoke_Click(object sender, EventArgs e)
        {
            var result = mydel.BeginInvoke(textBox1.Text, textBox2.Text, "EndInvoke", null, null);
            textBox3.Text = mydel.EndInvoke(result);
        }

        private void BtnCallback_Click(object sender, EventArgs e)
        {
            sync = SynchronizationContext.Current;
            var re = mydel.BeginInvoke(textBox1.Text, textBox2.Text, "Callback", callback, textBox3);
        }
        public Form1()
        {
            InitializeComponent();
        }
    }
}
