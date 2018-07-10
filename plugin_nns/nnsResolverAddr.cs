using Neo;
using Neo.Core;
using Neo.SmartContract;
using Neo.VM;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Data;
using Neo.Implementations.Blockchains.LevelDB;

namespace plugin_nns
{
    public partial class nnsResolverAddr : Form
    {
        //private InvocationTransaction tx;
        //private UInt160 script_hash;

        private DataTable dt = new DataTable();
        //private ContractParameter[] parameters;

        //定义nns注册器合约hash
        private static readonly string NnsResolverAddrScripthash = "706c89208c5b6016a054a58cc83aeda0d70f0f95";

        private static readonly Fixed8 net_fee = Fixed8.FromDecimal(0.001m);

        public nnsResolverAddr()
        {
            InitializeComponent();
            dt.Columns.Add("scripthash", typeof(string));
            dt.Columns.Add("key", typeof(string));
            dt.Columns.Add("value", typeof(string));
            dt.Columns.Add("time", typeof(DateTime));

            Neo.Implementations.Blockchains.LevelDB.LevelDBBlockchain.ApplicationExecuted += NCNotify;
        }

        private void NCNotify(object sender, ApplicationExecutedEventArgs e)
        {
            var a = e.ExecutionResults[0].ScriptHash;
            var b = UInt160.Parse(NnsResolverAddrScripthash);
            var c = UInt160.Parse("c191b3e4030b9105e59c6bb56ec0d1273cd43284");
            //DataRow r = dt.NewRow();
            //r[0] = a;
            //r[1] = b;
            //dt.Rows.Add(r);
            foreach (var result in e.ExecutionResults)
            {
                if ((a == b) || (a == c))
                {
                    var e2 = e.ExecutionResults;
                    foreach (NotifyEventArgs notify in result.Notifications)
                    {
                        string scripthash = notify.ScriptHash.ToString();
                        StackItem s = notify.State;
                        string key = (s as Neo.VM.Types.Array)[0].GetString();
                        string value = (s as Neo.VM.Types.Array)[1].GetByteArray().ToHexString();

                        var r = dt.NewRow();
                        r[0] = scripthash;
                        r[1] = key;
                        r[2] = value;
                        r[3] = DateTime.Now;
                        dt.Rows.Add(r);

                        //MessageBox.Show("0x" + value, key);
                    }
                }
            }
            //plugin_nns.api.SignAndShowInformation(tx);
            //plugin_nns.api.CurrentWallet;
        }

        private ContractParameter[] getQueryParas(string domain, string name, string subname)
        {
            ////格式化成hash160格式串
            //script_hash = UInt160.Parse(NnsRegistryScripthash);
            ////从区块链依据script_hash获取已部署的智能合约
            //ContractState contract = Blockchain.Default.GetContract(script_hash);
            //if (contract == null) return new ContractParameter[0];

            ////以智能合约参数定义初始化参数数据对象
            //parameters = contract.ParameterList.Select(p => new ContractParameter(p)).ToArray();
            //if (parameters.Any(p => p.Value == null)) return new ContractParameter[0];
            ////以键值对文本框值赋值参数数据对象
            ////parameters[0].Type = ContractParameterType.Signature;//Signature 不赋值

            ContractParameter[] parameters = new ContractParameter[]{
                //new ContractParameter(ContractParameterType.Signature),
                new ContractParameter(ContractParameterType.String){Value = "query"},
                new ContractParameter(ContractParameterType.Array){
                    Value = new ContractParameter[]{
                        new ContractParameter(ContractParameterType.String){Value = domain},
                        new ContractParameter(ContractParameterType.String){Value = name},
                        new ContractParameter(ContractParameterType.String){Value = subname}
                    }
                }
            };

            return parameters;
        }

        private ContractParameter[] getAlterParas(string domain, string name, string subname, string addr)
        {
            ContractParameter[] parameters = new ContractParameter[]{
                //new ContractParameter(ContractParameterType.Signature),
                new ContractParameter(ContractParameterType.String){Value = "alter"},
                new ContractParameter(ContractParameterType.Array){
                    Value = new ContractParameter[]{
                        new ContractParameter(ContractParameterType.String){Value = domain},
                        new ContractParameter(ContractParameterType.String){Value = name},
                        new ContractParameter(ContractParameterType.String){Value = subname},
                        new ContractParameter(ContractParameterType.String){Value = addr}
                    }
                }
            };

            return parameters;
        }

        private ContractParameter[] getDeleteParas(string domain, string name, string subname)
        {
            ContractParameter[] parameters = new ContractParameter[]{
                //new ContractParameter(ContractParameterType.Signature),
                new ContractParameter(ContractParameterType.String){Value = "delete"},
                new ContractParameter(ContractParameterType.Array){
                    Value = new ContractParameter[]{
                        new ContractParameter(ContractParameterType.String){Value = domain},
                        new ContractParameter(ContractParameterType.String){Value = name},
                        new ContractParameter(ContractParameterType.String){Value = subname}
                    }
                }
            };

            return parameters;
        }

        private void ExeContract(ContractParameter[] contractParameters)
        {
            InvocationTransaction tx = new InvocationTransaction();
            UInt160 script_hash;

            string txScript = string.Empty;

            //格式化成hash160格式串
            script_hash = UInt160.Parse(NnsResolverAddrScripthash);
            //从区块链依据script_hash获取已部署的智能合约
            ContractState contract = Blockchain.Default.GetContract(script_hash);
            if (contract == null) return;
            //将智能合约信息以Json格式显示出来
            //string contractJsonStr = JsonConvert.SerializeObject(contract);

            //以参数数据对象更新脚本
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(script_hash, contractParameters);
                txScript = sb.ToArray().ToHexString();
            }

