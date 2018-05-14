using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo.SmartContract.Debug
{
    public class LogScript
    {
        public LogScript parent;
        public string hash;
        public List<LogOp> ops = new List<LogOp>();
        public LogScript(string hash)
        {
            this.hash = hash;
        }
        public MyJson.JsonNode_Object ToJson()
        {
            MyJson.JsonNode_Object script = new MyJson.JsonNode_Object();
            script.SetDictValue("hash", this.hash);
            MyJson.JsonNode_Array array = new MyJson.JsonNode_Array();
            script.SetDictValue("ops", array);
            foreach (var op in ops)
            {
                array.Add(op.ToJson());
            }
            return script;
        }
    }
    public class LogOp
    {
        public int addr;
        public VM.OpCode op;
        //public string syscall;
        //public string[] syscallinfo;
        public VM.ExecutionStackRecord.Op[] stack;
        public VM.StackItem opresult;
        public byte[] opparam;
        public LogOp(int addr, VM.OpCode op)
        {
            this.addr = addr;
            this.op = op;
        }
        public LogScript subScript;
        public static MyJson.JsonNode_Object StatkItemToJson(VM.StackItem item)
        {
            MyJson.JsonNode_Object json = new MyJson.JsonNode_Object();
            var type = item.GetType().Name;
            if (type == "InteropInterface")
            {
                var _interface = (item as VM.Types.InteropInterface).GetInterface<VM.IInteropInterface>();
                if (_interface == null)
                {
                    json.SetDictValue(type, "<null>");
                }
                else
                {
                    json.SetDictValue(type, (item as VM.Types.InteropInterface).GetInterface<VM.IInteropInterface>().GetType().Name);
                }
            }

            else if (type == "Boolean")
            {
                json.SetDictValue(type, item.GetBoolean().ToString());
            }
            else if (type == "ByteArray")
            {
                json.SetDictValue(type, item.GetByteArray().ToHexString());
            }
            else if (type == "Integer")
            {
                json.SetDictValue(type, item.GetBigInteger().ToString());
            }
            else if (item is VM.Types.Array)//item is VM.Types.Struct || 是struct 一定是array
            {
                MyJson.JsonNode_Array array = new MyJson.JsonNode_Array();
                json.SetDictValue(type, array);
                foreach (var i in (item as VM.Types.Array).ToArray())
                {
                    array.Add(StatkItemToJson(i));
                }
            }
            return json;

        }
        public MyJson.JsonNode_Object ToJson()
        {
            MyJson.JsonNode_Object _op = new MyJson.JsonNode_Object();
            _op.SetDictValue("addr", addr);
            _op.SetDictValue("op", op.ToString());
            if (this.stack != null)
            {
                MyJson.JsonNode_Array array = new MyJson.JsonNode_Array();
                _op.SetDictValue("stack", array);
                foreach (var r in stack)
                {
                    if (r.ind > 0)
                    {
                        array.AddArrayValue(r.type.ToString() + "|" + r.ind);
                    }
                    else
                    {
                        array.AddArrayValue(r.type.ToString());
                    }
                }
            }
            if (opresult != null)
            {
                _op.SetDictValue("result", StatkItemToJson(opresult));
            }
            if (opparam != null)
            {
                _op.SetDictValue("param", opparam.ToHexString());
            }
            if (subScript != null)
            {
                _op.SetDictValue("subscript", subScript.ToJson());
            }

            return _op;
        }
    }


    public class FullLog
    {
        public static string Path = "fulllogs";
        public static List<Neo.UInt256> txids = new List<UInt256>();
        public static void RegNeedLog(Neo.UInt256 txid)
        {
            if (txids.Contains(txid) == false)
            {
                txids.Add(txid);
            }
        }
        public static bool TestNeedLog(Neo.UInt256 txid)
        {
            if (txids.Contains(txid))
                return true;

            return false;
        }
        public LogScript script = null;
        public string error = null;
        public VM.VMState state = VM.VMState.NONE;

        LogScript curScript = null;
        LogOp curOp = null;
        public void LoadScript(string hash)
        {
            if (script == null)
            {
                script = new LogScript(hash);
                curScript = script;
            }
            else
            {
                if (curOp == null)
                    return;
                curOp.subScript = new LogScript(hash);
                curOp.subScript.parent = curScript;
                curScript = curOp.subScript;
            }
        }
        public void NextOp(int addr, VM.OpCode op)
        {
            LogOp _op = new LogOp(addr, op);
            curScript.ops.Add(_op);
            curOp = _op;
            if (op == VM.OpCode.RET || op == VM.OpCode.TAILCALL)
            {
                if (curScript.parent != null)
                    curScript = curScript.parent;
            }
        }
        public void SetParam(VM.OpCode _op, byte[] data)
        {
            var op = curScript.ops.Last();
            if (op.op == _op)
            {
                op.opparam = data;
            }
        }
        public void OPStackRecord(VM.ExecutionStackRecord.Op[] records)
        {
            curOp.stack = records;
        }
        public void OpResult(VM.StackItem item)
        {
            curOp.opresult = item;
        }
        public void Error(string info)
        {
            this.error = info;
        }
        public void Finish(VM.VMState state)
        {
            this.state = state;
        }
        public string SaveToString()
        {
            var json = new MyJson.JsonNode_Object();
            json.SetDictValue("script", script.ToJson());
            if (string.IsNullOrEmpty(error) == false)
                json.SetDictValue("error", error);
            json.SetDictValue("VMState", state.ToString());

            StringBuilder sb = new StringBuilder();
            json.ConvertToStringWithFormat(sb, 0);

            var jsontxt = sb.ToString();

            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsontxt);
            var ms = new System.IO.MemoryStream(data);

            var bts = llvm.QuickFile.ToBytes(ms);


            return bts.ToHexString();
        }

        public void Save(string filename)
        {
            var path = System.IO.Path.GetDirectoryName(filename);
            if (System.IO.Directory.Exists(path) == false)
                System.IO.Directory.CreateDirectory(path);

            //System.IO.File.Delete(filename + ".json");
            System.IO.File.Delete(filename);


            var text = SaveToString();

            //System.IO.File.WriteAllText(filename, text);
            //byte[] data = System.Text.Encoding.UTF8.GetBytes();

            //var bts = llvm.QuickFile.ToBytes(filename);
            //var compressor = new SevenZip.SevenZipCompressor();
            //compressor.CompressionMethod = SevenZip.CompressionMethod.Lzma;
            //compressor.CompressionLevel = SevenZip.CompressionLevel.Fast;
            //compressor.FastCompression = true;

            ////compressor.path = path;
            //compressor.CompressFiles(filename, System.IO.Path.GetFullPath(filename + ".json"));
            System.IO.File.WriteAllText(filename, text);
            //System.IO.File.Delete(filename + ".json");

        }
        public static FullLog Load(string filename)
        {
            return null;
        }
    }
}
