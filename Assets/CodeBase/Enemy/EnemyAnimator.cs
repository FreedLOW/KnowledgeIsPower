using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Win = Animator.StringToHash("Win");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Attack = Animator.StringToHash("Attack_1");

        private readonly int idleStateHash = Animator.StringToHash("idle");
        private readonly int attackStateHash = Animator.StringToHash("attack01");
        private readonly int walkStateHash = Animator.StringToHash("Move");
        private readonly int deathStateHash = Animator.StringToHash("die");

        private Animator animator;

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public AnimatorState State { get; private set; }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void DeathAnimation() => animator.SetTrigger(Die);
        
        public void HitAnimation() => animator.SetTrigger(Hit);
        
        public void WinAnimation() => animator.SetTrigger(Win);

        public void Move(float speed)
        {
            animator.SetBool(IsMoving, true);
            animator.SetFloat(Speed, speed);
        }

        public void StopMoving() => animator.SetBool(IsMoving, false);

        public void PlayAttack() => animator.SetTrigger(Attack);

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            State = StateFor(stateHash);
            StateExited?.Invoke(State);
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            
            if (stateHash == idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == walkStateHash)
                state = AnimatorState.Walking;
            else if (stateHash == attackStateHash)
                state = AnimatorState.Attack;
            else if (stateHash == deathStateHash)
                state = AnimatorState.Died;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}