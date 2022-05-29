using UnityEngine;

namespace DashAttack.Game.Managers
{
    public class InputManager : MonoBehaviour
    {
        private InputActions actions;

        public static InputManager Instance { get; private set; }

        public float Move => actions.Default.Move.ReadValue<float>();

        private void Start()
        {
            if (Instance is not null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);

            actions = new InputActions();
        }

        private void OnEnable()
        {
            actions.Enable();
        }

        private void OnDisable()
        {
            actions.Disable();
        }
    }
}
