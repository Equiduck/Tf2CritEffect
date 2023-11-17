using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace TF2HitSystem
{
    public class TF2CriticalEffect : ModPlayer
    {
        public static SoundStyle CritSound;

        public override void Load()
        {
            if (!Main.dedServ) // Only load the sound if not on a dedicated server
            {
                CritSound = new SoundStyle("Tf2CritEffect/Sounds/tf2-crit-sound")
                {
                    Volume = 0.8f,
                    PitchVariance = 0.0f,
                    MaxInstances = 20
                };
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                // Change combat text to green and append "beep beep sheep"
                int index = -1;
                for (int i = 99; i >= 0; --i)
                {
                    if (Main.combatText[i].lifeTime == 120)
                    {
                        index = i;
                        break;
                    }
                }

                if (index != -1)
                {
                    Main.combatText[index].text = "Critical Hit!";
                    Main.combatText[index].color = new Color(50, 205, 50); // RGB for green color
                }

                // Stop the default hit sound
                SoundStyle hitSound = target.HitSound.Value;
                SoundEngine.FindActiveSound(hitSound)?.Stop();

                // Play the custom critical hit sound at the target's position
                SoundEngine.PlaySound(CritSound, target.Center);
            }

            // Call the base method
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}
