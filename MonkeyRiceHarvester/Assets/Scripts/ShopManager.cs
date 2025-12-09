using UnityEngine;

public class ShopManager : MonoBehaviour
{
    
    [System.Serializable]
    public class ShopItem
    {
        public int bagIndex;
        public string itemName;
        public int bananaCost;
    }

    [Header("# Shop Items")]
    public ShopItem[] shopItems;

    public void PurchaseBag(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= shopItems.Length)
            return;

        ShopItem item = shopItems[itemIndex];
        int bagIndex = item.bagIndex;

        if (GameManager.instance.isBagTypeUnlocked[bagIndex])
        {
            
        }



    }



}
