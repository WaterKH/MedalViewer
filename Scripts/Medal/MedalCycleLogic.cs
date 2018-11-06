using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MedalCycleLogic : MonoBehaviour
{
    // This field can be accesed through our singleton instance,
    // but it can't be set in the inspector, because we use lazy instantiation
    public bool stopped;
    public bool firstPass = true;
	public Dictionary<GameObject, int> medalChildrenHolders = new Dictionary<GameObject, int>();

    // Static singleton instance
	private static MedalCycleLogic instance;

    // Static singleton property
	public static MedalCycleLogic Instance
    {
        // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
		get { return instance ?? (instance = new GameObject("MedalCycleLogic").AddComponent<MedalCycleLogic>()); }
    }

    void Start()
    {
		StartCoroutine(CycleMedals(medalChildrenHolders));
    }

    // Instance method, this method can be accesed through the singleton instance
	public IEnumerator CycleMedals(Dictionary<GameObject, int> medals)
	{
		while(!stopped)
		{
			foreach(var m in medals)
			{
				var currMedal = m.Key.transform.GetChild(m.Value);

				m.Key.GetComponent<RawImage>().texture = currMedal.GetComponent<RawImage>().texture;
			}

			List<GameObject> keys = new List<GameObject>(medals.Keys);
			foreach(var key in keys)
			{
				medals[key] = (medals[key] + 1) % key.transform.childCount;
			}

			yield return new WaitForSeconds(1.0f);

		}
	}

	public void CycleMedals()
	{
		stopped = false;
		firstPass = true;
		StartCoroutine(CycleMedals(medalChildrenHolders));
	}

	public void StopCycleMedals()
	{
		stopped = true;
		firstPass = false;
	}
}