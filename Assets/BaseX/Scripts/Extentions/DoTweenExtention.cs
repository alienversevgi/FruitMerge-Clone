using DG.Tweening;
using UnityEngine;

namespace BaseX.Extentions
{
    public static class DoTweenExtention
    {
        public static Sequence DoJumpZ(
            this Transform target,
            Vector3 endValue,
            float jumpPower,
            int numJumps,
            float duration,
            bool snapping = false)
        {
            if (numJumps < 1)
                numJumps = 1;

            float startPosZ = target.position.z; // Z-axis instead of Y-axis
            float offsetZ = -1f;
            bool offsetZSet = false;

            Sequence s = DOTween.Sequence();

            // Tween for Z-axis jumping
            Tween zTween = DOTween.To(
                    () => target.position,
                    x => target.position = x,
                    new Vector3(0.0f, 0.0f, jumpPower), // Z-axis jump power
                    duration / (numJumps * 2))
                .SetOptions(AxisConstraint.Z, snapping)
                .SetEase(Ease.OutQuad)
                .SetRelative()
                .SetLoops(numJumps * 2, LoopType.Yoyo)
                .OnStart(() => startPosZ = target.position.z);

            // Horizontal movement on X and Y axes, and joining with Z-axis jump
            s.Append(DOTween.To(
                        () => target.position,
                        x => target.position = x,
                        new Vector3(endValue.x, endValue.y, 0.0f), // Move on X and Y axes
                        duration)
                    .SetOptions(AxisConstraint.X | AxisConstraint.Y, snapping)
                    .SetEase(Ease.Linear))
                .Join(zTween)
                .SetTarget(target)
                .SetEase(DOTween.defaultEaseType);

            // Adjust the Z-axis based on the jump progress
            zTween.OnUpdate(() =>
            {
                if (!offsetZSet)
                {
                    offsetZSet = true;
                    offsetZ = s.isRelative ? endValue.z : endValue.z - startPosZ;
                }

                Vector3 position = target.position;
                position.z += DOVirtual.EasedValue(0.0f, offsetZ, zTween.ElapsedPercentage(), Ease.OutQuad);
                target.position = position;
            });

            return s;
        }
        
        public static Sequence DoLocalJumpZ(
            this Transform target,
            Vector3 endValue,
            float jumpPower,
            int numJumps,
            float duration,
            bool snapping = false)
        {
            if (numJumps < 1)
                numJumps = 1;

            float startPosZ = target.localPosition.z; // Z-axis instead of Y-axis
            float offsetZ = -1f;
            bool offsetZSet = false;

            Sequence s = DOTween.Sequence();

            // Tween for Z-axis jumping
            Tween zTween = DOTween.To(
                    () => target.localPosition,
                    x => target.localPosition = x,
                    new Vector3(0.0f, 0.0f, jumpPower), // Z-axis jump power
                    duration / (numJumps * 2))
                .SetOptions(AxisConstraint.Z, snapping)
                .SetEase(Ease.OutQuad)
                .SetRelative()
                .SetLoops(numJumps * 2, LoopType.Yoyo)
                .OnStart(() => startPosZ = target.localPosition.z);

            // Horizontal movement on X and Y axes, and joining with Z-axis jump
            s.Append(DOTween.To(
                        () => target.localPosition,
                        x => target.localPosition = x,
                        new Vector3(endValue.x, endValue.y, 0.0f), // Move on X and Y axes
                        duration)
                    .SetOptions(AxisConstraint.X | AxisConstraint.Y, snapping)
                    .SetEase(Ease.Linear))
                .Join(zTween)
                .SetTarget(target)
                .SetEase(DOTween.defaultEaseType);

            // Adjust the Z-axis based on the jump progress
            zTween.OnUpdate(() =>
            {
                if (!offsetZSet)
                {
                    offsetZSet = true;
                    offsetZ = s.isRelative ? endValue.z : endValue.z - startPosZ;
                }

                Vector3 position = target.localPosition;
                position.z += DOVirtual.EasedValue(0.0f, offsetZ, zTween.ElapsedPercentage(), Ease.OutQuad);
                target.localPosition = position;
            });

            return s;
        }
    }
}