using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;
using ReLogic.Content;

namespace TRAEProject.NewContent.Items.Misc.Mounts
{
    public class HeatproofSaddle : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Heatproof Saddle");
            // Tooltip.SetDefault("Summons a ridable lava walker mount");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item79; 
            Item.noMelee = true;// this item doesn't do any melee damage
            Item.mountType = ModContent.MountType<Mounts.LavamanderMount>();
        }
    }
    public class LavamanderMount : ModMount
	{


		public override void SetStaticDefaults() {
            // Movement
            MountData.jumpHeight = 20; // How high the mount can jump.
			MountData.acceleration = 0.25f; // The rate at which the mount speeds up.
            MountData.jumpSpeed = 8.01f; // The rate at which the player and mount ascend towards (negative y velocity) the jump height when the jump button is presssed.
			MountData.blockExtraJumps = false; // Determines whether or not you can use a double jump (like cloud in a bottle) while in the mount.
			MountData.constantJump = true; // Allows you to hold the jump button down.
			MountData.heightBoost = 8; // Height between the mount and the ground
			MountData.fallDamage = 0.2f; // Fall damage multiplier.
			MountData.runSpeed = 5.6f; // The speed of the mount
			MountData.dashSpeed = 6f; // The speed the mount moves when in the state of dashing.
			MountData.flightTimeMax = 0; // The amount of time in frames a mount can be in the state of flying.

			// Misc
			MountData.fatigueMax = 0;
			MountData.buff = ModContent.BuffType<LavamanderBuff>(); // The ID number of the buff assigned to the mount.

			// Effects
			MountData.spawnDust = DustID.Torch; // The ID of the dust spawned when mounted or dismounted.

			// Frame data and player offsets
			MountData.totalFrames = 5; // Amount of animation frames for the mount
			MountData.playerYOffsets = Enumerable.Repeat(5, MountData.totalFrames).ToArray(); // Fills an array with values for less repeating code
			MountData.xOffset = 0;
			MountData.yOffset = 16;
			MountData.playerHeadOffset = 0;
			MountData.bodyFrame = 3;
			// Standing
			MountData.standingFrameCount = 0;
			MountData.standingFrameDelay = 0;
			MountData.standingFrameStart = 0;
			// Running
			MountData.runningFrameCount = 5;
			MountData.runningFrameDelay = 16;
			MountData.runningFrameStart = 0;
			// Flying
			MountData.flyingFrameCount = 0;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;
			// In-air
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 12;
			MountData.inAirFrameStart = 0;
			// Idle
			MountData.idleFrameCount = 0;
			MountData.idleFrameDelay = 0;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = false;
			// Swim
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;

			if (!Main.dedServ) {
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}
    }
    public class LavamanderBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lavamander");
            // Description.SetDefault("Dont get cold feet again");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true; 
        }

        public override void Update(Player player, ref int buffIndex)
        {
			player.GetModPlayer<Mobility>().TRAEwaterwalk = true;
            player.mount.SetMount(ModContent.MountType<LavamanderMount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }


}