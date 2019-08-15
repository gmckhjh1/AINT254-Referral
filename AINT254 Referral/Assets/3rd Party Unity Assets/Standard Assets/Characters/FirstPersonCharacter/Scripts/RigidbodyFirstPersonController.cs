using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
    public class RigidbodyFirstPersonController : MonoBehaviour
    {
        [Serializable]
        public class MovementSettings
        {
            public float ForwardSpeed = 10.0f;   // Speed when walking forward
            public float BackwardSpeed = 4.0f;  // Speed when walking backwards
            public float StrafeSpeed = 4.0f;    // Speed when walking sideways
            public float RunMultiplier = 0.0f;   // Speed when sprinting
	        public KeyCode RunKey = KeyCode.LeftShift;
            [HideInInspector] public float JumpForce = 30f;
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector] public float CurrentTargetSpeed = 8f;

#if !MOBILE_INPUT
            private bool m_Running;
#endif

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
	            if (input == Vector2.zero) return;
				if (input.x > 0 || input.x < 0)
				{
					//strafe
					CurrentTargetSpeed = StrafeSpeed;
				}
				if (input.y < 0)
				{
					//backwards
					CurrentTargetSpeed = BackwardSpeed;
				}
				if (input.y > 0)
				{
					//forwards
					//handled last as if strafing and moving forward at the same time forwards speed should take precedence
					CurrentTargetSpeed = ForwardSpeed;
				}
#if !MOBILE_INPUT
	            if (Input.GetKey(RunKey))
	            {
		            CurrentTargetSpeed *= RunMultiplier;
		            m_Running = true;
	            }
	            else
	            {
		            m_Running = false;
	            }
#endif
            }

#if !MOBILE_INPUT
            public bool Running
            {
                get { return m_Running; }
            }
