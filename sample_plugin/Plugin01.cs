﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neo.GUIPlugin;

namespace plugin_sample
{
    public class Plugin01 : Neo.GUIPlugin.IPlugin
    {
        public string Name => "Nep5 tool";

        public string[] GetMenus()
        {
            return new string[] { "Nep5 tool", "test tx" };
        }
        IAPI api;
        public void Init(IAPI api)
        {
            this.api = api;
        }

        public void OnMenu(string menu)
        {
            if(menu == "Nep5 tool")
            {
                var f1 = new Form1();
                f1.api = this.api;
                f1.ShowDialog();
            }
            else if(menu== "test tx")
            {
                var tt = new TestTran();
                tt.api = this.api as IAPI2;
                tt.ShowDialog();

            }
        }
    }
}
