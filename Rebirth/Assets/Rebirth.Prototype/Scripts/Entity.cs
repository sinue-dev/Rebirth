using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace Rebirth.Prototype
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entity : MonoBehaviour
    {
        public string Name;

        public GameObject EntityHead;
        public GameObject EntityLeftHand;
        public GameObject EntityRightHand;

        public Weapon LeftHandItem;
        public Weapon RightHandItem;

        public Item InteractableItem;

        private Text _nameObject;
        public Text NamePrefab;

        public Transform NameTransform;

        // [SyncVar] NetworkIdentity: errors when null
        // [SyncVar] Entity: SyncVar only works for simple types
        // [SyncVar] GameObject is the only solution where we don't need a custom
        //           synchronization script (needs NetworkIdentity component!)
        // -> we still wrap it with a property for easier access, so we don't have
        //    to use target.GetComponent<Entity>() everywhere
        [Header("Target")]
        GameObject _target;
        public Entity target
        {
            get { return _target != null ? _target.GetComponent<Entity>() : null; }
            set { _target = value != null ? value.gameObject : null; }
        }

        [Header("Stats")]
        private CharacterStats stats;
        public CharacterStats Stats
        {
            get { return stats; }
        }

        [Header("Damage Popup")]
        [SerializeField]
        GameObject damagePopupPrefab;


        [HideInInspector]
        public Animator animator;
        [HideInInspector]
        new public Collider collider;
        [HideInInspector]
        public Rigidbody rb;

        // networkbehaviour ////////////////////////////////////////////////////////
        // cache components on server and clients
        protected virtual void Awake()
        {
            stats = GetComponent<CharacterStats>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            collider = GetComponentInChildren<Collider>();
            
            //StartCoroutine(DisplayName());
        }

        void Start()
        {

        }

        public virtual void Setup(string name)
        {
            Name = name;            
        }

        // entity logic will be implemented with a finite state machine
        // -> we should react to every state and to every event for correctness
        // -> we keep it functional for simplicity
        // note: can still use LateUpdate for Updates that should happen in any case
        void Update()
        {
             UpdateClient();
        }

        // update for client.
        protected abstract void UpdateClient();

        public void LookAtY(Vector3 pos)
        {
            transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));
        }

        enum PopupType { Normal, Block, Crit };

        void RpcShowDamagePopup(PopupType popupType, int amount, Vector3 pos)
        {
            // spawn the damage popup (if any) and set the text
            // (-1 = block)
            if (damagePopupPrefab)
            {
                var popup = (GameObject)Instantiate(damagePopupPrefab, pos, Quaternion.identity);
                if (popupType == PopupType.Normal)
                    popup.GetComponentInChildren<TextMesh>().text = amount.ToString();
                else if (popupType == PopupType.Block)
                    popup.GetComponentInChildren<TextMesh>().text = "<i>Block!</i>";
                else if (popupType == PopupType.Crit)
                    popup.GetComponentInChildren<TextMesh>().text = amount + " Crit!";
            }
        }

        // deal damage at another entity
        // (can be overwritten for players etc. that need custom functionality)
        // (can also return the set of entities that were hit, just in case they are
        //  needed when overwriting it)

        public virtual HashSet<Entity> DealDamageAt(Entity entity, int n, float aoeRadius = 0f)
        {
            // build the set of entities that were hit within AoE range
            var entities = new HashSet<Entity>();

            // add main target in any case, because non-AoE skills have radius=0
            entities.Add(entity);

            // add all targets in AoE radius around main target
            var colliders = Physics.OverlapSphere(entity.transform.position, aoeRadius); //, layerMask);
            foreach (var c in colliders)
            {
                var candidate = c.GetComponentInParent<Entity>();
                // overlapsphere cast uses the collider's bounding volume (see
                // Unity scripting reference), hence is often not exact enough
                // in our case (especially for radius 0.0). let's also check the
                // distance to be sure.
                if (candidate != null && candidate != this && candidate.Stats.Health > 0 &&
                    Vector3.Distance(entity.transform.position, candidate.transform.position) < aoeRadius)
                    entities.Add(candidate);
            }

            // now deal damage at each of them
            foreach (var e in entities)
            {
                int damageDealt = 0;
                var popupType = PopupType.Normal;

                // don't deal any damage if target is invincible
                if (!e.Stats.IsInvincible)
                {
                    // block? (we use < not <= so that block rate 0 never blocks)
                    if (e.GetComponent<CharacterControllerCustom>() != null)
                    {
                        if (e.GetComponent<CharacterControllerCustom>().isBlocking)
                        {
                            popupType = PopupType.Block;
                        }
                        else
                        {
                            damageDealt = Mathf.Max(n - 10, 1); // 10 = e.Stats.cArmor.FinalValue;

                            // critical hit?
                            //if (Random.value < crit)
                            //{
                            //    damageDealt *= 2;
                            //    popupType = PopupType.Crit;

                            e.Stats.Health -= damageDealt;
                        }
                    }
                    else
                    {
                        e.Stats.Health -= damageDealt;
                    }
                }

                // show damage popup in observers via ClientRpc
                // showing them above their head looks best, and we don't have to
                // use a custom shader to draw world space UI in front of the entity
                // note: we send the RPC to ourselves because whatever we killed
                //       might disappear before the rpc reaches it
                var bounds = e.GetComponentInChildren<Collider>().bounds;
                RpcShowDamagePopup(popupType, damageDealt, new Vector3(bounds.center.x, bounds.max.y, bounds.center.z));

                // let's make sure to pull aggro in any case so that archers
                // are still attacked if they are outside of the aggro range
                e.OnAggro(this);
            }

            return entities;
        }

        // aggro ///////////////////////////////////////////////////////////////////
        // this function is called by the AggroArea (if any) on clients and server
        public virtual void OnAggro(Entity entity) { }

        private void OnDestroy()
        {
            // Cleanup the name object
            if (_nameObject != null) Destroy(_nameObject);
        }

        public void MoveToRandomSpawnPoint()
        {
            var spawns = FindObjectsOfType<NetworkStartPosition>();
            var spawn = spawns[Random.Range(0, spawns.Length)];
            if(spawn != null)
                transform.position = spawn.transform.position;
        }

        public IEnumerator DisplayName()
        {
            // Create a player name
            _nameObject = Instantiate(NamePrefab).GetComponent<Text>();
            _nameObject.text = Name ?? ".";
            _nameObject.transform.SetParent(FindObjectOfType<Canvas>().transform);

            while (true)
            {
                if ((_nameObject.text != Name) && (Name != null))
                    _nameObject.text = Name;

                // While we're still "online"
                _nameObject.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, NameTransform.position) + Vector2.up * 30;

                yield return 0;
            }
        }
    }
}