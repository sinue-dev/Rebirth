using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class CharacterStats : MonoBehaviour
    {
        //// TOUGHNESS
        public CStats_Toughness cToughness = new CStats_Toughness("TOUGHNESS", 0);
        // NUTRITION
        public CStats_Nutrition cNutrition = new CStats_Nutrition("NUTRITION", 0);
        // STRENGTH
        public CStats_Strength cStrength = new CStats_Strength("STRENGTH", 0);
        // AGILITY
        public CStats_Agility cAgility = new CStats_Agility("AGILITY", 0);
        // SURVIVAL
        public CStats_Survival cSurvival = new CStats_Survival("SURVIVAL", 0);


        //*** TOUGHNESS
        // HEALTH
        public CToughness_Health cHealth = new CToughness_Health("HEALTH", 100);
        // ARMOR

        // COLD INSULATION
        public CToughness_HeatInsulation cColdInsulation = new CToughness_HeatInsulation("COLDINSULATION", 0);

        // HEAT INSULATION
        public CToughness_HeatInsulation cHeatInsulation = new CToughness_HeatInsulation("HEATINSULATION", 0);

        //*** NUTRITION    
        // HUNGER
        public CNutrition_Hunger cHunger = new CNutrition_Hunger("HUNGER", 100);

        // THIRST
        public CNutrition_Thirst cThirst = new CNutrition_Thirst("THIRST", 100);

        // FATIGUE
        public CNutrition_Fatigue cFatigue = new CNutrition_Fatigue("FATIGUE", 100);

        //*** STRENGTH
        // MELEE
        public CStrength_Melee cMelee = new CStrength_Melee("MELEE", 10);

        //*** AGILITY
        // ENDURANCE
        public CAgility_Endurance cEndurance = new CAgility_Endurance("ENDURANCE", 100);

        #region Health
        [Header("Health")]
      
        protected bool invincible = false;
      
        protected bool RegenerateHealth = true;
      
        protected float HealthRegenerationRate = 1;
      
        protected float HealthFallRate = 1;

        public bool IsInvincible
        {
            get { return invincible; }
            set { invincible = value; }
        }

        public float Health = 100;

        #endregion

        #region Hunger
        [Header("Hunger")]
      
        protected float HungerFallRate = 1;

        public float Hunger = 100;


        #endregion

        #region Thirst

        [Header("Thirst")]
      
        protected float ThirstFallRate = 1;
        public float Thirst = 100;


        #endregion

        #region Fatigue

        [Header("Fatigue")]
      
        protected float FatigueFallRate = 1;
        public float Fatigue = 100;

        #endregion

        #region Endurance

        [Header("Endurance")]
      
        protected bool RegenerateEndurance = true;
      
        protected float EnduranceRegenerationRate = 1;
      
        protected float EnduranceFallRate = 1;
        public float endurance = 100;

        public float Endurance
        {
            get { return Mathf.Min(endurance, cEndurance.FinalValue); }
            set { endurance = Mathf.Clamp(value, 0, cEndurance.FinalValue); }
        }

        void OnChangeEndurance(float endurance)
        {
            //character.cCharacter.endurance = endurance;
        }

        #endregion

        [Header("Stats")]
      
        protected int toughness = 0;
      
        protected int nutrition = 0;
      
        protected int strength = 0;
      
        protected int agility = 0;
      
        protected int survival = 0;

      
        protected float armor = 0;
        //public float Armor
        //{
        //    get { return Mathf.Min(armor, cArmor.FinalValue); }
        //    set { armor = Mathf.Clamp(value, 0, cArmor.FinalValue); }
        //}

      
        protected float coldInsulation = 0;
        public float ColdInsulation
        {
            get { return Mathf.Min(coldInsulation, cColdInsulation.FinalValue); }
            set { coldInsulation = Mathf.Clamp(value, 0, cColdInsulation.FinalValue); }
        }

      
        protected float heatInsulation = 0;
        public float HeatInsulation
        {
            get { return Mathf.Min(heatInsulation, cHeatInsulation.FinalValue); }
            set { heatInsulation = Mathf.Clamp(value, 0, cHeatInsulation.FinalValue); }
        }

        //[Header("Weight")]
        //[SyncVar, SerializeField]
        //protected float Vitals_Weight = 0;

        //public float Weight
        //{
        //    get { return Mathf.Min(Vitals_Weight, 100f); }
        //    set { Vitals_Weight = Mathf.Clamp(value, 0, 100); }
        //}

 
        private RebirthPlayerController character;

        void Start()
        {
            character = GetComponent<RebirthPlayerController>();

            #region Health
            cHealth.AddAttribute(cToughness);
            Health = cHealth.FinalValue;

            cHealth.AddAttribute(new CAttribute("Health_RegenerationRate", HealthRegenerationRate));
            cHealth.AddAttribute(new CAttribute("Health_FallRate", HealthFallRate));
            #endregion

            #region Hunger / Thirst / Fatigue
            Hunger = cHunger.FinalValue;
            cHunger.AddAttribute(new CAttribute("Hunger_FallRate", HungerFallRate));

            Thirst = cThirst.FinalValue;
            cThirst.AddAttribute(new CAttribute("Thirst_FallRate", ThirstFallRate));

            cFatigue.AddAttribute(cAgility);
            Fatigue = cFatigue.FinalValue;
            cFatigue.AddAttribute(new CAttribute("Fatigue_FallRate", FatigueFallRate));
            #endregion

            #region Endurance
            cEndurance.AddAttribute(cAgility);
            endurance = cEndurance.FinalValue;
            cEndurance.AddAttribute(new CAttribute("Endurance_RegenerationRate", EnduranceRegenerationRate));
            cEndurance.AddAttribute(new CAttribute("Endurancea_FallRate", EnduranceFallRate));
            #endregion

            #region Weight / Endurance
            //weight.AddAttribute(endurance);
            //Vitals_Weight = 0; // Calculate Weight from Inventory Items
            #endregion

            InvokeRepeating("UpdateUI", 0.1f, 0.1f);
        }

        void Update()
        {
            Cmd_UpdateHealth();
            Cmd_UpdateHunger();
            Cmd_UpdateThirst();
            Cmd_UpdateEndurance();
        }

        void UpdateUI()
        {
            //if(GameManager.singleton.GUIController.UIVitals == null) return;
                
            //if (GameManager.singleton.GUIController.UIVitals.UI_Health != null)
            //{
            //    GameManager.singleton.GUIController.UIVitals.UI_Health.value = (float)System.Math.Round(Health / cHealth.FinalValue, 2);
            //    GameManager.singleton.GUIController.UIVitals.UI_Health.percentText.text = Mathf.Round((Health / cHealth.FinalValue) * 100).ToString() + "%";
            //    GameManager.singleton.GUIController.UIVitals.UI_Health.absolutText.text = Mathf.Round(Health).ToString() + " / " + Mathf.Round(cHealth.FinalValue).ToString();
            //}
            //if (GameManager.singleton.GUIController.UIVitals.UI_Stamina != null)
            //{
            //    GameManager.singleton.GUIController.UIVitals.UI_Stamina.value = (float)System.Math.Round(Endurance / cEndurance.FinalValue, 2);
            //    GameManager.singleton.GUIController.UIVitals.UI_Stamina.percentText.text = Mathf.Round((Endurance / cEndurance.FinalValue) * 100).ToString() + "%";
            //    GameManager.singleton.GUIController.UIVitals.UI_Stamina.absolutText.text = Mathf.Round(Endurance).ToString() + " / " + Mathf.Round(cEndurance.FinalValue).ToString();
            //}
            //if (GameManager.singleton.GUIController.UIVitals.UI_Hunger != null)
            //{
            //    GameManager.singleton.GUIController.UIVitals.UI_Hunger.value = (float)System.Math.Round(Hunger / cHunger.FinalValue, 2);
            //    GameManager.singleton.GUIController.UIVitals.UI_Hunger.percentText.text = Mathf.Round((Hunger / cHunger.FinalValue) * 100).ToString() + "%";
            //    GameManager.singleton.GUIController.UIVitals.UI_Hunger.absolutText.text = Mathf.Round(Hunger).ToString() + " / " + Mathf.Round(cHunger.FinalValue).ToString();
            //}
            //if (GameManager.singleton.GUIController.UIVitals.UI_Thirst != null)
            //{
            //    GameManager.singleton.GUIController.UIVitals.UI_Thirst.value = (float)System.Math.Round(Thirst / cThirst.FinalValue, 2);
            //    GameManager.singleton.GUIController.UIVitals.UI_Thirst.percentText.text = Mathf.Round((Thirst / cThirst.FinalValue) * 100).ToString() + "%";
            //    GameManager.singleton.GUIController.UIVitals.UI_Thirst.absolutText.text = Mathf.Round(Thirst).ToString() + " / " + Mathf.Round(cThirst.FinalValue).ToString();
            //}
            ////if (GameManager.singleton.GUIController.UIVitals.UI_Weight != null)
            ////{
            ////    GameManager.singleton.GUIController.UIVitals.UI_Weight.value = (float)System.Math.Round(Vitals_Weight / weight.FinalValue, 2);
            ////    GameManager.singleton.GUIController.UIVitals.UI_Weight.percentText.text = Mathf.Round((Vitals_Weight / weight.FinalValue) * 100).ToString() + "%";
            ////    GameManager.singleton.GUIController.UIVitals.UI_Weight.absolutText.text = Mathf.Round(Vitals_Weight).ToString() + " / " + Mathf.Round(weight.FinalValue).ToString();
            ////}
            //if (GameManager.singleton.GUIController.UIVitals.UI_Fatigue != null)
            //{
            //    GameManager.singleton.GUIController.UIVitals.UI_Fatigue.value = (float)System.Math.Round(Fatigue / cFatigue.FinalValue, 2);
            //    GameManager.singleton.GUIController.UIVitals.UI_Fatigue.percentText.text = Mathf.Round((Fatigue / cFatigue.FinalValue) * 100).ToString() + "%";
            //    GameManager.singleton.GUIController.UIVitals.UI_Fatigue.absolutText.text = Mathf.Round(Fatigue).ToString() + " / " + Mathf.Round(cFatigue.FinalValue).ToString();
            //}
        }

        void Cmd_UpdateHealth()
        {
            RpcUpdateHealth();
        }

        public void RpcUpdateHealth()
        {
            if (cHealth.Attributes.Count == 0) return;
            // HEALTH REGENERATION
            HealthRegenerationRate = cHealth.Attributes["Health_RegenerationRate"].FinalValue;
            // = new CAttribute("Health_RegenerationRate", HealthRegenerationRate);

            if (Health < cHealth.FinalValue && Health > 0)
            {
                if ((cHunger.hungerState == HungerStates_e.Satiated) && (cThirst.thirstState == ThirstStates_e.Quenched))
                {
                    RegenerateHealth = true;
                    Health += Time.deltaTime * HealthRegenerationRate * 2f;
                }
                else if ((cHunger.hungerState == HungerStates_e.Satiated) || (cThirst.thirstState == ThirstStates_e.Quenched))
                {
                    RegenerateHealth = true;
                    Health += Time.deltaTime * HealthRegenerationRate;
                }
                else
                {
                    RegenerateHealth = true;
                    Health += Time.deltaTime * HealthRegenerationRate;
                }
            }
            else if (Health >= cHealth.FinalValue)
            {
                Health = cHealth.FinalValue;
            }

            //// HEALTH DECREASE WHEN STARVING OR DEHYDRATED
            //health.Attributes["Health_FallRate"] = new CAttribute("Health_FallRate", HealthFallRate);

            //if ((hungerState == HungerStates_e.Starving) && (thirstState == ThirstStates_e.Dehydrated))
            //{
            //    RegenerateHealth = false;
            //    Vitals_Health -= Time.deltaTime * health.Attributes["Health_FallRate"].FinalValue * 1.5f; // *2 weil beides leer ist
            //}
            //else if ((hungerState == HungerStates_e.Starving) || (thirstState == ThirstStates_e.Dehydrated))
            //{
            //    RegenerateHealth = false;
            //    Vitals_Health -= Time.deltaTime * health.Attributes["Health_FallRate"].FinalValue;
            //}

            if (Health <= 0)
            {
                Health = 0;
                StartCoroutine(character.Controller._Death());
                // DEATH
                Debug.Log("YOU DIED!");
            }
        }

        void Cmd_UpdateHunger()
        {
            RpcUpdateHunger();
        }

        public void RpcUpdateHunger()
        {
            if (cHunger.Attributes.Count == 0) return;

            // HUNGER FALL RATE
            //hunger.Attributes["Hunger_FallRate"] = new CAttribute("Hunger_FallRate", HungerFallRate * ((false) ? 2 : 1)); // Beim Sprinten FallRate verdoppeln

            if (Hunger > 0)
            {
                Hunger -= Time.deltaTime * cHunger.Attributes["Hunger_FallRate"].FinalValue;
            }
            else if (Hunger <= 0)
            {
                Hunger = 0;
            }
            else if (Hunger >= cHunger.FinalValue)
            {
                Hunger = cHunger.FinalValue;
            }

            cHunger.UpdateHungerState(Hunger);

            switch (cHunger.hungerState)
            {
                case HungerStates_e.Gluttony:
                    cHealth.Attributes["Health_RegenerationRate"].SetOrAddRawBonus(new CRawBonus("Hunger_HealthRegenerationRate", cHealth.Attributes["Health_RegenerationRate"].BaseValue, 0.3f)); // -30%
                    break;
                case HungerStates_e.Satiated:
                    cHealth.Attributes["Health_RegenerationRate"].SetOrAddRawBonus(new CRawBonus("Hunger_HealthRegenerationRate", cHealth.Attributes["Health_RegenerationRate"].BaseValue, -0.1f)); // +10%
                    break;
                case HungerStates_e.Peckish:
                    cHealth.Attributes["Health_RegenerationRate"].SetOrAddRawBonus(new CRawBonus("Hunger_HealthRegenerationRate", cHealth.Attributes["Health_RegenerationRate"].BaseValue, 0.0f)); // +10%
                    break;
                case HungerStates_e.Hungry:
                    cHealth.Attributes["Health_RegenerationRate"].SetOrAddRawBonus(new CRawBonus("Hunger_HealthRegenerationRate", cHealth.Attributes["Health_RegenerationRate"].BaseValue, 0.3f)); // -30%
                    break;
                case HungerStates_e.VeryHungry:
                    cHealth.Attributes["Health_RegenerationRate"].SetOrAddRawBonus(new CRawBonus("Hunger_HealthRegenerationRate", cHealth.Attributes["Health_RegenerationRate"].BaseValue, 0.6f)); // -60%
                    break;
                case HungerStates_e.Starving:
                    cHealth.Attributes["Health_RegenerationRate"].SetOrAddRawBonus(new CRawBonus("Hunger_HealthRegenerationRate", cHealth.Attributes["Health_RegenerationRate"].BaseValue, 0.9f)); // -90%
                    break;
            }
        }

        void Cmd_UpdateThirst()
        {
            RpcUpdateThirst();
        }

        public void RpcUpdateThirst()
        {
            if (cThirst.Attributes.Count == 0) return;

            // THIRST FALL RATE
            //thirst.Attributes["Thirst_FallRate"] = new CAttribute("Thirst_FallRate", ThirstFallRate * ((false) ? 2 : 1)); // Beim Sprinten FallRate verdoppeln

            if (Thirst > 0)
            {
                Thirst -= Time.deltaTime * cThirst.Attributes["Thirst_FallRate"].FinalValue;
            }
            else if (Thirst <= 0)
            {
                Thirst = 0;
            }
            else if (Thirst >= cThirst.FinalValue)
            {
                Thirst = cThirst.FinalValue;
            }

            cThirst.UpdateThirstState(Thirst);

            switch (cThirst.thirstState)
            {
                case ThirstStates_e.Quenched:
                    cEndurance.Attributes["Endurance_RegenerationRate"].SetOrAddRawBonus(new CRawBonus("Thirst_EnduranceRegenerationBonus", 0f, 0.1f)); // +10%
                                                                                                                                                        //Debug.Log(stamina.FinalValue);
                    break;
                case ThirstStates_e.SlightlyThirsty:
                    cEndurance.Attributes["Endurance_RegenerationRate"].SetOrAddRawBonus(new CRawBonus("Thirst_EnduranceRegenerationBonus", 0f, 0.0f)); // 0%
                    break;
                case ThirstStates_e.Thirsty:
                    cEndurance.Attributes["Endurance_RegenerationRate"].SetOrAddRawBonus(new CRawBonus("Thirst_EnduranceRegenerationBonus", 0f, -0.3f)); // -30%
                    break;
                case ThirstStates_e.VeryThirsty:
                    cEndurance.Attributes["Endurance_RegenerationRate"].SetOrAddRawBonus(new CRawBonus("Thirst_EnduranceRegenerationBonus", 0f, -0.6f)); // -60%
                    break;
                case ThirstStates_e.Dehydrated:
                    cEndurance.Attributes["Endurance_RegenerationRate"].SetOrAddRawBonus(new CRawBonus("Thirst_EnduranceRegenerationBonus", 0f, -0.9f)); // -90%
                    break;
            }
        }

        void Cmd_UpdateEndurance()
        {
            RpcUpdateEndurance();
        }

        public void RpcUpdateEndurance()
        {
            if (cEndurance.Attributes.Count == 0) return;

            if (false)
            {
                // ENDURANCE FALL RATE
                cEndurance.Attributes["Endurance_FallRate"] = new CAttribute("Endurance_FallRate", EnduranceFallRate);

                if (Endurance > 0)
                {
                    Endurance -= Time.deltaTime * cEndurance.Attributes["Endurance_FallRate"].FinalValue;
                }
                else if (Endurance < 0)
                {
                    Endurance = 0;
                }
                else if (Endurance >= cEndurance.FinalValue)
                {
                    Endurance = cEndurance.FinalValue;
                }
            }
            else
            {
                // STAMINA REGENERATION
                //EnduranceRegenerationRate = cEndurance.Attributes["Endurance_RegenerationRate"].FinalValue * ((false) ? 2 : 1); // Im stehen 4x höhere Stamina Regeneration

                if (Endurance >= 0 && Endurance < cEndurance.FinalValue)
                {
                    Endurance += Time.deltaTime * EnduranceRegenerationRate;
                }
                else if (Endurance < 0)
                {
                    Endurance = 0;
                }
                else if (Endurance >= cEndurance.FinalValue)
                {
                    Endurance = cEndurance.FinalValue;
                }
            }
        }

        public void RpcTakeDamage(float damage)
        {
            Health -= damage;
        }

        public void Cmd_Revive(float healthPercentage)
        {
            RpcRevive(healthPercentage);
        }

        public void RpcRevive(float healthPercentage)
        {
            if (character.Controller.isDead)
            {
                StartCoroutine(character.Controller._Revive());
                Health = Mathf.RoundToInt(cHealth.FinalValue * healthPercentage);
            }
        }

    }

    #region Stat classes
    public class CStats_Toughness : CAttribute
    {
        public CStats_Toughness(string name, float value) : base(name, value)
        {

        }
    }

    public class CStats_Nutrition : CAttribute
    {
        public CStats_Nutrition(string name, float value) : base(name, value)
        {

        }
    }

    public class CStats_Strength : CAttribute
    {
        public CStats_Strength(string name, float value) : base(name, value)
        {

        }
    }

    public class CStats_Agility : CAttribute
    {
        public CStats_Agility(string name, float value) : base(name, value)
        {

        }
    }

    public class CStats_Survival : CAttribute
    {
        public CStats_Survival(string name, float value) : base(name, value)
        {

        }
    }

    public class CToughness_Health : CDependantAttribute
    {
        public CToughness_Health(string name, float value) : base(name, value)
        {

        }
    }

    public class CToughness_ColdInsulation : CDependantAttribute
    {
        public CToughness_ColdInsulation(string name, float value) : base(name, value)
        {

        }
    }

    public class CToughness_HeatInsulation : CDependantAttribute
    {
        public CToughness_HeatInsulation(string name, float value) : base(name, value)
        {

        }
    }

    public class CNutrition_Hunger : CDependantAttribute
    {
        public HungerStates_e hungerState = HungerStates_e.Satiated;

        // HUNGER PERCENTAGE VALUES 
        public const float hunger_Gluttony = 1.2f;
        public const float hunger_Satiated = 1.0f;
        public const float hunger_Peckish = 0.6f;
        public const float hunger_Hungry = 0.4f;
        public const float hunger_VeryHungry = 0.1f;
        public const float hunger_Starving = 0.0f;

        public CNutrition_Hunger(string name, float value) : base(name, value)
        {

        }

        public void UpdateHungerState(float Hunger)
        {
            //Attributes["Hunger_FallRate"] = new CAttribute("Hunger_FallRate", HungerFallRate * ((false) ? 2 : 1)); // Beim Sprinten FallRate verdoppeln

            if (Hunger >= FinalValue * hunger_Satiated && Hunger <= FinalValue * hunger_Gluttony) // GLUTTONY
            {
                hungerState = HungerStates_e.Gluttony;
            }
            else if (Hunger > FinalValue * hunger_Peckish && Hunger <= FinalValue * hunger_Satiated) // SATIATED
            {
                hungerState = HungerStates_e.Satiated;
            }
            else if (Hunger > FinalValue * hunger_Hungry && Hunger <= FinalValue * hunger_Peckish) // PECKISH
            {
                hungerState = HungerStates_e.Peckish;
            }
            else if (Hunger > FinalValue * hunger_VeryHungry && Hunger <= FinalValue * hunger_Hungry) // HUNGRY
            {
                hungerState = HungerStates_e.Hungry;
            }
            else if (Hunger > FinalValue * hunger_Starving && Hunger <= FinalValue * hunger_VeryHungry) // VERYHUNGRY
            {
                hungerState = HungerStates_e.VeryHungry;
            }
            else if (Hunger <= FinalValue * hunger_Starving) // STARVING
            {
                hungerState = HungerStates_e.Starving;
            }
        }
    }

    public class CNutrition_Thirst : CDependantAttribute
    {
        public ThirstStates_e thirstState = ThirstStates_e.Quenched;

        // THIRST PERCENTAGE VALUES 
        public const float thirst_Quenched = 1f;
        public const float thirst_SlightlyThirsty = 0.6f;
        public const float thirst_Thirsty = 0.4f;
        public const float thirst_VeryThirsty = 0.1f;
        public const float thirst_Dehydrated = 0.0f;

        public CNutrition_Thirst(string name, float value) : base(name, value)
        {

        }

        public void UpdateThirstState(float Thirst)
        {
            if (Thirst >= FinalValue * thirst_SlightlyThirsty && Thirst <= FinalValue * thirst_Quenched) // QUENCHED
            {
                thirstState = ThirstStates_e.Quenched;
            }
            else if (Thirst > FinalValue * thirst_Thirsty && Thirst <= FinalValue * thirst_SlightlyThirsty) // SLIGHTLYTHIRSTY
            {
                thirstState = ThirstStates_e.SlightlyThirsty;
            }
            else if (Thirst > FinalValue * thirst_VeryThirsty && Thirst <= FinalValue * thirst_Thirsty) // THIRSTY
            {
                thirstState = ThirstStates_e.Thirsty;
            }
            else if (Thirst > FinalValue * thirst_Dehydrated && Thirst <= FinalValue * thirst_VeryThirsty) // VERYTHIRSTY
            {
                thirstState = ThirstStates_e.VeryThirsty;
            }
            else if (Thirst <= FinalValue * thirst_Dehydrated) // DEHYDRATED
            {
                thirstState = ThirstStates_e.Dehydrated;
            }
        }
    }

    public class CNutrition_Fatigue : CDependantAttribute
    {
        public CNutrition_Fatigue(string name, float value) : base(name, value)
        {

        }
    }

    public class CStrength_Melee : CDependantAttribute
    {
        public CStrength_Melee(string name, float value) : base(name, value)
        {

        }
    }


    public class CAgility_Endurance : CDependantAttribute
    {
        public CAgility_Endurance(string name, float value) : base(name, value)
        {

        }
    }
    #endregion

    #region Base Attribute Classes
    public class CDependantAttribute : CAttribute
    {
        protected Dictionary<string, CAttribute> _attributes;

        public CDependantAttribute(string name, float value) : base(name, value)
        {
            _attributes = new Dictionary<string, CAttribute>();
        }

        public Dictionary<string, CAttribute> Attributes
        {
            get { return _attributes; }
        }

        public void AddAttribute(CAttribute attr)
        {
            if (!_attributes.ContainsKey(attr.Name)) _attributes.Add(attr.Name, attr);
        }

        public void RemoveAttribute(CAttribute attr)
        {
            if (_attributes.ContainsKey(attr.Name))
            {
                _attributes.Remove(attr.Name);
            }
        }

        public override float CalculateValue()
        {
            _finalValue = BaseValue;

            ApplyRawBonuses();

            ApplyFinalBonuses();

            return _finalValue;
        }
    }

    public abstract class CBaseAttribute
    {
        protected string _attrName;
        protected float _baseValue;
        protected float _baseMultiplier;

        public CBaseAttribute(string name, float value, float multiplier = 0)
        {
            this._attrName = name;
            this._baseValue = value;
            this._baseMultiplier = multiplier;
        }

        public string Name
        {
            get { return _attrName; }
        }
        public float BaseValue
        {
            get { return _baseValue; }
        }
        public float BaseMultiplier
        {
            get { return _baseMultiplier; }
        }
    }

    public class CAttribute : CBaseAttribute
    {
        private Dictionary<string, CRawBonus> _rawBonuses;
        private Dictionary<string, CFinalBonus> _finalBonuses;

        protected float _finalValue;

        public CAttribute(string name, float value) : base(name, value)
        {
            _rawBonuses = new Dictionary<string, CRawBonus>();
            _finalBonuses = new Dictionary<string, CFinalBonus>();

            _finalValue = BaseValue;
        }

        public void AddRawBonus(CRawBonus bonus)
        {
            if (!_rawBonuses.ContainsKey(bonus.Name)) _rawBonuses.Add(bonus.Name, bonus);
        }

        public void SetOrAddRawBonus(CRawBonus bonus)
        {
            if (!_rawBonuses.ContainsKey(bonus.Name)) _rawBonuses.Add(bonus.Name, bonus);
            else
            {
                _rawBonuses[bonus.Name] = new CRawBonus(bonus.Name, bonus.BaseValue, bonus.BaseMultiplier);
            }
        }

        public void AddFinalBonus(CFinalBonus bonus)
        {
            if (!_finalBonuses.ContainsKey(bonus.Name)) _finalBonuses.Add(bonus.Name, bonus);
        }

        public void SetOrAddFinalBonus(CFinalBonus bonus)
        {
            if (!_finalBonuses.ContainsKey(bonus.Name)) _finalBonuses.Add(bonus.Name, bonus);
            else
            {
                _finalBonuses[bonus.Name] = new CFinalBonus(bonus.Name, bonus.BaseValue, bonus.BaseMultiplier);
            }
        }

        public void RemoveRawBonus(CRawBonus bonus)
        {
            if (_rawBonuses.ContainsKey(bonus.Name))
            {
                _rawBonuses.Remove(bonus.Name);
            }
        }

        public void RemoveFinalBonus(CFinalBonus bonus)
        {
            if (_finalBonuses.ContainsKey(bonus.Name))
            {
                _finalBonuses.Remove(bonus.Name);
            }
        }

        protected void ApplyRawBonuses()
        {
            // Adding value from raw
            float rawBonusValue = 0;
            float rawBonusMultiplier = 0;
            foreach (KeyValuePair<string, CRawBonus> kvp in _rawBonuses)
            {
                CRawBonus bonus = kvp.Value;
                rawBonusValue += bonus.BaseValue;
                rawBonusMultiplier += bonus.BaseMultiplier;
            }

            _finalValue += rawBonusValue;
            _finalValue *= (1 + rawBonusMultiplier);
        }

        protected void ApplyFinalBonuses()
        {
            // Adding value from final
            float finalBonusValue = 0;
            float finalBonusMultiplier = 0;

            foreach (KeyValuePair<string, CFinalBonus> kvp in _finalBonuses)
            {
                CFinalBonus bonus = kvp.Value;
                finalBonusValue += bonus.BaseValue;
                finalBonusMultiplier += bonus.BaseMultiplier;
            }

            _finalValue += finalBonusValue;
            _finalValue *= (1 + finalBonusMultiplier);
        }

        public virtual float CalculateValue()
        {
            _finalValue = BaseValue;

            ApplyRawBonuses();

            ApplyFinalBonuses();

            return _finalValue;
        }

        public float FinalValue
        {
            get { return CalculateValue(); }
        }

        //The tricky part is the calculateValue() method.First, it sums up all the values that the raw bonuses add to the attribute, and also sums up all the multipliers.After that, it adds the sum of all raw bonus values to the starting attribute, and then applies the multiplier.Later, it does the same step for the final bonuses, but this time applying the values and multipliers to the half-calculated final attribute value.
    }

    #region Bonuses
    public class CRawBonus : CBaseAttribute
    {
        public CRawBonus(string name, float value = 0, float multiplier = 0) : base(name, value, multiplier)
        {

        }
    }

    public class CFinalBonus : CBaseAttribute
    {
        public CFinalBonus(string name, float value = 0, float multiplier = 0) : base(name, value, multiplier)
        {

        }
    }
    #endregion

    #endregion

}