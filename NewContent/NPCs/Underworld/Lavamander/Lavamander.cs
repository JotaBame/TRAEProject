using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TRAEProject.NewContent.Items.Misc.Mounts;
using TRAEProject.NewContent.NPCs.Banners;

using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.NPCs.Underworld.Lavamander
{
    public class LavamanderNPC : ModNPC
    {
        public override void SetStaticDefaults()
		{
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			// DisplayName.SetDefault("Lavamander"); 
            Main.npcFrameCount[NPC.type] = 5;
        }

		public override void SetDefaults()
		{
			NPC.width = 94;
			NPC.height = 22;
            NPC.aiStyle = 3;
            AIType = NPCID.WalkingAntlion;
			//AnimationType = NPCID.WalkingAntlion;
			NPC.value = 2000;
			NPC.damage = 35;
			NPC.defense = 12;
			NPC.lifeMax = 180;
			NPC.lavaImmune = true;
			NPC.HitSound = SoundID.NPCHit26;
			NPC.DeathSound = SoundID.NPCDeath29;
			NPC.knockBackResist = 0.25f;
			DrawOffsetY = -4;
			NPC.scale = 1.05f;         
			Banner = NPC.type; 
            BannerItem = ItemType<LavamanderBanner>();
        }
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
				new FlavorTextBestiaryInfoElement("Reptilian Lava Walkers. They hang near lava and attack only in self defense, meaning they could be domesticated with enough patience.")
			});
		}
		float dustTimer = 0;
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<HeatproofSaddle>(), 25));
        }
        float jump = 0;
        public override void AI()
        {

			NPC.noGravity = false;
			int num = 1;
			int num2 = 1;
			int num3 = (int)((NPC.position.X + (NPC.width / 2)) / 16f);
			int num4 = (int)(NPC.Bottom.Y / 16f);
			for (int j = num3 - num; j <= num3 + num; j++)
			{
				for (int k = num4 - num2; k < num4 + num2; k++)
				{
					if (Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 0 ||
						Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 2 ||
						Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 1
						)
					{
						if (NPC.velocity.Y > 0)
							NPC.velocity.Y = 0;
						NPC.noGravity = true;
						jump++;
						if (NPC.Distance(NPC.GetTargetData().Center) <= 300f)
							jump += 3; // jumps way more often if it can reach you
						if (jump >= 750f) // We have to force it to jump, its normal AI won't let it jump while "water walking"
						{
							jump = 0;
							NPC.velocity.Y = -8f;
							NPC.velocity.X *= 2f;

						}
					}
				}
			}
		}
		public override bool PreKill()
		{
            for (int i = 0; i < 4; i++)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("LavamanderGore4").Type, 1f);
            }
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("LavamanderGore1").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("LavamanderGore2").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("LavamanderGore3").Type, 1f);
            return true;

		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
 
            if (!NPC.downedPlantBoss)
            {
                return SpawnCondition.Underworld.Chance * 0.25f;
            }

            return 0f;
        }
        int frame = 0;
		public override void FindFrame(int frameHeight)
        {

			if (NPC.velocity.X == 0f)
			{
				NPC.frame.Y = 0;
			}
			if (NPC.direction < 0 && NPC.velocity.X < 0f)
			{
				NPC.spriteDirection = -1;
			}
			if (NPC.direction > 0 && NPC.velocity.X > 0f)
			{
				NPC.spriteDirection = 1;
			}
			if (NPC.frame.Y / frameHeight < 2)
			{ 
				NPC.frame.Y = frameHeight * 2;
			}
			NPC.frameCounter += 1f + Math.Abs(NPC.velocity.X) / 2f;
			if (NPC.frameCounter > 12.0)
			{
				NPC.frame.Y += frameHeight;
				NPC.frameCounter = 0.0;
			}
			if (NPC.frame.Y / frameHeight >= Main.npcFrameCount[NPC.type])
			{
				NPC.frame.Y = frameHeight * 2;
			}
			if (NPC.velocity.Y != 0f)
			{
				NPC.frame.Y = 0;
			}
		}
    }

}