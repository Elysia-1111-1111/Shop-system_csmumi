namespace Shop_System_csmumi_v._0._1._0
{
    public partial class 超商結帳系統 : Form
    {
        public 超商結帳系統()
        {
            InitializeComponent();
        }

        // 1. 10 顆數字鍵共用的點擊事件（記得去介面把 10 顆按鈕的 Click 通通連連看到這裡）
        private void btnNumber_Click(object sender, EventArgs e)
        {
            // 一行代碼，直接呼叫外包的鍵盤 Class，並告訴它要把數字吐給 textBox1
            keybroadHelper.HandleNunberClick(sender, textBox1);
        }

        // 2. 右下角大 Enter 鍵的點擊事件_____________________________________________________________________________________________
        private void btnEnter_Click(object sender, EventArgs e)
        {
            // 一行代碼，直接委託外包的鍵盤 Class，去觸發 textBox1_KeyDown
            keybroadHelper.HandleEnterClick(textBox1, textBox1_KeyDown);
        }
        //-----------------------------------------------------------------------------------------------------------------------
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // 可以放即時搜尋建議或驗証，暫時留空
        }
        //-----------------------------------------------------------------------------------------------------------------

        // 🏪 全域變數：超商的「商品總庫存清單」（假資料，等畫面寫好再抽換成 JSON 檔）
        private List<Product> ProductStockList = new List<Product>()
        {
        new Product { ID = "001", Name = "茶裏王日式綠茶", Price = 25, Stock=100 },
        new Product { ID = "002", Name = "科學麵脆麵", Price = 10 ,Stock =100},
        new Product { ID = "003", Name = "純喫茶紅茶", Price = 20 ,Stock = 100}
        };

        // 🛒 全域變數：目前正在結帳的「購物車明細清單」
        private List<CartItem> MyShoppingCart = new List<CartItem>();
        // 🛒 全域變數：目前正在結帳的「購物車明細清單」
       

        // 💰 💡 全域變數：目前這筆單的折扣率（預設是 1.0 代表不打折）
        // 如果打 9 折就改成 0.9，打 85 折就改成 0.85
        private double currentDiscountRate = 1.0;


        //-------------------------------------------------------------------------------------------------------------------
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // 1. 檢查是不是按下鍵盤上的 Enter 鍵
            if (e.KeyCode == Keys.Enter)
            {
                // 抓取你在搜尋框打的字（例如打 "001"），Trim() 可以清除不小心按到的前後空白空间
                string inputID = textBox1.Text.Trim();

                // 防呆：如果店員什麼都沒打就手殘按 Enter，直接 return 結束，什麼都不做
                if (string.IsNullOrEmpty(inputID)) return;

                // 2. 萬能搜尋：去我們的「商品總庫存清單」裡，撈看看有沒有人的 ID 跟打進去的字一模一樣？
                // kvp => 代表逐一檢查清單裡的每一項商品
                Product foundProduct = ProductStockList.Find(kvp => kvp.ID == inputID);

                // 3. 判斷有沒有撈到商品
                if (foundProduct != null)

                {
                    if (foundProduct.Stock <= 0)
                    {
                        System.Windows.Forms.MessageBox.Show($"警告：【{foundProduct.Name}】庫存耗盡！請先補貨。", "庫存不足", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Clear();
                        return; // 直接中斷整個事件，下面塞進購物車的程式碼完全不會被執行
                    }//
                     // 🎉 找到了！接下來檢查這個商品是不是已經出現在購物車裡了？
                    CartItem existingItem = MyShoppingCart.Find(item => item.productInfo.ID == inputID);

                    if (existingItem != null)
                    {
                        // 狀況 A：如果購物車裡早就有了，直接把購買數量 + 1 
                        existingItem.Quantity++;
                    }
                    else
                    {
                        // 狀況 B：如果是全新的商品，新建立一列購物車明細
                        CartItem newItem = new CartItem();
                        newItem.productInfo = foundProduct; // 把撈到的商品塞進去
                        newItem.Quantity = 1;               // 預設數量給 1

                        MyShoppingCart.Add(newItem);        // 推進購物車 List 陣列
                    }
                    foundProduct.Stock--;

                    // 💡 4. 資料更新完畢後，呼叫你寫的 UpdateUI 方法來刷新大黑框！
                    UpdateUI();

                    // 貼心防呆：刷完商品後，自動清空搜尋框，方便店員一秒刷下一個商品
                    textBox1.Clear();
                }
                else
                {
                    // 💀 查無此商品，無情彈窗警告
                    System.Windows.Forms.MessageBox.Show("查無此商品條碼，請重新輸入！", "系統提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Clear();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------

        private void UpdateUI()
        {
            // 1. 每次刷新畫面時，先把大黑框（dataGridView1）舊的 Rows 清空，防止資料重複疊加
            dataGridView1.Rows.Clear();

            // 💡 2. 宣告一個臨時的原價累加器（每次重新整理都從 0 元開始算）
            int totalAmount = 0;

            // 3. 用 foreach 迴圈，把目前購物車（MyShoppingCart）裡的商品一筆筆抽出來填表
            foreach (var item in MyShoppingCart)
            {
                if (item == null || item.productInfo == null) continue;

                // 建立表格的一列（依序是：商品ID、品名、數量、小計）
                object[] row = new object[]
                {
            item.productInfo.ID,
            item.productInfo.Name,
            item.Quantity,
            item.SubTotal
                };
                dataGridView1.Rows.Add(row);

                // 💡 4. 關鍵運算：每填入一列商品，就順手把它的「小計金額」加進原價總額裡
                totalAmount += item.SubTotal;
            }

            // 💡 5. 【折扣與實付金額大運算】
            // 拿剛剛加總好的原價總額，去乘以你目前的全域折扣率（currentDiscountRate）
            // Math.Round 負責四捨五入，最後 (int) 強制轉成整數（因為超商不收小數點的錢）
            int finalPayAmount = (int)Math.Round(totalAmount * currentDiscountRate);

            // 💡 6. 核心連動：把最終算出來的金額，無時差地塞給負責顯示的 label13！
            // ─────────────── ❌ 原本寫錯的名字 ───────────────
            // label13.Text = $"$ {finalPayAmount} (已扣折價)";

            // ⬇️⬇️⬇️ 💡 請手打修正成真正的青藍色數字控制項名字（以 label5 為例） ⬇️⬇️⬇️

            if (currentDiscountRate < 1.0)
            {
                // 讓青藍色數字顯示折扣後的錢
                label5.Text = $"{finalPayAmount} (已扣折價)";
            }
            else
            {
                // 正常沒打折時，讓青藍色數字顯示原價
                label5.Text = $"{finalPayAmount}";
            }
        }
        //------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 🖱️ 點擊「改數量」按鈕時觸發
        /// </summary>
        private void btnChangeQty_Click(object sender, EventArgs e)
        {
            // 呼叫剛才寫好的工具人，把大黑框 (dataGridView1)、購物車 (MyShoppingCart)、輸入框 (textBox1) 還有刷新大招 (UpdateUI) 一併傳過去！
            keybroadHelper.ChangeQuantity(dataGridView1, MyShoppingCart, textBox1, UpdateUI);
        }

        /// <summary>
        /// 🖱️ 點擊「現金結帳」按鈕時觸發
        /// </summary>
        private void btnCashPay_Click(object sender, EventArgs e)
        {
            // 1. 先用迴圈加總目前購物車裡的總金額
            int totalAmount = 0;
            foreach (var item in MyShoppingCart)
            {
                if (item != null) totalAmount += item.SubTotal;
            }

            if (totalAmount == 0)
            {
                System.Windows.Forms.MessageBox.Show("購物車內無商品，無法結帳！", "系統提示");
                return;
            }

            // 2. 抓取店員在 textBox1 輸入的顧客實收金額
            if (int.TryParse(textBox1.Text.Trim(), out int cashReceived))
            {
                // 3. 檢查錢夠不夠
                if (cashReceived < totalAmount)
                {
                    System.Windows.Forms.MessageBox.Show($"實收金額不足！還差 {totalAmount - cashReceived} 元。", "結帳失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                // 4. 計算找零
                int change = cashReceived - totalAmount;

                // 5. 將找零金額顯示在你的 label6 畫面上（請自己確認你顯示找零的 Label 叫什麼名字，改掉 label6）
                label6.Text = $"找零：$ {change}";

                // 🎉 彈出大成功的結帳收據發票
                System.Windows.Forms.MessageBox.Show($"結帳成功！\n總金額：{totalAmount} 元\n實收：{cashReceived} 元\n找零：{change} 元", "交易完成", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                // 6. 清空購物籃、洗乾淨畫面、清空輸入框，高高興興接下一位客人！
                MyShoppingCart.Clear();
                UpdateUI();
                textBox1.Clear();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("請在輸入框輸入【實收金額】後，再點擊現金結帳！", "系統提示");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 🖱️ 點擊「自訂折扣」大按鈕（智慧引導輸入版）
        /// </summary>
        private void btnCustomDiscount_Click(object sender, EventArgs e)
        {
            // 防呆：購物車沒東西，就不用玩打折了
            if (MyShoppingCart.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("購物車空空的，無法套用折扣！", "系統提示");
                return;
            }

            string inputText = textBox1.Text.Trim();

            // 💡 UX 引導：如果目前輸入框不是以 "0." 開頭，代表店員還沒輸入折數
            if (string.IsNullOrEmpty(inputText) || !inputText.StartsWith("0."))
            {
                // 🎉 貼心自動幫店員填好 "0."
                textBox1.Text = "0.";

                // 強制把游標移到最末端，讓他可以直接用右邊數字鍵打折數
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.Focus();

                return; // 佈置完 "0." 後暫時中斷，等待店員打完數字按第二次
            }

            // 💡 當店員打好數字（例如變成 "0.85"），第二次按下此按鈕時，直接呼叫外包核心！
            keybroadHelper.ApplyCustomDiscount(
                MyShoppingCart,
                textBox1,
                UpdateUI,
                val => currentDiscountRate = val // 隔空把折數寫回主畫面的全域變數
            );
        }
        //____________________________________________________________________________________________________________________________________________________________
        private void Form1_Load(object sender, EventArgs e)
        {
            // 表單載入初始化
        }

        private void button17_Click(object sender, EventArgs e)
        {

        }
        //________________________________________________________________________
        /// <summary>
        /// 🖱️ 點擊螢幕「<-」倒退按鈕
        /// </summary>
        private void btnBackspace_Click(object sender, EventArgs e)
        {
            // 呼叫工具人去幫 textBox1 刪掉最後一個字
            keybroadHelper.HandleBackspace(textBox1);
        }

        /// <summary>
        /// 🖱️ 點擊螢幕「Del」清除按鈕
        /// </summary>
        private void btnDel_Click(object sender, EventArgs e)
        {
            // 呼叫工具人去把 textBox1 徹底清空
            keybroadHelper.HandleClear(textBox1);
        }
    }
}
