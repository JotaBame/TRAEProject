using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Prefixes
{
	//thic class determines which weapon can get what prefix
    public class GiveWeaponsPrefixes : GlobalItem
    {

		public override bool InstancePerEntity => true;
		public override GlobalItem Clone(Item item, Item itemClone)
		{
			return base.Clone(item, itemClone);
		}
		public bool canGetRangedModifiers = false;
		public bool canGetBoomerangModifers = false;
		public bool canGetYoyoModifers = false;
        public override void SetDefaults(Item item)
        {
            switch(item.type)
            {
				case ItemID.VampireKnives:
				case ItemID.ShadowFlameKnife:
					canGetRangedModifiers = true;
					break;
				case ItemID.Mace:
				case ItemID.FlamingMace:
				case ItemID.BallOHurt:
				case ItemID.TheMeatball:
				case ItemID.BlueMoon:
				case ItemID.Sunfury:
				case ItemID.DaoofPow:
				case ItemID.DripplerFlail:
				case ItemID.FlowerPow:
				case ItemID.Flairon:
				case ItemID.ChainKnife:
				case ItemID.ChainGuillotines:
				case ItemID.KOCannon:
				case ItemID.GolemFist:
				case ItemID.Anchor:
				case ItemID.WoodenBoomerang:
				case ItemID.EnchantedBoomerang:
				case ItemID.IceBoomerang:
				case ItemID.Shroomerang:
				case ItemID.ThornChakram:
				case ItemID.Flamarang:
				case ItemID.CombatWrench:
				case ItemID.BouncingShield:
				case ItemID.Bananarang:
				case ItemID.FlyingKnife:
				case ItemID.LightDisc:
				case ItemID.PaladinsHammer:
				case ItemID.PossessedHatchet:
					canGetBoomerangModifers = true;
					break;
			}
			if(ItemID.Sets.Yoyo[item.type])
            {
				canGetYoyoModifers = true;
            }
			
        }
		public override bool PreReforge(Item item)
        {
			item.GetGlobalItem<BoomerangFlailStats>().AP = 0;
			item.GetGlobalItem<YoyoStats>().range = 1f;
			item.GetGlobalItem<YoyoStats>().speed = 1f;

			return base.PreReforge(item);
		}
		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			//reset custom modifiable stats before addign a new prefix
			//item.GetGlobalItem<BoomerangFlailStats>().AP = 0;
			//item.GetGlobalItem<YoyoStats>().range = 1f;
			//item.GetGlobalItem<YoyoStats>().speed = 1f;

			if (canGetRangedModifiers)
			{
				int num = 0;
				#region pick
				int num11 = rand.Next(35);
				if (num11 == 0)
				{
					num = 16;
				}
				if (num11 == 1)
				{
					num = 17;
				}
				if (num11 == 2)
				{
					num = 18;
				}
				if (num11 == 3)
				{
					num = 19;
				}
				if (num11 == 4)
				{
					num = 20;
				}
				if (num11 == 5)
				{
					num = 21;
				}
				if (num11 == 6)
				{
					num = 22;
				}
				if (num11 == 7)
				{
					num = 23;
				}
				if (num11 == 8)
				{
					num = 24;
				}
				if (num11 == 9)
				{
					num = 25;
				}
				if (num11 == 10)
				{
					num = 58;
				}
				if (num11 == 11)
				{
					num = 36;
				}
				if (num11 == 12)
				{
					num = 37;
				}
				if (num11 == 13)
				{
					num = 38;
				}
				if (num11 == 14)
				{
					num = 53;
				}
				if (num11 == 15)
				{
					num = 54;
				}
				if (num11 == 16)
				{
					num = 55;
				}
				if (num11 == 17)
				{
					num = 39;
				}
				if (num11 == 18)
				{
					num = 40;
				}
				if (num11 == 19)
				{
					num = 56;
				}
				if (num11 == 20)
				{
					num = 41;
				}
				if (num11 == 21)
				{
					num = 57;
				}
				if (num11 == 22)
				{
					num = 42;
				}
				if (num11 == 23)
				{
					num = 44;
				}
				if (num11 == 24)
				{
					num = 45;
				}
				if (num11 == 25)
				{
					num = 46;
				}
				if (num11 == 26)
				{
					num = 47;
				}
				if (num11 == 27)
				{
					num = 48;
				}
				if (num11 == 28)
				{
					num = 49;
				}
				if (num11 == 29)
				{
					num = 50;
				}
				if (num11 == 30)
				{
					num = 51;
				}
				if (num11 == 31)
				{
					num = 59;
				}
				if (num11 == 32)
				{
					num = 60;
				}
				if (num11 == 33)
				{
					num = 61;
				}
				if (num11 == 34)
				{
					num = 82;
				}
				#endregion

				return num;
			}
			if(canGetBoomerangModifers)
            {
				int num13 = rand.Next(23);
				int num = 0;
                #region pick vanilla
                
                if (num13 == 0)
				{
					num = 36;
				}
				if (num13 == 1)
				{
					num = 37;
				}
				if (num13 == 2)
				{
					num = 38;
				}
				if (num13 == 3)
				{
					num = 53;
				}
				if (num13 == 4)
				{
					num = 54;
				}
				if (num13 == 5)
				{
					num = 55;
				}
				if (num13 == 6)
				{
					num = 39;
				}
				if (num13 == 7)
				{
					num = 40;
				}
				if (num13 == 8)
				{
					num = 56;
				}
				if (num13 == 9)
				{
					num = 41;
				}
				if (num13 == 10)
				{
					num = 57;
				}
				if (num13 == 11)
				{
					num = 59;
				}
				if (num13 == 12)
				{
					num = 60;
				}
				if (num13 == 13)
				{
					num = 61;
				}
				if (num13 == 14)
				{
					num = 84;
				}
				#endregion

				switch(num13)
                {
					case 14:
						num = PrefixType<Devastating>();
						break;
					case 15:
						num = PrefixType<Brutal>();
						break;
					case 16:
						num = PrefixType<Kinetic>();
						break;
					case 17:
						num = PrefixType<Spiked>();
						break;
					case 18:
						num = PrefixType<Aerodynamic>();
						break;
					case 19:
						num = PrefixType<Piercing>();
						break;
					case 20:
						num = PrefixType<Dense>();
						break;
					case 21:
						num = PrefixType<Enchanted>();
						break;
					case 22:
						num = PrefixType<Pathetic>();
						break;
				}

				return num;
			}
			if(canGetYoyoModifers)
            {
				int num13 = rand.Next(22);
				if(item.type == ItemID.Terrarian)
                {
					num13 = rand.Next(23);
                }
				int num = 0;
				#region pick vanilla

				if (num13 == 0)
				{
					num = 36;
				}
				if (num13 == 1)
				{
					num = 37;
				}
				if (num13 == 2)
				{
					num = 38;
				}
				if (num13 == 3)
				{
					num = 53;
				}
				if (num13 == 4)
				{
					num = 54;
				}
				if (num13 == 5)
				{
					num = 55;
				}
				if (num13 == 6)
				{
					num = 39;
				}
				if (num13 == 7)
				{
					num = 40;
				}
				if (num13 == 8)
				{
					num = 56;
				}
				if (num13 == 9)
				{
					num = 41;
				}
				if (num13 == 10)
				{
					num = 57;
				}
				if (num13 == 11)
				{
					num = 59;
				}
				if (num13 == 12)
				{
					num = 60;
				}
				if (num13 == 13)
				{
					num = 61;
				}
				if (num13 == 14)
				{
					num = 84;
				}
				#endregion

				switch (num13)
				{
					case 14:
						num = PrefixType<Radical>();
						break;
					case 15:
						num = PrefixType<Extreme>();
						break;
					case 16:
						num = PrefixType<Pounding>();
						break;
					case 17:
						num = PrefixType<Zoned>();
						break;
					case 18:
						num = PrefixType<Relentless>();
						break;
					case 19:
						num = PrefixType<Tricky>();
						break;
					case 20:
						num = PrefixType<Extended>();
						break;
					case 21:
						num = PrefixType<Bad>();
						break;
					case 22:
						num = PrefixID.Legendary2;
						break;
				}

				return num;
			}
			return base.ChoosePrefix(item, rand);
		}
    }
}