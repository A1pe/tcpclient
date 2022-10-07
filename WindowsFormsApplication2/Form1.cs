using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using myStruct;

namespace TcpClients1
{
    

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 파일 탐색 기능
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            String file_path = null;
            openFileDialog1.InitialDirectory = "C:\\";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file_path = openFileDialog1.FileName;
                label1.Text = file_path;
                label2.Text = (GetFileSize(file_path) / 1000).ToString() + " KB";
            }
        }

        /// <summary>
        /// 프로그레스 바 초기화
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Step = 10;
        }

        public long GetFileSize(String filePath)
        {
            long fileSize = 0;
            if (File.Exists(filePath))
            {
                FileInfo info = new FileInfo(filePath);
                fileSize = info.Length;
            }
            return fileSize;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            
            try
            {
                /////////////////////////////////////////////// sock setting

                Socket sock = null;

                // async task
                var sockTask = Task.Run(() =>
                {
                    IPEndPoint ep;
                    sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ep = new IPEndPoint(IPAddress.Parse(textBox1.Text), int.Parse(textBox2.Text));
                    sock.Connect(ep);

                });
                await sockTask;


                ////////////////////////////////////////////// header start

                // async task
                var headerTask = Task.Run(() =>
                {
                    // declare a variable in local task
                    HTask ht = new HTask(sock);            
                    ht.task();     
                });

                await headerTask;

                
                ///////////////////////////////////////////// file setting
                FTask ft = null;
                // async task
                var fileTask = Task.Run(() =>
                {            
                    // declare a variable in local task
                    ft = new FTask(label1.Text, textBox2.Text,sock);
                    ft.task();
                });

                await fileTask;

                
                
                ///////////////////////////////////////////// progress bar setting
                
                progressBar1.Minimum = 0;
                progressBar1.Maximum = (int)(ft.getFileSz() / 1000) + 1;
                label7.Text = "0 / 0";
                progressBar1.Value = progressBar1.Minimum;

                //////////////////////////////////////////// payload start

                ulong value=0;
                PTask pt = new PTask(sock, ft.getFileSz(), ft.getFS());
                pt.task();
                int values = 0;
                while (pt.getFlag())
                {
                    // async task
                    var payloadTask = Task.Run(() =>
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        
                        pt.task2();
                        value += pt.getLen();
                        sw.Stop();
                    });
                    await payloadTask;
                    
                    progressBar1.Value = ((int)value / 1000);
                    label7.Text = progressBar1.Value.ToString() + " / " + progressBar1.Maximum.ToString() + "KB";
                    
                    

                }
                ft.getFS().Close();
                sock.Close();

                progressBar1.Value = progressBar1.Maximum;
                label7.Text = progressBar1.Maximum.ToString() + " / " + progressBar1.Maximum.ToString() + "KB";
                MessageBox.Show("파일 전송이 완료되었습니다.");
            
                
                
            }
           
            ///////////////////////////////////////////// exception handling

            catch (System.FormatException)
            {
                MessageBox.Show("파일 및 서버 정보를 입력해주세요.");
            }
            catch (System.Net.Sockets.SocketException)
            {
                MessageBox.Show("포트 및 ip를 다시 확인해주세요.");
            }
            

        }

        /// <summary>
        /// 입력한 서버 정보 표시 버튼
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            String Server_ip = textBox1.Text;
            String Server_port = textBox2.Text;
            label5.Text = "서버 ip : " + Server_ip + "\n" + "서버 port : " + Server_port;
        }
    }
}


 