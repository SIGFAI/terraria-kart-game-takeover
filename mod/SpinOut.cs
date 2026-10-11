using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Any enemy hit by a shell, a banana or a kart crash spins out like in a kart race.</summary>
public class SpinOut : GlobalNPC
{
    public override bool InstancePerEntity => true;
    public int Spin;

    public static void Apply(NPC n, int ticks = 50)
    {
        if (n == null || !n.active || n.boss) return;
        var g = n.GetGlobalNPC<SpinOut>();
        bool fresh = g.Spin <= 0;
        g.Spin = System.Math.Max(g.Spin, ticks);
        if (!fresh) return;
        n.velocity.Y = -6f;
        Mix.Sound("spinout", n.Center, 0.7f);
        Mix.Burst(n.Center, DustID.GoldCoin, 12, 20f, 3f);
        Mix.Popup(n.Top, "SPIN OUT!", Color.Orange);
    }

    public override void PostAI(NPC npc)
    {
        if (Spin <= 0) return;
        Spin--;
        npc.rotation += 0.5f * (npc.direction == 0 ? 1 : npc.direction);
        npc.velocity.X *= 0.97f;
        if (Spin % 3 == 0) {
            Dust d = Dust.NewDustPerfect(npc.Center + Main.rand.NextVector2Circular(npc.width / 2f, npc.height / 2f), DustID.Smoke,
                Main.rand.NextVector2Circular(2f, 2f), 120, Color.White, 1.4f);
            d.noGravity = true;
        }
        if (Spin == 0) npc.rotation = 0f;
    }
}
