namespace DashAttack.Characters
{
    using DashAttack.Characters.Movements.Dash;
    using UnityEngine;

    public class PlayerFeedbacks : MonoBehaviour
    {
        [SerializeField] private Color noDashColor;

        private DashMovement Dash { get; set; }
        private SpriteRenderer Renderer { get; set; }
        private Color BaseColor { get; set; }

        private void Start()
        {
            Dash = GetComponent<DashMovement>();
            Renderer = GetComponent<SpriteRenderer>();
            BaseColor = Renderer.color;
        }

        private void Update()
        {
            //Renderer.color = Dash.CanDash ? BaseColor : noDashColor;
        }
    }
}