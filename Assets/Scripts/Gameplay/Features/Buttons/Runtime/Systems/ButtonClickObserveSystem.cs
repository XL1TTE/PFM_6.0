
using Domain.CursorDetection.Components;
using Domain.UI.Requests;
using Domain.UI.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace UI.Buttons
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ButtonClickObserveSystem : ISystem
    {
        public World World { get; set; }

        private Filter _buttonsUnderCursor;

        private Event<ButtonClickedEvent> evt_btnClicked;

        private Stash<ButtonTag> stash_btnTag;

        public void OnAwake()
        {
            _buttonsUnderCursor = World.Filter
                .With<ButtonTag>()
                .With<UnderCursorComponent>()
                .Build();

            evt_btnClicked = World.GetEvent<ButtonClickedEvent>();
            stash_btnTag = World.GetStash<ButtonTag>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_buttonsUnderCursor.IsEmpty()) { return; }

            var clickedButton = _buttonsUnderCursor.First();
            if (stash_btnTag.Get(clickedButton).state == ButtonTag.State.Disabled) { return; }

            if (Input.GetMouseButtonDown(0))
            {
                evt_btnClicked.NextFrame(new ButtonClickedEvent
                {
                    ClickedButton = clickedButton
                });
            }
        }

        public void Dispose()
        {

        }
    }
}


