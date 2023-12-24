
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


        [Header("PrimeRail")]
        
        [DefaultValue(RailMode.Classic)]
		[DrawTicks]
        public RailMode railMode;

        [Header("BameRail")]

        [DefaultValue(12.5f)]
        [Range(8f, 16f)]
		[Increment(.25f)]
        public float bRailVel;

        [DefaultValue(5)]
        public int bRailEUpdates;

        [DefaultValue(120)]
        public int bRailTurnSpeed;

        [Header("QwertyRail")]
          
        [DefaultValue(10f)]
        [Range(8f, 16f)]
		[Increment(.25f)]
        public float qRailVel;

        [DefaultValue(7)]
        public int qRailEUpdates;

        [DefaultValue(90)]
        public int qRailTurnSpeed;

        [DefaultValue(40)]
        public int railStopAimTime;
        
        [DefaultValue(true)]
        public bool holdFireOnRage; 

        
 


    }
    public enum RailMode
    {
        Classic,
        BameNerf,
        QwertyNerf
    }
}