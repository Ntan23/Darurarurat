using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    #region Singleton
    public static ShopManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(this.gameObject);
    }
    #endregion

    [SerializeField] private ObjectsToBuySO[] objectsToBuy;
    [SerializeField] private BuyPanelUI buyPanelUI;
    [SerializeField] private Animator firstAidBoxAnimator;
    [SerializeField] private TextMeshProUGUI currentMoneyText;
    private int tempIndex;
    private GameObject tempGO;
    [SerializeField] private Collider[] objectCollider;
    private bool canInput;
    private MoneyManager mm;
    
    void Start() 
    {
        mm = MoneyManager.instance;

        CheckItem();

        ChangeIsUpgradeValue(true);

        currentMoneyText.text = mm.GetCurrentMoney().ToString("0.00") + " Rp";

        StartCoroutine(OpenBox());
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && canInput)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider != null)
                {
                    if(hit.collider.name == "non alcohol wipes" || hit.collider.name == "tisu basah non alkohol") tempIndex = 0;

                    if(hit.collider.name == "petroleum jelly") tempIndex = 1;

                    if(hit.collider.name == "hydrocortisone cream" || hit.collider.name == "krim hidrokortison") tempIndex = 2;

                    if(hit.collider.name == "gauze pad" || hit.collider.name == "kain kasa") tempIndex = 3;
                    
                    if(hit.collider.name == "bandage" || hit.collider.name == "perban")tempIndex = 4;
                    
                    tempGO = hit.collider.gameObject;

                    buyPanelUI.UpdateText(hit.collider.name, objectsToBuy[tempIndex].price);
                    buyPanelUI.OpenBuyPanel();
                    canInput = false;
                }
            }
        }
    }

    private void CheckItem()
    {
        /*if Object index - n value is 1 , its mean true (already unlocked and vice versa*/
        for(int i = 0; i < objectsToBuy.Length; i ++)
        {
            if(PlayerPrefs.GetInt("Object" + objectsToBuy[i].objectIndex.ToString(), 0) == 1) 
            {
                objectCollider[i].enabled = false;
                objectCollider[i].gameObject.GetComponent<ObjectsToBuyMaterialChanger>().UpdateMaterialColor(Color.white);
            }

            if(PlayerPrefs.GetInt("Object" + objectsToBuy[i].objectIndex.ToString(), 0) == 0) 
            {
                objectCollider[i].enabled = true;
                objectCollider[i].gameObject.GetComponent<ObjectsToBuyMaterialChanger>().UpdateMaterialColor(Color.black);
            }
        }
    }

    public void Buy()
    {
        if(mm.GetCurrentMoney() < objectsToBuy[tempIndex].price)
        {
            Debug.Log("Not Enough Money");

            NoBuy();
        }

        if(mm.GetCurrentMoney() >= objectsToBuy[tempIndex].price)
        {
            //currentMoney -= objectsToBuy[tempIndex].price;
            mm.DecreaseMoney(objectsToBuy[tempIndex].price);

            currentMoneyText.text = mm.GetCurrentMoney().ToString("0.00") + " Rp";

            PlayerPrefs.SetInt("Object" + objectsToBuy[tempIndex].objectIndex.ToString(), 1);
            mm.SaveMoney();

            tempGO.GetComponent<ObjectsToBuyMaterialChanger>().ChangeToUnhoverMaterial();
            tempGO.GetComponent<ObjectsToBuyMaterialChanger>().UpdateMaterialColor(Color.white);

            buyPanelUI.CloseBuyPanel();

            tempGO.GetComponent<Collider>().enabled = false;
        }

    }

    public void NoBuy()
    {
        tempGO.GetComponent<ObjectsToBuyMaterialChanger>().ChangeToUnhoverMaterial();
        tempGO.GetComponent<ObjectsToBuyMaterialChanger>().UpdateMaterialColor(Color.black);
        buyPanelUI.CloseBuyPanel();
    }

    IEnumerator OpenBox()
    {
        canInput = false;
        yield return new WaitForSeconds(1.0f);
        firstAidBoxAnimator.Play("Open");
        yield return new WaitForSeconds(2.0f);
        canInput = true;
    }

    public void SetCanInput(bool value) => canInput = value;

    public void ChangeIsUpgradeValue(bool value) 
    {
        if(value) PlayerPrefs.SetInt("IsUpgrade", 1);
        if(!value) PlayerPrefs.SetInt("IsUpgrade", 0);
    }
}
