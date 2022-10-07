using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace myStruct
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct _header_data
    {
        public uint seq_num;
        public uint bufsize;
        public uint crc32;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public char[] message;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        public char[] buffer;

    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct _header_data_info
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1017)]
        public _header_data[] informactions;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct file_box
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public char[] client_port;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public char[] filename;
        public uint filesize;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct file_box_info
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        public file_box[] informactions;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct _patrol_set
    {
        public int _patrol_sequence;
    }
}
