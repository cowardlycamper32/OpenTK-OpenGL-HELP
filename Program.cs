using TestOne;

namespace OpenTK_tutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "test"))
            {
                game.Run();
            }
        }
    }
}

