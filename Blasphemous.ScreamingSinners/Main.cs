using BepInEx;

namespace Blasphemous.ScreamingSinners
{
    [BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
    [BepInDependency("Blasphemous.ModdingAPI", "2.1.2")]
    [BepInDependency("Blasphemous.Framework.Items", "0.1.0")]
    public class Main : BaseUnityPlugin
    {
        public static ScreamingSinners ScreamingSinners { get; private set; }

        private void Start()
        {
            ScreamingSinners = new ScreamingSinners();
        }
    }
}
