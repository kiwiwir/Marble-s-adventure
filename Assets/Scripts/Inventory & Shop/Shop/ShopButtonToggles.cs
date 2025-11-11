using UnityEngine;

public class ShopButtonToggles : MonoBehaviour
{
    public void OpenPotionsShop()
    {
        if (ShopKeeper.currentShopKeeper != null)
        {
            ShopKeeper.currentShopKeeper.OpenPotionsShop();
        }
    }
    public void OpenEdibleShop()
    {
        if (ShopKeeper.currentShopKeeper != null)
        {
            ShopKeeper.currentShopKeeper.OpenEdibleShop();
        }
    }
    public void OpenOtherShop()
    {
        if(ShopKeeper.currentShopKeeper != null)
        {
            ShopKeeper.currentShopKeeper.OpenOtherShop();
        }
    }
}
