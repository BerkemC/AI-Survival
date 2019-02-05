using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionSequence : MonoBehaviour {

	public enum allPreconditions{highHealth,lowHealth,highAmmo,lowAmmo,enemyClose,enemyFar,noEnemy};
	public enum allEffects{moreAmmo,moreHealth,meleeStrike,rangedStrike,escape,lowerAmmo,lowerHealth};

	public string sequenceName;

	public List<allPreconditions> preconditions;
	public List<allEffects> effects;
	public List<allPreconditions> bonusConditions;

	public ActionSequence(string name, List<allPreconditions> preconditions,List<allEffects> effects,List<allPreconditions> bonus)
    {
		this.preconditions = preconditions;
		this.effects = effects;
		this.bonusConditions = bonus;
		sequenceName = name;
	}

	public bool IsConditionValid(allPreconditions condition)
    {
		foreach(allPreconditions x in preconditions)
        {
			if (x == condition) { return true; }	
		}
		return false;
	}

	public bool IsConditionValidForBonus(allPreconditions condition)
    {
		foreach(allPreconditions x in bonusConditions)
        {
			if (x == condition) { return true; }
		}
		return false;
	}


	public bool IsEffectValid(allEffects effect)
    {
		foreach(allEffects x in effects)
        {
			if (x == effect) {return true; }
		}

		return false;
	}
}
