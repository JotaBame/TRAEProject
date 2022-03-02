using Microsoft.Xna.Framework;
using System;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject;

namespace TRAEProject.Changes.Items
{
    public class OnHitItems : ModPlayer
    {
        public int LastHitDamage = 0;
        public int BaghnakhHeal = 0;
        public int magicCuffsCount = 0;
        public bool NewbeesOnHit = false;
        public bool NewstarsOnHit = false;
        public float newthorns = 0f;
        public float runeCooldown = 0;
        public float runethorns = 0f;
        public int beedamage = 1;
        
        public override void ResetEffects()
        {
            magicCuffsCount = 0;
            NewbeesOnHit = false;
            NewstarsOnHit = false;
            newthorns = 0f;
            runethorns = 0f;
        }
        public override void UpdateDead()
        {
            magicCuffsCount = 0;
            NewbeesOnHit = false;
            runethorns = 0f;        
            runeCooldown = 0;
            newthorns = 0f;
            NewstarsOnHit = false;
            beedamage = 0;
        }
        public override void PostUpdate()
        {
            if (runeCooldown > 0)
            {
                --runeCooldown;
            }
            if (runethorns > 0f && runeCooldown == 0) // For rune set's visual effect
            {
                Vector2 position23 = new Vector2(Player.position.X, Player.position.Y + 2f);
                int width22 = Player.width;
                int height22 = Player.height;
                float speedX6 = Player.velocity.X * 0.2f;
                float speedY6 = Player.velocity.Y * 0.2f;
                Dust dust = Dust.NewDustDirect(position23, width22, height22, 106, speedX6, speedY6, 100, default, 1.2f);
                dust.noGravity = true;
                dust.velocity.X *= 0.1f + Main.rand.Next(30) * 0.01f;
                dust.velocity.Y *= 0.1f + Main.rand.Next(30) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(6) * 0.1f;
            }
        }
        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            int findbuffIndex = Player.FindBuffIndex(BuffID.Frozen);
            if (findbuffIndex != -1)
            {
               Player.DelBuff(findbuffIndex);
            };
            LastHitDamage = damage;
            BaghnakhHeal = 0;
            if (damage > 1)
            {
				if (damage > 1000)
				{
					damage = 1000;
                }
				if (magicCuffsCount > 0)
                {
                    int manaRestored = damage * magicCuffsCount;
                    Player.GetModPlayer<Mana>().GiveManaOverloadable(manaRestored);
                }
                int[] spread = { 1, 2 };
                if (NewstarsOnHit)
                {
                    TRAEMethods.SpawnProjectilesFromAbove(Player, Player.position, 2 + (damage / 33), 400, 600, spread, 20, ProjectileID.StarCloakStar, 100, 2f, Player.whoAmI);
                }
                if (NewbeesOnHit)
                {
                    if (!Player.HasBuff(BuffID.ShadowDodge))
                    {
                        beedamage = damage;
                    }
                    TRAEMethods.SpawnProjectilesFromAbove(Player, Player.position, 3 + (damage / 33), 400, 600, spread, 20, ProjectileType<BuzzyStar>(), beedamage, 2f, Player.whoAmI);
                }
                if (runethorns > 0f && runeCooldown == 0)
                {
                    RuneThorns(damage);
                }
            }
            if (newthorns > 0f)
            {
                Thorns(damage);
            }
            if (Player.honeyCombItem != null && !Player.honeyCombItem.IsAir)
            {
                Player.AddBuff(BuffID.Honey, 300 + damage * 4);
            }
            if (Player.panic)
            {
                Player.AddBuff(BuffID.Panic, 300 + damage * 4);
            }
            if (Player.longInvince && damage > 100)
            {
                int invintime = (int)((damage - 100) * 0.6); // every point of damage past 100 adds 0.01 seconds of invincibility. 
                Player.immuneTime += invintime;
            }
        }
        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            int findbuffIndex = Player.FindBuffIndex(BuffID.Frozen);
            if (findbuffIndex != -1)
            {
                Player.DelBuff(findbuffIndex);
            };
            LastHitDamage = damage;
            BaghnakhHeal = 0;
            if (damage > 1)
            {		
		        if (damage > 1000)
				{
					damage = 1000;
                }
                if (magicCuffsCount > 0)
                {
                    int manaRestored = damage * magicCuffsCount;
                    Player.statMana += manaRestored;
                    Player.ManaEffect(manaRestored);
                }
                int[] spread = { 1, 2 };
                if (NewstarsOnHit)
                {
                    TRAEMethods.SpawnProjectilesFromAbove(Player, Player.position, 2 + (damage / 33), 400, 600, spread, 20, ProjectileID.HallowStar, 100, 2f, Player.whoAmI);
                }
                if (NewbeesOnHit)
                {
                    if (!Player.HasBuff(BuffID.ShadowDodge))
                    {
                        beedamage = damage;
                    }
                    TRAEMethods.SpawnProjectilesFromAbove(Player, Player.position, 3 + (damage / 33), 400, 600, spread, 20, ProjectileType<BuzzyStar>(), beedamage, 2f, Player.whoAmI);
                }
                if (runethorns > 0f && runeCooldown == 0)
                {
                    RuneThorns(damage);
                }
                if (newthorns > 0f)
                {
                    Thorns(damage);
                }
                if (!Player.HasBuff(BuffID.ShadowDodge))
                {
                    beedamage = damage;
                    int starcount = 2 + damage / 33;
                }
                if (Player.honeyCombItem != null && !Player.honeyCombItem.IsAir)
                {
                    Player.AddBuff(BuffID.Honey, 300 + damage * 4, false);
                }
                if (Player.panic)
                {
                    Player.AddBuff(BuffID.Panic, 300 + damage * 4, false);
                }
                if (Player.longInvince && damage > 100)
                {
                    int invintime = (int)(damage - 100 * 0.6); // every point of damage past 100 adds 0.01 seconds of invincibility. 
                    Player.immuneTime += invintime;
                }            

            }
        }

        void RuneThorns(int damage) 
        {
			runeCooldown = 300;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, Player.position);
            for (int i = 0; i < 50; ++i)
            {
                Vector2 position10 = new Vector2(Player.position.X, Player.position.Y);
                Dust dust = Dust.NewDustDirect(position10, Player.width, Player.height, 106, 0f, 0f, 100, default(Color), 2.5f);
                dust.velocity *= 3f;
                dust.noGravity = true;
            }
            foreach (NPC enemy in Main.npc)
            {
                float distance = 150f;
                Vector2 newMove = enemy.Center - Player.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                int direction = -1;
                if (enemy.position.X + (enemy.width / 2) < Player.position.X + (enemy.width / 2))
                {
                    direction = 1;
                }
                if (!enemy.dontTakeDamage && enemy.active && !enemy.friendly && !enemy.immortal && distanceTo < distance)
                {
                    int thorndamage = (int)(damage * runethorns + enemy.defense * 0.5);
                    if (thorndamage > 1000)
                    {
                        thorndamage = 1000;
                    }
                    if (enemy.type == NPCID.TheDestroyerBody)
                        thorndamage /= 10;
                    if (enemy.type == NPCID.TheDestroyerTail)
                        thorndamage /= 40;
                    Player.ApplyDamageToNPC(enemy, thorndamage, 10, -direction, false);
                    for (int i = 0; i < 20; ++i)
                    {
                        Vector2 position10 = new Vector2(enemy.position.X, enemy.position.Y);
                        Dust dust = Dust.NewDustDirect(position10, enemy.width, enemy.height, 106, 0f, 0f, 100, default, 2.5f);
                        dust.velocity *= 2f;
                        dust.noGravity = true;
                    }
                }
            }
        }
        void Thorns(int damage)
        {
            foreach (NPC enemy in Main.npc)
            {
                float distance = 500f;
                Vector2 newMove = enemy.Center - Player.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                int direction = -1;
                if (enemy.position.X + (enemy.width / 2) < Player.position.X + (enemy.width / 2))
                {
                    direction = 1;
                }
                if (!enemy.dontTakeDamage && enemy.active && !enemy.friendly && !enemy.immortal && distanceTo < distance)
                {
                    if (enemy.type == NPCID.TheDestroyerTail)
                        damage /= 4;

                    int thorndamage = (int)(damage * newthorns + enemy.defense * 0.5);
                    if (thorndamage > 1000)
                        thorndamage = 1000;
                    Player.ApplyDamageToNPC(enemy, thorndamage, 10, -direction, false);
                }
            }
        }
    }
}
