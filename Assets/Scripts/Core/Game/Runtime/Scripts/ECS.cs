using Scellecs.Morpeh;

namespace Game
{

    public static partial class ECS
    {
        public const int SCENE_ENTRY_POINT = -1000;
        public static World m_CurrentWorld = World.Default;

        public static void Reset() => m_CurrentWorld = World.Default;
    }
}
