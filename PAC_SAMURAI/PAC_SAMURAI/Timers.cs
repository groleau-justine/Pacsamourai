using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    class Timers
    {
        private float timer;
        private float timerBonus;
        private Boolean bonus;
        private Map map;

        public Timers(Map map)
        {
            timer = 0f;
            timerBonus = 0f;
            bonus = false;
            this.map = map;
        }

        /**
            * Fonction gérant le timer pour le mode invincible contre les fantômes
            * Elle incrémente le timer et renvoie un boolean pour indiquer s'il 
            * est écoulé ou pas
            */
        public Boolean gererTimerInvincible(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timer += timeElapsed;
            if (timer > 5000)
            {
                timer = 0f;
                return false;
            }
            else
            {
                return true;
            }
        }

        /**
         * Fonction gérant le timer pour les bonus
         * Elle incrémente le timer et renvoie un boolean pour indiquer s'il 
         * est écoulé ou pas
         */
        public void lancerTimerBonus(GameTime gameTime)
        {
            //Initialiation du générateur de nb aléatoires pour les bonus
            Random aleatoire = new Random((int)gameTime.TotalGameTime.TotalMilliseconds);

            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timerBonus += timeElapsed;

            if (timerBonus > 5000)
            {
                timerBonus = 0f;
                if (bonus)
                {
                    map.removeBonus();
                    bonus = false;
                }
                else
                {
                    map.drawBonus(aleatoire);
                    bonus = true;
                }
            }
        }
    }
}
