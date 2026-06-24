using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms; // 💡 確保有這一行，TextBox 和 DataGridView 才認得到

namespace Shop_System_csmumi_v._0._1._0
{
    public static class keybroadHelper
    {
        /// <summary>
        /// 數字鍵共用法
        /// </summary>
        public static void HandleNunberClick(object sender, TextBox targetTextBox)
        {
            Button clickButton = sender as Button;
            if (clickButton == null || targetTextBox == null) return;

            string numberText = clickButton.Text;
            targetTextBox.Text += numberText;
            targetTextBox.SelectionStart = targetTextBox.Text.Length;
            targetTextBox.Focus();
        }

        /// <summary>
        /// 虛擬 Enter 鍵大魔法
        /// </summary>
        public static void HandleEnterClick(TextBox targetTextBox, KeyEventHandler keyDownHandler)
        {
            if (targetTextBox != null && keyDownHandler != null)
            {
                KeyEventArgs fakeEnter = new KeyEventArgs(Keys.Enter);
                keyDownHandler(targetTextBox, fakeEnter);
            }
        }

        /// <summary>
        /// 💡 修改購物車選定項目數量的大魔法
        /// </summary>
        public static void ChangeQuantity(DataGridView dgv, List<CartItem> cartList, TextBox inputTextBox, Action updateUiAction)
        {
            if (dgv.CurrentRow == null || string.IsNullOrEmpty(inputTextBox.Text.Trim())) return;

            if (int.TryParse(inputTextBox.Text.Trim(), out int newQty))
            {
                if (newQty <= 0)
                {
                    System.Windows.Forms.MessageBox.Show("購買數量必須大於 0！", "系統提示");
                    return;
                }

                string selectedProductID = dgv.CurrentRow.Cells[0].Value.ToString();
                CartItem targetItem = cartList.Find(item => item.productInfo.ID == selectedProductID);

                if (targetItem != null)
                {
                    targetItem.Quantity = newQty;
                    updateUiAction();
                    inputTextBox.Clear();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("請輸入正確的數字格式！", "輸入錯誤");
                inputTextBox.Clear();
            }
        }

        /// <summary>
        /// 💡 倒退鍵 (<-) 大魔法
        /// </summary>
        public static void HandleBackspace(TextBox targetTextBox)
        {
            if (targetTextBox != null && targetTextBox.Text.Length > 0)
            {
                targetTextBox.Text = targetTextBox.Text.Substring(0, targetTextBox.Text.Length - 1);
                targetTextBox.SelectionStart = targetTextBox.Text.Length;
                targetTextBox.Focus();
            }
        }

        /// <summary>
        /// 💡 清除鍵 (Del) 大魔法
        /// </summary>
        public static void HandleClear(TextBox targetTextBox)
        {
            if (targetTextBox != null)
            {
                targetTextBox.Clear();
                targetTextBox.Focus();
            }
        }

        // ─────────────── 💡 這是新增的動態折扣核心 ───────────────

        /// <summary>
        /// 💡 自訂動態折扣大魔法（從輸入框抓折數）
        /// </summary>
        public static void ApplyCustomDiscount(List<CartItem> cartList, TextBox inputTextBox, Action updateUiAction, Action<double> setDiscountRateAction)
        {
            string inputText = inputTextBox.Text.Trim();

            // 嘗試將 "0.8" 之類的小數轉成 double 浮點數
            if (double.TryParse(inputText, out double inputRate))
            {
                if (inputRate <= 0 || inputRate > 1.0)
                {
                    System.Windows.Forms.MessageBox.Show("錯誤的折扣區間！倍率必須大於 0 且小於等於 1.0。", "折扣失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    inputTextBox.Clear();
                    return;
                }

                // 🎉 驗證成功，把算好的折數傳回主畫面
                setDiscountRateAction(inputRate);

                // 叫主畫面刷新金額
                updateUiAction();

                System.Windows.Forms.MessageBox.Show($"已成功套用自訂折扣：【 {inputRate * 10} 折】！", "折扣成功");
                inputTextBox.Clear();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("請輸入正確的小數格式！例如：0.85", "輸入錯誤");
                inputTextBox.Clear();
            }
        }
    }
}