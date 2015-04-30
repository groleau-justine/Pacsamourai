using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    class Objet
    {
        const int tailleWidth = 32;
        const int tailleHeight = 32;

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
