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
        //Textures de l'objet
        private List<Texture2D> listeTextures = new List<Texture2D>();
        private List<Texture2D> listeTexturesInvincible = new List<Texture2D>();
     
        public ObjetAnime(List<Texture2D> listeTextures, List<Texture2D> listeTexturesInvincible)
        {
            this.listeTextures = listeTextures;
            this.listeTexturesInvincible = listeTexturesInvincible;
        }

        public List<Texture2D> ListeTextures
        {
            get { return listeTextures; }
            set { listeTextures = value; }
        }

        public List<Texture2D> ListeTexturesInvincible
        {
            get { return listeTexturesInvincible; }
            set { listeTexturesInvincible = value; }
        }
    }
}
