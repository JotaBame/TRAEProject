
using System.ComponentModel;
using Terraria.ModLoader.Config;



namespace TRAEProject 
{
    //[Label("Global Configurations")]
    public class TRAEConfig : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        //[Header("Boss Changes")]

        //[Label("Plantera Rework")]
        //[Tooltip("Experimental Plantera Rework, buggy as hell")]
        //[ReloadRequired] //None required
        [DefaultValue(false)]
        public bool PlanteraRework { get; set; }

        //[Label("Twins Rework")]
        //[Tooltip("Recommended when mixing multiple mods with boss changes.")]
        //[ReloadRequired] //None required
        [DefaultValue(true)]
        public bool TwinsRework { get; set; }
        //[Label("Cultist Changes")]
        //[Tooltip("Recommended when mixing multiple mods with boss changes.")]
        //[ReloadRequired] //None required     
        [DefaultValue(true)]
        public bool CultistChanges { get; set; }
        //[Label("Deerclops Changes")]
        //[Tooltip("Recommended when mixing multiple mods with boss changes.")]
        //[ReloadRequired] //None required     
        [DefaultValue(true)]
        public bool DeerclopsChanges { get; set; }
        //[Label("Fishron Changes")]
        //[Tooltip("Recommended when mixing multiple mods with boss changes.")]
        //[ReloadRequired] //None required     
        [DefaultValue(true)]
        public bool FishronChanges { get; set; }
        //[Label("Queen Bee Changes")]
        //[Tooltip("Recommended when mixing multiple mods with boss changes.")]
        //[ReloadRequired] //None required     
        [DefaultValue(true)]
        public bool QBChanges { get; set; }
        //[Label("Skeletron Changes")]
        //[Tooltip("Recommended when mixing multiple mods with boss changes.")]
        //[ReloadRequired] //None required     
        [DefaultValue(true)]
        public bool SkeletronChanges { get; set; }
    }
}