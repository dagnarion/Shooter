using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class GatherInput : MonoBehaviour
{
   [SerializeField] private PlayerInput PlayerInput;
   [SerializeField] private InputActionReference MoveActionRef;
   [SerializeField] private InputActionReference VerticalRef;
   private InputActionMap playerMap;
   private InputActionMap uiMap;

   public float HorizontalInput { get; private set; }
   public float VerticalInput { get; private set; }
   
   private void Awake()
   {
      playerMap = PlayerInput.actions.FindActionMap("Player");
      uiMap = PlayerInput.actions.FindActionMap("UI");
      playerMap.Enable();
   }
   
   private void Update()
   {
      HorizontalInput = MoveActionRef.action.ReadValue<float>();
      VerticalInput = VerticalRef.action.ReadValue<float>();
   }
}
