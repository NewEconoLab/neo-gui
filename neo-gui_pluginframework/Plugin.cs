using Neo.Core;
using Neo.Implementations.Wallets.EntityFramework;
using Neo.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Neo.GUIPlugin
{
    public class PluginTool
    {
        public Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();
        public List<string> errors = new List<string>();
        public List<string> failPlugin = new List<string>();

        public void LoadDlls(IAPI api, string path = "plugins", string searchPattern = "plugin_*.dll")
        {
            //loadplugin
            var files = System.IO.Directory.GetFiles(path, searchPattern);
            foreach (var file in files)
            {
                try
                {
                    var dll = System.Reflection.Assembly.LoadFile(System.IO.Path.GetFullPath(file));
                    foreach (var t in dll.ExportedTypes)
                    {
                        var b = t.GetInterfaces().Contains(typeof(IPlugin));
                        if (b)
                        {
                            var plugin = t.Assembly.CreateInstance(t.FullName) as IPlugin;
                            var name = plugin.Name;
                            plugins.Add(name, plugin);
                        }
                    }
                }
                catch (Exception err)
                {
                    string errstr = "error load:" + file + "  err:" + err.Message;
                    errors.Add(errstr);
                    Console.WriteLine(errstr);
                }

            }
            foreach (var plugin in plugins)
            {
                try
                {
                    plugin.Value.Init(api);
                }
                catch (Exception err)
                {
                    string errstr = "error init:" + plugin.Key + "  err:" + err.Message;
                    failPlugin.Add(plugin.Key);
                    Console.WriteLine(errstr);
                }
            }
        }
        public void InitMenu(System.Windows.Forms.ToolStripMenuItem rootmenu)
        {
            System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem();
            item.Text = "good";
            rootmenu.DropDownItems.Add(item);
            foreach (var plugin in plugins)
            {
                try
                {
                    System.Windows.Forms.ToolStripMenuItem pitem = new System.Windows.Forms.ToolStripMenuItem();
                    pitem.Text = plugin.Key;
                    rootmenu.DropDownItems.Add(pitem);

                    foreach (var n in plugin.Value.GetMenus())
                    {
                        System.Windows.Forms.ToolStripMenuItem citem = new System.Windows.Forms.ToolStripMenuItem();
                        citem.Text = n;

                        pitem.DropDownItems.Add(citem);
                        var __plugin = plugin.Value;
                        var __menu = n;
                        citem.Click += (s, e) =>
                          {
                              try
                              {
                                  __plugin.OnMenu(__menu);
                              }
                              catch(Exception err)
                              {
                                  MessageBox.Show("error:" + err.Message);
                              }
                          };
                    }
                }
                catch (Exception err)
                {
                }
            }
        }
    }

    public interface IAPI
    {
        void SignAndShowInformation(Transaction tx);
        LocalNode LocalNode
        {
            get;
        }
        Neo.Wallets.Wallet CurrentWallet
        {
            get;
        }
    }
    public interface IAPI2 : IAPI
    {
        void SendRaw(Transaction tx);
    }

    public interface IPlugin
    {
        void Init(IAPI api);
        string Name
        {
            get;
        }
        string[] GetMenus();
        void OnMenu(string menu);
    }
}
