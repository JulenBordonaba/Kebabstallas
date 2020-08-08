using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Effects/Create Effect")]
public class EffectData : ScriptableObject
{
    public string id;
    public float duration;
    public Sprite icon;
    public float damageReduction;
    public float slow;
    public float velocity;


    //estos 3 son los nuevos
    public float attackSpeed;
    public float attackDistance;
    public float attackDamage;


    //public float maneuverability;
    //public float acceleration;
    //public float shotDamage;
    //[Tooltip("Invierte los controles de la nave")]
    //public bool invertControls = false;
    [Tooltip("Impide usar las habilidades")]
    public bool silenceAbilities = false;
    //[Tooltip("Impide usar las combustibles")]
    //public bool silenceFuels = false;
    //[Tooltip("Activar si el efecto no es permanente")]
    

    public bool permanent = false;
    public DOT dot = new DOT();
    public AudioClip effectClip;

    #region PhotonSerialize
    public byte classId { get; set; }

    public static object Deserialize(byte[] data)
    {
        EffectData result = new EffectData();
        result.classId = data[0];
        return result;
    }

    public static byte[] Serialize(object customType)
    {
        EffectData c = (EffectData)customType;
        return new byte[] { c.classId };
    }
    #endregion
}

[System.Serializable]
public class DOT
{

    public DOT()
    {
        loopTime = 0;
        damagePerTick = 0;
        dotEffect = null;
    }
    public float loopTime;
    public float damagePerTick;
    public Coroutine dotEffect;
}
