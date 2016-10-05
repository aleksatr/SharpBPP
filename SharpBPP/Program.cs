using SharpBPP.Forms;
using System;

namespace SharpBPP
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            MainForm mf = new MainForm();
            mf.ShowDialog();
        }

    }
}
