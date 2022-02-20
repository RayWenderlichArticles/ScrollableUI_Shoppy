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

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Store : MonoBehaviour, IStore
{
    [SerializeField] private float wallet = 1000f;
    [SerializeField] private TextMeshProUGUI walletText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject[] items;
    [SerializeField] private int amount = 10;
    [SerializeField] private float discount = 0.3f;
    [SerializeField] private List<GameObject> cart;
    [SerializeField] private ScrollRect scrollRect;

    [Header("Status Texts")]
    [SerializeField] private string saleStatus = "We have an amazing sale on!";
    [SerializeField] private string addToCartStatus = "Added to cart...";
    [SerializeField] private string removeFromCartStatus = "Removed from cart...";
    [SerializeField] private string itemsPurchasedStatus = "Purchase complete...";

    private void Start()
    {
        UpdateWallet(0);
        for (int i = 0; i < amount; i++)
        {
            StoreItem item = Instantiate(items[Random.Range(0, items.Length)], transform).GetComponent<StoreItem>();
            item.Initialize(this);
        }
        scrollRect.onValueChanged.AddListener(GetScrollValue);
    }

    public void UpdateWallet(float price)
    {
        wallet += price;
        walletText.text = $"W ${wallet.ToString(CultureInfo.CurrentCulture)}";
    }

    public void AddToCart(GameObject item)
    {
        cart.Add(item);
    }

    public void RemoveFromCart(GameObject item)
    {
        cart.Remove(item);
    }

    public void UpdateStatus(string text)
    {
        statusText.text = text;
    }

    public void GetScrollValue(UnityEngine.Vector2 pos)
    {
        Debug.Log(amount * pos);
    }

    public float GetDiscount()
    {
        return discount;
    }

    public void Buy()
    {
        var totalPrice = 0f;

        foreach (var item in cart)
        {
            totalPrice -= item.GetComponentInChildren<StoreItem>().GetPrice();
            item.GetComponentInChildren<StoreItem>().TurnSelectionOutlineOff();
            Vector3.Lerp(item.transform.localScale, Vector3.zero, 1f);
            Destroy(item);
        }

        UpdateWallet(totalPrice - (totalPrice * discount));

        totalPrice = 0f;
        cart.Clear();

        UpdateStatus(GetStatusTextPurchaseComplete());
    }

    public float GetTotalPriceOfCart()
    {
        var totalPrice = 0f;

        foreach (var item in cart)
        {
            totalPrice += item.GetComponentInChildren<StoreItem>().GetPrice();
        }

        return totalPrice - (totalPrice * discount);
    }

    public string GetStatusTextSale()
    {
        return saleStatus;
    }

    public string GetStatusTextAddToCart()
    {
        return addToCartStatus;
    }
    public string GetStatusTextRemoveFromCart()
    {
        return removeFromCartStatus;
    }
    public string GetStatusTextPurchaseComplete()
    {
        return itemsPurchasedStatus;
    }
}
