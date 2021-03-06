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
using TRAEProject.NewContent.NPCs.Boomxie;
using TRAEProject.NewContent.Items.Weapons.Summoner.Sentries.BoomfrogStaff;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.NPCs.Froggabomba

{
    public class Froggabomba : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Froggabomba"); // Automatic from .lang files
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
                new FlavorTextBestiaryInfoElement("Speedy, explodey and self-duplicating, these frog-like creatures hunt their prey with their own lives.")
            }); 
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneUnderworldHeight)
            {
                return 0.10f;
            }
            return 0f;
        }
        int damagestored = 0;
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            damagestored += damage;
            if (damagestored > 30)
            {
                int smallBoomxiesToSpawn = damagestored / 30;
                for (int i = 0; i < smallBoomxiesToSpawn; i++)
                {
                    damagestored -= 30;
                    NPC.life -= 30;
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<FroggabombaClone>());
                }
            }
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            damagestored += damage;
            if (damagestored > 30)
            {
                int smallBoomxiesToSpawn = damagestored / 30;
                for (int i = 0; i < smallBoomxiesToSpawn; i++)
                {
                    damagestored -= 30;
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<FroggabombaClone>());
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.ExplosivePowder, 4)); 
            npcLoot.Add(ItemDropRule.Common(ItemType<BoomfrogStaff>(), 20));

        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 2; i++)
            {
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, DustID.Torch);
                dust.scale = 0.5f;
            }
        }
        public override void OnKill()
        {
            Vector2 zero = new Vector2(0, 0);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, zero, ProjectileType<Boom>(), NPC.damage, 0);
        }
    }
    public class FroggabombaClone : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Froggabomba"); // Automatic from .lang files
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
            NPC.lifeMax = 30;
			NPC.scale = 0.8f;
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
        public override void AI()
        {
            NPC.dontTakeDamage = false;
    
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
                if (NPC.velocity.Y < -10f)
                {
                    NPC.velocity.Y = -10f;
                }

            }
        }
        public override void OnKill()
        {
            Vector2 zero = new Vector2(0, 0);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, zero, ProjectileType<Boom>(), NPC.damage, 0);
        }
    }

}