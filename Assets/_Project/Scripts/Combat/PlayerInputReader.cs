using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Combat
{
    /// <summary>
    /// 读取 Settings/Input/Player.inputactions 的 Gameplay 图。
    /// 资产没挂上时什么都不做，避免空场景报错。
    /// </summary>
    public sealed class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] InputActionAsset actions;
        [SerializeField] PlayerMotor2D motor;
        [SerializeField] RollAbility roll;
        [SerializeField] WeaponController weapon;

        InputActionMap gameplay;
        InputAction move;
        InputAction jump;
        InputAction rollAction;
        InputAction attack;
        InputAction skillA;
        InputAction skillB;

        void Awake()
        {
            if (motor == null)
                motor = GetComponent<PlayerMotor2D>();
            if (roll == null)
                roll = GetComponent<RollAbility>();
            if (weapon == null)
                weapon = GetComponent<WeaponController>();

            if (actions == null)
                return;

            gameplay = actions.FindActionMap("Gameplay", false);
            if (gameplay == null)
                return;

            move = gameplay.FindAction("Move", false);
            jump = gameplay.FindAction("Jump", false);
            rollAction = gameplay.FindAction("Roll", false);
            attack = gameplay.FindAction("Attack", false);
            skillA = gameplay.FindAction("SkillA", false);
            skillB = gameplay.FindAction("SkillB", false);
        }

        void OnEnable()
        {
            if (gameplay != null)
                gameplay.Enable();
        }

        void OnDisable()
        {
            if (gameplay != null)
                gameplay.Disable();
        }

        void Update()
        {
            if (move == null)
                return;

            var axis = move.ReadValue<Vector2>();
            if (motor != null)
            {
                motor.SetMoveAxis(axis.x);
                if (jump != null && jump.WasPressedThisFrame())
                    motor.RequestJump();
            }

            if (roll != null && rollAction != null && rollAction.WasPressedThisFrame())
            {
                var sign = axis.x == 0f ? 1f : Mathf.Sign(axis.x);
                roll.TryRoll(new Vector2(sign, 0f));
            }

            if (weapon == null)
                return;

            if (attack != null && attack.WasPressedThisFrame())
                weapon.TryPrimaryAttack();
            if (skillA != null && skillA.WasPressedThisFrame())
                weapon.TrySkill(0);
            if (skillB != null && skillB.WasPressedThisFrame())
                weapon.TrySkill(1);
        }
    }
}
