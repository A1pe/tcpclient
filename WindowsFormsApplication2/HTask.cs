using myStruct;
using System.Runtime.InteropServices;
using System;
using System.Net.Sockets;

public class HTask{

    _patrol_set partrol;
    byte[] tmpbuf;
    Socket sock;

    public HTask(Socket userSock) {
        partrol = new _patrol_set();
        partrol._patrol_sequence = 0;
        this.sock = userSock;
        
    }
    public void bufCastByteToStruct() {
        this.tmpbuf = castings.StructureToByte(partrol);
    }
    public void bufCastStructToByte(byte[] userBuf) {
        this.partrol = (_patrol_set)castings.ByteToStructure(userBuf, typeof(_patrol_set));
    }

    public void task() {
        bufCastByteToStruct(); 
        sock.Send(tmpbuf, SocketFlags.None);
        sock.Receive(tmpbuf, SocketFlags.None);
        bufCastStructToByte(tmpbuf);
        this.partrol._patrol_sequence = this.partrol._patrol_sequence + 1;
        bufCastByteToStruct();
        sock.Send(tmpbuf, SocketFlags.None);
    }


}