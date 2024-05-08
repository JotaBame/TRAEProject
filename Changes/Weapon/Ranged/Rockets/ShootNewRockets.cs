using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes.Weapon.Ranged.Rockets;
using TRAEProject.NewContent.Items.Weapons.Ranged.Launchers.SkullCannon;
using TRAEProject.NewContent.Items.Weapons.Ranged.Launchers.T3Launcher;
using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;
namespace TRAEProject.Changes.Weapons.Rockets
{
    public class ShootNewRockets : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
        {
            if (weapon.type == ItemID.RocketLauncher || weapon.type == ItemType<SkullCannon>() || weapon.type == ItemType<AdamantiteLauncher>() || weapon.type == ItemType<TitaniumBazooka>())
            {
                switch (ammo.type)
                {
                    case ItemID.RocketI:
                        type = ProjectileType<Rocket>();
                        break;
                    case ItemID.RocketII:
                        type = ProjectileType<DestructiveRocket>();
                        break;
                    case ItemID.RocketIII:
                        type = ProjectileType<SuperRocket>();
                        break;
                    case ItemID.RocketIV:
                        type = ProjectileType<DirectRocket>();
                        break;
                    case ItemID.MiniNukeI:
                        type = ProjectileType<MiniNuke>();
                        break;
                    case ItemID.MiniNukeII:
                        type = ProjectileType<DestructiveMiniNuke>();
                        break;
                    case ItemID.ClusterRocketI:
                        type = ProjectileType<ClusterRocket>();
                        break;
                    case ItemID.ClusterRocketII:
                        type = ProjectileType<HeavyRocket>();
                        break;
                }
                if (ammo.type == ItemType<LuminiteRocket>())
                {
                    type = ProjectileType<LuminiteRocketP>();
                }
            }
            if (weapon.type == ItemID.GrenadeLauncher)
            {
                switch (ammo.type)
                {
                    case ItemID.RocketI:
                        type = ProjectileType<Grenade>();
                        break;
                    case ItemID.RocketII:
                        type = ProjectileType<DestructiveGrenade>();
                        break;
                    case ItemID.RocketIII:
                        type = ProjectileType<SuperGrenade>();
                        break;
                    case ItemID.RocketIV:
                        type = ProjectileType<DirectGrenade>();
                        break;
                    case ItemID.MiniNukeI:
                        type = ProjectileType<MiniNukeGrenade>();
                        break;
                    case ItemID.MiniNukeII:
                        type = ProjectileType<DestructiveMiniNukeGrenade>();
                        break;
                    case ItemID.ClusterRocketI:
                        type = ProjectileType<ClusterGrenade>();
                        break;
                    case ItemID.ClusterRocketII:
                        type = ProjectileType<HeavyGrenade>();
                        break;
                }
                if (ammo.type == ItemType<LuminiteRocket>())
                {
                    type = ProjectileType<LuminiteGrenade>();
                }
            }
            if (weapon.type == ItemID.SnowmanCannon)
            {
                switch (ammo.type)
                {
                    case ItemID.RocketI:
                        type = ProjectileType<SnowmanRocket>();
                        break;
                    case ItemID.RocketII:
                        type = ProjectileType<DestructiveSnowmanRocket>();
                        break;
                    case ItemID.RocketIII:
                        type = ProjectileType<SuperSnowmanRocket>();
                        break;
                    case ItemID.RocketIV:
                        type = ProjectileType<DirectSnowmanRocket>();
                        break;
                    case ItemID.MiniNukeI:
                        type = ProjectileType<MiniNukeSnowman>();
                        break;
                    case ItemID.MiniNukeII:
                        type = ProjectileType<DestructiveMiniNukeSnowman>();
                        break;
                    case ItemID.ClusterRocketI:
                        type = ProjectileType<ClusterSnowmanRocket>();
                        break;
                    case ItemID.ClusterRocketII:
                        type = ProjectileType<HeavySnowmanRocket>();
                        break;
                }
                if (ammo.type == ItemType<LuminiteRocket>())
                {
                    type = ProjectileType<LuminiteSnowmanRocket>();
                }
            }
            if (weapon.type == ItemID.ProximityMineLauncher)
            {
                switch (ammo.type)
                {
                    case ItemID.RocketI:
                        type = ProjectileType<Mine>();
                        break;
                    case ItemID.RocketII:
                        type = ProjectileType<DestructiveMine>();
                        break;
                    case ItemID.RocketIII:
                        type = ProjectileType<SuperMine>();
                        break;
                    case ItemID.RocketIV:
                        type = ProjectileType<DirectMine>();
                        break;
                    case ItemID.MiniNukeI:
                        type = ProjectileType<MiniNukeMine>();
                        break;
                    case ItemID.MiniNukeII:
                        type = ProjectileType<DestructiveMiniNukeMine>();
                        break;
                    case ItemID.ClusterRocketI:
                        type = ProjectileType<ClusterMine>();
                        break;
                    case ItemID.ClusterRocketII:
                        type = ProjectileType<HeavyMine>();
                        break;
                }
                if (ammo.type == ItemType<LuminiteRocket>())
                {
                    type = ProjectileType<LuminiteMine>();
                }
            }

            switch (ammo.type)
            {
                case ItemID.StyngerBolt:
                    type = ProjectileType<TRAEProject.NewContent.Projectiles.Stynger>();
                    return;
            }

        }
    }
}