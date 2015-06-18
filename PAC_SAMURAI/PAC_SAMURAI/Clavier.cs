using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace PAC_SAMURAI
{
    /**
     * Classe permettant de gérer les événements clavier
     */
    class Clavier
    {
        private Pacsamurai pacsamurai;
        private KeyboardState oldState;               
        private Map map;

        public Clavier(Pacsamurai pacsamurai, KeyboardState oldState, Map map)
        {
            this.pacsamurai = pacsamurai;
            this.oldState = oldState;
            this.map = map;
        }

        public void gererDeplacement(int x, int y, GameTime gameTime)
        {           
            pacsamurai.setPosition(x, y, gameTime);
        }

        public void update(GameTime gameTime)
        {                           
            //Récupération de l'état du clavier
            KeyboardState newState = Keyboard.GetState();

            //Changement de direction suivant la touche appuyée

            //Si la touche flèche du haut a été appuyé
            if (newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))
            {
                //On va en haut
                int x = -1, y = 0;
                gererDeplacement(x, y, gameTime);  
            }

            //Si la touche flèche du bas a été appuyé
            if (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
            {
                //On va en bas
                int x = 1, y = 0;
                gererDeplacement(x, y, gameTime);  
            }

            //Si la touche flèche de gauche a été appuyé
            if (newState.IsKeyDown(Keys.Left) && !oldState.IsKeyDown(Keys.Left))
            {
                //On va à gauche
                int x = 0, y = -1;
                gererDeplacement(x, y, gameTime); 
            }

            //Si la touche flèche de droite a été appuyé
            if (newState.IsKeyDown(Keys.Right) && !oldState.IsKeyDown(Keys.Right))
            {
                //On va à droite
                int x = 0, y = 1;
                gererDeplacement(x, y, gameTime); 
            }

            oldState = newState;
        }
    }
}
