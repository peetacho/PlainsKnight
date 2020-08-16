using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{

    public MeleeWeapons meleeWeapons;
    public RangedWeapons rangedWeapons;

    SpriteRenderer sr;
    UnityEngine.UI.Button btn;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (meleeWeapons != null)
        {
            initStatsM();
        }
        else if (rangedWeapons != null)
        {
            initStatsR();
        }
    }

    void initStatsM()
    {
        sr.sprite = meleeWeapons.artwork;
        gameObject.name = meleeWeapons.name;
    }

    void initStatsR()
    {
        sr.sprite = rangedWeapons.artwork;
        gameObject.name = rangedWeapons.name;
    }

    public void receiveWeapon()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            FindObjectOfType<CanvasManager>().toggleInteract(true);

            btn = CanvasManager.interactButton;
            btn.onClick.AddListener(taskOnClick);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<CanvasManager>().toggleInteract(false);
            btn.onClick.RemoveListener(taskOnClick);
        }
    }

    void taskOnClick()
    {
        List<MainWeapon> ow = GetWeapon.obtainedWeapons;
        int maxWeaponsAmt = 2;
        int cwIndex = GetWeapon.currentWeaponIndex;

        // allows only two weapons in inventory
        if (ow.Count < maxWeaponsAmt)
        {
            if (meleeWeapons != null)
            {
                // FindObjectOfType<GetWeapon>().mw = meleeWeapons;
                ow.Add(meleeWeapons);
            }
            if (rangedWeapons != null)
            {
                // FindObjectOfType<GetWeapon>().rw = rangedWeapons;
                ow.Add(rangedWeapons);
            }
        }
        else if (ow.Count >= maxWeaponsAmt)
        {
            // this creates a new weapon item game object that will replace the weapon the player is currently holding.
            // creates a drop weapon on ground effect
            GameObject resourcesWeaponItem = Resources.Load<GameObject>("Prefabs/WeaponItem");
            GameObject newWeaponItem = Instantiate(resourcesWeaponItem, transform.position, Quaternion.identity);
            // sees if current weapon is of type rangedweapons
            if (ow[cwIndex].GetType() == typeof(RangedWeapons))
            {
                newWeaponItem.GetComponent<WeaponItem>().rangedWeapons = (RangedWeapons)ow[cwIndex];
            }
            // sees if current weapon is of type meleeweapons
            else if (ow[cwIndex].GetType() == typeof(MeleeWeapons))
            {
                newWeaponItem.GetComponent<WeaponItem>().meleeWeapons = (MeleeWeapons)ow[cwIndex];
            }

            if (meleeWeapons != null)
            {
                // FindObjectOfType<GetWeapon>().mw = meleeWeapons;
                ow[cwIndex] = meleeWeapons;
            }
            if (rangedWeapons != null)
            {
                // FindObjectOfType<GetWeapon>().rw = rangedWeapons;
                ow[cwIndex] = rangedWeapons;
            }
        }

        // // for debugging
        // string str = "";
        // foreach (MainWeapon mainWeapon in ow)
        // {
        //     str += mainWeapon.ToString() + " ";
        // }
        // print(str);

        Destroy(gameObject);
        // print("got weapon item");
        FindObjectOfType<CanvasManager>().toggleInteract(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
