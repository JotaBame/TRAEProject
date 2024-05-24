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
using System.Collections.Generic;
 
namespace TRAEProject.Changes.Accesory
{
    public class OnHitAccessories : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            switch (item.type)
            {
                case ItemID.CrossNecklace:
                    player.GetModPlayer<OnHitEffects>().crossNecklace = true;
                    player.longInvince = false;
                    break;
                case ItemID.StarVeil:
                    player.GetModPlayer<OnHitEffects>().crossNecklace = true;
                    player.longInvince = false;
                    player.starCloakItem = null;
                    player.GetModPlayer<OnHitEffects>().starCloaks += 1;
                    break;
                case ItemID.SweetheartNecklace:
                case ItemID.PanicNecklace:
                    player.GetModPlayer<OnHitEffects>().panicNecklaces += 1;
					player.panic = false;
                    break;
        
                case ItemID.MagicCuffs:
                    player.GetModPlayer<OnHitEffects>().magicCuffsCount += 1;
                    player.statManaMax2 -= 20;
                    player.magicCuffs = false;
                    break;
                case ItemID.CelestialCuffs:
                    player.statManaMax2 -= 20;
                    player.GetModPlayer<OnHitEffects>().magicCuffsCount += 1;
                    player.magicCuffs = false;
                    player.GetModPlayer<Mana>().celestialCuffsOverload = true;
                    break;
                case ItemID.StarCloak:
                    player.starCloakItem = null;
                    player.GetModPlayer<OnHitEffects>().starCloaks += 1;
                    break;
                case ItemID.Shackle:
                    player.GetModPlayer<OnHitEffects>().shackles += 1;

                    break;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.Shackle:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\nTemporarily increases defense when damaged";
                        }
                    }
                    break;
                case ItemID.MagicCuffs:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Restores 3 mana for every point of damage taken";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Can go over maximum mana";
                        }
                    }
                    break;
                case ItemID.CelestialCuffs:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Restores 3 mana for every point of damage taken";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Increases pickup range for mana stars";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "Mana restores can go over maximum mana";
                        }
                    }
                    break;
                case ItemID.CrossNecklace:
                case ItemID.StarVeil:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nMore effective on strong hits";
                        }
                    }
                    break;
            }
        }
    }
    public class OnHitEffects : ModPlayer
    {
        public int LastHitDamage = 0;
        public int BaghnakhHeal = 0;
        public int magicCuffsCount = 0;
        public int panicNecklaces = 0;
        public int shackles = 0;
        public int starCloaks = 0;
        public bool crossNecklace = false;
        public float newthorns = 0f;
        public float runeCooldown = 0;
        public float runethorns = 0f;

        public override void ResetEffects()
        {
            crossNecklace = false;
            shackles = 0;
            panicNecklaces = 0;
            magicCuffsCount = 0;
            starCloaks = 0;
            newthorns = 0f;
            runethorns = 0f; 
        }
        public override void UpdateDead()
        {
            crossNecklace = false;
            shackles = 0;
            panicNecklaces = 0;
            magicCuffsCount = 0;
            runethorns = 0f;        
            runeCooldown = 0;
            newthorns = 0f;
            starCloaks = 0;
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
 
        public override void PostHurt(Player.HurtInfo info)
        {
            if (crossNecklace)
            {
                int invintime = (int)((float)info.Damage * 3 / 5); // every point of info.Damage adds 0.0083 seconds 

                Player.immuneTime += invintime;

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
                if (starCloaks > 0)
                {
                    TRAEMethods.SpawnProjectilesFromAbove(Player, Player.position, 2 + (info.Damage / 33) * starCloaks, 400, 600, spread, 20, ProjectileID.StarCloakStar, 100, 2f, Player.whoAmI);
                }
                if(Main.myPlayer == Player.whoAmI) //only run this code for the local player
                {
                    if (runethorns > 0f && runeCooldown == 0)
                    {
                        RuneThorns(info.Damage);
                    }
                    if (newthorns > 0f)
                    {
                        Thorns(info.Damage);
                    }
                }
                if (shackles > 0)
                {
                    Player.AddBuff(BuffType<ShackledDefenses>(), info.Damage * 4 * shackles + 300); // 1 more second for every 15 damage

                }
                if (panicNecklaces > 0)
                {
                    Player.AddBuff(BuffID.Panic, 240 + info.Damage * 4 * panicNecklaces); // 1 more second for every 15 damage
                }
        
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
