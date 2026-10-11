using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>The rainbow "?" box: drive through it or hit it to get a random kart item.</summary>
public class ItemBox : ModNPC
{
    public override void SetDefaults()
    {
        NPC.width = 34; NPC.height = 36;
        NPC.lifeMax = 1; NPC.damage = 0; NPC.defense = 0; NPC.knockBackResist = 0f;
        NPC.aiStyle = -1; NPC.noGravity = true; NPC.noTileCollide = true; NPC.dontCountMe = true;
        NPC.HitSound = SoundID.Item4;
    }

    public override void AI()
    {
        NPC.ai[0]++;
        NPC.velocity = new Vector2(0f, (float)System.Math.Sin(NPC.ai[0] * 0.06f) * 0.5f);
        NPC.rotation = (float)System.Math.Sin(NPC.ai[0] * 0.04f) * 0.12f;
        NPC.timeLeft = 600;
        Color c = Main.hslToRgb((NPC.ai[0] * 0.01f) % 1f, 1f, 0.6f);
        Mix.Light(NPC.Center, c);
        if (Main.rand.NextBool(6)) {
            Dust d = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Circular(20f, 20f), DustID.RainbowMk2,
                new Vector2(0f, -0.6f), 0, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.6f), 1.1f);
            d.noGravity = true;
        }
    }

    public override Color? GetAlpha(Color drawColor) => Color.White;

    public override void OnKill()
    {
        Mix.Sound("itemget", NPC.Center);
        for (int i = 0; i < 30; i++) {
            Dust d = Dust.NewDustPerfect(NPC.Center, DustID.RainbowMk2, Main.rand.NextVector2Circular(6f, 6f), 0,
                Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.6f), 1.5f);
            d.noGravity = true;
        }
        Player p = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)];
        int[] pool = { ModContent.ItemType<RedShell>(), ModContent.ItemType<BananaPeel>(), ModContent.ItemType<TurboCan>() };
        int type = Main.rand.Next(pool);
        Mix.Give(type, 1, p);
        Mix.Popup(NPC.Top, Lang.GetItemNameValue(type) + "!", Color.Cyan);
        if (type == pool[2]) p.AddBuff(ModContent.BuffType<TurboBuff>(), 300);
        else Mix.Arm(type);
    }
}
