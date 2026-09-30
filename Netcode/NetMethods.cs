using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TRAEProject.NewContent.Structures.Echosphere;

namespace TRAEProject.Netcode
{
    public enum ModPacketType : byte
    {
        RequestEchospherePositionData = 0,
        SendEchospherePositionData = 1,
    }
    public static class NetMethods
    {
        public static void SendRequestEchospherePositionData()
        {
            ModPacket packet = GetPacket();
            packet.Write((byte)ModPacketType.RequestEchospherePositionData);
            packet.Send();
        }
        public static void ReadRequestEchospherePositionData(BinaryReader reader, int whoAmI)
        {
            ModPacket packet = GetPacket();
            packet.Write((byte)ModPacketType.SendEchospherePositionData);
            packet.WriteVector2(EchosphereGeneratorSystem.echosphereTopLeft);
            packet.WriteVector2(EchosphereGeneratorSystem.echosphereBottomRight);
            packet.Send(whoAmI);
        }

        public static void ReadSendEchospherePositionData(BinaryReader reader)
        {
            Vector2 topLeft = reader.ReadVector2();
            Vector2 bottomRight = reader.ReadVector2();
            EchosphereGeneratorSystem.echosphereTopLeft = topLeft;
            EchosphereGeneratorSystem.echosphereBottomRight = bottomRight;
            EchosphereGeneratorSystem.echosphereCenter = (topLeft + bottomRight) * .5f;
        }

        static ModPacket GetPacket()
        {
            return TRAEProj.Instance.GetPacket();
        }

        
    }
}
