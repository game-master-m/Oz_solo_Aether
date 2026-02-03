using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Aether.Components;

namespace Aether.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class PlayerInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float horizontal = 0f;
            float vertical = 0f;
            bool isGathering = false;

            if (UnityEngine.InputSystem.Keyboard.current != null)
            {
                var kb = UnityEngine.InputSystem.Keyboard.current;
                if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) horizontal -= 1f;
                if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) horizontal += 1f;
                if (kb.wKey.isPressed || kb.upArrowKey.isPressed) vertical += 1f;
                if (kb.sKey.isPressed || kb.downArrowKey.isPressed) vertical -= 1f;
                
                isGathering = kb.spaceKey.isPressed;
            }

            float2 input = new float2(horizontal, vertical);

            if (math.lengthsq(input) > 1f)
            {
                input = math.normalize(input);
            }

            foreach (var (playerInput, tag) in SystemAPI.Query<RefRW<PlayerInput>, RefRO<PlayerTag>>())
            {
                playerInput.ValueRW.MoveInput = input;
                playerInput.ValueRW.IsGathering = isGathering;
            }
        }
    }
}
