using Neo.GUIPlugin;

namespace plugin_multisign
{
    public class plugin_multisign : IPlugin
    {
        public string Name => "多签插件";

        public string[] GetMenus()
        {
            return new string[] { "多签" };
        }

        public void OnMenu(string menu)
        {
            MSign sign = new MSign();
            sign.ShowDialog();
        }

        public static IAPI api;
        public void Init(IAPI api) => plugin_multisign.api = api;
    }
}
