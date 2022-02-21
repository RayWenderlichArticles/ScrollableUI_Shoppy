/*Copyright (c) 2021 Razeware LLC

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom
the Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

Notwithstanding the foregoing, you may not use, copy, modify,
merge, publish, distribute, sublicense, create a derivative work,
and/or sell copies of the Software in any work that is designed,
intended, or marketed for pedagogical or instructional purposes
related to programming, coding, application development, or
information technology. Permission for such use, copying,
modification, merger, publication, distribution, sublicensing,
creation of derivative works, or sale is expressly withheld.

This project and source code may use libraries or frameworks
that are released under various Open-Source licenses. Use of
those libraries and frameworks are governed by their own
individual licenses.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.*/

using System.Globalization;
using TMPro;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private int id = 0;
    [SerializeField] private string itemName;
    [SerializeField] private float price = 5f;
    [SerializeField] private GameObject outline;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject salePanel;

    private Store store;
    private bool isSelected = false;
    private float discount = 0;

    private void ApplyDiscount()
    {
        if (discount > 0)
        {
            ShowSaleBanner();
            store.UpdateStatus(store.GetStatusTextSale());
            priceText.text = $"${(price - (price * discount)).ToString(CultureInfo.CurrentCulture)} (${price.ToString(CultureInfo.CurrentCulture)})";
        }
        else
        {
            priceText.text = $"${price.ToString(CultureInfo.CurrentCulture)}";
            salePanel.SetActive(false);
        }
    }

    public void Initialize(IStore store)
    {
        this.store = (Store)store;
        discount = this.store.GetDiscount();
        nameText.text = itemName.ToUpper();
        ApplyDiscount();
    }

    public float GetPrice()
    {
        return price;
    }

    public void ShowSaleBanner()
    {
        salePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{discount * 100f}% OFF!";
        salePanel.SetActive(true);
    }

    public void TurnSelectionOutlineOff()
    {
        isSelected = false;
        outline.SetActive(false);
    }

    public void Select()
    {

        isSelected = !isSelected;
        outline.SetActive(isSelected);

        if (isSelected)
        {
            store.AddToCart(gameObject);
            store.UpdateStatus($"{store.GetStatusTextAddToCart()} | Total Cost: ${store.GetTotalPriceOfCart().ToString()}");
            
        }
        else
        {
            store.RemoveFromCart(gameObject);
            store.UpdateStatus($"{store.GetStatusTextRemoveFromCart()} | Total Cost: ${store.GetTotalPriceOfCart().ToString()}");
        }
    }
}
