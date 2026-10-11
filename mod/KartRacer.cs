using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>A goblin in a red go-kart that races straight at the player, honking.</summary>
public class KartRacer : ModNPC
{
    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 2;

    public override void SetDefaults()
    {
        NPC.width = 72; NPC.height = 50;
        NPC.lifeMax = 150; NPC.damage = 18; NPC.defense = 2; NPC.knockBackResist = 0.6f; NPC.value = 120;
        NPC.aiStyle = NPCAIStyleID.Fighter; AIType = NPCID.Zombie;
        NPC.HitSound = SoundID.NPCHit1; NPC.DeathSound = SoundID.NPCDeath1;
    }

    public override void AI()
    {
        NPC.TargetClosest(true);
        var g = NPC.GetGlobalNPC<SpinOut>();
        if (g.Spin <= 0)
            NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, NPC.direction * 5.2f, 0.12f);

        if (System.Math.Abs(NPC.velocity.X) > 2.5f && NPC.velocity.Y == 0f && Main.rand.NextBool(2)) {
            Vector2 tail = NPC.Center + new Vector2(-NPC.direction * 38f, 16f);
            Dust s = Dust.NewDustPerfect(tail, DustID.Smoke, new Vector2(-NPC.direction * 1.5f, -0.5f), 100, Color.Gray, 1.3f);
            s.noGravity = true;
            if (Main.rand.NextBool(3)) {
                Dust f = Dust.NewDustPerfect(tail, DustID.Torch, new Vector2(-NPC.direction * 2.5f, 0f), 0, default, 1.2f);
                f.noGravity = true;
            }
        }
        NPC.ai[3]++;
        if (NPC.ai[3] > 180 && Main.rand.NextBool(120) && Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) < 900f) {
            NPC.ai[3] = 0;
            Mix.Sound("honk", NPC.Center, 0.7f, Main.rand.NextFloat(-0.2f, 0.3f));
        }
    }

    public override void FindFrame(int frameHeight)
    {
        NPC.spriteDirection = NPC.direction;
        NPC.frameCounter += 1 + System.Math.Abs(NPC.velocity.X);
        NPC.frame.Y = (int)(NPC.frameCounter / 8 % 2) * frameHeight;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        for (int i = 0; i < 8; i++) {
            Dust d = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Circular(24f, 16f), DustID.Torch,
                Main.rand.NextVector2Circular(4f, 4f), 0, default, 1.4f);
            d.noGravity = true;
        }
        if (NPC.life > 0) { if (hit.Damage > 12 && NPC.GetGlobalNPC<SpinOut>().Spin <= 0) SpinOut.Apply(NPC, 30); return; }
        int[] confetti = { DustID.Confetti, DustID.Confetti_Blue, DustID.Confetti_Green, DustID.Confetti_Pink, DustID.Confetti_Yellow };
        for (int i = 0; i < 45; i++) {
            Dust d = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Circular(24f, 16f), Main.rand.Next(confetti),
                Main.rand.NextVector2Circular(6f, 6f) + new Vector2(0, -2f), 0, default, 1.5f);
            d.noGravity = false;
        }
        Mix.Burst(NPC.Center, DustID.Smoke, 15, 24f, 4f);
        Mix.Sound(SoundID.Item14, NPC.Center);
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BananaPeel>(), 4));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TurboCan>(), 6));
    }
}
