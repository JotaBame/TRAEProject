using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TRAEProject.Changes.Projectiles.Spears;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Weapon
{
    class SpearItems : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public int altShoot = -1;
        public bool canGetMeleeModifiers = false;
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {

                case ItemID.Spear:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<BasicSpear>();
                    altShoot = ProjectileType<BasicSpearThrow>();

                    item.useTime = item.useAnimation = 27;
                    item.shootSpeed = 6; //only the throw uses this
                    break;

				case ItemID.TheRottedFork:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<RottedFork>();
					altShoot = ProjectileType<RottedForkThrow>();

					item.damage = 21;
					item.shootSpeed = 6.5f; //only the throw uses this
					break;

				case ItemID.BoneJavelin:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<BoneSpear>();
					altShoot = ProjectileType<BoneSpearThrow>();

					item.useTime = item.useAnimation = 20;
					item.damage = 16;
					item.DamageType = DamageClass.Melee;
					item.autoReuse = false;
					item.consumable = false;
					item.maxStack = 1;
					item.value = Item.sellPrice(silver: 30);
					break;
				case ItemID.Javelin:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<Javelin>();
					altShoot = ProjectileType<JavelinThrow>();
					item.useTime = item.useAnimation = 18;
					item.DamageType = DamageClass.Melee;
					item.autoReuse = false;
					item.consumable = false;
					item.maxStack = 1;
					item.value = Item.sellPrice(silver: 30);
					break;
				case ItemID.ThunderSpear:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<StormSpear>();
					altShoot = ProjectileType<StormSpearThrow>();

					item.damage = 18;
					item.shootSpeed = 7; //only the throw uses this
					break;
				case ItemID.Trident:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<Trident>();
					altShoot = ProjectileType<TridentThrow>();

					item.shootSpeed = 9; //only the throw uses this
					break;
				case ItemID.DarkLance:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<DarkLance>();
					altShoot = ProjectileType<DarkLanceThrow>();

					item.shootSpeed = 11.5f; //only the throw uses this
					item.damage = 21;
					item.useTime = item.useAnimation = 26;
					item.value = Item.sellPrice(silver: 50);
					break;
				case ItemID.Swordfish:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<Swordfish>();
					altShoot = ProjectileType<SwordfishThrow>();
					break;
				case ItemID.ObsidianSwordfish:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<ObsidianSwordfish>();
					altShoot = ProjectileType<ObsidianSwordfishThrow>();

					item.damage = 30;
					item.shootSpeed = 10; //only the throw uses this
					break;
				case ItemID.CobaltNaginata:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<CobaltNaginata>();
					altShoot = ProjectileType<CobaltNaginataThrow>();

					item.damage = 30;
					item.useTime = item.useAnimation = 24;
					item.shootSpeed = 13; //only the throw uses this
					item.crit = 40;
					break;
				case ItemID.PalladiumPike:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<PalladiumPike>();
					altShoot = ProjectileType<PalladiumPikeThrow>();

					item.channel = true;
					item.damage = 45;
					break;
				case ItemID.MythrilHalberd:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<MythrilHalberd>();
					altShoot = ProjectileType<MythrilHalberdThrow>();

					item.damage = 49;
					item.channel = true;
					break;
				case ItemID.OrichalcumHalberd:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<OrichalcumHookbill>();
					altShoot = ProjectileType<OrichalcumHookbillThrow>();

					item.damage = 40;
					item.channel = true;
					item.SetNameOverride("Orichalcum Billhook");
					break;
				case ItemID.AdamantiteGlaive:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<AdamantiteGlaive>();
					altShoot = ProjectileType<AdamantiteGlaiveThrow>();

					item.damage = 50;
					item.shootSpeed = 10; //only the throw uses this
					break;
				case ItemID.TitaniumTrident:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<TitaniumTrident>();
					altShoot = ProjectileType<TitaniumTridentThrow>();

					item.damage = 52;
					item.shootSpeed = 14; //only the throw uses this
					item.useTime = item.useAnimation = 25;
					break;
				case ItemID.Gungnir:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<Gungnir>();
					altShoot = ProjectileType<GungnirThrow>();

					item.damage = 56;
					item.shootSpeed = 12; //only the throw uses this
					item.useAnimation = 22;
					item.useTime = 33;
					break;
				case ItemID.ChlorophytePartisan:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<ChloroPartisan>();
					altShoot = ProjectileType<ChloroPartisanThrow>();

					item.shootSpeed = 12f; //only the throw uses this
					item.useTime = item.useAnimation = 30;
					break;
				case ItemID.MonkStaffT2:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<GhastlyGlaive>();
					altShoot = ProjectileType<GhastlyGlaiveThrow>();

					item.damage = 70;
					item.shootSpeed = 12; //only the throw uses this
					item.channel = false;
					break;
				case ItemID.MushroomSpear:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<MushroomSpear>();
					altShoot = ProjectileType<MushroomSpearThrow>();

					item.shootSpeed = 8; //only the throw uses this
					item.damage = 100;
					item.useTime = item.useAnimation = 50;
					item.value = Item.buyPrice(platinum: 2);
					break;
				case ItemID.NorthPole:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<NorthPole>();
					altShoot = ProjectileType<NorthPoleThrow>();

					item.shootSpeed = 9; //only the throw uses this
					item.damage = 107;
					item.useTime = item.useAnimation = 24;
					break;
				case ItemID.ScourgeoftheCorruptor:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<SoTC>();
					altShoot = ProjectileType<SoTCThrow>();
					 
					item.damage = 70;
					item.useTime = item.useAnimation = 24;
					item.autoReuse = false;
					item.shootSpeed = 9;
					break;
				case ItemID.DayBreak:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<Daybreak>();
					altShoot = ProjectileType<DaybreakThrow>();
					item.damage = 94;
					item.useTime = item.useAnimation = 20;
					item.autoReuse = false;
					break;

			}
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (altShoot != -1)
            {
                return true;
            }
            return base.AltFunctionUse(item, player);
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (altShoot != -1)
            {
                if (player.altFunctionUse == 2)
                {
                    type = altShoot;
                    player.itemAnimation = player.itemAnimationMax = item.useTime / 3;
                }
				else if(item.type == ItemID.Trident)
                {
					type = ProjectileType<Trident>(); 
				}
            }
        }
        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if(canGetMeleeModifiers)
            {
				int num = 0;
				#region pick
				int num9 = rand.Next(40);
				if (num9 == 0)
				{
					num = 1;
				}
				if (num9 == 1)
				{
					num = 2;
				}
				if (num9 == 2)
				{
					num = 3;
				}
				if (num9 == 3)
				{
					num = 4;
				}
				if (num9 == 4)
				{
					num = 5;
				}
				if (num9 == 5)
				{
					num = 6;
				}
				if (num9 == 6)
				{
					num = 7;
				}
				if (num9 == 7)
				{
					num = 8;
				}
				if (num9 == 8)
				{
					num = 9;
				}
				if (num9 == 9)
				{
					num = 10;
				}
				if (num9 == 10)
				{
					num = 11;
				}
				if (num9 == 11)
				{
					num = 12;
				}
				if (num9 == 12)
				{
					num = 13;
				}
				if (num9 == 13)
				{
					num = 14;
				}
				if (num9 == 14)
				{
					num = 15;
				}
				if (num9 == 15)
				{
					num = 36;
				}
				if (num9 == 16)
				{
					num = 37;
				}
				if (num9 == 17)
				{
					num = 38;
				}
				if (num9 == 18)
				{
					num = 53;
				}
				if (num9 == 19)
				{
					num = 54;
				}
				if (num9 == 20)
				{
					num = 55;
				}
				if (num9 == 21)
				{
					num = 39;
				}
				if (num9 == 22)
				{
					num = 40;
				}
				if (num9 == 23)
				{
					num = 56;
				}
				if (num9 == 24)
				{
					num = 41;
				}
				if (num9 == 25)
				{
					num = 57;
				}
				if (num9 == 26)
				{
					num = 42;
				}
				if (num9 == 27)
				{
					num = 43;
				}
				if (num9 == 28)
				{
					num = 44;
				}
				if (num9 == 29)
				{
					num = 45;
				}
				if (num9 == 30)
				{
					num = 46;
				}
				if (num9 == 31)
				{
					num = 47;
				}
				if (num9 == 32)
				{
					num = 48;
				}
				if (num9 == 33)
				{
					num = 49;
				}
				if (num9 == 34)
				{
					num = 50;
				}
				if (num9 == 35)
				{
					num = 51;
				}
				if (num9 == 36)
				{
					num = 59;
				}
				if (num9 == 37)
				{
					num = 60;
				}
				if (num9 == 38)
				{
					num = 61;
				}
				if (num9 == 39)
				{
					num = 81;
				}
				#endregion
				return num;
            }
            return base.ChoosePrefix(item, rand);
        }
    }
}
