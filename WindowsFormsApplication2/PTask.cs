using myStruct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

    public class PTask{

        _header_data payloadata;
        _header_data recvpayloadata;

        uint nowseq;
        uint nextseq;
        uint prevseq;
        bool Dflag;
        
        Socket sock;
        ulong isize;
        System.IO.FileStream fs;

        byte[] buffer;
        ulong iLen;
        byte[] recvBuf;
        char[] buffers;
        int idx = 0;

        public PTask(Socket userSock, ulong fdsz, System.IO.FileStream fs){

            payloadata = new _header_data();
            recvpayloadata = new _header_data();

            nowseq = 0;
            nextseq = 0;
            prevseq = 0;
            Dflag = false;

            sock = userSock;
            isize = fdsz;
            this.fs = fs;
            //buffers =  new char[1000];
            recvBuf = new byte[1017];
           
        }

        public void task() {
            buffer = new byte[isize];
            fs.Read(buffer, 0, (int)isize);
            fs.Position = 0;
            setStructAndGetValue(fs);
            
            payloadata.seq_num = 0;
            payloadata.crc32 = CRC32.crc32(0, buffer, buffer.Length);
            setMessage("DATA\0");

            sock.Send(castings.StructureToByte(payloadata), SocketFlags.None);
            sock.Receive(recvBuf, SocketFlags.None);
            getRecvData();
            setSequence();
        }

        public void task2()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            setStructAndGetValue( fs);
            setSequence2(payloadata.seq_num);

            sock.Send(castings.StructureToByte(payloadata), SocketFlags.None);
            //recvpayloadata.buffer = rebuf;
            //recvpayloadata.message = remsg;
            sock.Receive(recvBuf, SocketFlags.None);

            getRecvData();
            setSequence();
            sw.Stop();
            //setStructAndGetValue(fs);

           // setSequence2(payloadata.seq_num);

           // sock.Send(castings.StructureToByte(payloadata), SocketFlags.None);
            
           // sw.Reset();
        }


        public void getRecvData()
        {
            recvpayloadata = (_header_data)castings.ByteToStructure(recvBuf, typeof(_header_data));
            StringBuilder stber = new StringBuilder();

            dynamic message = recvpayloadata.message;
            dynamic tmpstrrrr = new char[5];
            for (int i = 0; i < 5; i++)
            {
                stber.Append(message[i]);
            }
            String str = stber.ToString();
            setMessage(str);
        }

        public void setStructAndGetValue(System.IO.FileStream fsread)
        {
            buffers = new char[1000];
            UInt32 nums = (uint)System.Text.Encoding.ASCII.GetChars(buffer, idx, ((int)isize - idx > 999) ? 999 : (int)isize - idx, buffers, 0);
            if ((int)isize - idx <= 999)
                getFlag();
            
            idx += (int)nums;
            payloadata.buffer = buffers;
            payloadata.bufsize = nums;
            iLen = nums;
        }

        public void setMessage(string str)
        {
            if (str.Equals("DONE\0"))
            {
                this.Dflag = true;
            }
            else {
                payloadata.message = "DATA\0".ToCharArray();
            }
        }

        public void setSequence2(uint seq)
        {
            this.nowseq = seq;
        }

        public void setSequence()
        {
            nowseq++;
            nextseq = nowseq + 1;
            if (nowseq > 1)
            {
                prevseq = nowseq - 2;
            }

            if (recvpayloadata.seq_num == nowseq)
            {
                payloadata.seq_num = nextseq;
            }
            else {
                payloadata.seq_num = prevseq;
            }
        }
        public bool getFlag() {
            return !Dflag;
        }
        public ulong getLen() {
            return iLen;
        }


        
    }
