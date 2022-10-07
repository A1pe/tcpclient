using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myStruct;
using System.Net.Sockets;

    public class FTask
    {
        System.IO.FileStream fsread;
        System.IO.FileInfo fi;
        ulong fileSz;
        string[] filepaths;
        char[] filepath;
        byte[] tmpbuf;
        char[] tBoxText;

        Socket sock;
        file_box boxes;

        public FTask(String str1, String str2,Socket userSock) {
            sock = userSock;
            boxes = new file_box();
            boxes.client_port = new char[5];
            boxes.filename = new char[10];
            fi = new System.IO.FileInfo(str1);
            fsread = new System.IO.FileStream(str1, System.IO.FileMode.Open);
            fileSz = (ulong)fi.Length;

            filepaths = str1.Split('\\');
            filepath = filepaths[filepaths.Length - 1].ToCharArray();
            tBoxText = str2.ToCharArray();

            boxes.filesize = (uint)fileSz;
          
        }

        public void task() {
            setFileStructInfo(boxes.client_port, tBoxText);
            setFileStructInfo(boxes.filename, filepath);
            tmpbuf = castings.StructureToByte(boxes);
            sock.Send(tmpbuf, SocketFlags.None);
        }

        private void setFileStructInfo(char[] p, char[] tBoxText)
        {
            for (int i = 0; ; i++)
            {
                if (i == p.Length || i == tBoxText.Length)
                {
                    p[i] = '\0';
                    break;
                }
                p[i] = tBoxText[i];
            }
        }
        public ulong getFileSz() {
            return fileSz;
        }
        public System.IO.FileStream getFS() {
            return fsread;
        }
        public System.IO.FileInfo getFI() {
            return fi;
        }
    }

