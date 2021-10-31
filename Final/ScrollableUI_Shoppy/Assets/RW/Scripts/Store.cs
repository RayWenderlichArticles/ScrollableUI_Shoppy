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

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Store : MonoBehaviour, IStore
{
    [SerializeField] float wallet = 1000f;
    [SerializeField] TextMeshProUGUI walletText;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] GameObject[] items;
    [SerializeField] int amount = 10;
    [SerializeField] float discount = 0.3f;
    [SerializeField] List<GameObject> cart;

    [Header("Status Texts")]
    [SerializeField] string saleStatus = "We have an amazing sale on!";
    [SerializeField] string addToCartStatus = "Added to cart...";

    void Start()
    {
        UpdateWallet(0);
        for (int i = 0; i < amount; i++)
        {
            Instantiate(items[Random.Range(0, items.Length)], transform);
        }
    }

    public void UpdateWallet(float amount)
    {
        wallet += amount;
        walletText.text = $"WALLET: ${wallet.ToString()}";
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
        }

        UpdateWallet(totalPrice - (totalPrice * discount));
    }

    public void Sell()
    {
        throw new System.NotImplementedException();
    }

    public string GetStatusTextSale()
    {
        return saleStatus;
    }

    public string GetStatusTextAddToCart()
    {
        return addToCartStatus;
    }
}
