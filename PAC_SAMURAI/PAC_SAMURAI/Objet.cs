using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    // Classe mère de l'ensemble des objets du jeux Pac-man
    public class Objet
    {
        //Texture de l'objet
        private Texture2D texture;

        public Objet(Texture2D texture)
        {
            this.texture = texture;
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

    }
}
