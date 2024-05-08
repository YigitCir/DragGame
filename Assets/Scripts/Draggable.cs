using System;
using DefaultNamespace.Configs;
using DefaultNamespace.Helpers;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class Draggable : MonoBehaviour
    {
        [field: SerializeField]
        public DragState State { get; private set; }
        
        [field: SerializeField]
        public Rigidbody RB { get; private set; }

        [field: SerializeField]
        public Collider Collider { get; private set; }
        
        [field: SerializeField]
        public MeshRenderer MR { get; private set; }

        [field: SerializeField]
        public int MaxHealth { get; private set; }


        private float _currentHealth;
        private DragConfig _dragConfig;
        private Vector3 _lastFrameVelocity;
        
        

        private void Start()
        {
            _currentHealth = MaxHealth;
            _dragConfig = DragConfig.Instance;
            SetState(DragState.Free);
        }

        private void LateUpdate()
        {
            _lastFrameVelocity = RB.velocity;
            if (State == DragState.Free)
            {
                var desiredSpeed = _dragConfig.BaseTargetXSpeed / RB.mass;
                if (_lastFrameVelocity.magnitude < desiredSpeed)
                {
                    var targetSpeed = Vector3.Lerp(_lastFrameVelocity,Vector3.right * desiredSpeed, 0.1f);
                    RB.velocity = targetSpeed;
                    //RB.AddForce(Vector3.right,ForceMode.Force);
                }
            }
        }

        protected Sequence _disappearSeq;
        
        protected virtual void Disappear()
        {
            _disappearSeq = DOTween.Sequence();
            _disappearSeq.Append(transform.DOMove(transform.position.With(y: 0f), 0.1f));
            _disappearSeq.Join(transform.DOScale(new Vector3(0.5f,0f,0.5f), 0.3f))
                .OnStart(StartDisappear)
                .OnComplete(CompleteDisappear);
        }

        private void StartDisappear()
        {
            Collider.enabled = false;
            RB.useGravity = false;
        }
        
        private void CompleteDisappear()
        {
            Destroy(this.gameObject);
        }

        #region Collisions
        
        protected virtual void OnCollisionEnter(Collision collision)
        {
            switch (State)
            {
                case DragState.Dragging:
                    OnDraggingCollision(collision);
                    return;
                case DragState.Fall:
                    OnFallingCollision(collision);
                    return;
                case DragState.Free:
                    OnFreeCollision(collision);
                    return;
            }
        }
        
        protected virtual void OnFreeCollision(Collision collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                
                return;
            }
            
            if (collision.gameObject.CompareTag("Enemy"))
            {
                
                return;
            }
            
            if (collision.gameObject.CompareTag("Castle"))
            {
                Disappear();
                CastleEvents.InvokeCastleDamageTakenEvent();
                return;
            }
        }
        
        protected virtual void OnDraggingCollision(Collision collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                ObjectDragger.Free();
                return;
            }
            
            if (collision.gameObject.CompareTag("Enemy"))
            {
                ObjectDragger.Free();
                return;
            }
            
            if (collision.gameObject.CompareTag("Castle"))
            {
                ObjectDragger.Free();
                Disappear();
                CastleEvents.InvokeCastleDamageTakenEvent();
                return;
            }
        }

        
        protected virtual void OnFallingCollision(Collision collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                //Debug.Log("Collided with speed "+_lastFrameVelocity);
                SetState(DragState.Free);
                CreateImpact();
                TakeSpeedBasedDamage();
            }
            
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (collision.gameObject.TryGetComponent<Draggable>(out var d))
                {
                    if (d.State == DragState.Free)
                    {
                        SetState(DragState.Free);
                    }
                    d.TakeImpactDamage(GetSpeedBasedDamage());
                }
                CreateImpact();
                TakeSpeedBasedDamage();
            }
            
            if (collision.gameObject.CompareTag("Castle"))
            {
                Destroy(this.gameObject);
                CastleEvents.InvokeCastleDamageTakenEvent();
            }
        }

        
        #endregion
        
        protected virtual float GetSpeedBasedDamage()
        {
            return Mathf.Abs(_lastFrameVelocity.x) * _dragConfig.XSpeedToDamage +
                   Mathf.Abs(_lastFrameVelocity.y) * _dragConfig.YSpeedToDamageMultiplier;
        }

        protected virtual void TakeSpeedBasedDamage()
        {
            var dmg = GetSpeedBasedDamage();
            dmg = Mathf.RoundToInt(dmg);
            Debug.Log($"{this.gameObject.name}: Took " + dmg + " speed damage");
            ReduceHealth(dmg);
        }

        protected virtual void ReduceHealth(float dmg)
        {
            if(_currentHealth<=0) return;
            _currentHealth -= dmg;
            if(_currentHealth<=0) Disappear();
            else
            {
                DamageEffect();   
            }
        }

        void DamageEffect()
        {
            transform.DOPunchScale(Vector3.down * 0.45f, 0.3f);
        }

        protected virtual void TakeImpactDamage(float dmg)
        {
            Debug.Log($"{this.gameObject.name}: Took " + dmg + " impact damage");
            ReduceHealth(dmg);
        }

        protected virtual void CreateImpact()
        {
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(RB.position, 5f,Vector3.up);
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent<Draggable>(out var impactedObj))
                {
                    if(impactedObj==this) continue;
                    var explosionMultiplier = RB.mass * RB.velocity.magnitude;
                    //impactedObj.RB.AddExplosionForce(_dragConfig.BaseExplosionForce * explosionMultiplier, RB.position + Vector3.down,_dragConfig.BaseExplosionRadius * explosionMultiplier);
                }
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            
        }

        public void MoveTowards(Vector3 targetPos)
        {
            var position = RB.position;
            targetPos[2] = position.z;
            var dir = targetPos - position;

            Debug.DrawLine(position,targetPos);

            var velocity = dir / RB.mass;
            if (dir.magnitude > _dragConfig.CloseEnoughRange) velocity = dir.normalized * _dragConfig.BaseDragSpeed / RB.mass;
            if (dir.y < 0) velocity *= _dragConfig.DragDownMultiplier;
            RB.velocity = velocity;
        }

        public void LetGo()
        {
            if (RB.position.y > 4 || RB.velocity.y > 4)
            {
                SetState(DragState.Fall);
            }
            else
            {
                SetState(DragState.Free);
            }
        }

        public void SetState(DragState state)
        {
            State = state;
            MR.material = MaterialProvider.Instance.GetDraggableMaterial(State);
        }
        
    }

    public enum DragState
    {
        Free,
        Dragging,
        Fall
    }
}