using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    // Classe mère de l'ensemble des objets animés du jeux Pac-man
    public class ObjetAnime
    {
        //Texture de l'objet
        private List<Texture2D> texture;
        
        public ObjetAnime(List<Texture2D> texture)
        {
            this.texture = texture;
        }

        public List<Texture2D> Texture
        {
            get { return texture; }
            set { texture = value; }
        }
    }
}