            //**************
            //构建交易并试运行获取预计gas
            //**************
            //if (tx == null) tx = new InvocationTransaction();
            tx.Version = 1;
            //将之前生成的脚本导入交易类
            tx.Script = txScript.HexToBytes();
            //以下（似乎）都是初始化，没有自定义值
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            if (tx.Inputs == null) tx.Inputs = new CoinReference[0];
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Scripts == null) tx.Scripts = new Witness[0];
            //使用智能合约引擎试运行交易
            ApplicationEngine engine = ApplicationEngine.Run(tx.Script, tx);
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine($"VM State: {engine.State}");
            //sb.AppendLine($"Gas Consumed: {engine.GasConsumed}");
            //sb.AppendLine($"Evaluation Stack: {new Neo.IO.Json.JArray(engine.EvaluationStack.Select(p => p.ToParameter().ToJson()))}");
            //textBox7.Text = sb.ToString();
            if (!engine.State.HasFlag(VMState.FAULT))
            {
                //根据试运行交易结果获得预计gas花费
                tx.Gas = engine.GasConsumed - Fixed8.FromDecimal(10);
                if (tx.Gas < Fixed8.Zero) tx.Gas = Fixed8.Zero;
                tx.Gas = tx.Gas.Ceiling();
                //Fixed8 fee = tx.Gas.Equals(Fixed8.Zero) ? net_fee : tx.Gas;
                //labGasFee.Text = fee + " gas";
                //butPutData.Enabled = true;
            }
            else
            {
                MessageBox.Show("执行失败");
                return;
            }

            InvocationTransaction it = new InvocationTransaction();
            //已测试通过的交易构建正式的关联钱包的交易对象
            Fixed8 fee = tx.Gas.Equals(Fixed8.Zero) ? net_fee : Fixed8.Zero;
            it = plugin_nns.api.CurrentWallet.MakeTransaction(new InvocationTransaction
            {
                Version = tx.Version,
                Script = tx.Script,
                Gas = tx.Gas,
                Attributes = tx.Attributes,
                Inputs = tx.Inputs,
                Outputs = tx.Outputs
            }, fee: fee);

            //用本地钱包签名交易并发送
            //Helper.SignAndShowInformation(it);
            Transaction t = it;
            if (t == null)
            {
                MessageBox.Show("余额不足");
                return;
            }
            ContractParametersContext context;
            try
            {
                context = new ContractParametersContext(t);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("非同步块");
                return;
            }
            plugin_nns.api.CurrentWallet.Sign(context);
            if (context.Completed)
            {
                context.Verifiable.Scripts = context.GetScripts();
                var a = context.ToJson();
                plugin_nns.api.CurrentWallet.ApplyTransaction(t);
                plugin_nns.api.LocalNode.Relay(t);

                //InformationBox.Show(t.Hash.ToString(), Strings.SendTxSucceedMessage, Strings.SendTxSucceedTitle);

                string tid = t.Hash.ToString();
                txtTXID.Text = tid;

                //操作日志写入目录下SQLite数据库
                //qmz_opLog log = new qmz_opLog();
                //log.doSQLliteLog(txtScriptHash.Text, txtKey.Text, txtValue.Text, (decimal)it.Gas, tid);
            }
            else
            {
                MessageBox.Show("签名发送失败");
                return;
                //InformationBox.Show(context.ToString(), Strings.IncompletedSignatureMessage, Strings.IncompletedSignatureTitle);
            }

        }

        private static string[] splitNNS(string nns)
        {
            string[] nnsS = nns.Split('.');
            string domain = nnsS[nnsS.Length - 1];
            string name = nnsS[nnsS.Length - 2];
            string subname = string.Join(".", nnsS);
            if (subname.Length <= (domain.Length + name.Length + 1))
            {
                subname = "";
            }
            else
            {
                subname = subname.Substring(0, subname.Length - domain.Length - name.Length - 2);
            }

            return new string[] { domain, name, subname };
        }

        private static byte[] NameHash(string nns)
        {
            Neo.Cryptography.Crypto c = new Neo.Cryptography.Crypto();

            byte[] namehash = c.Hash256(Encoding.UTF8.GetBytes(string.Join("", splitNNS(nns))));

            return namehash;
        }

        private void butQuery_Click(object sender, EventArgs e)
        {
            //StorageKey sk = new StorageKey
            //{
            //    ScriptHash = UInt160.Parse(NnsResolverAddrScripthash),
            //    Key = NameHash(txtName.Text)
            //};
            //var storageItem = Blockchain.Default.GetStorageItem(sk);
            //string value = "null";
            //if (storageItem != null)
            //{ value = Encoding.UTF8.GetString(storageItem.Value); }

            //txtAddr.Text = value;

            string[] nnsS = splitNNS(txtName.Text);
            ExeContract(getQueryParas(nnsS[0], nnsS[1], nnsS[2]));
        }

        private void butAlter_Click(object sender, EventArgs e)
        {
            string[] nnsS = splitNNS(txtName.Text);
            ExeContract(getAlterParas(nnsS[0], nnsS[1], nnsS[2], txtAddr.Text));
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            string[] nnsS = splitNNS(txtName.Text);
            ExeContract(getDeleteParas(nnsS[0], nnsS[1], nnsS[2]));
        }

        private void timerFresh_Tick(object sender, EventArgs e)
        {
            dgvNotify.DataSource = dt;
            dgvNotify.Sort(dgvNotify.Columns[3], System.ComponentModel.ListSortDirection.Ascending);
            dgvNotify.Refresh();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show(plugin_nns.api.CurrentWallet.GetKeys().First().PublicKey.EncodePoint(true).ToHexString());
        //}
    }
}
