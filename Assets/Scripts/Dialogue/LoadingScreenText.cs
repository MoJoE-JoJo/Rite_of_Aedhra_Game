using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreenText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private List<string> tooltips = new List<string>()
    {
        "\"Never trust a Dvarger! They lie and they cheat and they steal. And they drink far too well for their size.\"\n-Clan-Elder Eirik",
        "Nox, the god of death, sea, and winter, guides lost souls to the realm beyond. After her death, the seas grew uneasy, and the turmoiled souls would rest no more.",
        "The great volcano Unbal spews fire and ash, blocking out the sun creating the Ashen Wastes.\nOnly Ignar, god of summer, fire, and war, can calm its rage.",
        "Earth, nature, and autumn, those were the domains of Gayith, and in his absence the lands withered.\n\nA black suffocating fog, the Arnsgoth, envelopes the woods, making them nearly impossible to traverse.",
        "Odurn upheld the mind, family, and spring. His death made a curse sweep across the land, maddening the mortal men, bringing ruin and end to civilization.",
        "\"But their greatest deceit, was tricking us into worship metal and stone. Our greatest shame was letting them.\"\n-Clan-Elder Envar",
        "Beware of the Eik, for they stalk the woods.\n\nWith few in numbers they scurry away, but when many, they tear the flesh off mortal bones.",
        "The Goleg, forged of metal and magma, behemmoths of strength, made by the Dvarger to serve and obey. They construct and build what they do not have the strength to do themselves."
    };
    public List<GameObject> models;

    // Start is called before the first frame update
    void Start()
    {
        var index = Random.Range(0, tooltips.Count);
        text.text = tooltips[index];
        models[index].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
