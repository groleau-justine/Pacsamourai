using System;

namespace PAC_SAMURAI
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (Pacsamourai game = new Pacsamourai())
            {
                game.Run();
            }
        }
    }
#endif
}

