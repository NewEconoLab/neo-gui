﻿using Neo;
using Neo.Wallets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace plugin_multisign
{
    public partial class MSign : Form
    {
        Dictionary<string, string> dAsset = new Dictionary<string, string>();
        Dictionary<string, UInt160> dAddress = new Dictionary<string, UInt160>();



        public MSign()
        {
            InitializeComponent();

            dAsset.Add("NEO", "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
            dAsset.Add("GAS", "602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
        }

        private void sign_Load(object sender, EventArgs e)
        {
            if (plugin_multisign.api.CurrentWallet != null)
            {
                var currWallet = plugin_multisign.api.CurrentWallet;
                foreach (var s in currWallet.GetAccounts())
                {
                    var addrOut = s.ScriptHash;
                    string addrStr = Wallet.ToAddress(addrOut);
                    comboBoxName.Items.Add(addrStr);
                }

                comboBoxName.SelectedIndex = 0;
                comboBoxName.Refresh();

                comAsset.Enabled = true;

                comAsset.Items.Add("NEO");
                comAsset.Items.Add("GAS");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 选择资产类型，更新余额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comAsset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currWallet = plugin_multisign.api.CurrentWallet;
            balanceText.Text = currWallet.GetBalance(UInt256.Parse(dAsset[comAsset.Text])).ToString();
        }

        /// <summary>
        /// 添加签名私钥
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string wif = wifText.Text.ToString();
            if (wif.Length != 52) return;

            for (int j = 0; j < wifsList.Items.Count; j++)
            {
                if (wifsList.Items[j].ToString() == wif)
                {
                    return;
                }
            }

            wifsList.Items.Add(wif);
        }

        /// <summary>
        /// 转账操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
