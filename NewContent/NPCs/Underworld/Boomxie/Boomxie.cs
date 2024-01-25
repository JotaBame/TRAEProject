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
using TRAEProject.NewContent.Items.Weapons.Magic.WillOfTheWisp;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.ModLoader.Utilities;

namespace TRAEProject.NewContent.NPCs.Underworld.Boomxie

{
    public class Boomxie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            // DisplayName.SetDefault("Boom Pixie"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 4; // make sure to set this for your modnpcs.
        }
        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 40;
            NPC.aiStyle = 22;
            AIType = NPCID.Pixie;
            AnimationType = NPCID.Pixie;
            NPC.damage = 50;
            NPC.defense = 10;
            NPC.lifeMax = 80;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.knockBackResist = 0.5f;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.OnFire3] = true;
            Banner = NPC.type;
            BannerItem = ItemType<BoomxieBanner>();
        }
        int spamTimer = 0;
        public override void AI()
        {
            NPC.TargetClosest(false);
            spamTimer++;
            if (spamTimer > 360)
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<LittleBoomxie>());
                spamTimer = 0;
            }
            if (NPC.life < NPC.lifeMax * 0.5f)
            {
                spamTimer += 3;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("The adult stage of Lavaflies. They defend themselves by leaving volatile dust behind. ")
            }); 
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneBeach && Main.remixWorld)
                return SpawnCondition.Underworld.Chance * 0.25f;
            if (NPC.downedPlantBoss)
            {
                return SpawnCondition.Underworld.Chance * 0.025f;
            }
            return SpawnCondition.Underworld.Chance * 0.15f; 
        

        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.ExplosivePowder, 2, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ItemType<WillOfTheWisp>(), 20));
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(-2, -2), Main.rand.NextFloat(2, 2));
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, DustID.Torch);
                dust.scale = 0.5f;
            }
        }
        public override void OnKill()
        {
            Vector2 zero = new Vector2(0, 0);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, zero, ProjectileType<Boom>(), 30, 0);
        }
    }
    public class LittleBoomxie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            }; NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            // DisplayName.SetDefault("Boom Dust"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 1; // make sure to set this for your modnpcs.
        }
        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 14;
            NPC.damage = 20;
            NPC.defense = 4;
            NPC.lifeMax = 20;
            NPC.knockBackResist = 1f;
        }
        public override void UpdateLifeRegen(ref int damage)
        {
            NPC.lifeRegen -= 20;
            if (damage < 5)
            {
                damage = 5;
            }
            return;
        }
        public override void AI()
        {
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
                NPC.velocity.Y -= 0.5f;
                if (NPC.velocity.Y < -1f)
                {
                    NPC.velocity.Y = -1f;
                }

            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.ExplosivePowder, 10));
        }
        public override void OnKill()
        {
            Vector2 zero = new Vector2(0, 0);
            if (!Main.expertMode && !Main.masterMode)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, zero, ProjectileType<Boom>(), 30, 0);
            }
            else
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.BurningSphere);
            }
        }
    }
    public class Boom : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 5;
        }
        public bool runonce = false; 
        public override void AI()
        {
            if (!runonce)
            {
                runonce = true;
                TRAEMethods.Explode(Projectile, 150);
                TRAEMethods.DefaultExplosion(Projectile);
            }
        }  
    }
}