// using Domain.UI.Components;
// using Scellecs.Morpeh;
// using Unity.IL2CPP.CompilerServices;

// namespace UI.Systems
// {
//     [Il2CppSetOption(Option.NullChecks, false)]
//     [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
//     [Il2CppSetOption(Option.DivideByZeroChecks, false)]
//     public sealed class FpsShowSystem : ISystem
//     {
//         public World World { get; set; }

//         private Filter filter_uiRefs;
//         private Stash<SharedUIRefsComponent> stash_uiRefs;

//         private int Frames = 0;
//         private float Time = 0;


//         public void OnAwake()
//         {
//             filter_uiRefs = World.Filter.With<SharedUIRefsComponent>().Build();

//             stash_uiRefs = World.GetStash<SharedUIRefsComponent>();
//         }

//         public void OnUpdate(float deltaTime)
//         {
//             if (filter_uiRefs.IsEmpty()) { return; }

//             Frames++;
//             Time += deltaTime;
//             if (Time >= 1.0f)
//             {
//                 ref var uiRefs = ref stash_uiRefs.Get(filter_uiRefs.First());
//                 uiRefs.FpsCounter.TextMesh.text = Frames.ToString();
//                 Frames = 0;
//                 Time = 0;
//             }
//         }

//         public void Dispose()
//         {

//         }
//     }
// }


