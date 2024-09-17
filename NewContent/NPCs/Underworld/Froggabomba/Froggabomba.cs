using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.NPCs.Banners;
using TRAEProject.NewContent.NPCs.Underworld.Boomxie;
using TRAEProject.NewContent.Items.Weapons.Summoner.Sentries.BoomfrogStaff;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.ModLoader.Utilities;

namespace TRAEProject.NewContent.NPCs.Underworld.Froggabomba

{
    public class Froggabomba : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            // DisplayName.SetDefault("Froggabomba"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 4; // make sure to set this for your modnpcs.
        }
        public override void SetDefaults()
        {
            NPC.width = 45;
            NPC.height = 38;
            NPC.aiStyle = 41;
            AIType = NPCID.Pixie;
            AnimationType = NPCID.Pixie;
            NPC.damage = 30;
            NPC.defense = 10;
            NPC.lifeMax = 150;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit33; 
            NPC.DeathSound = SoundID.NPCDeath36; 
            NPC.knockBackResist = 0.5f;
            Banner = NPC.type;
            BannerItem = ItemType<FroggabombaBanner>(); 

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("Speedy, explodey and self-duplicating, these frog-like creatures carry their offspring on their stomachs.")
            }); 
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.SpawnTileType != TileID.ObsidianBrick && spawnInfo.SpawnTileType != TileID.HellstoneBrick)
            {
                if (!NPC.downedPlantBoss)
                    return SpawnCondition.Underworld.Chance * 0.12f;
                else
                    return SpawnCondition.Underworld.Chance * 0.03f;
            }
            return SpawnCondition.Underworld.Chance * 0f;
        }
    
   
 
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.ExplosivePowder, 2)); 
            npcLoot.Add(ItemDropRule.Common(ItemType<BoomfrogStaff>(), 16));

        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 2; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, DustID.Torch);
                dust.scale = 0.5f;
            }
        }
        public override void OnKill()
        {
            int count = Main.rand.Next(3, 6);
            for (int i = 0; i < count; i++)
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + Main.rand.Next(-10, 10), (int)NPC.Center.Y, NPCType<FroggabombaClone>());
            }
 
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, 0), ProjectileType<Boom>(), 30, 0, ai0: 125);
        }
        float jump = 0;

        public override void AI()
        {
            NPC.dontTakeDamage = false;
            jump++;
            if (NPC.Distance(NPC.GetTargetData().Center) <= 300f)
                jump += 3; // jumps way more often if it cant reach you
            if (jump >= 600f && NPC.velocity.Y == 0f)  
            {
                jump = 0;
                NPC.velocity.Y = -9f;
                NPC.velocity.X *= 1f;

            }
            if (NPC.wet)
            {
                if (NPC.collideY)
                {
                    NPC.velocity.Y = -2f;
                }
                if (NPC.velocity.Y < 0f && NPC.ai[3] == NPC.position.X)
                {
                    NPC.direction *= -1;
                    NPC.ai[2] = 200f;
                }
                if (NPC.velocity.Y > 0f)
                {
                    NPC.ai[3] = NPC.position.X;
                }

                if (NPC.velocity.Y > 2f)
                {
                    NPC.velocity.Y *= 0.9f;
                }
                else if (NPC.directionY < 0)
                {
                    NPC.velocity.Y -= 0.8f;
                }
                NPC.velocity.Y -= 0.75f;
                if (NPC.velocity.Y < -10f)
                {
                    NPC.velocity.Y = -10f;
                }

            }
        }
        }
    public class FroggabombaClone : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            // DisplayName.SetDefault("Froggabomba"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 4; // make sure to set this for your modnpcs.
        }
        public override void SetDefaults()
        {
            NPC.width = 45;
            NPC.height = 38;
            NPC.aiStyle = 41;
            AIType = NPCID.Pixie;
            AnimationType = NPCID.Pixie;
            NPC.damage = 30;
            NPC.defense = 10;
            NPC.lifeMax = 60;
			NPC.scale = 0.85f;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit33;
            NPC.DeathSound = SoundID.NPCDeath36;
            NPC.knockBackResist = 0.5f; 
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.ExplosivePowder, 10));
            npcLoot.Add(ItemDropRule.Common(ItemType<BoomfrogStaff>(), 80));

        }
        float jump = 0;
 
        public override void AI()
        {
            NPC.dontTakeDamage = false;
            jump++;
            if (NPC.Distance(NPC.GetTargetData().Center) <= 300f)
                jump += 3; // jumps way more often if it can reach you
            if (jump >= 600f) // We have to force it to jump, its normal AI won't let it jump while "water walking"
            {
                jump = 0;
                NPC.velocity.Y = -10f;
                NPC.velocity.X *= 2.5f;

            }
            if (NPC.wet)
            {
                if (NPC.collideY)
                {
                    NPC.velocity.Y = -2f;
                }
                if (NPC.velocity.Y < 0f && NPC.ai[3] == NPC.position.X)
                {
                    NPC.direction *= -1;
                    NPC.ai[2] = 200f;
                }
                if (NPC.velocity.Y > 0f)
                {
                    NPC.ai[3] = NPC.position.X;
                }

                if (NPC.velocity.Y > 2f)
                {
                    NPC.velocity.Y *= 0.9f;
                }
                else if (NPC.directionY < 0)
                {
                    NPC.velocity.Y -= 0.8f;
                }
                NPC.velocity.Y -= 0.75f;
                if (NPC.velocity.Y < -10f)
                {
                    NPC.velocity.Y = -10f;
                }

            }
        }
        public override void OnKill()
        {
            Vector2 zero = new Vector2(0, 0);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, zero, ProjectileType<Boom>(), 30, 0, ai0: 100);
        }
    }

}