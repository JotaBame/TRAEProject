
using System.ComponentModel;
using Terraria.ModLoader.Config;



namespace TRAEProject 
{
    public class TRAEConfig : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("GameMechanics")]

        [ReloadRequired]
        [DefaultValue(true)]
        public bool ManaRework { get; set; }
        [ReloadRequired]
        [DefaultValue(true)]
        public bool MobilityRework { get; set; }

            [ReloadRequired]
            [DefaultValue(true)]
            public bool TerrasparkLineRework { get; set; }

        [ReloadRequired]
        [DefaultValue(true)]
        public bool DefenseRework { get; set; }

        [ReloadRequired]
        [DefaultValue(true)]
        public bool Resprites { get; set; }
        [DefaultValue(false)]
        public bool FurtherImprovedMovement { get; set; }

    }
    public class BossConfig : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        [Header("BossChanges")]

        [DefaultValue(true)]
        public bool EOCChanges { get; set; }
        [DefaultValue(true)]
        public bool BoCChanges { get; set; }
        [DefaultValue(true)]
        public bool EoWChanges { get; set; }
        [DefaultValue(true)]
        public bool QBChanges { get; set; }
       
        [DefaultValue(true)]
        public bool EoWChanges { get; set; }
        [DefaultValue(true)]
        public bool BoCChanges { get; set; }
		
        [DefaultValue(true)]
        public bool SkeletronChanges { get; set; }
        
        [DefaultValue(true)]
        public bool DeerclopsChanges { get; set; }
         
        [DefaultValue(true)] 
        public bool TwinsRework { get; set; }

        [DefaultValue(true)]
        public bool PrimeRework { get; set; }

        [DefaultValue(true)]
        public bool DestroyerRework { get; set; }

        [DefaultValue(false)]
        public bool PlanteraRework { get; set; }
        
        [DefaultValue(true)]
        public bool PlanteraTentacle { get; set; }

        [DefaultValue(true)]
        public bool FishronChanges { get; set; }

         [DefaultValue(true)]
        public bool CultistChanges { get; set; }
 

        

        
 


    }
}