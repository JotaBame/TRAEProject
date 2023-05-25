using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.NPCs.Underworld.Beholder;
using Terraria.ID;
using System;
using TRAEProject.Changes.NPCs.Miniboss.Santa;
using System.Threading;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria.Audio;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace TRAEProject.NewContent.Items.BeholderItems
{
    class ScrollOfWipeout : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Scroll of Wipeout");
            // Tooltip.SetDefault("Deals massive damage to all nearby enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = Item.useAnimation = 90;
            Item.rare = ItemRarityID.Lime;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (!player.HasBuff<Wipeout>())
                {
                    SoundEngine.PlaySound(SoundID.Roar with { MaxInstances = 0 }, player.position);

                    return true;
                }
                if (player.HasBuff<Wipeout>())
                {
                    return false;
                }
            }
            return true;
        }
    }
    public class WipeoutCooldown : ModPlayer
    {

        public int wipeoutCooldown = 0;
        public override void UpdateDead()
        {
            wipeoutCooldown = 0;
        }
        public override void PostUpdate()
        {
            if (wipeoutCooldown > 0)
                wipeoutCooldown--;
        }
        bool killNPCs = false;

        public override void PostItemCheck()
        {

            if (Player.HeldItem.type == ItemType<ScrollOfWipeout>() && Player.itemAnimation > 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Item item = Player.HeldItem;


                if (Main.rand.Next(2) == 0)
                {
                    Dust.NewDust(Player.position, Player.width, Player.height, DustID.Blood, 0f, 0f, 150, default(Color), 1.1f);
                }
                if (Player.ItemTimeIsZero)
                {

                    Player.ApplyItemTime(item);
                }
                else if (Player.itemTime == item.useTime / 2)
                {

                    if (Player.GetModPlayer<WipeoutCooldown>().wipeoutCooldown != 0)
                        killNPCs = true;
                    for (int k = 0; k < 200; k++)
                    {
                        NPC nPC = Main.npc[k];
                        if (!killNPCs)
                        {
                            if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(Player.Center, nPC.Center) <= 3000f)
                            {
                                for (int l = 0; l < 10; l++)
                                {
                                    Dust.NewDust(nPC.position, nPC.width, nPC.height, DustID.Blood, nPC.velocity.X * 0.5f, nPC.velocity.Y * 0.5f, 150, default(Color), 1.5f);
                                }
                                Player.ApplyDamageToNPC(nPC, 1000, 0f, 0, crit: false);
                            }
                        }
                        else
                        {
                            if (nPC.active && nPC.friendly && !nPC.dontTakeDamage)
                            {
                                for (int l = 0; l < 10; l++)
                                {
                                    Dust.NewDust(nPC.position, nPC.width, nPC.height, DustID.Blood, nPC.velocity.X * 0.5f, nPC.velocity.Y * 0.5f, 150, default(Color), 1.5f);
                                }
                                Player.ApplyDamageToNPC(nPC, 1000, 0f, 0, crit: false);
                            }
                        }

                    }
                    if (killNPCs)
                    {
                        SoundEngine.PlaySound(SoundID.ForceRoarPitched with { MaxInstances = 0 }, Player.position);

                        Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " tried to break the rules"), 666, 0);
                    }
                    Player.AddBuff(BuffType<Wipeout>(), 8 * 60 * 60);
                    Player.GetModPlayer<WipeoutCooldown>().wipeoutCooldown = 8 * 60 * 60; // 8 minutes
                    for (int l = 0; l < 70; l++)
                    {
                        Dust.NewDust(Player.position, Player.width, Player.height, DustID.Blood, Player.velocity.X * 0.5f, Player.velocity.Y * 0.5f, 150, default(Color), 1.5f);
                    }

                }
            }
        }
    }
    public class Wipeout: ModBuff
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wiped Out");
            // Description.SetDefault("The scroll needs to recharge");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }
    }

}