#endif
        }


        [Serializable]
        public class AdvancedSettings
        {
            public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
            public float stickToGroundHelperDistance = 0.5f; // stops the character
            public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
            public bool airControl; // can the user control the direction that is being moved in the air
            [Tooltip("set it to 0.1 or more if you get stuck in wall")]
            public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
        }


        public Camera cam;
        public Camera effectsCam;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public MouseLook mouseLookCam2 = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();


        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation;
        private Vector3 m_GroundContactNormal;
        private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded, isDamaged, isEffectTriggered;

        private PlayerDodgeControllerExtension m_dodgeController; // Add reference to PlayerController
        private Vector2 m_dodgeInput; //Get axis input for use in calling dodge methods
        [SerializeField] private Transform hitBody;//Raycast for vault detection
        [SerializeField] private Transform hitKnee;//Raycast for slide detection
        private bool isHitBody;
        private bool isHitKnee;
        
        [SerializeField] private GameObject damageEffectPanel;//Panel damage effect reference

        public Vector3 Velocity
        {
            get { return m_RigidBody.velocity; }
        }

        public bool Grounded
        {
            get { return m_IsGrounded; }
        }

        public bool Jumping
        {
            get { return m_Jumping; }
        }

        public bool Running
        {
            get
            {
 #if !MOBILE_INPUT
				return movementSettings.Running;
#else
	            return false;
#endif
            }
        }


        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            mouseLook.Init (transform, cam.transform);
            mouseLookCam2.Init(transform, effectsCam.transform);
            m_dodgeController = GetComponent<PlayerDodgeControllerExtension>();

            CrossPlatformInputManager.SwitchActiveInputMethod(CrossPlatformInputManager.ActiveInputMethod.Hardware);
        }


        private void Update()
        {
            Debug.DrawLine(hitKnee.transform.position, hitKnee.transform.forward, Color.green);
            Debug.DrawRay(hitBody.transform.position, hitBody.transform.forward, Color.green);

            RotateView();
            m_dodgeInput = GetInput();
            
            //Call appropriate methods from PlayerDodgeControllerExtension
            //based on the input from the player.
            if(CrossPlatformInputManager.GetButtonDown("Jump") && m_dodgeInput.x > 0)
            {                
                m_dodgeController.StartDodge("Right");
                Debug.Log("Controller triggering dodge right");
            }
            else if(CrossPlatformInputManager.GetButtonDown("Jump") && m_dodgeInput.x < 0)
            {
                m_dodgeController.StartDodge("Left");
            }
            else if(CrossPlatformInputManager.GetButtonDown("Jump") && !m_Jump)
            {
                m_dodgeController.StartDodge("Jump");
                /*
                //use switch statement to call appropriate dodge method 
                //based on the result of raycasting from 2 points.
                switch (CheckRayDodge())
                {         
                    
                    case "Vault":
                        StartCoroutine(m_dodgeController.Vault());
                        break;

                    case "Slide":
                        StartCoroutine(m_dodgeController.Slide());
                        break;

                    default:
                        m_dodgeController.StartDodge("Jump");
                        break;
                }         
                */
            }
            //Don't allow the player to jump backwards
            else if (CrossPlatformInputManager.GetButtonDown("Jump") && m_dodgeInput.y < 0)
            {                
                return;
            }
        }

        public void InitiateDamageEffects()
        {
            if (!isEffectTriggered)
            {
                StartCoroutine("ControllerDamageEffects");
            }
            else return;
        }

        /// <summary>
        /// Set isDamaged true for period of time.
        /// PLayer movemnt controls will be reversed in the FixedUpdate method.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ControllerDamageEffects()
        {
            isDamaged = true;
            damageEffectPanel.SetActive(true);

            yield return new WaitForSeconds(2f);

            isDamaged = false;
            damageEffectPanel.SetActive(false);
        }
        /// <summary>
        /// Use ray casts to check for level geometry in front of player
        /// at certain heights.
        /// </summary>
        /// <returns></returns>
        private string CheckRayDodge()
        {
            RaycastHit rayKnee;
            RaycastHit rayBody;

            isHitBody = false;
            isHitKnee = false;
            
            //Check if raycast from body hits geometry
            if (Physics.Raycast(hitBody.transform.position, hitKnee.transform.forward,
                out rayBody, 3f))
            {
                if (rayBody.transform.tag == "Geometry")
                {
                    isHitBody = true;//Record true if raycast hits geometry
                }
            }

            //Chec if raycast from knee hits geometry
            if(Physics.Raycast(hitKnee.transform.position, hitKnee.transform.forward,
                out rayKnee, 3f))
            {
                if (rayKnee.transform.tag == "Geometry")
                {
                    isHitKnee = true;//Record true if raycast hits geometry
                }
            }

            //If both rays hit geometry then return null.
            //If only knee hits then return vault.
            //IF only body hits then retun slide else return null
            if(isHitKnee && isHitBody)
            {
                return null;
            }
            else if (isHitKnee)
            {
                return "Vault";
            }
            else if (isHitBody)
            {
                return "Slide";
            }
            else
            {
                return null;
            }
        }

        private void FixedUpdate()
        {            
            GroundCheck();
            Vector2 input = GetInput();

            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
            {
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

                //If player is damaged reverse the controls and increase their sensitivity
                if (isDamaged == true)
                {
                    desiredMove.x = -desiredMove.x * movementSettings.CurrentTargetSpeed * 2;
                    desiredMove.z = -desiredMove.z * movementSettings.CurrentTargetSpeed * 2;
                    desiredMove.y = -desiredMove.y * movementSettings.CurrentTargetSpeed * 2;
                }
                else
                {
                    desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
                    desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
                    desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;
                }

                if (m_RigidBody.velocity.sqrMagnitude <
                    (movementSettings.CurrentTargetSpeed*movementSettings.CurrentTargetSpeed))
                {
                    m_RigidBody.AddForce(desiredMove*SlopeMultiplier(), ForceMode.Impulse);
                }
            }

            if (m_IsGrounded)
            {
                m_RigidBody.drag = 5f;

                if (m_Jump)
                {
                    m_RigidBody.drag = 0f;
                    m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                    m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                    m_Jumping = true;
                }

                if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
                {
                    m_RigidBody.Sleep();
                }
            }
            else
            {
                m_RigidBody.drag = 0f;
                if (m_PreviouslyGrounded && !m_Jumping)
                {
                    //StickToGroundHelper();
                }
            }
            m_Jump = false;
        }

        
        private float SlopeMultiplier()
        {
            float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
            return movementSettings.SlopeCurveModifier.Evaluate(angle);
        }
        
        /*
        private void StickToGroundHelper()
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height/2f) - m_Capsule.radius) +
                                   advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
                {
                    m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
                }
            }
        }
        */


        private Vector2 GetInput()
        {
            
            Vector2 input = new Vector2
                {
                    x = CrossPlatformInputManager.GetAxis("Horizontal"),
                    y = CrossPlatformInputManager.GetAxis("Vertical")
                };
			movementSettings.UpdateDesiredTargetSpeed(input);
            return input;
        }


        private void RotateView()
        {
            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

            mouseLook.LookRotation (transform, cam.transform);
            mouseLookCam2.LookRotation(transform, effectsCam.transform);

            if (m_IsGrounded || advancedSettings.airControl)
            {
                // Rotate the rigidbody velocity to match the new direction that the character is looking
                Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
                m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
            }
        }

        /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
        private void GroundCheck()
        {
            m_PreviouslyGrounded = m_IsGrounded;
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height/2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_IsGrounded = true;
                m_GroundContactNormal = hitInfo.normal;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundContactNormal = Vector3.up;
            }
            if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
            {
                m_Jumping = false;
            }
        }
    }
}
