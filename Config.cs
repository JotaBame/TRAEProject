
using System.ComponentModel;
using Terraria.ModLoader.Config;



namespace TRAEProject 
{
    
    public class TRAEConfig : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        [Header("BossChanges")]
        [DefaultValue(true)]

        public bool EOCChanges { get; set; }

        //[ReloadRequired] //None required     
        [DefaultValue(true)]
        public bool QBChanges { get; set; }

        //[ReloadRequired] //None required     
        [DefaultValue(true)]
        public bool SkeletronChanges { get; set; }
        
        [DefaultValue(true)]
        public bool DeerclopsChanges { get; set; }
        //[ReloadRequired] //None required     
        [DefaultValue(true)] 
        
        public bool TwinsRework { get; set; }
        //[ReloadRequired] //None required     
        [DefaultValue(false)]
        public bool PlanteraRework { get; set; }

        //[ReloadRequired] //None required
        [DefaultValue(true)]
        public bool FishronChanges { get; set; }

        //[ReloadRequired] //None required     
        [DefaultValue(true)]
        public bool CultistChanges { get; set; }
        //[ReloadRequired] //None required     
 


    }
}