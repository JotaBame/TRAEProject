using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TRAEProject.Common;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Materials;
using Terraria.GameContent.ItemDropRules;
using TRAEProject.NewContent.NPCs.Banners;

namespace TRAEProject.NewContent.NPCs.Underworld.ObsidianBasilisk
{
	// These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
	internal class ObsidianBasiliskHead : WormHead
	{
		public override int BodyType => ModContent.NPCType<ObsidianBasiliskBody>();

		public override int TailType => ModContent.NPCType<ObsidianBasiliskTail>();

		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Obsidian Basilisk");
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            //var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { // Influences how the NPC looks in the Bestiary
            //	//CustomTexturePath = "ExampleMod/Content/NPCs/ExampleWorm_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
            //	Position = new Vector2(40f, 24f),
            //	PortraitPositionXOverride = 0f,
            //	PortraitPositionYOverride = 12f
            //};
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

		public override void SetDefaults() {
            // Head is 10 defence, body 20, tail 30.
            NPC.CloneDefaults(NPCID.DiggerHead);
            NPC.width = 50;
            NPC.height = 86;
			NPC.knockBackResist = 0f;
            //NPC.aiStyle = 3;
            //AIType = NPCID.WalkingAntlion;
            //AnimationType = NPCID.WalkingAntlion;
            NPC.value = 10000;
            NPC.damage = 100;
            NPC.defense = 30;
            NPC.lifeMax = 26000; NPC.scale = 1.1f;
            NPC.lavaImmune = true;
			NPC.GetGlobalNPC<Freeze>().freezeImmune = true; NPC.GetGlobalNPC<Stun>().stunImmune = true;
            NPC.takenDamageMultiplier *= 2f;
            NPC.GetGlobalNPC<UnderworldEnemies>().HellMinibossThatSpawnsInPairs = true;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath24;
            BannerItem = ItemType<ObsidianBasiliskBanner>();
            Banner = NPC.type;

            //NPC.aiStyle = -1;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<ObsidianScale>(), 1, 2, 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.Spaghetti, 10));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("The Bone Serpents of the Underworld regained their scales and their power. With a heavily armored exoskeleton they relentlessly pursue any intruders.")
			});
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 20; i++)
            {
                float radius = 300 / 41.67f;
                Vector2 speed = Main.rand.NextVector2CircularEdge(radius, radius);
                Dust d = Dust.NewDustPerfect(NPC.Center, DustID.Torch, speed, Scale: 3f);
                if (Main.rand.NextBool(3))
                {
                    d.scale *= Main.rand.NextFloat(1.25f, 1.5f);
                    d.velocity *= Main.rand.NextFloat(1.25f, 1.5f);
                }
                d.noGravity = true;
            }
        }
        public override void Init() {
			// Set the segment variance
			// If you want the segment length to be constant, set these two properties to the same value
			MinSegmentLength = 75;
			MaxSegmentLength = 75;

			CommonWormInit(this);
		}

		// This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
		internal static void CommonWormInit(Worm worm) {
			// These two properties handle the movement of the worm
			worm.MoveSpeed = 20f;
			worm.Acceleration = 0.15f;
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return NPC.GetGlobalNPC<UnderworldEnemies>().MinibossSpawn(spawnInfo);

        }
        public override void OnKill()
        {

            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("ObsidianBasiliskHead_Gore").Type, 1f);
        }
    }

    internal class ObsidianBasiliskBody : WormBody
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Obsidian Basilisk");
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Daybreak] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerBody);
            NPC.damage = 75;
            NPC.defense = 125; NPC.HitSound = SoundID.NPCHit4;
            NPC.GetGlobalNPC<Freeze>().freezeImmune = true;
            NPC.GetGlobalNPC<Stun>().stunImmune = true;

            NPC.aiStyle = -1; NPC.scale = 1.2f;
        }

        public override void Init()
        {
            ObsidianBasiliskHead.CommonWormInit(this);
        }
        public override void PostAI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Some of these conditions are possble if the body/tail segment was spawned individually
                // Kill the segment if the segment NPC it's following is no longer valid
                if (NPC.life == 0 && !NPC.active)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("ObsidianBasiliskBody_Gore").Type, 1f);

                }
            }
        }

    }

    internal class ObsidianBasiliskTail : WormTail
	{
		public override void SetStaticDefaults() 
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.DiggerTail); NPC.damage = 80;
			NPC.defense = 50;
			NPC.aiStyle = -1;
			NPC.scale = 1.2f; NPC.HitSound = SoundID.NPCHit41;
            NPC.GetGlobalNPC<Freeze>().freezeImmune = true;
            NPC.GetGlobalNPC<Stun>().stunImmune = true;

        }
        public override void PostAI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Some of these conditions are possble if the body/tail segment was spawned individually
                // Kill the segment if the segment NPC it's following is no longer valid
                if (NPC.life == 0 && !NPC.active)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("ObsidianBasiliskTail_Gore").Type, 1f);

                }
            }

        }


        public override void Init() {
			ObsidianBasiliskHead.CommonWormInit(this);
		}
	}
}