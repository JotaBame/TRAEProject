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
        public bool NewstarsOnHit = false;
        public float newthorns = 0f;
        public float runeCooldown = 0;
        public float runethorns = 0f;
        public bool Frozen = false;

        public override void ResetEffects()
        {
            magicCuffsCount = 0;
            NewstarsOnHit = false;
            newthorns = 0f;
            runethorns = 0f; 
        }
        public override void UpdateDead()
        {
            magicCuffsCount = 0;
            runethorns = 0f;        
            runeCooldown = 0;
            newthorns = 0f;
            NewstarsOnHit = false;
        }
  
        public override void PostUpdateEquips()
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
        public override void OnHurt(Player.HurtInfo info)
        {

            LastHitDamage = info.Damage;
            BaghnakhHeal = 0;
            if (info.Damage > 1)
            {
				if (info.Damage > 1000)
				{
					info.Damage = 1000;
                }
				if (magicCuffsCount > 0)
                {
                    int manaRestored = info.Damage * magicCuffsCount * 3;
                    Player.GetModPlayer<Mana>().GiveManaOverloadable(manaRestored);
                }
                int[] spread = { 1, 2 };
                if (NewstarsOnHit)
                {
                    TRAEMethods.SpawnProjectilesFromAbove(Player, Player.position, 2 + (info.Damage / 33), 400, 600, spread, 20, ProjectileID.StarCloakStar, 100, 2f, Player.whoAmI);
                }
                if (runethorns > 0f && runeCooldown == 0)
                {
                    RuneThorns(info.Damage);
                }
            }
            if (newthorns > 0f)
            {
                Thorns(info.Damage);
            }
            if (Player.panic)
            {
                Player.AddBuff(BuffID.Panic, 300 + info.Damage * 4);
            }
            if (Player.longInvince)
            {
                int invintime = (int)(info.Damage * 3 / 5); // every point of info.Damage adds 0.01 seconds 
                if (invintime > 120)
                    invintime = 120;
                Player.immuneTime += invintime - 40; // cross necklace adds .67 seconds so we need to substract that from the total.
            }
        }

        void RuneThorns(int damage) 
        {
			runeCooldown = 120;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 }, Player.position);
            for (int i = 0; i < 50; ++i)
            {
                Vector2 position10 = new Vector2(Player.position.X, Player.position.Y);
                Dust dust = Dust.NewDustDirect(position10, Player.width, Player.height, 106, 0f, 0f, 100, default, 2.5f);
                dust.velocity *= 3f;
                dust.noGravity = true;
            }
            foreach (NPC enemy in Main.npc)
            {
                float distance = 300f;
                Vector2 newMove = enemy.Center - Player.Center;
                float distanceTo = MathF.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
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
                float distanceTo = MathF.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
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
