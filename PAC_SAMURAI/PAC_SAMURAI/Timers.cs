using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    public class Timers
    {
        private float timer;
        private float timerMortPac;
        private float timerBonus;
        private float timerFantome;
        private float timerPeur;
        private Boolean bonus;
        private Map map;

        public Timers(Map map)
        {
            timer = 0f;
            timerMortPac = 0f;
            timerBonus = 0f;
            timerFantome = 0f;
            timerPeur = 0f;
            bonus = false;
            this.map = map;
        }

        /**
         * Fonction gérant le timer pour le mode invincible contre les fantômes
         * Elle incrémente le timer et renvoie un boolean pour indiquer s'il 
         * est écoulé ou pas.
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
         * est écoulé ou pas.
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

        /**
         * Fonction gérant le timer pour les fantômes
         * Elle incrémente le timer et renvoie un boolean pour indiquer s'il 
         * est écoulé ou pas.
         */
        public Boolean lancerTimerFantome(GameTime gameTime, int better)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timerFantome += timeElapsed;

            if (timerFantome > better)
            {
                timerFantome = 0f;
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * Fonction gérant le timer pour le mode peur des fantômes
         * Elle incrémente le timer et renvoie un boolean pour indiquer s'il 
         * est écoulé ou pas.
         */
        public Boolean gererTimerPeur(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timerPeur += timeElapsed;

            if (timerPeur > 5000)
            {
                timerPeur = 0f;
                return false;
            }
            else
            {
                timerFantome = 0f;
                return true;
            }
        }

        /**
         * Fonction gérant le timer pour le mode mort du pacman
         * Elle incrémente le timer et renvoie un boolean pour indiquer s'il 
         * est écoulé ou pas.
         */
        public Boolean gererTimerMort(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            timerMortPac += timeElapsed;

            if (timerMortPac > 5000)
            {
                timerMortPac = 0f;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
