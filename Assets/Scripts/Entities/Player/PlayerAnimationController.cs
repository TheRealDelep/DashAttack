using System.Collections;
using UnityEngine;

using static DashAttack.Entities.Player.PlayerStateEnum;
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

        private void Start()
        {
            material = GetComponent<SpriteRenderer>().material;

            controller.Subscribe(Running, OnEnter, () => StartCoroutine(SquareToRound()));
            controller.Subscribe(Running, OnLeave, () => StartCoroutine(RoundToSquare()));
        }

        private IEnumerator SquareToRound()
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

        private IEnumerator RoundToSquare()
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
