using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TRAEProject.NewContent.Items.Misc.PermaBuffs
{
	public class PermaBuffs : ModPlayer
	{
		/*
		public bool speedWish;
		public override void PostUpdateBuffs()
		{
			if(speedWish)
			{
				Player.moveSpeed += 0.15f;
				if (Math.Abs(Player.velocity.X) >= 5f && Main.rand.NextBool(10) && Player.velocity.Y == 0)
				{
                    int num4 = Dust.NewDust(new Vector2(Player.position.X + (Player.width / 2), Player.position.Y + Player.height + Player.gfxOffY), Player.width / 2, 6, DustID.MagicMirror, 0f, 0f, 0, default(Color), 0.8f);
                    Main.dust[num4].scale *= 1f + (float)Main.rand.Next(20, 40) * 0.01f;
                    Main.dust[num4].noGravity = true;
                    Main.dust[num4].noLight = true;
                    Main.dust[num4].velocity *= 0.001f;
                    Main.dust[num4].velocity.Y -= 0.003f;
                    Main.dust[num4].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                }
            }
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) 
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)Player.whoAmI);
			packet.Write((bool)speedWish);
			packet.Send(toWho, fromWho);
		}

		// Called in ExampleMod.Networking.cs
		public void ReceivePlayerSync(BinaryReader reader)
		{
			speedWish = reader.ReadBoolean();
		}

		public override void CopyClientState(ModPlayer clientClone) {
			PermaBuffs clone = clientClone as PermaBuffs;
			clone.speedWish = speedWish;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) 
		{
			PermaBuffs clone = clientPlayer as PermaBuffs;

			if (speedWish != clone.speedWish)
			{
				SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
			}
		}

		// NOTE: The tag instance provided here is always empty by default.
		// Read https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound to better understand Saving and Loading data.
		public override void SaveData(TagCompound tag) 
		{
			tag["speedWish"] = speedWish;
		}

		public override void LoadData(TagCompound tag) 
		{
			speedWish = tag.GetBool("speedWish");
		}
		*/
	}
}