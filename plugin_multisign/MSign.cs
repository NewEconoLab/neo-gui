using Neo;
using Neo.Core;
using Neo.SmartContract;
using Neo.Wallets;
using System;
using System.Collections.Generic;
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
            wifText.Text = "";
        }

        /// <summary>
        /// 转账操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Transaction tx = MakeTran();
            Sign(tx);
        }

        private Transaction MakeTran()
        {
            Transaction tx = new ContractTransaction();
            List<TransactionAttribute> attributes = new List<TransactionAttribute>();
            tx.Attributes = attributes.ToArray();


            TransactionOutput[] outputs = new TransactionOutput[1];
            outputs[0] = new TransactionOutput();
            outputs[0].AssetId = UInt256.Parse(dAsset[comAsset.Text]);

            outputs[0].Value = new BigDecimal(Fixed8.Parse(amountText.Text).GetData(), 8).ToFixed8();

            outputs[0].ScriptHash = Wallet.ToScriptHash(targetAddr.Text.ToString());
            tx.Outputs = outputs;
            tx = plugin_multisign.api.CurrentWallet.MakeTransaction(tx, change_address: plugin_multisign.api.CurrentWallet.GetChangeAddress(), fee: Fixed8.Zero);
            return tx;
        }

        private void Sign(Transaction tx)
        {
            ContractParametersContext context;
            try
            {
                context = new ContractParametersContext(tx);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("无效的操作");
                return;
            }

            for (int j = 0; j < wifsList.Items.Count; j++)
            {
                var prikey = Wallet.GetPrivateKeyFromWIF(wifsList.Items[j].ToString());
                KeyPair key = new KeyPair(prikey);
                WalletAccount account = plugin_multisign.api.CurrentWallet.GetAccount(context.ScriptHashes[0]);
                byte[] signature = context.Verifiable.Sign(key);
                context.AddSignature(account.Contract, key.PublicKey, signature);
            }

            if (context.Completed)
            {
                MessageBox.Show("转账完成");
                context.Verifiable.Scripts = context.GetScripts();
                plugin_multisign.api.CurrentWallet.ApplyTransaction(tx);
                plugin_multisign.api.LocalNode.Relay(tx);
                
            }
        }
    }
}
