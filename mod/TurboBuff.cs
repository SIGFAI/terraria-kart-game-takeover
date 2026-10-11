using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class TurboBuff : ModBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.maxRunSpeed = 13f; player.accRunSpeed = 13f; player.runAcceleration *= 3f;
        Vector2 tail = player.Bottom + new Vector2(-player.direction * 26f, -8f);
        for (int i = 0; i < 2; i++) {
            Dust d = Dust.NewDustPerfect(tail, i == 0 ? DustID.Torch : DustID.Flare, new Vector2(-player.direction * Main.rand.NextFloat(2f, 6f), Main.rand.NextFloat(-1f, 1f)), 0, default, 1.8f);
            d.noGravity = true;
        }
        Mix.Light(player.Center, new Color(255, 140, 40));
    }
}
