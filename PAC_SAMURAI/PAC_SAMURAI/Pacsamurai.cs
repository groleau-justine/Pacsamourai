using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    public class Pacsamurai : ObjetAnime
    {
        private int sonX;
        private int sonY;
        private int sesVies;
        private int score;
        private Map map;
        private Texture2D texture;
        private Timers timer;
        private Boolean invincible;

        public Boolean Invincible
        {
            get { return invincible; }
            set { invincible = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        private int numTexture;

        public int NumTexture
        {
            get { return numTexture; }
            set { numTexture = value; }
        }

        public int SonX
        {
            get { return sonX; }
        }

        public int SonY
        {
            get { return sonY; }
            set { sonY = value; }
        }

        public int Vies
        {
            get { return sesVies; }
            set { sesVies = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }


        public Pacsamurai(List<Texture2D> listeTextures, List<Texture2D> listeTexturesInvincible, Map map) : base(listeTextures, listeTexturesInvincible)
        {
            this.score = 0;
            this.sesVies = 3;
            this.map = map;
            this.numTexture = 1;
            this.texture = base.ListeTextures[1];
            for (int ligneX = 0; ligneX < map.MaxMapX; ligneX++)
            {
                for (int ligneY = 0; ligneY < map.MaxMapY; ligneY++)
                {
                    if (map.MapGame[ligneX, ligneY] == 'P')
                    {
                        sonX = ligneX;
                        sonY = ligneY;
                    }
                }
            }
            this.timer = new Timers(map);
            this.invincible = false;
        }

        /** Méthode qui modifie le X de pacsamurai (la ligne où il se trouve)
         *  et modifie sa texture suivant la direction désirée et l'action
         **/ 
        public void setSonX(int unX, char caseMap, List<Texture2D> listeTextures)
        {
            this.sonX = sonX + unX;
            
            //Si le pacsamurai va vers le haut
            if (unX == -1)
            {
                //Si il mange un sushi
                if (caseMap == 'S')
                {
                    //On change l'image par un sabre dégainé
                    texture = listeTextures[4];
                    numTexture = 4;
                }
                else
                {
                    texture = listeTextures[0];
                    numTexture = 0;
                }               
            }
            //Si le pacsamurai va vers le bas
            else if (unX == 1)
            {
                //Si il mange un sushi
                if (caseMap == 'S')
                {
                    //On change l'image par un sabre dégainé
                    texture = listeTextures[6];
                    numTexture = 6;
                }
                else
                {
                    texture = listeTextures[2];
                    numTexture = 2;
                }
            }
        }

        /** Méthode qui modifie le Y de pacsamurai (la colonne où il se trouve)
         *  et modifie sa texture suivant la direction désirée et l'action 
         **/ 
        public void setSonY(int unY, char caseMap, List<Texture2D> listeTextures)
        {
            this.sonY = sonY + unY;

            //Si le pacsamurai va vers la gauche
            if (unY == -1)
            {
                //Si il mange un sushi
                if (caseMap == 'S')
                {
                    //On change l'image par un sabre dégainé
                    texture = listeTextures[7];
                    numTexture = 7;
                }
                else
                {
                    texture = listeTextures[3];
                    numTexture = 3;
                }              
            }
            //Si le pacsamurai va vers la droite
            else if (unY == 1)
            {
                //Si il mange un sushi
                if (caseMap == 'S')
                {
                    //On change l'image par un sabre dégainé
                    texture = listeTextures[5];
                    numTexture = 5;
                }
                else
                {
                    texture = listeTextures[1];
                    numTexture = 1;
                }              
            }
        }

        /**
         * Méthode permettant de gérer la position du pacsamurai
         */
        public void setPosition(int x, int y, GameTime gameTime)
        {
            if (positionCoherente(sonX + x, sonY + y, gameTime) == true && map.MapGame[sonX + x, sonY + y] != '0')
            {
                char caseMap = map.MapGame[sonX + x, sonY + y];
                List<Texture2D> listeTextures;
    
                //Met l'ancienne position du pacman à 1 (endroit ou il peut se déplacer)
                map.MapGame[sonX, sonY] = '1';
                              
                /*** Gestion du score incrémenté par 10 ****/
                if (caseMap == 'S')
                {
                    score+=10;
                }

                /*** Gestion du pouvoir invincible déblanché par les makis ***/
                // Si le pacsamurai se dirige vers un maki
                if (caseMap == 'M')
                {
                    //On lance le timer du mode invincible
                    invincible = timer.gererTimerInvincible(gameTime);
                }
                if (invincible == true)
                {
                    listeTextures = base.ListeTexturesInvincible;
                }
                else
                {
                    listeTextures = base.ListeTextures;
                }

                /*** Gestion des bonus ***/
                if (caseMap == 'B')
                {
                    score += 100;
                }

                //On modifie ses coordonnées
                setSonX(x, caseMap, listeTextures);
                setSonY(y, caseMap, listeTextures);              

                //On déplace Pacsamourai à ses nouvelles coordonnées
                map.MapGame[sonX, sonY] = 'P';
            }
        }

        // Vérifie si la position du pacsamurai est bien dans la map
        public Boolean positionCoherente(int x, int y, GameTime gameTime)
        {
            Boolean ok=false;
            if (x >= 0 && x <= map.MaxMapX && y >= 0)
            {
                ok=true;
            }

            /*** Gestion des cas particuliés  ***/

            // Quand le pacsamurai se déplace le plus à droite 
            // et se retrouve à gauche de la map (passage de la postion y=20 à y=0)
            if (y >= map.MaxMapY)
            {
                setPosition(0, -sonY, gameTime);
            }
            // Quand le pacsamurai se déplace le plus à gauche 
            // et se retrouve à droite de la map (passage de la postion y=0 à y=20)
            if (y == -1)
            {
                setPosition(0, map.MaxMapY - 1, gameTime);
            }
            return ok;
        }

    }
}
