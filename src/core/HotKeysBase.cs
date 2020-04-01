

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace sweepnet.core
{

    public class HotKey
    {
        public Keys key;
        public string name;
        public HotKey(Keys key_, string name_)
        {
            key = key_; name = name_;
        }
    }

    public static class AllHotKeys
    {
        public static Dictionary<String, HotKey> list = new Dictionary<string, HotKey>();

        static AllHotKeys()
        {
            list["review"] = new HotKey(Keys.J, "j,J");
            list["next"] = new HotKey(Keys.K, "k,K");
            list["revert"] = new HotKey(Keys.L, "l,L");
            list["scroll"] = new HotKey(Keys.Space, "space");
            list["authors"] = new HotKey(Keys.F, "F,f");
        }
    }

    //----------\\
    
    public delegate void MouseMovedEvent();

    public class GlobalMouseHandler : IMessageFilter
    {
        private const int WM_MOUSEMOVE = 0x0200;

        public event MouseMovedEvent TheMouseMoved;

        #region IMessageFilter Members

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x020A) //WM_MOUSEWHEEL
            {
                if (TheMouseMoved != null)
                {
                    TheMouseMoved();
                }
            }
            // Always allow message to continue to the next filter control
            return false;
        }

        #endregion
    }
}
