using System.Collections;
using UnityEngine;

using static DashAttack.Gameplay.Behaviours.Enums.RunState;
using static DashAttack.Utilities.StateMachine.StateEvent;

namespace DashAttack.Entities.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Material material;
        [SerializeField] private PlayerController controller;

        [SerializeField, Range(0, float.PositiveInfinity)] private float squareToRoundTime;
        [SerializeField, Range(0, float.PositiveInfinity)] private float roundToSquareTime;

        [SerializeField, Range(0, 1)] private float minRoundness;
        [SerializeField, Range(0, 1)] private float maxRoundness;

        private IEnumerator currentCrt;

        private void Start()
        {
            material = GetComponent<SpriteRenderer>().material;

            controller.SubscribeRun(Accelerating, OnEnter, () => StartCRT(RoundingCrt()));
            controller.SubscribeRun(Braking, OnEnter, () => StartCRT(SquaringCrt()));
            controller.SubscribeRun(Rest, OnEnter, () => StartCRT(SquaringCrt()));
        }

        private void StartCRT(IEnumerator crt)
        {
            if (currentCrt is not null)
            {
                StopCoroutine(currentCrt);
            }

            currentCrt = crt;
            StartCoroutine(currentCrt);
        }

        private IEnumerator RoundingCrt()
        {

            var currentRoundness = material.GetFloat("_Roundness");

            while (currentRoundness < maxRoundness)
            {
                currentRoundness += (maxRoundness - minRoundness) / squareToRoundTime * Time.deltaTime;
                material.SetFloat("_Roundness", currentRoundness);
                yield return null;
            }

            currentRoundness = maxRoundness;
        }

        private IEnumerator SquaringCrt()
        {
            var currentRoundness = material.GetFloat("_Roundness");

            while (currentRoundness > minRoundness)
            {
                currentRoundness -= (maxRoundness - minRoundness) / roundToSquareTime * Time.deltaTime;
                material.SetFloat("_Roundness", currentRoundness);
                yield return null;
            }

            currentRoundness = minRoundness;
        }
    }

}
