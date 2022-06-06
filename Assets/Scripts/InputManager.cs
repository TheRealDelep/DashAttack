using UnityEngine;

namespace DashAttack
{
    public class InputManager : MonoBehaviour
    {
        private InputActions actions;

        private bool lastFixedFrameJump;

        public static InputManager Instance { get; private set; }

        public float Move { get; private set; }

        public bool Jump { get; private set; }

        public bool JumpPressedThisFixedFrame { get; private set; }

        public bool JumpReleasedThisFixedFrame { get; private set; }

        private void Awake()
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

        private void Update()
        {
            Move = actions.Default.Move.ReadValue<float>();
            Jump = actions.Default.Jump.ReadValue<float>() > 0;
        }

        private void FixedUpdate()
        {
            JumpPressedThisFixedFrame = Jump && !lastFixedFrameJump;
            JumpReleasedThisFixedFrame = lastFixedFrameJump && !Jump;

            lastFixedFrameJump = Jump;
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